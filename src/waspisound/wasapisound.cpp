/*
 * Copyright (c) 2015 Mehrzad Chehraz (mehrzady@gmail.com)
 * Released under the MIT License
 * http://chehraz.ir/mit_license
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#include "wasapisound.h"
#include <cmath>
#include <queue>
#include <assert.h>

BOOL AddFormat(FORMATINFO *pFormats, int count, LPWAVEFORMATEX pwfx) {	
	FORMATINFO format;
	format.nChannels = pwfx->nChannels;
	format.nSamplesPerSecond = pwfx->nSamplesPerSec;
	format.wBitsPerSample = pwfx->wBitsPerSample;
	pFormats[count] = format;
	return TRUE;
}

BOOL ContainsFormat(FORMATINFO *pFormats, int count, LPWAVEFORMATEX pwfx) {
	for (int i = 0; i < count; i++) {
		FORMATINFO format = pFormats[i];
		if (format.nChannels == pwfx->nChannels &&
			 format.nSamplesPerSecond == pwfx->nSamplesPerSec &&
			 format.wBitsPerSample == pwfx->wBitsPerSample) {
			return TRUE;
		}
	}
	return FALSE;
}

HRESULT EnumAudioClientFormats(IAudioClient *pAudioClient, LPWASAPISOUND lpws, LPWSTR lpDeviceName,
									    LPWAVEFORMATEX pwfx, DWORD cbwfx, WSENUMDEVICEFORMATSCB callback) {
	if(cbwfx < sizeof(WAVEFORMATEX)) {
		return E_OUTOFMEMORY;
	}
	// Get list of formats to test if supported
	FORMATINFO *pTestFormats = NULL;
	int nTestFormats = GetTestFormats(&pTestFormats);

	// Allocate memory for callback formats
	// We use this list to prevent calling back more than once for a format
	FORMATINFO *pReturnedFormats = new FORMATINFO[nTestFormats + 1]; 
	int nReturnedFormats = 0;

	// Iterate through test list
	for (int i = 0; i < nTestFormats; i++) {
		BOOL ret = FALSE;

		// Fill the WAVEFORMATEX from FORMATINFO
		FORMATINFO formatInfo = pTestFormats[i];
		pwfx->wBitsPerSample = formatInfo.wBitsPerSample;
		pwfx->nChannels = formatInfo.nChannels;
		pwfx->nSamplesPerSec = formatInfo.nSamplesPerSecond;
		pwfx->wFormatTag = WAVE_FORMAT_PCM;
		pwfx->cbSize = 0;
		pwfx->nBlockAlign = (pwfx->wBitsPerSample / 8) * pwfx->nChannels;
		pwfx->nAvgBytesPerSec = pwfx->nChannels * pwfx->nSamplesPerSec * (pwfx->wBitsPerSample / 8);

		// Check if the format is supported
		WAVEFORMATEX *pwfxToCall = NULL;
		WAVEFORMATEX *pwfxClosest = NULL;
		HRESULT hr = pAudioClient->IsFormatSupported(AUDCLNT_SHAREMODE_SHARED, pwfx, &pwfxClosest);
		
		if (S_OK == hr) {
			pwfxToCall = pwfx;
		}
		else if (pwfxClosest != NULL) {			
			pwfxToCall = pwfxClosest;
		}
		else {
			// Format not supported and no close format was found
			continue;
		}
		// Check if the callback is already called for the format or not
		BOOL alreadyCalled = ContainsFormat(pReturnedFormats, nReturnedFormats, pwfxToCall);		
		if (!alreadyCalled) {
			// and if the buffer is large enough
			DWORD size = pwfxToCall->cbSize + sizeof(WAVEFORMATEX);
			BOOL bufferEnough = size <= cbwfx;

			if (bufferEnough) {
				// Copy format to the buffer (in case of using a closest format)
				if (pwfxToCall != pwfx) {
					memcpy(pwfx, pwfxToCall, size);
				}
				// Call the callback
				ret = !callback(lpws, lpDeviceName, pwfx);				
				AddFormat(pReturnedFormats, nReturnedFormats++, pwfx);
			}		
		}
		if (pwfxClosest != NULL) {					
			CoTaskMemFree(pwfxClosest);					
		}
		if (ret) {
			delete[] pReturnedFormats;
			return S_OK;
		}
	}
	// Also get current format of the audio client
	WAVEFORMATEX *pcwfx = NULL;
	HRESULT hr = pAudioClient->GetMixFormat(&pcwfx);
	if (S_OK == hr) {
		// Check if the buffer is large enough
		DWORD size = pcwfx->cbSize + sizeof(WAVEFORMATEX);
		BOOL bufferEnough = size <= cbwfx;
		if (bufferEnough) {
			// Check if the callback is already called for the format or not
			BOOL alreadyCalled = ContainsFormat(pReturnedFormats, nReturnedFormats, pcwfx);	
			if (!alreadyCalled) {
				// Call the callback
				callback(lpws, lpDeviceName, pcwfx);
			}
		}
		CoTaskMemFree(pcwfx);
	}
	delete[] pReturnedFormats;
	return S_OK;
}

HRESULT EnumEndPointDevices(EDataFlow dataFlow, LPWASAPISOUND lpws, LPWSDEVICE lpDevice, WSENUMDEVICECB callback) {
	// Get device format from dataFlow
	DWORD dwType;
	switch (dataFlow) {
		case eCapture:
			dwType = WS_DEVICE_TYPE_INPUT;
			break;
		case eRender:
			dwType = WS_DEVICE_TYPE_OUTPUT;
			break;
		default:
			return E_INVALIDARG;
	}
	// Get device collection
	IMMDeviceCollection *pDevices = NULL;
	HRESULT hr = lpws->pEnumerator->EnumAudioEndpoints(dataFlow, DEVICE_STATE_ACTIVE, &pDevices);
	if (hr != S_OK) {			
		return hr;
	}
	// Get device count
	UINT cDevices = 0;
	hr = pDevices->GetCount(&cDevices);
	if (hr != S_OK) {
		pDevices->Release();
		return hr;
	}
	IMMDevice *pDevice;
	IPropertyStore *pPropStore;
	// Enumerate devices
	for (UINT i = 0; i < cDevices; i++) {
		hr = pDevices->Item(i, &pDevice);
		if (hr != S_OK) {	
			break;
		}
		// Get device id 
		LPWSTR pId = NULL;
		hr = pDevice->GetId(&pId);
		if (hr != S_OK) {	
			break;
		}
		if (lstrlenW(pId) >= (int)lpDevice->nMaxId) {
			// Device id is too long, cannot be copied to the szId field
			hr = E_INVALIDARG;
			break;; 
		}
		lstrcpynW(lpDevice->szId, pId, (int)lpDevice->nMaxId - 1);

		hr = pDevice->OpenPropertyStore(STGM_READ, &pPropStore);
		// Get device name
		PROPVARIANT varName;
		PropVariantInit(&varName);
		pPropStore->GetValue(PKEY_Device_FriendlyName, &varName);
		lstrcpynW(lpDevice->szName, varName.pwszVal, (int)lpDevice->nMaxName - 1);
		PropVariantClear(&varName);

		// Get device description
		PROPVARIANT varDesc;
		PropVariantInit(&varDesc);
		pPropStore->GetValue(PKEY_Device_DeviceDesc, &varDesc);
		lstrcpynW(lpDevice->szDescription, varDesc.pwszVal, (int)lpDevice->nMaxDesc - 1);
		PropVariantClear(&varDesc);

		pPropStore->Release();

		// Set device type (depending on the dataFlow arg
		lpDevice->dwType = dwType;		
		
		// Call the callback function
		callback(lpws, lpDevice);

		// Release device
		pDevice->Release();
	}
	// Release device collection
	pDevices->Release();
	return hr;
}

UINT GetTestFormats(FORMATINFO** ppTestFormatInfo) {
	static int nBitsPerSampleItem = sizeof(bitsPerSampleList) / sizeof(short);
	static int nChannelsItem = sizeof(nChannelsList) / sizeof(short);
	static int nSampleRateItem = sizeof(sampleRateList) / sizeof(DWORD);
	static int nTestFormats = nBitsPerSampleItem * nSampleRateItem * nChannelsItem;
	static FORMATINFO* pTestFormatInfo = NULL;

	// Check if the list is not already made
	if (pTestFormatInfo == NULL) {
		// Allocate memory for the list
		pTestFormatInfo = new FORMATINFO[nTestFormats];

		// Iterate through bits per samples, channels and sample rates
		// to make formats by combining them
		int iTestFormat = 0;
		for (int ibpp = 0; ibpp < nBitsPerSampleItem; ibpp++) {
			for (int ichannels = 0; ichannels < nChannelsItem; ichannels++) {
				for (int irate = 0; irate < nSampleRateItem; irate++) {
					FORMATINFO formatInfo;
					formatInfo.wBitsPerSample = bitsPerSampleList[ibpp];
					formatInfo.nChannels = nChannelsList[ichannels];
					formatInfo.nSamplesPerSecond = sampleRateList[irate];
					pTestFormatInfo[iTestFormat++] = formatInfo;
				}
			}
		}
	}
	*ppTestFormatInfo = pTestFormatInfo;
	return nTestFormats;
}

void Record(LPWASAPISOUND lpws) {	
	IAudioCaptureClient *pCaptureClient = lpws->pAudioCAptureClient;
	LPWAVEFORMATEX pwfx = lpws->pwfx;
	UINT frameSize = pwfx->nChannels * (pwfx->wBitsPerSample / 8);
	UINT bufferLength = (UINT)((1000.0 * lpws->nBufferFrames) / pwfx->nSamplesPerSec);

	while (lpws->state == WS_STATE_RECORDING) {
		BYTE *pData;
		UINT32 packetLength;
		UINT packetFrames = 0;
		UINT framesRead = 0;
		
		// Sleep for half of the buffer length, so that the buffer gets filled
		DWORD sleepLength = bufferLength / 2;
		DWORD beforeSleep = GetTickCount();
		// It's quite a long time so we Sleep(1) in a loop.
		// In this way, if recording gets stopped by the client
		// we can return immidiately.
		do {
			Sleep(1);
			if (lpws->state != WS_STATE_RECORDING) {
				break;
			}
		} while (GetTickCount() - beforeSleep < sleepLength);

		// Get number of frames available
		HRESULT hr = pCaptureClient->GetNextPacketSize(&packetLength);
		if (hr != S_OK) {
			return;
			// return hr;
		}		
	
		std::queue<SOUNDPACKET> *packetQueue = new std::queue<SOUNDPACKET>();
		while (packetLength > 0) {
			UINT32 framesAvailable;
			DWORD flags;
			BOOL silence = FALSE;

			hr = pCaptureClient->GetBuffer(&pData, &framesAvailable, &flags, NULL, NULL);
			if (hr != S_OK) {
				break;
			}
			// Check if result or flags indicate a silence
			SOUNDPACKET soundPacket;
			if ((flags & AUDCLNT_BUFFERFLAGS_SILENT)) {
				soundPacket.length = framesAvailable;
				soundPacket.pData = NULL;
				framesRead += framesAvailable;
			}
			else {
				UINT packetSize = framesAvailable * frameSize;
				BYTE *pPacketData = new BYTE[packetSize];
				memcpy(pPacketData, pData, packetSize);
				framesRead += framesAvailable; 						

				soundPacket.length = framesAvailable;
				soundPacket.pData = pPacketData;	
			}
			packetQueue->push(soundPacket);
			hr = pCaptureClient->ReleaseBuffer(framesAvailable);
			if (hr != S_OK) {
				break;
			}
			hr = pCaptureClient->GetNextPacketSize(&packetLength);
			if (hr != S_OK) {
				break;
			}
		}
		assert(hr == S_OK);
		if (hr != S_OK) {
			delete packetQueue;
			return;
		}
		WaitForSingleObject(lpws->hSyncSemaphore, INFINITE);
		std::queue<SOUNDPACKET>* pQueue = (std::queue<SOUNDPACKET>*)lpws->pQueue;
		
		// Put new packets into the queue
		UINT maxQueueSizeInBytes = (UINT)(defaultQueueLength / 1000.0 * pwfx->nSamplesPerSec * pwfx->nBlockAlign);
		while (!packetQueue->empty()) {
			// Get next recorded packet
			SOUNDPACKET soundPacket = packetQueue->front();

			// Make sure we have free space for the packet
			UINT soundPacketDataSize = 0;
			if (soundPacket.pData != NULL) {
				// Get packet size in bytes
				soundPacketDataSize = soundPacket.length * frameSize;

				// Check if packet is larger than maximum possible size
				// This should never happen
				if (soundPacketDataSize > maxQueueSizeInBytes) {
					delete[] soundPacket.pData;
					lpws->nBytesLost += soundPacketDataSize;
					continue;
				}

				// Drop packets from main queue util we get space for new packet
				while (lpws->nBytesInQueue + soundPacketDataSize > maxQueueSizeInBytes) {
					SOUNDPACKET frontPacket = pQueue->front();
					if (frontPacket.pData != NULL) {
						UINT frontPacketDataSize = frontPacket.length * frameSize;
						lpws->nBytesLost += frontPacketDataSize;
						lpws->nBytesInQueue -= frontPacketDataSize;
						delete[] frontPacket.pData;
					}					
					pQueue->pop();
				}
			}			
			pQueue->push(soundPacket);
			lpws->nBytesInQueue += soundPacketDataSize;
			packetQueue->pop();
		}
		delete packetQueue;
		ReleaseSemaphore(lpws->hSyncSemaphore, 1, NULL);
		if (hr != S_OK) {
			return;
		}
	}
}
extern "C" {	
	WASAPISOUNDAPI HRESULT WSEnumDevices(LPWASAPISOUND lpws, LPWSDEVICE lpDevice, WSENUMDEVICECB callback) {
		if (lpws == NULL || lpDevice == NULL || callback == NULL) {
			return E_POINTER;
		}
		if (lpDevice == NULL || callback == NULL) {
			return E_POINTER;
		}
		// Enumerate capture devices
		HRESULT hr = EnumEndPointDevices(eCapture, lpws, lpDevice, callback);
		if (hr != S_OK) {
			return hr;
		}
		// Enumerate render devices
		hr = EnumEndPointDevices(eRender, lpws, lpDevice, callback);		
		return hr;
   }
	WASAPISOUNDAPI HRESULT WSEnumDeviceFormats(LPWASAPISOUND lpws, LPWSTR lpDeviceId, LPWAVEFORMATEX pwfx, 
															 DWORD cbwfx, WSENUMDEVICEFORMATSCB callback) {		
		if (lpws == NULL || lpDeviceId == NULL || callback == NULL) {
			return E_POINTER;
		}
		// Get device by device enumerator
		IMMDevice *pDevice = NULL;
		HRESULT hr = lpws->pEnumerator->GetDevice(lpDeviceId, &pDevice);
		if (hr != S_OK) {	
			return hr;
		}
		// Get audio client
		IAudioClient* pAudioClient = NULL;
		hr = pDevice->Activate(IID_IAudioClient, CLSCTX_ALL, NULL, (void**)&pAudioClient);
		if (hr != S_OK) {	
			pDevice->Release();
			return hr;
		}
		// Enumerate formats
		hr = EnumAudioClientFormats(pAudioClient, lpws, lpDeviceId, pwfx, cbwfx, callback);

		pAudioClient->Release();
		pDevice->Release();
		return hr; 
   }
   WASAPISOUNDAPI HRESULT WSGetBufferLength(LPWASAPISOUND lpws, UINT *lpnBufferLength) {
		if (lpws == NULL || lpnBufferLength == NULL) {
			return E_POINTER;
		}
		if (lpws->state < WS_STATE_OPEN) {
			return E_FAIL;
		}
		*lpnBufferLength = defaultQueueLength;
		return S_OK;
	}
	WASAPISOUNDAPI HRESULT WSGetDeviceId(LPWASAPISOUND lpws, LPWSTR lpszDeviceId, DWORD iMaxLength) {
		if (lpws == NULL || lpszDeviceId == NULL) {
			return E_POINTER;
		}
		if (lpws->lpszDeviceId == NULL) {
			if (iMaxLength > 0) {
				lpszDeviceId[0] = (WCHAR)0;
			}
			return S_OK;
		}
		lstrcpynW(lpszDeviceId, lpws->lpszDeviceId, iMaxLength);
		return S_OK;
	}
	WASAPISOUNDAPI HRESULT WSGetFormat(LPWASAPISOUND lpws, LPWAVEFORMATEX pwfx, DWORD cbwfx) {
		if (lpws == NULL || pwfx == NULL) {
			return E_POINTER;
		}
		if (lpws->pwfx == NULL) {
			ZeroMemory(&pwfx, cbwfx);
			return S_OK;
		}
		DWORD size = (sizeof(WAVEFORMATEX) + lpws->pwfx->cbSize);
		if (cbwfx < size) {
			return E_OUTOFMEMORY;
		}
		memcpy(pwfx, lpws->pwfx, size);
		return S_OK;
	}
	WASAPISOUNDAPI HRESULT WSGetNumDevices(LPWASAPISOUND lpws, UINT *pcDevices) {
		if (lpws == NULL || pcDevices == NULL) {
			return E_POINTER;
		}		
		IMMDeviceCollection *pDevices = NULL;
		HRESULT hr = lpws->pEnumerator->EnumAudioEndpoints(eAll, DEVICE_STATE_ACTIVE, &pDevices);
		if (hr != S_OK) {	
			return hr;
		}
		hr = pDevices->GetCount(pcDevices);
		pDevices->Release();
		return hr; 
   }
	WASAPISOUNDAPI HRESULT WSGetPacketLength(LPWASAPISOUND lpws, UINT *lpnPacketLength) {
		if (lpws == NULL || lpnPacketLength == NULL) {
			return E_POINTER;
		}
		if (lpws->state < WS_STATE_OPEN) {
			return E_FAIL;
		}
		LPWAVEFORMATEX pwfx = lpws->pwfx;
		*lpnPacketLength = (UINT)((lpws->nBufferFrames * 1000.0) / pwfx->nSamplesPerSec);
		return S_OK;
   }
	WASAPISOUNDAPI HRESULT WSInit(LPWASAPISOUND *lppws) {
		if (lppws == NULL) {
			return E_POINTER;
		}
		LPWASAPISOUND pws = (LPWASAPISOUND)new WASAPISOUND();
		ZeroMemory(pws, sizeof(WASAPISOUND));

		HRESULT hr = CoCreateInstance(CLSID_MMDeviceEnumerator,
											   NULL,
											   CLSCTX_ALL,
											   IID_IMMDeviceEnumerator,
											   (void**)&pws->pEnumerator);
		if (hr != S_OK) {
			delete pws;
			return hr;
		}
		pws->state = WS_STATE_INIT;
		*lppws = pws;
		return S_OK; 
	}
	WASAPISOUNDAPI HRESULT WSOpen(LPWASAPISOUND lpws) {
		if (lpws->state >= WS_STATE_OPEN) {
			return E_FAIL;
		}
		if (lpws->lpszDeviceId == NULL) {
			return E_FAIL;
		}
		if (lpws->pwfx == NULL) {
			return E_FAIL;
		}
		// Get device
		IMMDevice *pDevice = NULL;
		HRESULT hr = lpws->pEnumerator->GetDevice(lpws->lpszDeviceId, &pDevice);
		if (hr != S_OK) {	
			return hr;
		}
		// Get device data flow
		IMMEndpoint *pEndpoint;
		hr = pDevice->QueryInterface(IID_IMMEndpoint, (void**)&pEndpoint);
		if (hr != S_OK) {	
			pDevice->Release();
			return hr;
		}
		EDataFlow dataFlow;
		hr = pEndpoint->GetDataFlow(&dataFlow);
		if (hr != S_OK) {	
			pEndpoint->Release();
			pDevice->Release();
			return hr;
		}
		hr = pEndpoint->Release();
		// Get audio client		
		IAudioClient *pAudioClient = NULL;
		hr = pDevice->Activate(IID_IAudioClient, CLSCTX_ALL, NULL, (void**)&pAudioClient);
		if (hr != S_OK) {	
			pDevice->Release();
			return hr;
		}
		// Initialize audio client
		DWORD streamFlags = 0;
		if (dataFlow == eRender) {
			// Loopback recoring on render devices (e.g speakers)
			streamFlags = AUDCLNT_STREAMFLAGS_LOOPBACK;
		}
		UINT hnsBufferDuration = defaultBufferLength * REFTIMES_PER_MS;
		hr = pAudioClient->Initialize(AUDCLNT_SHAREMODE_SHARED, streamFlags,
											   hnsBufferDuration, 0, lpws->pwfx, NULL);		
		if (hr != S_OK) {
			pDevice->Release();
			pAudioClient->Release();
			return hr;
		}

		// Get buffer size
		UINT nBufferFrames;
		hr = pAudioClient->GetBufferSize(&nBufferFrames);
		if (hr != S_OK) {
			pDevice->Release();	
			pAudioClient->Release();
			return hr;
		}
		LPWAVEFORMATEX pwfx = lpws->pwfx;
		lpws->nBufferFrames = nBufferFrames;
		lpws->nBytesLost = 0;
		lpws->nBytesInQueue = 0;
		lpws->nFramesInQueue = 0;

		// Get audio capture client
		IAudioCaptureClient *pAudioCaptureClient = NULL;
		hr = pAudioClient->GetService(IID_IAudioCaptureClient, (void**)&pAudioCaptureClient);
		if (hr != S_OK) {
			pAudioClient->Release();
			pDevice->Release();
			return hr;
		}	
		pDevice->Release();
		// Set context struct fields		
		lpws->pAudioCAptureClient = pAudioCaptureClient;
		lpws->pAudioClient = pAudioClient;
		lpws->state = WS_STATE_OPEN;
		return S_OK;
   }
	WASAPISOUNDAPI HRESULT WSRead(LPWASAPISOUND lpws, LPBYTE lpBuffer, DWORD offset, DWORD length, BOOL end,
											LPINT lpBytesRead) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->state != WS_STATE_RECORDING) {
			return E_FAIL;
		}
		if (end) {
			lpws->state = WS_STATE_STOPPING;
			// Wait for the recorder thread
			WaitForSingleObject(lpws->hRecordThread, INFINITE);
		}

		UINT bytesRead = 0;
		WAVEFORMATEX *pwfx = lpws->pwfx;
		UINT frameSize = pwfx->nChannels * (pwfx->wBitsPerSample / 8);		

		WaitForSingleObject(lpws->hSyncSemaphore, INFINITE);
		// Return 'zero's for lost bytes
		if (lpws->nBytesLost > 0) {
			UINT nEmptyDataBytes = min(lpws->nBytesLost, length);
			for (UINT i = 0; i < nEmptyDataBytes; i++) {
				lpBuffer[offset + i] = 0;
			}
			lpws->nBytesLost -= nEmptyDataBytes;
			bytesRead += nEmptyDataBytes;
		}
		// Copy packets from the queue to the buffer
		std::queue<SOUNDPACKET>* pQueue = (std::queue<SOUNDPACKET>*)lpws->pQueue;
		while (bytesRead < length && !pQueue->empty()) {
			// Get next packet
			SOUNDPACKET soundPacket = pQueue->front();
			INT packetSize = soundPacket.length * frameSize;

			// Check if the buffer is large enough to contain the whole packet
			INT remaining = (INT)length - (INT)bytesRead;
 			if (packetSize <= remaining) {
				if (soundPacket.pData != NULL) {
					memcpy(lpBuffer + bytesRead, soundPacket.pData, packetSize);
					delete[] soundPacket.pData;
				}
				else {
					// It is a silence packet, write zero bytes.
					memset(lpBuffer + bytesRead, 0, packetSize);
				}
				bytesRead += packetSize;
				pQueue->pop();
				if (soundPacket.pData != NULL) {
					lpws->nBytesInQueue -= packetSize;
				}
			}
			else {
				if (end) {
					// If it is the last read, copy data bytes as much as possible
					// Do not care if entire packed is not copied.
					if (soundPacket.pData != NULL) {
						memcpy(lpBuffer + bytesRead, soundPacket.pData, remaining);
					}
					else {
						// It is a silence packet, write zero bytes.
						memset(lpBuffer + bytesRead, 0, remaining);
					}
				}
				// Cannot copy more packets to the buffer
				break;
			}
		}
		ReleaseSemaphore(lpws->hSyncSemaphore, 1, NULL);
		*lpBytesRead = bytesRead;
		return S_OK;
   }
	WASAPISOUNDAPI HRESULT WSSetDeviceId(LPWASAPISOUND lpws, LPWSTR lpszDeviceId) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->lpszDeviceId != NULL) {
			delete[] lpws->lpszDeviceId;
			lpws->lpszDeviceId = NULL;
		}
		if (lpszDeviceId != NULL) {
			// Check if the device id is valid
			IMMDevice *pDevice = NULL;
			HRESULT hr = lpws->pEnumerator->GetDevice(lpszDeviceId, &pDevice);
			if (hr != S_OK) {	
				return hr;
			}
			pDevice->Release();

			// Copy device id to the related field
			DWORD len = lstrlenW(lpszDeviceId);
			LPWSTR pdevid = new WCHAR[len + 1];
			lstrcpyW(pdevid, lpszDeviceId);
			lpws->lpszDeviceId = pdevid;
		}
		return S_OK;
	}
	WASAPISOUNDAPI HRESULT WSSetFormat(LPWASAPISOUND lpws, LPWAVEFORMATEX pwfx) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->pwfx != NULL) {
			delete lpws->pwfx;
			lpws->pwfx = NULL;
		}
		if (pwfx != NULL) {
			DWORD size = (sizeof(WAVEFORMATEX) + pwfx->cbSize);
			BYTE *pbwfx = new BYTE[size];
			memcpy(pbwfx, pwfx, size);
			lpws->pwfx = (LPWAVEFORMATEX)pbwfx;
		}
		return S_OK;
	}
	WASAPISOUNDAPI HRESULT WSStart(LPWASAPISOUND lpws) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->state != WS_STATE_OPEN) {
			return E_FAIL;
		}

		HANDLE hRecordThread = CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)&Record, lpws, CREATE_SUSPENDED, 0);
		if (hRecordThread == NULL) {
			return E_FAIL;
		}

		HANDLE hSyncSemaphore = CreateSemaphore(NULL, 1, 1, NULL);
		if (hSyncSemaphore == NULL) {
			CloseHandle(hRecordThread);
			return E_FAIL;
		}

		HRESULT hr = lpws->pAudioClient->Start();
		if (hr != S_OK) {
			CloseHandle(hRecordThread);
			CloseHandle(hSyncSemaphore);
			return hr;
		}
		lpws->pQueue = (LPVOID*)new std::queue<SOUNDPACKET>();

		lpws->hSyncSemaphore = hSyncSemaphore;
		lpws->hRecordThread = hRecordThread;
		lpws->state = WS_STATE_RECORDING;
		DWORD retval = ResumeThread(hRecordThread);

		return S_OK; 
	}
	WASAPISOUNDAPI HRESULT WSStop(LPWASAPISOUND lpws) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->state == WS_STATE_OPEN) {
			return S_OK;
		}
		if (lpws->state == WS_STATE_RECORDING) {
			lpws->state = WS_STATE_STOPPING;
			// Wait for the recorder thread
			WaitForSingleObject(lpws->hRecordThread, INFINITE);
		}
		lpws->state = WS_STATE_OPEN;		

		std::queue<SOUNDPACKET>* pQueue = (std::queue<SOUNDPACKET>*)lpws->pQueue;
		while (!pQueue->empty()) {
			SOUNDPACKET packet = pQueue->front();
			if (packet.pData != NULL) {
				delete packet.pData;
			}
			pQueue->pop();
		}
		delete pQueue;
		lpws->pQueue = NULL;
		lpws->nBytesInQueue = 0;
		HRESULT hr = lpws->pAudioClient->Stop();

		CloseHandle(lpws->hSyncSemaphore);
		lpws->hSyncSemaphore = NULL;

		CloseHandle(lpws->hRecordThread);
		lpws->hRecordThread = NULL;
		return hr; 
	}
	WASAPISOUNDAPI HRESULT WSUninit(LPWASAPISOUND lpws) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->pEnumerator != NULL) {
			lpws->pEnumerator->Release();
			lpws->pEnumerator = NULL;
		}
		if (lpws->lpszDeviceId != NULL) {
			delete[] lpws->lpszDeviceId;
			lpws->lpszDeviceId = NULL;
		}
		delete lpws; 
		return S_OK; 
   }
	WASAPISOUNDAPI HRESULT WSClose(LPWASAPISOUND lpws) {
		if (lpws == NULL) {
			return E_POINTER;
		}
		if (lpws->state <= WS_STATE_INIT) {
			return S_OK;
		}
		if (lpws->state == WS_STATE_RECORDING) {
			WSStop(lpws);
		}
		if (lpws->pAudioCAptureClient != NULL) {
			lpws->pAudioCAptureClient->Release();
		}
		if (lpws->pAudioClient != NULL) {
			lpws->pAudioClient->Release();
		}
		lpws->state = WS_STATE_INIT;
		return S_OK; 
   }
}