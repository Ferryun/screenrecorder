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
   using System.Text;
   using System.Windows.Forms;

   public class WindowFinder : IDisposable {
      #region Fields
      private static Color frameColor = SystemColors.WindowText;
      // private static readonly int frameWidth = 3; // pixels

      private bool disposed;
      private IntPtr hWnd;
      private bool isFinding;
      private string text;
      #endregion

      #region Properties
      public IntPtr Handle {
         get {
            return hWnd;
         }
         set {
            hWnd = value;
         }
      }
      public bool IsFinding {
         get {
            return this.isFinding;
         }
      }
      public string Text {
         get {
            return text;
         }
      }
      #endregion

      #region Methods
      public void BeginFind() {
         this.EndFind();
         this.text = string.Empty;
         this.isFinding = true;
      }
      private static void DrawFrame(IntPtr hWnd) {
         IntPtr hDC = WindowInterop.GetWindowDC(hWnd);
         if (hDC == IntPtr.Zero) {
            return;
         }
         try {
            using (Graphics graphics = Graphics.FromHdc(hDC)) {
               Rectangle rect = GetFrameRect(hWnd);
               ControlPaint.DrawReversibleFrame(rect, frameColor, FrameStyle.Thick);
               rect.Inflate(-2, -2);
               ControlPaint.DrawReversibleFrame(rect, frameColor, FrameStyle.Thick);
            }
         }
         finally {
            WindowInterop.ReleaseDC(hWnd, hDC);
         }
      }
      public IntPtr Find() {
         if (!this.isFinding) {
            return IntPtr.Zero;
         }
         IntPtr hNewWnd;
         WindowInterop.POINT pt = new WindowInterop.POINT();
         if (false == WindowInterop.GetCursorPos(out pt)) {
            hNewWnd = IntPtr.Zero;
         }
         else {
            hNewWnd = WindowInterop.WindowFromPoint(pt);
         }
         // Check if it is the previous handle
         if (hNewWnd == this.hWnd) {
            return hNewWnd;
         }
         // Make sure the window does not belog to current process
         if (hNewWnd != IntPtr.Zero) {
            uint processId;
            WindowInterop.GetWindowThreadProcessId(hNewWnd, out processId);
            int currentProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
            if (processId == currentProcessId) {
               hNewWnd = IntPtr.Zero;
            }
         }
         // Clear previous frame
         if (this.hWnd != IntPtr.Zero) {
            DrawFrame(this.hWnd);
         }
         if (hNewWnd != IntPtr.Zero) {
            // Get window text
            this.text = GetText(hNewWnd);
            // Draw new frame
            DrawFrame(hNewWnd);
         }
         else {
            this.text = string.Empty;
         }
         // Return new handle
         this.hWnd = hNewWnd;
         return this.hWnd;
      }
      public IntPtr EndFind() {
         if (!this.isFinding) {
            return IntPtr.Zero;
         }
         this.isFinding = false;
         if (this.hWnd != IntPtr.Zero) {
            IntPtr result = this.hWnd;
            DrawFrame(this.hWnd);
            this.hWnd = IntPtr.Zero;
            return result;
         }
         return IntPtr.Zero;
      }
      private static Rectangle GetFrameRect(IntPtr hWnd) {
         Rectangle windowRect;
         WindowInterop.RECT rect;
         WindowInterop.POINT point = new WindowInterop.POINT(0, 0);
         WindowInterop.WINDOWPLACEMENT wp = WindowInterop.WINDOWPLACEMENT.Default;
         // Get window border information
         WindowInterop.WINDOWINFO wi = new WindowInterop.WINDOWINFO(null);
         WindowInterop.GetWindowInfo(hWnd, ref wi);
         // Get window placement information (maximized, minimized, etc)
         WindowInterop.GetWindowPlacement(hWnd, out wp);
         // Get window rectangle
         WindowInterop.GetWindowRect(hWnd, out rect);
         if (wp.ShowCmd == WindowInterop.ShowWindowCommands.Maximize) {
            rect.left += (int)wi.cxWindowBorders;
            rect.top += (int)wi.cyWindowBorders;
            rect.right -= (int)wi.cxWindowBorders;
            rect.bottom -= (int)wi.cyWindowBorders;
            windowRect = new Rectangle(rect.left, rect.top, rect.right, rect.bottom);
         }
         else {
            windowRect = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
         }
         return windowRect;
      }
      private static string GetText(IntPtr hWnd) {
         int length = (int)WindowInterop.SendMessage(hWnd, WindowInterop.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
         StringBuilder sb = new StringBuilder(length + 1);
         WindowInterop.SendMessage(hWnd, WindowInterop.WM_GETTEXT, (IntPtr)sb.Capacity, sb);
         return sb.ToString();
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
               this.EndFind();
            }
            // Note disposing has been done.
            disposed = true;
         }
      }
      #endregion
   }
}
