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
namespace Atf.ScreenRecorder.Screen {
   using System;
   using System.Drawing;
   using System.Runtime.InteropServices;
   using System.Text;
   internal class Screenshot : IDisposable {
      #region
      private static readonly int errorMessagLength = 256;
      private int bitsPerPixel;
      private int bufferLength = -1;
      private bool disposed;
      private IntPtr pScreenShot;
      private IntPtr pBuffer;
      #endregion

      #region Methods
      public void Close() {
         if (pBuffer != IntPtr.Zero) {
            Marshal.FreeHGlobal(this.pBuffer);
            this.pBuffer = IntPtr.Zero;
         }
         if (this.pScreenShot != IntPtr.Zero) {
            int hr = ScreenshotInterop.SSClose(this.pScreenShot);
            // Throw ?
            this.pScreenShot = IntPtr.Zero;
         }
         this.bufferLength = -1;
      }
      private static string GetErrorMessage(int errorCode) {
         StringBuilder sb = new StringBuilder(errorMessagLength);
         ScreenshotInterop.SSFormatError(errorCode, sb);
         return sb.ToString();
      }
      public void Open(int width, int height) {
         this.Close();
         int hr = ScreenshotInterop.SSOpen(width, height, ref this.pScreenShot);
         if (hr != 0) {
            throw new ScreenshotException(hr, string.Format("SSOpen failed: {0} (error code = 0x{1:X8}).",
                                                             GetErrorMessage(hr), hr));            
         }
         bool succeed = false;
         try {
            this.bitsPerPixel = ScreenshotInterop.SSGetBitsPerPixel(this.pScreenShot);
            this.bufferLength = ScreenshotInterop.SSGetBufferLength(this.pScreenShot);
            this.pBuffer = Marshal.AllocHGlobal(bufferLength);
            succeed = true;
         }
         finally {
            if (!succeed) {
               this.Close();
            }
         }
      }
      public void Take(Point location, bool cursor) {
         if (this.pScreenShot == IntPtr.Zero) {
            throw new InvalidOperationException();
         }
         int hr = ScreenshotInterop.SSTake(this.pScreenShot, location.X, location.Y, this.pBuffer, cursor);
         if (hr != 0) {
            throw new ScreenshotException(hr, string.Format("SSTake failed: {0} (error code = 0x{1:X8}).",
                                                             GetErrorMessage(hr), hr));
         }
      }
      #endregion

      #region Properties
      public int BitsPerPixel {
         get {
            if (this.pScreenShot == IntPtr.Zero) {
               throw new InvalidOperationException();
            }
            return this.bitsPerPixel;
         }
      }
      public IntPtr Buffer {
         get {
            if (this.pScreenShot == IntPtr.Zero) {
               throw new InvalidOperationException();
            }
            return this.pBuffer;
         }
      }
      public int BufferLength {
         get {
            if (this.pScreenShot == IntPtr.Zero) {
               throw new InvalidOperationException();
            }
            return this.bufferLength;
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
