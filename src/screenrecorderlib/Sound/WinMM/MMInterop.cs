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
   static class MMInterop {
      public const int CALLBACK_NULL = 0x1;
      public const int CALLBACK_FUNCTION = 0x00030000;
      public const int CALLBACK_WINDOW = 0x00010000;
      public const int CALLBACK_EVENT = 0x00050000;
      public const int MMSYSERR_NODRIVER = 6;
      public const ushort WAVE_FORMAT_PCM = 0x1;
      public enum WIMMessages : int {
         MM_WIM_OPEN = 0x03BE,
         MM_WIM_CLOSE = 0x03BF,
         MM_WIM_DATA = 0x03C0
      }
      public enum WaveFormat {
         WAVE_INVALIDFORMAT = 0x00000000       /* invalid format */,
         WAVE_FORMAT_1M08 = 0x00000001       /* 11.025 kHz, Mono,   8-bit  */,
         WAVE_FORMAT_1S08 = 0x00000002       /* 11.025 kHz, Stereo, 8-bit  */,
         WAVE_FORMAT_1M16 = 0x00000004       /* 11.025 kHz, Mono,   16-bit */,
         WAVE_FORMAT_1S16 = 0x00000008       /* 11.025 kHz, Stereo, 16-bit */,
         WAVE_FORMAT_2M08 = 0x00000010       /* 22.05  kHz, Mono,   8-bit  */,
         WAVE_FORMAT_2S08 = 0x00000020       /* 22.05  kHz, Stereo, 8-bit  */,
         WAVE_FORMAT_2M16 = 0x00000040       /* 22.05  kHz, Mono,   16-bit */,
         WAVE_FORMAT_2S16 = 0x00000080       /* 22.05  kHz, Stereo, 16-bit */,
         WAVE_FORMAT_4M08 = 0x00000100       /* 44.1   kHz, Mono,   8-bit  */,
         WAVE_FORMAT_4S08 = 0x00000200       /* 44.1   kHz, Stereo, 8-bit  */,
         WAVE_FORMAT_4M16 = 0x00000400       /* 44.1   kHz, Mono,   16-bit */,
         WAVE_FORMAT_4S16 = 0x00000800       /* 44.1   kHz, Stereo, 16-bit */,
         WAVE_FORMAT_44M08 = 0x00000100       /* 44.1   kHz, Mono,   8-bit  */,
         WAVE_FORMAT_44S08 = 0x00000200       /* 44.1   kHz, Stereo, 8-bit  */,
         WAVE_FORMAT_44M16 = 0x00000400       /* 44.1   kHz, Mono,   16-bit */,
         WAVE_FORMAT_44S16 = 0x00000800       /* 44.1   kHz, Stereo, 16-bit */,
         WAVE_FORMAT_48M08 = 0x00001000       /* 48     kHz, Mono,   8-bit  */,
         WAVE_FORMAT_48S08 = 0x00002000       /* 48     kHz, Stereo, 8-bit  */,
         WAVE_FORMAT_48M16 = 0x00004000       /* 48     kHz, Mono,   16-bit */,
         WAVE_FORMAT_48S16 = 0x00008000       /* 48     kHz, Stereo, 16-bit */,
         WAVE_FORMAT_96M08 = 0x00010000       /* 96     kHz, Mono,   8-bit  */,
         WAVE_FORMAT_96S08 = 0x00020000       /* 96     kHz, Stereo, 8-bit  */,
         WAVE_FORMAT_96M16 = 0x00040000       /* 96     kHz, Mono,   16-bit */,
         WAVE_FORMAT_96S16 = 0x00080000       /* 96     kHz, Stereo, 16-bit */,
      }
      [Flags]
      public enum WaveHdrFlags : uint {
         WHDR_DONE = 1,
         WHDR_PREPARED = 2,
         WHDR_BEGINLOOP = 4,
         WHDR_ENDLOOP = 8,
         WHDR_INQUEUE = 16
      }
      [StructLayout(LayoutKind.Sequential, Size = 18)]
      public struct WAVEFORMATEX {
         public ushort wFormatTag;
         public ushort nChannels;
         public uint nSamplesPerSec;
         public uint nAvgBytesPerSec;
         public ushort nBlockAlign;
         public ushort wBitsPerSample;
         public ushort cbSize;
      }
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public struct WAVEHDR {
         public IntPtr lpData; // pointer to locked data buffer
         public uint dwBufferLength; // length of data buffer
         public uint dwBytesRecorded; // used for input only
         public IntPtr dwUser; // for client's use
         public WaveHdrFlags dwFlags; // assorted flags (see defines)
         public uint dwLoops; // loop control counter
         public IntPtr lpNext; // PWaveHdr, reserved for driver
         public IntPtr reserved; // reserved for driver
      }
      [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public struct WAVEINCAPS {
         public ushort wMid;
         public ushort wPid;
         public uint vDriverVersion;
         [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
         public string szPname;
         public uint dwFormats;
         public ushort wChannels;
         public ushort wReserved1;
      }

      public delegate void waveInProc(IntPtr hwi, WIMMessages uMsg, IntPtr dwInstance, IntPtr dwParam1, IntPtr dwParam2);

      [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
      public static extern int waveInGetDevCaps(uint uDeviceID, ref WAVEINCAPS pwic, uint cbwic);

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern uint waveInGetNumDevs();

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern int waveInAddBuffer(IntPtr hwi, IntPtr pwh, uint cbwh);

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern uint waveInClose(IntPtr hwi);

      [DllImport("winmm.dll")]
      public static extern int waveInOpen(ref IntPtr hWaveIn, uint deviceId, IntPtr pwfx, waveInProc dwCallBack, uint dwInstance, uint dwFlags);

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern int waveInPrepareHeader(IntPtr hwi, IntPtr pwh, uint cbwh);

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern int waveInReset(IntPtr hwi);

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern int waveInStart(IntPtr hwi);

      [DllImport("winmm.dll", EntryPoint = "waveInStop", SetLastError = true)]
      public static extern int waveInStop(IntPtr hwi);

      [DllImport("winmm.dll", SetLastError = true)]
      public static extern int waveInUnprepareHeader(IntPtr hwi, IntPtr pwh, uint cbwh);

      public class WaveFormatExMarshaler : ICustomMarshaler {
         #region Feilds
         private static WaveFormatExMarshaler marshaler = null;
         #endregion

         #region Methods
         public static ICustomMarshaler GetInstance(string cookie) {
            if (marshaler == null) {
               marshaler = new WaveFormatExMarshaler();
            }
            return marshaler;
         }
         #endregion

         #region ICustomMarshaler Members
         public void CleanUpManagedData(object ManagedObj) {
         }
         public void CleanUpNativeData(IntPtr pNativeData) {
            Marshal.FreeHGlobal(pNativeData);
         }
         public int GetNativeDataSize() {
            return Marshal.SizeOf(typeof(WAVEFORMATEX));
         }
         public IntPtr MarshalManagedToNative(object ManagedObj) {
            SoundFormat wfx = (SoundFormat)ManagedObj;
            return wfx.ToPtr();
         }
         public object MarshalNativeToManaged(IntPtr pNativeData) {
            return new SoundFormat(pNativeData);
         }
         #endregion
      }

   }
}
