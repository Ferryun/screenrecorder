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
namespace Atf.ScreenRecorder.Sound {
   using System;
   using System.Runtime.InteropServices;
   static class WSSoundInterop {
      public const int WS_MAXNAMELEN = 256;

      [Flags]
      public enum DeviceType  {
         Input = 0x00000001,
         Output = 0x00000002,
      };

      [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
      public struct DEVICE {
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WS_MAXNAMELEN)]
         public string szDescription;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WS_MAXNAMELEN)]
         public string szId;
         public uint nMaxDesc;
         public uint nMaxId;
         public uint nMaxName;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst=WS_MAXNAMELEN)]
         public string szName;
         public uint dwType;
      }

      public delegate bool WSEnumDevicesCallback(IntPtr lpws, ref DEVICE lpDevice);
      public delegate bool WSEnumDeviceFormatsCallback(IntPtr lpws, string deviceId, IntPtr pwfx);

      [DllImport("wasapisound.dll")]
      public static extern int WSClose(IntPtr lpws);

      [DllImport("wasapisound.dll")]
      public static extern int WSEnumDevices(IntPtr lpws, ref DEVICE lpDevice, WSEnumDevicesCallback callback);

      [DllImport("wasapisound.dll", CharSet=CharSet.Unicode)]
      public static extern int WSEnumDeviceFormats(IntPtr lpws, string lpDeviceId, IntPtr pwfx, int cbwfx, WSEnumDeviceFormatsCallback callback);

      [DllImport("wasapisound.dll")]
      public static extern int WSGetBufferLength(IntPtr lpws, out uint lpnBufferLength);

      [DllImport("wasapisound.dll", CharSet=CharSet.Unicode)]
      public static extern int WSGetDeviceId(IntPtr lpws, string lpDeviceId, uint imaxlen);

      [DllImport("wasapisound.dll")]
      public static extern int WSGetFormat(IntPtr lpws, IntPtr pwfx, uint cbwfx);

      [DllImport("wasapisound.dll")]
      public static extern int WSGetNumDevices(IntPtr lpws, ref uint pcDevices);

      [DllImport("wasapisound.dll")]
      public static extern int WSGetPacketLength(IntPtr intPtr, ref uint lpnPacketLength);

      [DllImport("wasapisound.dll")]
      public static extern int WSInit(ref IntPtr lpws);

      [DllImport("wasapisound.dll")]
      public static extern int WSOpen(IntPtr lppws);

      [DllImport("wasapisound.dll")]
      public static extern int WSRead(IntPtr lppws, byte[] buffer, uint offset, uint length, bool end, out uint bytesRead);

      [DllImport("wasapisound.dll", CharSet = CharSet.Unicode)]
      public static extern int WSSetDeviceId(IntPtr lpws, string lpDeviceId);

      [DllImport("wasapisound.dll")]
      public static extern int WSSetFormat(IntPtr lpws, IntPtr pwfx);

      [DllImport("wasapisound.dll")]
      public static extern int WSStart(IntPtr lppws);

      [DllImport("wasapisound.dll")]
      public static extern int WSStop(IntPtr lppws);

      [DllImport("wasapisound.dll")]
      public static extern int WSUninit(IntPtr lpws);
   }
}
