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
   using System.ComponentModel;
   using System.Runtime.InteropServices;
   [TypeConverter(typeof(SoundFormatConverter))]
   public class SoundFormat {
      #region Fields
      private static readonly int fixedSize = 18;
      private static readonly string mono = "Mono";
      private static readonly string stereo = "Stereo";

      private int avgBytesPerSecond;
      private int blockAlign;
      private int bitsPerSample;
      private ushort cbSize;
      private int channels;
      private byte[] extra;
      private int samplesPerSecond;
      private SoundFormatTag tag;
      #endregion

      #region Constructors
      public SoundFormat(SoundFormatTag tag, int bitsPerSample, int channels, int samplesPerSecond) {
         this.avgBytesPerSecond = (bitsPerSample / 8) * samplesPerSecond * channels;
         this.bitsPerSample = bitsPerSample;
         this.blockAlign = (bitsPerSample / 8) * channels;
         this.channels = channels;
         this.samplesPerSecond = samplesPerSecond;
         this.tag = tag;
      }
      public SoundFormat(SoundFormatTag tag, int bitsPerSample, int blockAlign, int channels, int samplesPerSecond,
                         int avgBytesPerSecond) {
         this.avgBytesPerSecond = avgBytesPerSecond;
         this.bitsPerSample = bitsPerSample;
         this.blockAlign = blockAlign;
         this.channels = channels;
         this.samplesPerSecond = samplesPerSecond;
         this.tag = tag;
      }
      public SoundFormat(IntPtr ptr) {
         MMInterop.WAVEFORMATEX wfx =
           (MMInterop.WAVEFORMATEX)Marshal.PtrToStructure(ptr, typeof(MMInterop.WAVEFORMATEX));
         this.avgBytesPerSecond = (int)wfx.nAvgBytesPerSec;
         this.blockAlign = wfx.nBlockAlign;
         this.channels = wfx.nChannels;
         this.samplesPerSecond = (int)wfx.nSamplesPerSec;
         this.bitsPerSample = wfx.wBitsPerSample;
         this.tag = (SoundFormatTag)wfx.wFormatTag;
         this.cbSize = wfx.cbSize;
         int extraBytes = wfx.cbSize;
         if (extraBytes > 0) {
            this.extra = new byte[extraBytes];
            Marshal.Copy(new IntPtr(ptr.ToInt64() + fixedSize), this.extra, 0, extraBytes);
         }
      }
      public SoundFormat(IntPtr ptr, int totalBytes) {
         MMInterop.WAVEFORMATEX wfx =
           (MMInterop.WAVEFORMATEX)Marshal.PtrToStructure(ptr, typeof(MMInterop.WAVEFORMATEX));
         this.avgBytesPerSecond = (int)wfx.nAvgBytesPerSec;
         this.blockAlign = wfx.nBlockAlign;
         this.channels = wfx.nChannels;
         this.samplesPerSecond = (int)wfx.nSamplesPerSec;
         this.bitsPerSample = wfx.wBitsPerSample;
         this.tag = (SoundFormatTag)wfx.wFormatTag;
         this.cbSize = wfx.cbSize;
         int extraBytes = Math.Max(wfx.cbSize, totalBytes - fixedSize);
         if (extraBytes > 0) {
            this.extra = new byte[extraBytes];
            Marshal.Copy(new IntPtr(ptr.ToInt64() + fixedSize), this.extra, 0, extraBytes);
         }
      }
      #endregion

      #region Properties
      public int AverageBytesPerSecond {
         get {
            return this.avgBytesPerSecond;
         }
      }
      public int BlockAlign {
         get {
            return this.blockAlign;
         }
      }
      public int BitsPerSample {
         get {
            return this.bitsPerSample;
         }
      }
      public ushort CbSize {
         get {
            return this.cbSize;
         }
      }
      public int Channels {
         get {
            return this.channels;
         }
      }
      public byte[] Extra {
         get {
            return this.extra;
         }
      }
      public static int FixedSize {
         get {
            return fixedSize;
         }
      }
      public int SamplesPerSecond {
         get {
            return this.samplesPerSecond;
         }
      }
      public SoundFormatTag Tag {
         get {
            return this.tag;
         }
      }
      public int ToalSize {
         get {
            return fixedSize + this.cbSize;
         }
      }
      #endregion

      #region Methods
      public SoundFormat Clone() {
         SoundFormat copy = (SoundFormat)this.MemberwiseClone();
         if (this.extra != null) {
            copy.extra = (byte[])this.extra.Clone();
         }
         return copy;
      }
      public override bool Equals(object obj) {
         SoundFormat soundFormat = obj as SoundFormat;
         if (soundFormat == null) {
            return false;
         } 
         return soundFormat.avgBytesPerSecond.Equals(this.avgBytesPerSecond) &&
                soundFormat.bitsPerSample.Equals(this.bitsPerSample) && 
                soundFormat.blockAlign.Equals(this.blockAlign) &&
                soundFormat.channels.Equals(this.channels) &&
                soundFormat.samplesPerSecond.Equals(this.samplesPerSecond); 
      }
      public override int GetHashCode() {
         return this.avgBytesPerSecond.GetHashCode() ^
                this.bitsPerSample.GetHashCode() ^
                this.blockAlign.GetHashCode() ^
                this.samplesPerSecond.GetHashCode(); 
      }
      public IntPtr ToPtr() {
         int extraBytes = this.extra != null ? this.extra.Length : 0;
         int totalBytes = fixedSize + extraBytes;
         IntPtr pwfx = Marshal.AllocHGlobal(totalBytes);
         KernelInterop.ZeroMemory(pwfx, new IntPtr(totalBytes));
         MMInterop.WAVEFORMATEX wfx = new MMInterop.WAVEFORMATEX();
         wfx.cbSize = this.cbSize;
         wfx.nAvgBytesPerSec = (uint)this.avgBytesPerSecond;
         wfx.nBlockAlign = (ushort)this.blockAlign;
         wfx.nChannels = (ushort)this.channels;
         wfx.nSamplesPerSec = (uint)this.samplesPerSecond;
         wfx.wBitsPerSample = (ushort)this.bitsPerSample;
         wfx.wFormatTag = (ushort)this.tag;
         Marshal.StructureToPtr(wfx, pwfx, false);
         if (extraBytes > 0) {
            Marshal.Copy(this.extra, 0, new IntPtr(pwfx.ToInt64() + fixedSize), extraBytes);
         }
         return pwfx;
      }
      public IntPtr ToPtr(int totalBytes) {
         IntPtr pwfx = Marshal.AllocHGlobal(totalBytes);
         KernelInterop.ZeroMemory(pwfx, new IntPtr(totalBytes));
         MMInterop.WAVEFORMATEX wfx = new MMInterop.WAVEFORMATEX();
         wfx.cbSize = this.cbSize;
         wfx.nAvgBytesPerSec = (uint)this.avgBytesPerSecond;
         wfx.nBlockAlign = (ushort)this.blockAlign;
         wfx.nChannels = (ushort)this.channels;
         wfx.nSamplesPerSec = (uint)this.samplesPerSecond;
         wfx.wBitsPerSample = (ushort)this.bitsPerSample;
         wfx.wFormatTag = (ushort)this.tag;
         Marshal.StructureToPtr(wfx, pwfx, false);
         return pwfx;
      }
      public override string ToString() {
         return string.Format("{0:} kbps, {1} kHz, {2} bit, {3}", (this.AverageBytesPerSecond * 8) / 1000,
                              (float)this.SamplesPerSecond / 1000, this.bitsPerSample,
                               this.Channels == 1 ? mono : stereo);

      }
      #endregion
   }  
}
