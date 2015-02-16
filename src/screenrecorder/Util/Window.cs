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
namespace Atf.ScreenRecorder.Util {
   using System;
   using System.Windows.Forms;
   class Window : NativeWindow, IDisposable {
      #region Fields
      private bool disposed;
      #endregion

      #region Constructors
      public Window() {
         // Create the handle for the window.
         this.CreateHandle(new CreateParams());
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
               this.DestroyHandle();
            }
            // Note disposing has been done.
            disposed = true;
         }
      }
      #endregion
   }
    
}
