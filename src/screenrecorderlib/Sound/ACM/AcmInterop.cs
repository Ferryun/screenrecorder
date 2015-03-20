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
namespace Atf.ScreenRecorder.Sound.Acm {
   using System;
   using System.Runtime.InteropServices;
   static class AcmInterop {
      public const int _DRVRESERVED = 10;
      public const int ACMFORMATDETAILS_FORMAT_CHARS = 128;
      public const int ACM_FORMATENUMF_WFORMATTAG = 0x00010000;
      public const int ACM_FORMATENUMF_NCHANNELS = 0x00020000;
      public const int ACM_FORMATENUMF_NSAMPLESPERSEC = 0x00040000;
      public const int ACM_FORMATENUMF_WBITSPERSAMPLE = 0x00080000;
      public const int ACM_FORMATENUMF_CONVERT = 0x00100000;
      public const int ACM_FORMATENUMF_SUGGEST = 0x00200000;
      public const int ACM_FORMATENUMF_HARDWARE = 0x00400000;
      public const int ACM_FORMATENUMF_INPUT = 0x00800000;
      public const int ACM_FORMATENUMF_OUTPUT = 0x01000000;
      public const int ACM_METRIC_MAX_SIZE_FORMAT = 50;
      public const int ACMERR_NOTPOSSIBLE = 512;
      public const int ACM_FORMATSUGGESTF_WFORMATTAG = 0x00010000;
      public const int ACM_FORMATSUGGESTF_NCHANNELS = 0x00020000;
      public const int ACM_FORMATSUGGESTF_NSAMPLESPERSEC = 0x00040000;
      public const int ACM_FORMATSUGGESTF_WBITSPERSAMPLE = 0x00080000;
      public const int ACM_STREAMCONVERTF_BLOCKALIGN = 0x00000004;
      public const int ACM_STREAMCONVERTF_START = 0x00000010;
      public const int ACM_STREAMCONVERTF_END = 0x00000020;
      public const int ACM_STREAMOPENF_QUERY =  0x00000001;
      public const int ACM_STREAMOPENF_ASYNC  =  0x00000002;
      public const int ACM_STREAMOPENF_NONREALTIME = 0x00000004;
      public const int ACM_STREAMSIZEF_SOURCE = 0x00000000;
      public const int ACM_STREAMSIZEF_DESTINATION = 0x00000001;
      public const int ACM_STREAMSIZEF_QUERYMASK =   0x0000000F;
      [StructLayout(LayoutKind.Sequential)]
      public struct ACMFORMATDETAILS {
         public int cbStruct;
         public int dwFormatIndex;
         public int dwFormatTag;
         public int fdwSupport;
         public IntPtr pwfx;
         public int cbwfx;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = ACMFORMATDETAILS_FORMAT_CHARS)]
         public string szFormat;
      }
      [StructLayout(LayoutKind.Sequential)]
      public struct ACMSTREAMHEADER {
         public int cbStruct; // sizeof(ACMSTREAMHEADER)
         public int fdwStatus; // ACMSTREAMHEADER_STATUSF_*
         public IntPtr dwUser; // user instance data for hdr
         public IntPtr pbSrc;
         public int cbSrcLength;
         public int cbSrcLengthUsed;
         public IntPtr dwSrcUser; // user instance data for src
         public IntPtr pbDst;
         public int cbDstLength;
         public int cbDstLengthUsed;
         public IntPtr dwDstUser; // user instance data for dst
         public int reserved0;
         public int reserved1;
         public int reserved2;
         public int reserved3;
         public int reserved4;
         public int reserved5;
         public int reserved6;
         public int reserved7;
         public int reserved8;
         public int reserved9;
      }

      public delegate bool acmDriverCallback(int hadid, UIntPtr dwInstance, int fdwSupport);
      public delegate bool acmFormatEnumCallback(IntPtr had, ref ACMFORMATDETAILS pafd, UIntPtr dwInstance, int fdwEnum);
      public delegate bool acmFormatTagEnumCallback(IntPtr hadid, ref ACMFORMATDETAILS pafd, UIntPtr dwInstance,  int fdwSupport);
      public delegate bool acmStreamConvertCallback(IntPtr has, uint uMsg, UIntPtr dwInstance, IntPtr lParam1, IntPtr Lparam2);

      [DllImport("Msacm32.dll")]
      public static extern int acmDriverClose(IntPtr had, int fdwClose);

      [DllImport("Msacm32.dll")]
      public static extern int acmDriverEnum(acmDriverCallback fnCallback, int dwInstance, int fdwEnum);

      [DllImport("Msacm32.dll")]
      public static extern int acmDriverOpen(out IntPtr phad, int hadid, int ddwOpen);

      [DllImport("Msacm32.dll")]
      public static extern int acmFormatEnum(IntPtr had, ref ACMFORMATDETAILS pafd, acmFormatEnumCallback fnCallback, IntPtr dwInstance, int fdwEnum);

      [DllImport("Msacm32.dll")]
      public static extern int acmFormatTagEnum (IntPtr had, ref ACMFORMATDETAILS paftd, acmFormatTagEnumCallback fnCallback, UIntPtr dwInstance, int fdwEnum);

      [DllImport("Msacm32.dll")]
      public static extern int acmFormatSuggest(IntPtr had, IntPtr pwfxSrc, IntPtr pwfxDst, int cbwfxDst, int fdwSuggest);

      [DllImport("Msacm32.dll")]
      public static extern int acmMetrics(IntPtr hao, uint uMetric, out int lpMetric);

      [DllImport("Msacm32.dll")]
      public static extern int acmStreamConvert(IntPtr has, ref ACMSTREAMHEADER pash, int pdwConvert);

      [DllImport("Msacm32.dll")]
      public static extern int acmStreamPrepareHeader(IntPtr has, ref ACMSTREAMHEADER pash, int fdwPrepare);

      [DllImport("Msacm32.dll")]
      public static extern int acmStreamOpen(out IntPtr phas, IntPtr had, IntPtr pwfxSrc, IntPtr pwfxDst, IntPtr pwfltr, IntPtr dwCallback, UIntPtr dwInstance,  int fdwOpen);

      [DllImport("Msacm32.dll")]
      public static extern int acmStreamSize(IntPtr has, int cbInput, out int pdwOutputBytes,int fdwSize);

      [DllImport("Msacm32.dll")]
      public static extern int acmStreamClose(IntPtr has, int fdwClose);

      [DllImport("Msacm32.dll")]
      public static extern int acmStreamUnprepareHeader(IntPtr has, ref ACMSTREAMHEADER pash, int fdwUnprepare);
   }
}
