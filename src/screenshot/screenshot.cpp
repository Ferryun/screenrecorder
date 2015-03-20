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
#include "screenshot.h"
extern "C" {
   __declspec(dllexport) HRESULT SSClose(SCREENSHOT *ps) {
		if (ps == NULL) {
			return E_POINTER;
		}
		ReleaseDDSurfaces(ps);
		ReleaseDirectDraw(ps);	
		delete ps;
		return S_OK; 
   }
	__declspec(dllexport) HRESULT SSDrawCursor(BYTE* pBitmapBits, INT xCursor, INT yCursor, INT x, INT y, INT width,
											   INT height, INT bpp) {
		// TODO: There may be much easier implementation using DrawIconEx or something...
		if (pBitmapBits == NULL) {
			return E_POINTER;
		}		
		if (bpp != BPP_16 && bpp != BPP_24) {
			return E_NOINTERFACE; // NOT SUPPORTED
		}
		// Get cursor info
		CURSORINFO ci;
		ci.cbSize = sizeof(CURSORINFO);
		BOOL gcResult = GetCursorInfo(&ci);
		if (!gcResult) {
			return E_FAIL;
		}
		if (ci.flags != CURSOR_SHOWING) {
			return E_FAIL;
		}
		HCURSOR hCursor = ci.hCursor;
		if (hCursor == NULL) {
			return E_FAIL;
		}

		// Get icon info from the cursor
		ICONINFO iconInfo;
		BOOL giResult = GetIconInfo(hCursor, &iconInfo);
		if (giResult == FALSE) {
			return E_FAIL;
		}
		if (iconInfo.hbmMask == NULL) {
			return E_FAIL;
		}

		// Cursor positon
		xCursor = (xCursor != -1 ? xCursor : ci.ptScreenPos.x) - iconInfo.xHotspot;
		yCursor = (yCursor != -1 ? yCursor : ci.ptScreenPos.y) - iconInfo.yHotspot;

		if (iconInfo.hbmColor != NULL) {
			// It is a colored cursor
			// Delete mask, we are using the colored bitmap
			DeleteObject(iconInfo.hbmMask);
			
			// Get bitmap object
			BITMAP colorBitmap;
			LONG goResult = GetObject(iconInfo.hbmColor, sizeof(BITMAP), (LPVOID)&colorBitmap);
			if (goResult == 0) {
				DeleteObject(iconInfo.hbmColor);
				return E_FAIL;
			}

			// Check bpp and number of plains
			// Only 32bit colored cursors are supported
			if (colorBitmap.bmBitsPixel != 32 || colorBitmap.bmPlanes != 1) {
				DeleteObject(iconInfo.hbmColor); 
				return E_FAIL;
			}

			// Get bitmap bits
			BYTE *pColorBits = new BYTE[colorBitmap.bmWidthBytes * colorBitmap.bmHeight];
			if (pColorBits == NULL) {
				DeleteObject(iconInfo.hbmColor);
				return E_FAIL;
			}
			LONG gbbResult = GetBitmapBits(iconInfo.hbmColor, colorBitmap.bmWidthBytes * colorBitmap.bmHeight, pColorBits);
			if (gbbResult == 0) {
				delete[] pColorBits;
				DeleteObject(iconInfo.hbmColor);
				return E_FAIL;
			}

			int cursorWidth = colorBitmap.bmWidth;
			int cursorHeight = colorBitmap.bmHeight;
			// Calculate intersect of cursor rectangle and bitmap rectangle
			// Cursor rectanle
			RECT cursorRect;
			cursorRect.bottom = yCursor + cursorHeight;
			cursorRect.left = xCursor;
			cursorRect.right = xCursor + cursorWidth;
			cursorRect.top = yCursor;

			// Bitmap rectangle
			RECT bitmapRect;
			bitmapRect.bottom = y + height;
			bitmapRect.left = x;
			bitmapRect.right = x + width;
			bitmapRect.top = y;
			
			// Get intersect of cursor rectangle and bitmap rectangle
			RECT intesectRect;
			BOOL intesectResult = IntersectRect(&intesectRect, &bitmapRect, &cursorRect);
			if (!intesectResult) {
				return E_FAIL;
			}

			// Offset intersect rect to cursor origin
			RECT drawRect = intesectRect;
			OffsetRect(&drawRect, -cursorRect.left, -cursorRect.top);
			
			// Copy cursor bits to the input bitmap
			int bytesPerPixel = (bpp / 8);
			int bitmapPitch = width * bytesPerPixel + ((width * bytesPerPixel) % PITCH_FACTOR);
			for (int i = drawRect.top; i < drawRect.bottom; i++) {
				for (int j = drawRect.left; j < drawRect.right; j++) {
					DWORD cursorPixel = *(DWORD*)&pColorBits[i * colorBitmap.bmWidthBytes + j * 4];
					// Get cursor pixel data
					BYTE cR = (BYTE)(cursorPixel & 0xFF);
					BYTE cG = (BYTE)((cursorPixel & 0xFF00) >> 8);
					BYTE cB = (BYTE)((cursorPixel & 0xFF0000) >> 16);
					BYTE cA = (BYTE)((cursorPixel & 0xFF000000) >> 24);
					// Calculate bitmap pixel index
					int pixelIndex = (height - (yCursor - y + i) - 1) * bitmapPitch + ((xCursor - x) + j) * bytesPerPixel;
					if (bpp == BPP_24) {
						// Get bitmap pixel data
						DWORD pixel = *(DWORD*)&pBitmapBits[pixelIndex];					
						BYTE bR = (BYTE)(pixel & 0xFF);
						BYTE bG = (BYTE)((pixel & 0xFF00) >> 8);
						BYTE bB = (BYTE)((pixel & 0xFF0000) >> 16);
						// Apply cursor pixel					
						pBitmapBits[pixelIndex] = ((cA * cR) + ((255 - cA) * bR)) / 255;
						pBitmapBits[pixelIndex + 1] = ((cA * cG) + ((255 - cA) * bG)) / 255;
						pBitmapBits[pixelIndex + 2] = ((cA * cB) + ((255 - cA) * bB)) / 255;
					}
					else if (bpp == BPP_16) { // RGB555	
						// Convert cursor pixel data to RGB555
						BYTE cR5 = cR >> 3;
						BYTE cG5 = cG >> 3;
						BYTE cB5 = cB >> 3;
						// Get bitmap pixel data
						WORD pixel = *(WORD*)&pBitmapBits[pixelIndex];				
						BYTE bR = (BYTE)(pixel & 0x1F);
						BYTE bG = (BYTE)((pixel & 0x3E0) >> 5);
						BYTE bB = (BYTE)((pixel & 0x7C00) >> 10);
						// Apply cursor pixel
						BYTE pR = ((cA * cR5) + ((255 - cA) * bR)) / 255;
						BYTE pG = ((cA * cG5) + ((255 - cA) * bG)) / 255;
						BYTE pB = ((cA * cB5) + ((255 - cA) * bB)) / 255;
						*(WORD*)&pBitmapBits[pixelIndex] = (1 << 15) | (pB << 10) | (pG << 5) | pB; // RGB555
					}
				}
			}
			// Delete colored bitmap
			delete[] pColorBits;
			DeleteObject(iconInfo.hbmColor);
			return S_OK;
		}
		else {
			// It is ablack & white cursor
			// Get mask bitmap
			BITMAP maskBitmap;
			int goResult = GetObject(iconInfo.hbmMask, sizeof(BITMAP), (LPVOID)&maskBitmap);
			if (goResult == 0) {
				DeleteObject(iconInfo.hbmMask);
				return E_FAIL;
			}

			// Check bpp and number of plains
			if (maskBitmap.bmBitsPixel != 1 || maskBitmap.bmPlanes != 1) {
				DeleteObject(iconInfo.hbmMask);
				return E_FAIL;
			}
			
			// Get bitmap bits
			BYTE *pMaskBits = new BYTE[maskBitmap.bmWidthBytes * maskBitmap.bmHeight];
			if (pMaskBits == NULL) {
				DeleteObject(iconInfo.hbmMask);
				return E_OUTOFMEMORY;
			}
			LONG gbbResult = GetBitmapBits(iconInfo.hbmMask, maskBitmap.bmWidthBytes * maskBitmap.bmHeight, pMaskBits);
			if (gbbResult == 0) {
				delete[] pMaskBits;
				DeleteObject(iconInfo.hbmMask);
				return E_FAIL;
			}
			int cursorWidth = maskBitmap.bmWidth;
			int cursorHeight = maskBitmap.bmHeight / 2;		

			// Get intersect of cursor rectangle and bitmap rectangle
			RECT cursorRect;
			SetRect(&cursorRect, xCursor, yCursor, xCursor + cursorWidth, yCursor + cursorHeight);

			RECT bitmapRect;
			SetRect(&bitmapRect, x, y, x + width, y + height);
	
			RECT intesectRect;
			BOOL intesectResult = IntersectRect(&intesectRect, &bitmapRect, &cursorRect);
			if (!intesectResult) {
				return E_FAIL;
			}

			// Offset intersect rect to cursor origin
			RECT drawRect = intesectRect;
			OffsetRect(&drawRect, -cursorRect.left, -cursorRect.top);

			// Copy cursor bits to the input bitmap
			int bytesPerPixel = (bpp / 8);
			int bitmapPitch = width * bytesPerPixel + ((width * bytesPerPixel) % PITCH_FACTOR);
			for (int i = drawRect.top; i < drawRect.bottom; i++) {
				for (int j = drawRect.left; j < drawRect.right; j++) {
					// Get AND mask
					BYTE andByte = *(pMaskBits + i * maskBitmap.bmWidthBytes +  j / 8);
					DWORD andMask = andByte & (0x80 >> (j % 8)) ? 0xFFFFFF : 0;
					// Get XOR mask
					BYTE xorByte = *(pMaskBits + (cursorHeight + i) * maskBitmap.bmWidthBytes +  j / 8);
					DWORD xorMask = xorByte & (0x80 >> (j % 8)) ? 0xFFFFFF : 0;
					// Calculate bitmap pixel index
					int pixelIndex = (height - (yCursor - y + i) - 1) * bitmapPitch + ((xCursor - x) + j) * bytesPerPixel;
					if (bpp == BPP_24) {
						// Get bitmap pixel
						DWORD pixel = *(DWORD*)&pBitmapBits[pixelIndex];
						// Apply masks
						pixel = (pixel & andMask) ^ xorMask;
						pBitmapBits[pixelIndex] = (BYTE)(pixel & 0xFF);
						pBitmapBits[pixelIndex + 1] = (BYTE)((pixel & 0xFF00) >> 8);
						pBitmapBits[pixelIndex + 2] = (BYTE)((pixel & 0xFF0000) >> 16);					
					}
					else if (bpp == BPP_16) {
						// Get bitmap pixel
						WORD pixel = *(WORD*)&pBitmapBits[pixelIndex];
						// Apply masks
						pixel = (pixel & (WORD)andMask) ^ (WORD)xorMask;					
						*(WORD*)&pBitmapBits[pixelIndex] = pixel;
					}
				}
			}
			// Delete mask bitmap
			DeleteObject(iconInfo.hbmMask);
			delete[] pMaskBits;
			return S_OK;
		}	
	}
	__declspec(dllexport) HRESULT SSFormatError(LONG error, LPWSTR pMessage) {
		if (pMessage == NULL) {
			return E_POINTER;
		}
		BOOL isSSError = FormatSSError(error, pMessage);
		if (isSSError) {
			return S_OK;
		}
		BOOL isDDError = FormatDDError(error, pMessage);
		if (isDDError) {
			return S_OK;
		}
		return E_INVALIDARG;
	}
   __declspec(dllexport) HRESULT SSOpen(INT width, INT height, int bpp, SCREENSHOT **pps) {	
		if (pps == NULL) {
			return E_POINTER;
		}
		SCREENSHOT *ps = new SCREENSHOT();
		ZeroMemory(ps, sizeof(SCREENSHOT));

		ps->size.cx = width;
		ps->size.cy = height;

		// Create directdraw instance
		HRESULT hr = CreateDirectDraw(ps);
		if (hr != S_OK) {
			SSClose(ps);
			return hr;
		}

		// Create Primary Surface & Temporary Surface
		hr = CreateDDSurfaces(ps);
		if (hr != S_OK) {
			SSClose(ps);
			return hr;
		}
		hr = SetOutputBitsPerPixel(ps, bpp);
		if (S_OK != hr) {
			SSClose(ps);
			return hr;
		}

		// Calculate pitch
		long pitch = ps->size.cx * (ps->dstBPP / 8);
		if (pitch % PITCH_FACTOR != 0) {
			pitch = pitch + PITCH_FACTOR - pitch % PITCH_FACTOR;
		}
		ps->pitch = pitch;
		*pps = ps;
		return S_OK;
  }
   __declspec(dllexport) HRESULT SSTake(SCREENSHOT* ps, INT x, INT y, BYTE *pBuffer) {
		if (ps == NULL) {
			return E_POINTER;
		}


		// Get Input rectangle
		RECT rect;
		SetRect(&rect, x, y, x + ps->size.cx, y + ps->size.cy);

		// Get screen rectangle
		RECT screenRect;
		SetRect(&screenRect, 0, 0, GetSystemMetrics(SM_CXSCREEN), GetSystemMetrics(SM_CYSCREEN));

		// Get intersect of screen rectangle and input rectangle
		RECT intersectRect;
		IntersectRect(&intersectRect, &screenRect, &rect);

		// Get final clip rectangle
		RECT clipRect;
		clipRect = intersectRect;
		OffsetRect(&clipRect, -rect.left, -rect.top);
		DWORD flags = DDBLTFAST_NOCOLORKEY|DDBLTFAST_WAIT;

		// Copy data from the primary surface to the temporary surface
		HRESULT hr = ps->lpTempSurface->BltFast(clipRect.left, clipRect.top, ps->lpPrimarySurface, &intersectRect,
												flags);			
		if (hr != S_OK) {
			if (hr == DDERR_SURFACELOST) {	// In case of UAC, Logon screen, etc. 																
				hr = ps->lpPrimarySurface->Restore();	// Result will be a black screenshot
				if (hr == S_OK) {
					hr = ps->lpTempSurface->BltFast(clipRect.left, clipRect.top, ps->lpPrimarySurface, &intersectRect,
													flags);	
				}
			}
			if (hr != S_OK) {
				return hr;
			}
		}
		// Fill DDSURFACEDESC Structure
		DDSURFACEDESC	ddsd;
		ZeroMemory(&ddsd, sizeof(DDSURFACEDESC));
		ddsd.dwSize = sizeof(DDSURFACEDESC);
		hr = ps->lpTempSurface->Lock(NULL, &ddsd, /*DDLOCK_DONOTWAIT|*/DDLOCK_WAIT, NULL);	
		if (hr != S_OK) {
			return hr;
		}
		if (ps->srcBPP == BPP_32) {
			Surface32ToBitmap24((DWORD*)ddsd.lpSurface, (BYTE*)pBuffer, ddsd.lPitch, ddsd.dwWidth, ddsd.dwHeight, 
								clipRect);
		}
		else if (ps->srcBPP == BPP_24) {
			Surface24ToBitmap24((BYTE*)ddsd.lpSurface, (BYTE*)pBuffer, ddsd.lPitch, ddsd.dwWidth, ddsd.dwHeight,
								clipRect);
		}
		else if (ps->srcBPP == BPP_16) {
			Surface16ToBitmap16((WORD*)ddsd.lpSurface, (WORD*)pBuffer, ddsd.lPitch, ddsd.dwWidth, ddsd.dwHeight,
								clipRect);
		}
		hr = ps->lpTempSurface->Unlock(NULL);
		if (hr != S_OK) {
			return hr;
		}
		return S_OK;
  }
}
HRESULT CreateDDSurfaces(SCREENSHOT *ps) {
	// Fill DDSURFACEDESC Structure
	DDSURFACEDESC ddsd;
	ZeroMemory(&ddsd, sizeof(DDSURFACEDESC));		
	ddsd.dwSize = sizeof(DDSURFACEDESC);
	ddsd.dwFlags = DDSD_CAPS;
	ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;		

	// Create Primary Surface
	HRESULT hr;
	LPDIRECTDRAW lpddraw = ps->lpddraw;
	hr = lpddraw->CreateSurface(&ddsd, &ps->lpPrimarySurface, NULL);	
	if (S_OK != hr) {
		return hr;
	}

	// Fill DDSURFACEDESC Structure
	ZeroMemory(&ddsd, sizeof(DDSURFACEDESC));		
	ddsd.dwSize = sizeof(DDSURFACEDESC);
	ddsd.dwFlags = DDSD_WIDTH|DDSD_HEIGHT|DDSD_CAPS;
	ddsd.dwWidth = ps->size.cx;
	ddsd.dwHeight = ps->size.cy;
	ddsd.ddsCaps.dwCaps = DDSCAPS_SYSTEMMEMORY;

	// Create Temporary Surface
	hr = lpddraw->CreateSurface(&ddsd, &ps->lpTempSurface, NULL);
	if (hr != S_OK) {
		ps->lpPrimarySurface->Release();
	}
	return hr;
}
HRESULT CreateDirectDraw(SCREENSHOT *ps) {
	// Create Direct Draw
	HRESULT hr = DirectDrawCreate(NULL, &ps->lpddraw, NULL);	
	if (S_OK != hr) {
		return hr;
	}

	// Set Co-operative Level
	hr = ps->lpddraw->SetCooperativeLevel(NULL, DDSCL_NORMAL);
	if (S_OK != hr) {
		ReleaseDirectDraw(ps);
		return hr;
	}
	return S_OK;
}
BOOL FormatDDError(LONG error, PWSTR pMessage) {
	int msgId = 0;
	switch (error) {
		case DDERR_DIRECTDRAWALREADYCREATED:
			msgId = IDS_DDERR_DIRECTDRAWALREADYCREATED;
			break;
		case DDERR_EXCEPTION:
			msgId = IDS_DDERR_EXCEPTION;
			break;
		case DDERR_EXCLUSIVEMODEALREADYSET:
			msgId = IDS_DDERR_EXCLUSIVEMODEALREADYSET;
			break;
		case DDERR_GENERIC:
			msgId = IDS_DDERR_GENERIC;
			break;
		case DDERR_HWNDALREADYSET:
			msgId = IDS_DDERR_HWNDALREADYSET;			
			break;
		case DDERR_HWNDSUBCLASSED:
			msgId = IDS_DDERR_HWNDSUBCLASSED;
			break;
		case DDERR_INCOMPATIBLEPRIMARY:
			msgId = IDS_DDERR_INCOMPATIBLEPRIMARY;			
			break;
		case DDERR_INVALIDCAPS:
			msgId = IDS_DDERR_INVALIDCAPS;
			break;
		case DDERR_INVALIDDIRECTDRAWGUID:
			msgId = IDS_DDERR_INVALIDDIRECTDRAWGUID;
			break;
		case DDERR_INVALIDOBJECT:
			msgId = IDS_DDERR_INVALIDOBJECT;
			break;
		case DDERR_INVALIDPARAMS:
			msgId = IDS_DDERR_INVALIDPARAMS;	
			break;
		case DDERR_INVALIDPIXELFORMAT:
			msgId = IDS_DDERR_INVALIDPIXELFORMAT;			
			break;
		case DDERR_INVALIDRECT:
			msgId = IDS_DDERR_INVALIDRECT;
			break;
		case DDERR_NOALPHAHW:
			msgId = IDS_DDERR_NOALPHAHW;
			break;
		case DDERR_NOBLTHW:
			msgId = IDS_DDERR_NOBLTHW;
			break;
		case DDERR_NOCOOPERATIVELEVELSET:
			msgId = IDS_DDERR_NOCOOPERATIVELEVELSET;
			break;
		case DDERR_NODIRECTDRAWHW:
			msgId = IDS_DDERR_NODIRECTDRAWHW;
			break;
		case DDERR_NOFLIPHW:
			msgId = IDS_DDERR_NOFLIPHW;
			break;
		case DDERR_NOOVERLAYHW:
			msgId = IDS_DDERR_NOOVERLAYHW;
			break;
		case DDERR_NOTLOCKED:
			msgId = IDS_DDERR_NOTLOCKED;
			break;
		case DDERR_OUTOFMEMORY:
			msgId = IDS_DDERR_OUTOFMEMORY;
			break;
		case DDERR_OUTOFVIDEOMEMORY:
			msgId = IDS_DDERR_OUTOFVIDEOMEMORY;
			break;
		case DDERR_PRIMARYSURFACEALREADYEXISTS:
			msgId = IDS_DDERR_PRIMARYSURFACEALREADYEXISTS;
			break;
		case DDERR_SURFACEBUSY:
			msgId = IDS_DDERR_SURFACEBUSY;
			break;
		case DDERR_SURFACELOST:
			msgId = IDS_DDERR_SURFACELOST;
			break;
		case DDERR_UNSUPPORTED:
			msgId = IDS_DDERR_UNSUPPORTED;
			break;
		case DDERR_WASSTILLDRAWING:
			msgId = IDS_DDERR_WASSTILLDRAWING;
			break;
		default:
			return FALSE; // Provider error code is not in range of expected DirectDraw error messages.
	}
	int result = LoadString(g_hModule, msgId, pMessage, 255);
	return result > 0;
}
BOOL FormatSSError(LONG error, PWSTR pMessage) {
	int msgId = 0;
	switch (error) {
		case E_INVALIDARG:
			msgId = IDS_SSERR_INVALIDARG;
			break;
		case E_NOINTERFACE:
			msgId = IDS_SSERR_NOTSUPPORTED;
			break;
		default:
			return FALSE;
	}
	int result = LoadString(g_hModule, msgId, pMessage, 255);
	return result > 0;
}
HRESULT ReleaseDDSurfaces(SCREENSHOT *ps) {
	// Release Temporary Surface
	if (ps->lpTempSurface) {
		ps->lpTempSurface->Release();
		ps->lpTempSurface = NULL;
	}

	// Release Primary Surface
	if (ps->lpPrimarySurface) {
		ps->lpPrimarySurface->Release();
		ps->lpPrimarySurface = NULL;
	}
	return S_OK;
}
HRESULT ReleaseDirectDraw(SCREENSHOT *ps) {
	// Release Direct Draw
	if (ps->lpddraw) {
		ps->lpddraw->Release();
		ps->lpddraw = NULL;
	}
	return S_OK;
}
HRESULT SetOutputBitsPerPixel(SCREENSHOT *ps, int bpp) {
	// Get Temporary Surface Description
	DDSURFACEDESC	ddsd;
	ZeroMemory(&ddsd, sizeof(DDSURFACEDESC));
	ddsd.dwSize = sizeof(DDSURFACEDESC);
	HRESULT hr = ps->lpTempSurface->GetSurfaceDesc(&ddsd);
	if (S_OK != hr) {
		return hr;
	}

	// Get source bpp based on the input bpp
	if (bpp == BPP_24 && BPP_32 == ddsd.ddpfPixelFormat.dwRGBBitCount) {
		ps->srcBPP = BPP_32;
		ps->dstBPP = BPP_24;
		return S_OK;
	}
	if (bpp == BPP_16 &&BPP_24 == ddsd.ddpfPixelFormat.dwRGBBitCount) {
		ps->srcBPP = BPP_24;
		ps->dstBPP = BPP_24;
		return S_OK;
	}
	if (bpp == BPP_16 &&BPP_16 == ddsd.ddpfPixelFormat.dwRGBBitCount) {
		ps->srcBPP = ps->dstBPP = BPP_16;
		return S_OK;
	}

	return E_NOINTERFACE; // NOT SUPPORTED
}
void Surface32ToBitmap24(DWORD *pSource, BYTE *pDestination, INT pitch, INT width, INT height, RECT rect) {
	const int bppSource = 4;
	const int bppDestination = 3;
	int destPadding = (width * bppDestination) % PITCH_FACTOR;	
	for (int i = height - 1; i >=0; i--) {
		DWORD *pdSource = (DWORD*)((BYTE*)pSource + i * pitch);
		for (int j = 0; j < width; j++) {
			if (i != 0 || j < width - 1) {
				if (IsPointInRect(rect, j, i)) {
					*((DWORD*)pDestination) = *pdSource;
				}
				else {
					*((DWORD*)pDestination) = OUTOFCLIPCLR;
				}
				pdSource++;
				pDestination += bppDestination;
			}
			else {
				if (IsPointInRect(rect, j, i)) {
					// Move last 3 bytes differently (To avoid stack/memory corruption.)
					*(WORD*)pDestination = *(WORD*)pdSource;
					pDestination += sizeof(WORD);
					*pDestination = *(BYTE*)(pdSource + sizeof(WORD));
					pDestination += sizeof(BYTE);
				}
				else {
					*(WORD*)pDestination = OUTOFCLIPCLR_R|OUTOFCLIPCLR_G;
					pDestination += sizeof(WORD);
					*pDestination = OUTOFCLIPCLR_B;
					pDestination += sizeof(BYTE);
				}
			}
		}
		pDestination += destPadding;		
	}
}
void Surface24ToBitmap24(BYTE *pSource, BYTE *pDestination, INT pitch, INT width, INT height, RECT rect) {
	const int bppSource = 3;
	const int bppDestination = 3;
	int destPadding = (width * bppDestination) % PITCH_FACTOR;	
	for (int i = height - 1; i >=0; i--) {
		BYTE *pbSource = pSource + i * pitch;
		for (int j = 0; j < width; j++) {
			if (IsPointInRect(rect, j, i)) {
				*((DWORD*)pDestination) = *(DWORD*)pbSource;
			}
			else {
				*((DWORD*)pDestination) = OUTOFCLIPCLR;
			}
			pbSource += bppSource;
			pDestination += bppDestination;
		}
		pDestination += destPadding;		
	}
}
void Surface16ToBitmap16(WORD *pSource, WORD *pDestination, INT pitch, INT width, INT height, RECT rect) {
	const int bppSource = 2;
	const int bppDestination = 2;
	int destPadding = (width * bppDestination) % PITCH_FACTOR;
	for (int i = height - 1; i >=0; i--) {
		WORD *pwSource = (WORD*)((BYTE*)pSource + i * pitch);
		for (int j = 0; j < width; j++) {
			// Convert RGB565 to RGB555 (Can it be optimized better?, or what if it is not RGB565?)
			if (IsPointInRect(rect, j, i)) {
				_asm {
					mov esi, pwSource;
					mov ax, WORD PTR [esi] 
					mov dl, al
					and dl, 00011111b
					shr ax, 5
					mov dh, al
					and dh, 00111111b
					shr dh, 1
					shr ax, 6
					shl ax, 5
					or al, dh
					shl ax, 5
					or al, dl
					mov edi, pDestination
					mov WORD PTR [edi], ax
				}
			}
			else {
				*pDestination = OUTOFCLIPCLR16;
			}
			pwSource++;
			pDestination++;
		}
		pDestination += destPadding;		
	}
}