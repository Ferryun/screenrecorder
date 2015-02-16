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
#ifndef __SCREENSHOT_H
#define __SCREENSHOT_H
#include <ddraw.h>
#include "resource.h"
#include "dllmain.h"

#define BPP_16			16
#define BPP_24			24
#define BPP_32			32
#define PITCH_FACTOR	4

struct SCREENSHOT {
	LPDIRECTDRAW			lpddraw;		
	LPDIRECTDRAWSURFACE	lpPrimarySurface;
	LPDIRECTDRAWSURFACE	lpTempSurface;
	SIZE						size;
	int						srcBPP;
	int						dstBPP;
	long						pitch;
};

HRESULT CreateDDSurfaces(SCREENSHOT *ps);
HRESULT CreateDirectDraw(SCREENSHOT *ps);
BOOL DrawCursor(BYTE* ppBitmapBits, INT x, INT y, INT width, INT height, INT bpp);
BOOL FormatDDError(LONG error, PWSTR pMessage);
BOOL FormatSSError(LONG error, PWSTR pMessage);
LONG GetOutputBitsPerPixel(SCREENSHOT *ps);
HRESULT ReleaseDirectDraw(SCREENSHOT *ps);
HRESULT ReleaseDDSurfaces(SCREENSHOT *ps);
void Surface32ToBitmap24(DWORD *pSource, BYTE *pDest, INT pitch, INT width, INT height);
void Surface24ToBitmap24(BYTE *pSource, BYTE *pDest, INT pitch, INT width, INT height);
void Surface16ToBitmap16(WORD *pSource, WORD *pDest, INT pitch, INT width, INT height);
#endif