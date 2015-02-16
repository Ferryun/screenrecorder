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
   using System.IO;
   using System.Runtime.InteropServices;
   class AviFile : IDisposable {
      #region Fields
      private static string xvidCodec = "xvid";
      private bool disposed;
      private bool opened;
      private IntPtr pAviFile;
      private IntPtr pAviStream;
      private IntPtr pAviCompressedStream;
      #endregion

      #region Methods      
      public void AddFrame(int index, IntPtr buffer, int length) {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         if (buffer == IntPtr.Zero) {
            throw new ArgumentException("buffer");
         }
         int hr = Avi32Interop.AVIStreamWrite(this.pAviCompressedStream, index, 1, buffer, length, 0, 0, 0);
         if (hr != 0) {
            throw new AviException(hr, string.Format("AVIStreamWrite failed, error code = 0x{0:X8}.", hr));
         }
      }
      public void Close() {
         if (this.pAviCompressedStream != IntPtr.Zero) {
            Avi32Interop.AVIStreamRelease(this.pAviCompressedStream);
            this.pAviCompressedStream = IntPtr.Zero;
         }
         if (this.pAviStream != IntPtr.Zero) {
            Avi32Interop.AVIStreamRelease(this.pAviStream);
            this.pAviStream = IntPtr.Zero;
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
      public void Open(string fileName, int width, int height, int colorDepth, int fps, AviCompressor compressor) {         
         if (string.IsNullOrEmpty(fileName)) {
            throw new ArgumentNullException("fileName");
         }
         if (compressor == null) {
            throw new ArgumentNullException("compressor");
         }
         if (this.opened) {
            throw new InvalidOperationException();
         }
         // Open AVI File
         int hr = Avi32Interop.AVIFileOpen(out this.pAviFile, fileName, Avi32Interop.OF_CREATE, IntPtr.Zero);
         if (hr != 0) {
            throw new AviException(hr, string.Format("AVIFileOpen failed, error code = 0x{0:X8}.", hr));
         }
         bool succeed = false;
         try {
            // Calculate Pitch
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
            hr = Avi32Interop.AVIFileCreateStream(this.pAviFile, out this.pAviStream, ref asf);
            if (hr != 0) {
               throw new AviException(hr, string.Format("AVIFileCreateStream failed, error code = 0x{0:X8}.", hr));
            }
            // Get compressor options
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
               hr = Avi32Interop.AVIMakeCompressedStream(out this.pAviCompressedStream, this.pAviStream,
                                                         ref compressorOptions, 0);
               if (hr != 0) {
                  throw new AviException(hr, string.Format("AVIMakeCompressedStream failed, error code = 0x{0:X8}.",
                                                           hr));
               }
               // Set stream format
               Avi32Interop.BITMAPINFOHEADER bih = new Avi32Interop.BITMAPINFOHEADER();
               bih.biBitCount = (ushort)colorDepth;
               bih.biCompression = 0; // BI_RGB
               bih.biHeight = height;
               bih.biPlanes = 1;
               bih.biSize = (uint)Marshal.SizeOf(bih);
               bih.biSizeImage = (uint)(pitch * height * (colorDepth / 8));
               bih.biWidth = width;
               hr = Avi32Interop.AVIStreamSetFormat(this.pAviCompressedStream, 0, ref bih,
                                                                      Marshal.SizeOf(bih));
               if (hr != 0) {
                  throw new AviException(hr, string.Format("AVIStreamSetFormat failed, error code = 0x{0:X8}.", hr));
               }
               this.opened = true;
            }
            finally {
               if (pParams != IntPtr.Zero) {
                  Marshal.FreeHGlobal(pParams);
               }
            }
            succeed = true;
         }
         finally {
            if (!succeed) {
               Close();
            }
         }
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         Dispose(true);
         // This object will be cleaned up by the Dispose method. 
         // Therefore, you should call GC.SupressFinalize to 
         // take this object off the finalization queue 
         // and prevent finalization code for this object 
         // from executing a second time.
         GC.SuppressFinalize(this);
      }
      protected virtual void Dispose(bool disposing) {
         // Check to see if Dispose has already been called. 
         if (!this.disposed) {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources. 
            if (disposing) {
               this.Close();
            }
            // Note disposing has been done.
            disposed = true;
         }
      }
      #endregion
   }
}
