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
namespace Atf.ScreenRecorder.Avi {
   using System;
   using System.Runtime.InteropServices;
   static class Avi32Interop {
      #region Constants
      public const uint ICERR_OK = 0;
      public const uint DRV_USER = 0x4000;
      public const uint ICM_USER = DRV_USER + 0;
      public const uint ICM_RESERVED = DRV_USER + 0x1000;
      public const uint ICM_GETSTATE = ICM_RESERVED + 0;
      public const uint ICM_COMPRESS_QUERY = ICM_USER + 6;
      public const uint ICM_CONFIGURE = ICM_RESERVED + 10;
      public const uint ICM_ABOUT = ICM_RESERVED + 11;
      public const uint ICM_GETDEFAULTQUALITY = ICM_RESERVED + 30;
      public const uint ICMF_CONFIGURE_QUERY = 1;
      public const uint ICMF_CHOOSE_KEYFRAME = 0x0001;
      public const uint ICMF_CHOOSE_DATARATE	= 0x0002;
      public const uint ICMF_CHOOSE_PREVIEW	= 0x0004;
      public const uint ICMF_CHOOSE_ALLCOMPRESSORS	= 0x0008;
      public const uint ICMODE_QUERY = 4;
      public const uint AVICOMPRESSF_VALID = 0x8;
      public const uint VIDCF_QUALITY = 0x1;
      public const uint OF_CREATE = 0x00001000;
      public static uint ICTYPE_VIDEO = mmioFOURCC('v', 'i', 'd', 'c');
      public static uint streamtypeVIDEO = mmioFOURCC('v', 'i', 'd', 's');
      #endregion

      #region Structs
      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      public struct AVICOMPRESSOPTIONS {
         public UInt32 fccType;
         public UInt32 fccHandler;
         public UInt32 dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
         public UInt32 dwQuality;
         public UInt32 dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
         public UInt32 dwFlags;
         public IntPtr lpFormat;
         public UInt32 cbFormat;
         public IntPtr lpParms;
         public UInt32 cbParms;
         public UInt32 dwInterleaveEvery;
      }

      [StructLayout(LayoutKind.Sequential, Pack = 1)]
      public struct AVISTREAMINFO {
         public UInt32 fccType;
         public Int32 fccHandler;
         public Int32 dwFlags;
         public Int32 dwCaps;
         public Int16 wPriority;
         public Int16 wLanguage;
         public Int32 dwScale;
         public Int32 dwRate;
         public Int32 dwStart;
         public Int32 dwLength;
         public Int32 dwInitialFrames;
         public Int32 dwSuggestedBufferSize;
         public Int32 dwQuality;
         public Int32 dwSampleSize;
         public RECT rcFrame;
         public Int32 dwEditCount;
         public Int32 dwFormatChangeCount;
         [MarshalAs(UnmanagedType.LPWStr)]
         public String szName;
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct BITMAPINFOHEADER {
         public uint biSize;
         public int biWidth;
         public int biHeight;
         public ushort biPlanes;
         public ushort biBitCount;
         public uint biCompression;
         public uint biSizeImage;
         public int biXPelsPerMeter;
         public int biYPelsPerMeter;
         public uint biClrUsed;
         public uint biClrImportant;

         public void Init() {
            biSize = (uint)Marshal.SizeOf(this);
         }
      }
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
      public struct ICINFO {
         [MarshalAs(UnmanagedType.U4)]
         public uint biSize;
         [MarshalAs(UnmanagedType.U4)]
         public uint fccType;
         [MarshalAs(UnmanagedType.U4)]
         public uint fccHandler;
         [MarshalAs(UnmanagedType.U4)]
         public uint dwFlags;
         [MarshalAs(UnmanagedType.U4)]
         public uint dwVersion;
         [MarshalAs(UnmanagedType.U4)]
         public uint dwVersionICM;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
         public string szName;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
         public string szDescription;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
         public string szDriver;
         public void Init() {
            biSize = (uint)Marshal.SizeOf(this);
         }
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct RECT {
         public int Left, Top, Right, Bottom;
         public RECT(int left, int top, int right, int bottom) {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
         }
      }
      #endregion

      #region Functions
      [DllImport("avifil32.dll", SetLastError = true)]
      public static extern int AVIFileCreateStream(IntPtr pfile, out IntPtr ppavi, ref AVISTREAMINFO psi);
     
      [DllImport("avifil32.dll")]
      public static extern int AVIStreamRelease(IntPtr pavi);

      [DllImport("avifil32.dll", CharSet = CharSet.Auto)]
      public static extern int AVIFileOpen(out IntPtr ppfile, string szFile, uint mode, IntPtr pclsidHandler);

