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
#ifndef __WASPISOUND_H
#define __WASPISOUND_H
#include "audioclient.h"
#include "Mmdeviceapi.h"
#include "Functiondiscoverykeys_devpkey.h"
#include "windows.h"
#include "dllmain.h"

#define WASAPISOUNDAPI __declspec(dllexport)

#define WS_DEVICE_TYPE_INPUT		0x00000001
#define WS_DEVICE_TYPE_OUTPUT		0x00000002
#define WS_MAXNAMELEN				256

#define WS_STATE_INIT				1
#define WS_STATE_OPEN				2
#define WS_STATE_RECORDING			3
#define WS_STATE_STOPPING			4

struct FORMATINFO {
	short wBitsPerSample;
	short nChannels;
	DWORD nSamplesPerSecond;
}; 

struct SOUNDPACKET {
	LPBYTE pData;
	UINT32 length;
};

typedef struct  {
	WCHAR		szDescription[WS_MAXNAMELEN];
	WCHAR		szId[WS_MAXNAMELEN];
	DWORD		nMaxDesc;
	DWORD    nMaxId;
	DWORD		nMaxName;
	WCHAR		szName[WS_MAXNAMELEN];
	UINT		dwType;
} WSDEVICE, FAR *LPWSDEVICE;

typedef struct {	
	IAudioClient*			pAudioClient;
	IAudioCaptureClient*	pAudioCAptureClient;
	UINT						nBufferFrames;
	UINT32					nBytesLost;
	UINT32					nBytesInQueue;
	LPWSTR					lpszDeviceId;
	UINT						dwDeviceType;
	IMMDeviceEnumerator *pEnumerator;
	UINT						nFramesInQueue;
	BOOL						opened;
	LPVOID				  *pQueue;
	HANDLE					hSyncSemaphore;
	int						state;
	HANDLE					hRecordThread;
	LPWAVEFORMATEX			pwfx;
} WASAPISOUND, FAR *LPWASAPISOUND;

typedef BOOL (CALLBACK* WSENUMDEVICECB)(LPWASAPISOUND lpws, LPWSDEVICE lpDevice);
typedef BOOL (CALLBACK* WSENUMDEVICEFORMATSCB)(LPWASAPISOUND lpws, LPWSTR lpDeviceId, LPWAVEFORMATEX pwfx);

const CLSID CLSID_MMDeviceEnumerator = __uuidof(MMDeviceEnumerator);
const IID IID_IAudioClient = __uuidof(IAudioClient);
const IID IID_IAudioCaptureClient = __uuidof(IAudioCaptureClient);
const IID IID_IMMDeviceEnumerator = __uuidof(IMMDeviceEnumerator);
const IID IID_IMMEndpoint = __uuidof(IMMEndpoint);

const int REFTIMES_PER_SEC = 10000000;
const int REFTIMES_PER_MS = 10000;
const int defaultQueueLength = 4000;
const int defaultBufferLength = 200;

const short bitsPerSampleList[] = { 8, 16 };
const short nChannelsList[] = {1, 2};
const DWORD sampleRateList[] = {11025, 22050, 44100, 48000, 96000};


BOOL AddFormat(FORMATINFO *pFormats, int count, LPWAVEFORMATEX pwfx);
HRESULT EnumAudioClientFormats(IAudioClient *pAudioClient, LPWASAPISOUND lpws, LPWSTR lpDeviceId,
									    WAVEFORMATEX *pwfx, DWORD cbpwfx, WSENUMDEVICEFORMATSCB callback);
UINT GetTestFormats(FORMATINFO **ppFormats);
#endif