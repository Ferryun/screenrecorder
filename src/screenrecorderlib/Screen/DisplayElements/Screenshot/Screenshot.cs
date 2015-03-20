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
   using System.Drawing.Imaging;
   using System.Runtime.InteropServices;
   using System.Text;
   internal class Screenshot : IDisplayElement, IDisposable {
      #region Fields
      private static readonly int errorMessagLength = 256;
      private bool disposed;
      private IntPtr pScreenshot;
      #endregion

      #region Properties
      private bool IsOpened {
         get {
            return this.pScreenshot != IntPtr.Zero;
         }
      }
      #endregion


      #region Methods
      private static string GetErrorMessage(int errorCode) {
         StringBuilder sb = new StringBuilder(errorMessagLength);
         ScreenshotInterop.SSFormatError(errorCode, sb);
         return sb.ToString();
      }   
      #endregion
      
      #region IDisplayElement Members
      public void Close() {
         if (this.IsOpened) {
            int hr = ScreenshotInterop.SSClose(this.pScreenshot);
            this.pScreenshot = IntPtr.Zero;
         }
      }
      public void Open(Bitmap bitmap) {
         this.Close();
         Size size = bitmap.Size;
         int bpp = Bitmap.GetPixelFormatSize(bitmap.PixelFormat);
         int hr = ScreenshotInterop.SSOpen(size.Width, size.Height, bpp, ref this.pScreenshot);
         if (hr != 0) {
            throw new ScreenException(hr,
               string.Format("SSOpen failed: {0} (error code = 0x{1:X8}).", GetErrorMessage(hr), hr));
         }
      }
      public void Prepare(Graphics graphics) {         
      }
      public void Render(RenderingContext context) {
         if (!this.IsOpened) {
            throw new InvalidOperationException();
         }
         Bitmap bitmap = context.Bitmap;
         Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
         Rectangle trackBounds = context.Bounds;
         BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, bitmap.PixelFormat);
         try {
            int hr = ScreenshotInterop.SSTake(this.pScreenshot, trackBounds.X, trackBounds.Y, bitmapData.Scan0);
            if (hr != 0) {
               throw new ScreenException(hr, string.Format("Screenshot.Take failed: {0} (error code = 0x{1:X8}).",
                                                               GetErrorMessage(hr), hr));
            }
         }
         finally {
            bitmap.UnlockBits(bitmapData);
         }
      }
      public void RenderSample(Graphics g, Size size) {
         throw new NotSupportedException();
      }
      public void Unprepare() {        
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