      [DllImport("avifil32.dll", CharSet = CharSet.Auto)]
      public static extern uint AVIFileRelease(IntPtr pfile);

      [DllImport("avifil32.dll",  CharSet = CharSet.Auto)]
      public static extern int AVIMakeCompressedStream(out IntPtr ppsCompressed, IntPtr aviStream,
                                                       ref AVICOMPRESSOPTIONS options, int clsidHandler);

      [DllImport("avifil32.dll", CharSet = CharSet.Auto)]
      public static extern bool AVISaveOptions(IntPtr hWnd, uint uiFlags, int nStreams, IntPtr pAviStream,
                                               IntPtr plpOptions);

      [DllImport("avifil32.dll",  CharSet = CharSet.Auto)]
      public static extern int AVIStreamSetFormat(IntPtr aviStream, Int32 lPos, ref BITMAPINFOHEADER lpFormat, 
                                                  Int32 cbFormat);
      [DllImport("avifil32.dll")]
      public static extern int AVIStreamWrite(IntPtr aviStream, Int32 lStart, Int32 lSamples, IntPtr lpBuffer,
                                              Int32 cbBuffer, Int32 dwFlags, Int32 plSampWritten, Int32 plBytesWritten);
      [DllImport("Msvfw32.dll", EntryPoint = "ICInfo", SetLastError = false)]
      public static extern int ICInfo(uint fccType, uint fccHandler, ref ICINFO lpicinfo);

      [DllImport("Msvfw32.dll", EntryPoint = "ICOpen", SetLastError = false)]
      public static extern IntPtr ICOpen(uint fccType, uint fccHandler, uint wMode);

      [DllImport("Msvfw32.dll", EntryPoint = "ICClose", SetLastError = false)]
      public static extern IntPtr ICClose(IntPtr hic);

      [DllImport("Msvfw32.dll", EntryPoint = "ICGetInfo", SetLastError = false)]
      public static extern int ICGetInfo(IntPtr hic, out ICINFO pciinfo, uint cb);

      [DllImport("Msvfw32.dll", EntryPoint = "ICSendMessage", SetLastError = false)]
      public static extern int ICSendMessage(IntPtr hic, uint Message, UIntPtr dw1, UIntPtr dw2);

      [DllImport("Msvfw32.dll", EntryPoint = "ICSendMessage", SetLastError = false)]
      public static extern int ICSendMessage(IntPtr hic, uint Message, ref BITMAPINFOHEADER dw1, UIntPtr dw2);

      [DllImport("Msvfw32.dll", EntryPoint = "ICSendMessage", SetLastError = false)]
      public static extern int ICSendMessage(IntPtr hic, uint Message, ref uint dw1, UIntPtr dw2);
      #endregion

      #region Macros
      public static uint mmioFOURCC(char ch0, char ch1, char ch2, char ch3) {
         return ((uint)(byte)(ch0) | ((uint)(byte)(ch1) << 8) | ((uint)(byte)(ch2) << 16) | ((uint)(byte)(ch3) << 24));
      }
      public static int ICCompressQuery(IntPtr hic, ref Avi32Interop.BITMAPINFOHEADER lpbiInput, IntPtr lpbiOutput) {
         return Avi32Interop.ICSendMessage(hic, Avi32Interop.ICM_COMPRESS_QUERY, ref lpbiInput,
                                            (UIntPtr)((ulong)lpbiOutput));
      }
      public static int ICConfigure(IntPtr hic, IntPtr hWnd) {
         return Avi32Interop.ICSendMessage(hic, Avi32Interop.ICM_CONFIGURE, (UIntPtr)(ulong)hWnd, UIntPtr.Zero);
      }
      public static int ICAbout(IntPtr hic, IntPtr hWnd) {
         return Avi32Interop.ICSendMessage(hic, Avi32Interop.ICM_ABOUT, (UIntPtr)(ulong)hWnd, UIntPtr.Zero);
      }
      public static int ICGetState(IntPtr hic, IntPtr pv, uint cb) {
         return Avi32Interop.ICSendMessage(hic, Avi32Interop.ICM_GETSTATE, (UIntPtr)(ulong)pv, (UIntPtr)((ulong)cb));
      }
      public static int ICGetDefaultQuality(IntPtr hic, ref uint pQuality) {
         return Avi32Interop.ICSendMessage(hic, Avi32Interop.ICM_GETDEFAULTQUALITY, ref pQuality,
                                            (UIntPtr)(sizeof(uint)));
      }
      #endregion

   }
}
