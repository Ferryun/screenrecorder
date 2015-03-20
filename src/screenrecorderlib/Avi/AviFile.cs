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
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.Sound;
   using System;
   using System.Drawing;
   using System.Drawing.Imaging;
   using System.Runtime.InteropServices;
using Atf.ScreenRecorder.Sound.Acm;
   public class AviFile : IDisposable {
      #region Fields
      private static string xvidCodec = "xvid";
      private bool audio;
      private AcmEncoder audioEncoder;
      private SoundFormat audioFormat;
      private bool disposed;
      private bool opened;
      private IntPtr pAudioStream;
      private IntPtr pAviFile;
      private IntPtr pVideoStream;
      private IntPtr pAviCompressedStream;
      private bool video;
      #endregion

      #region Methods
      public int AddFrame(IntPtr buffer, int index, int nSamples, int length) {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         if (!this.video) {
            throw new InvalidOperationException();
         }
         if (buffer == IntPtr.Zero) {
            throw new ArgumentException("buffer");
         }
         int nSamplesWrittern = 0;
         IntPtr stream;
         if (this.pAviCompressedStream != IntPtr.Zero) {
            // If compressor is set, use compressed stream
            stream = this.pAviCompressedStream;
         }
         else {
            stream = this.pVideoStream;
         }
         int hr = Avi32Interop.AVIStreamWrite(stream, index, nSamples, buffer, length, 0, ref nSamplesWrittern, 0);
         if (hr != 0) {
            throw new AviException("AVIStreamWrite", hr);
         }
         return nSamplesWrittern;
      }
      public int AddSilence(int index, int nSamples) {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         if (!audio) {
            throw new InvalidOperationException();
         }
         SoundFormat sourceFormat = this.audioEncoder != null ? this.audioEncoder.InputFormat : this.audioFormat;
         // TODO: Use smaller buffers instead of one BIG buffer
         byte[] silenceData = new byte[nSamples * sourceFormat.BlockAlign];
         return AddSound(index, silenceData, false);
      }
      public int AddSound(int index, byte[] data, bool isEnd) {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         if (!audio) {
            throw new InvalidOperationException();
         }
         if (data == null) {
            throw new ArgumentNullException("data");
         }
         if (this.audioEncoder != null) {
            SoundFormat sourceFormat = this.audioEncoder.InputFormat;
            int bufferSize = (int)(this.audioEncoder.BufferLength * (sourceFormat.AverageBytesPerSecond / 1000.0));
            double sampleRatio = (double)this.audioFormat.SamplesPerSecond / sourceFormat.SamplesPerSecond;
            
            // Encoder buffer size may be less than input data
            // So we have to break input data into separate buffers
            // Get source format, encoder buffer size
            if (data.Length <= bufferSize) {
               // On convert is enough
               int sourceSamples = data.Length / sourceFormat.BlockAlign;
               int convertedSamples = (int)(sourceSamples * sampleRatio);
               byte[] convertedData = this.audioEncoder.Convert(data, 0, data.Length, isEnd);
               return AddSoundPrivate(index, convertedData, convertedSamples);
            }
            else {
               // Break input data into buffers
               int nBuffers = (int)Math.Ceiling((double)data.Length / bufferSize);
               int nSamplesWritten = 0;
               for (int i = 0; i < nBuffers; i++) {
                  int thisBufferSize = Math.Min(bufferSize, data.Length - i * bufferSize);
                  int sourceSamples = thisBufferSize / sourceFormat.BlockAlign;
                  int convertedSamples = (int)(sourceSamples * sampleRatio);
                  bool isLastConvert = isEnd && i == nBuffers - 1;
                  byte[] convertedData = this.audioEncoder.Convert(data, i * bufferSize, thisBufferSize, isLastConvert);
                  nSamplesWritten += this.AddSoundPrivate(index + nSamplesWritten, convertedData, convertedSamples);
               }
               return nSamplesWritten;
            }            
         }
         else {
            int nSamples = data.Length / this.audioFormat.BlockAlign;
            return AddSoundPrivate(index, data, nSamples);
         }         
      }
      private int AddSoundPrivate(int index, byte[] data, int nSamples) {
         int nSamplesWritten = 0;
         int hr = Avi32Interop.AVIStreamWrite(this.pAudioStream, index, nSamples, data, data.Length, 0,
                                              ref nSamplesWritten, 0);
         if (hr != 0) {
            throw new AviException("AVIStreamWrite", hr);
         }
         return nSamplesWritten;
      }
      public void Close() {
         if (this.pAviCompressedStream != IntPtr.Zero) {
            Avi32Interop.AVIStreamRelease(this.pAviCompressedStream);
            this.pAviCompressedStream = IntPtr.Zero;
         }
         if (this.pVideoStream != IntPtr.Zero) {
            Avi32Interop.AVIStreamRelease(this.pVideoStream);
            this.pVideoStream = IntPtr.Zero;
         }
         if (this.pAudioStream != IntPtr.Zero) {
            Avi32Interop.AVIStreamRelease(this.pAudioStream);
            this.pAudioStream = IntPtr.Zero;
         }
         if (this.audioEncoder != null) {
            this.audioEncoder.Close();
            this.audioEncoder = null;
         }
         if (this.pAviFile != IntPtr.Zero) {
            Avi32Interop.AVIFileRelease(this.pAviFile);
            this.pAviFile = IntPtr.Zero;
         }        
         this.opened = false;
      }
      private static void ModifyXvidParams(IntPtr par, int size) {
         // Some hack to disable status window
         int exceptedSize = 3540;
         int firstByteOffset = 3488;
         int secBytesOffset = 3536;
         if (size != exceptedSize) {
            return;
         }
         byte[] bytes = new byte[size];
         Marshal.Copy(par, bytes, 0, size);
         if (bytes[firstByteOffset] == 1) {
            bytes[firstByteOffset] = 0;
         }
         if (bytes[secBytesOffset] == 1) {
            bytes[secBytesOffset] = 0;
         }
         Marshal.Copy(bytes, 0, par, size);
      }
      public void Open(string fileName, DisplayFormat videoFormat, int fps, VideoCompressor compressor, 
                       SoundFormat audioFormat, AcmEncoder audioEncoder) {
         if (this.opened) {
            throw new InvalidOperationException();
         }
         if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentNullException("fileName");
         }
         this.video = videoFormat != null;
         this.audio = audioFormat != null;
         if (!this.audio && !this.video) {
            // There is nothing to do!
            throw new InvalidOperationException();
         }
         // Open AVI File
         int hr = Avi32Interop.AVIFileOpen(out this.pAviFile, fileName, Avi32Interop.OF_CREATE, IntPtr.Zero);
         if (hr != 0) {
            throw new AviException("AVIFileOpen", hr);
         }         
         try {
            if (this.video) {
              this.SetupVideo(videoFormat, compressor, fps);
            }
            if (this.audio) {
               this.SetupAudio(audioFormat, audioEncoder);
            }
            this.opened = true;
         }
         finally {
            if (!this.opened) {
               this.Close();
            }
         }         
      }
      private void SetupAudio(SoundFormat audioFormat, AcmEncoder audioEncoder) {
         IntPtr pwfx = audioFormat.ToPtr();
         try {
            Avi32Interop.AVISTREAMINFO asi = new Avi32Interop.AVISTREAMINFO();
            asi.fccType = Avi32Interop.streamtypeAUDIO;
            asi.dwScale = audioFormat.BlockAlign;
            asi.dwRate = audioFormat.AverageBytesPerSecond;
            asi.dwStart = 0;
            asi.dwLength = -1;
            asi.dwInitialFrames = 0;
            asi.dwSuggestedBufferSize = 0;
            asi.dwQuality = -1;
            asi.dwSampleSize = audioFormat.BlockAlign;
            int hr = Avi32Interop.AVIFileCreateStream(this.pAviFile, out this.pAudioStream, ref asi);
            if (hr != 0) {
               throw new AviException("AVIStreamSetFormat", hr);
            }
            hr = Avi32Interop.AVIStreamSetFormat(this.pAudioStream, 0, pwfx, audioFormat.ToalSize);
            if (hr != 0) {
               throw new AviException("AVIStreamSetFormat", hr);
            }
            if (audioEncoder != null) {
               audioEncoder.Open();
            }
            this.audioFormat = audioFormat;
            this.audioEncoder = audioEncoder;
         }
         finally {
            Marshal.FreeHGlobal(pwfx);
         }
      }
      private void SetupVideo(DisplayFormat videoFormat, VideoCompressor compressor, int fps) {
         int colorDepth = Bitmap.GetPixelFormatSize(videoFormat.PixelFormat);
         int width = videoFormat.Width;
         int height = videoFormat.Height;
         // Calculate pitch
         int bytesPerPixel = colorDepth / 8;
         int pitch = width * bytesPerPixel;
         int pitch_factor = 4;
         if (pitch % pitch_factor != 0) {
            pitch = pitch + pitch_factor - pitch % pitch_factor;
         }
         // Create AVI Stream
         Avi32Interop.AVISTREAMINFO asf = new Avi32Interop.AVISTREAMINFO();
         asf.dwRate = fps;
         asf.dwSuggestedBufferSize = pitch * height * bytesPerPixel;
         asf.dwScale = 1;
         asf.fccType = Avi32Interop.streamtypeVIDEO;
         asf.szName = null;
         asf.rcFrame = new Avi32Interop.RECT(0, 0, width, height);
         int hr = Avi32Interop.AVIFileCreateStream(this.pAviFile, out this.pVideoStream, ref asf);
         if (hr != 0) {
            throw new AviException("AVIFileCreateStream", hr);
         }
         // Set stream format
         Avi32Interop.BITMAPINFOHEADER bih = new Avi32Interop.BITMAPINFOHEADER();
         bih.biBitCount = (ushort)colorDepth;
         bih.biCompression = 0; // BI_RGB
         bih.biHeight = videoFormat.Height;
         bih.biPlanes = 1;
         bih.biSize = (uint)Marshal.SizeOf(bih);
         bih.biSizeImage = (uint)(pitch * height * (colorDepth / 8));
         bih.biWidth = videoFormat.Width;
         if (compressor != null && !compressor.Equals(VideoCompressor.None)) {
            // Setup compressor
            this.SetupVideoCompressor(compressor);
            hr = Avi32Interop.AVIStreamSetFormat(this.pAviCompressedStream, 0, ref bih, Marshal.SizeOf(bih));
         }
         else {
            hr = Avi32Interop.AVIStreamSetFormat(this.pVideoStream, 0, ref bih, Marshal.SizeOf(bih));
         }
         if (hr != 0) {
            throw new AviException("AVIStreamSetFormat", hr);
         }
      }
      private void SetupVideoCompressor(VideoCompressor compressor) {
         Avi32Interop.AVICOMPRESSOPTIONS compressorOptions = new Avi32Interop.AVICOMPRESSOPTIONS();
         uint fccHandler = compressor.FccHandler;
         compressorOptions.fccType = Avi32Interop.ICTYPE_VIDEO;
         compressorOptions.fccHandler = fccHandler;
         compressorOptions.dwQuality = (uint)compressor.Quality;
         // Open compressor
         IntPtr hic = Avi32Interop.ICOpen(Avi32Interop.ICTYPE_VIDEO, fccHandler, Avi32Interop.ICMODE_QUERY);
         if (hic == IntPtr.Zero) {
            int errorCode = Marshal.GetLastWin32Error();
            throw new AviException(errorCode, string.Format("ICOpen failed, error code = 0x{0:X8}.", errorCode));
         }
         // Get number of bytes required for params
         uint cbParams = (uint)Avi32Interop.ICGetState(hic, IntPtr.Zero, 0);
         IntPtr pParams = Marshal.AllocHGlobal((int)cbParams);
         try {
            // Get params
            int retval = Avi32Interop.ICGetState(hic, pParams, cbParams);
            compressorOptions.cbParms = cbParams;
            // If the Xvid Video Codec is selected, hack params to hide status window!
            if (string.Equals(compressor.FccHandlerString, xvidCodec)) {
               ModifyXvidParams(pParams, (int)cbParams);
            }
            compressorOptions.lpParms = pParams;
            compressorOptions.dwFlags = Avi32Interop.AVICOMPRESSF_VALID;
            // Close compressor
            Avi32Interop.ICClose(hic);
            // Make compressed stream               
            int hr = Avi32Interop.AVIMakeCompressedStream(out this.pAviCompressedStream, this.pVideoStream,
                                                          ref compressorOptions, 0);
            if (hr != 0) {
               throw new AviException(hr, string.Format("AVIMakeCompressedStream failed, error code = 0x{0:X8}.",
                                                        hr));
            }
         }
         finally {
            if (pParams != IntPtr.Zero) {
               Marshal.FreeHGlobal(pParams);
            }
         }
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
      protected virtual void Dispose(bool disposing) {
         if (!this.disposed) {
            if (disposing) {
               this.Close();
            }
            disposed = true;
         }
      }
      #endregion
   }
}
