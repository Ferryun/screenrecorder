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
   using System.ComponentModel;
   class BoundsTracker {
      #region Fields
      private IntPtr hWnd;
      private Rectangle partialBounds;
      private TrackingType type;
      #endregion

      #region Constructors
      public BoundsTracker(Rectangle partialBounds) {
         this.partialBounds = PreviewBounds(partialBounds);
         this.type = TrackingType.Fixed;
      }
      public BoundsTracker(Rectangle partialBounds, bool trackMouseCursor) {
         this.partialBounds = PreviewBounds(partialBounds);
         this.type = trackMouseCursor ? TrackingType.MouseCursor : TrackingType.Fixed;
      }
      public BoundsTracker() {
         this.type = TrackingType.Full;
      }
      public BoundsTracker(IntPtr hWnd) {
         this.hWnd = hWnd;
         this.type = TrackingType.Window;
      }
      #endregion

      #region Properties
            public Rectangle Bounds {
         get {
            switch (this.type) {
               case TrackingType.Fixed:
                  return this.partialBounds;
               case TrackingType.Full:
                  return GetPrimaryScreenBounds();
               case TrackingType.MouseCursor:
                  return this.GetBoundsAroundCursor();
               case TrackingType.Window:
                  return this.GetWindowBounds();
               default:
                  throw new InvalidOperationException();
            }
         }
      }
      public IntPtr HWnd {
         get {
            return this.hWnd;
         }
      }
      public TrackingType Type {
         get {
            return this.type;
         }
      }
      #endregion

      #region Methods
      private Rectangle GetBoundsAroundCursor() {
         Point cursorPos = GetCursorPos();
         int xCursor = cursorPos.X;
         int yCursor = cursorPos.Y;
         Rectangle bounds = this.partialBounds;
         bounds.Offset(-bounds.X, -bounds.Y);
         bounds.Offset(xCursor - partialBounds.Width / 2, yCursor - partialBounds.Height / 2);
         return bounds;
      }
      private static Point GetCursorPos() {
         Point cursorPos;
         if (User32Interop.GetCursorPos(out cursorPos)) {
            return cursorPos;
         }
         return Point.Empty;
      }
      private static Rectangle GetPrimaryScreenBounds() {
         int width = User32Interop.GetSystemMetrics(User32Interop.SystemMetric.SM_CXSCREEN);
         int height = User32Interop.GetSystemMetrics(User32Interop.SystemMetric.SM_CYSCREEN);
         return new Rectangle(0, 0, width, height);
      }
      private Rectangle GetWindowBounds() {
         if (this.hWnd == IntPtr.Zero) {
            throw new TrackingException("Window handle is not specified.");
         }
         WindowInterop.WINDOWINFO wi = new WindowInterop.WINDOWINFO(null);
         WindowInterop.WINDOWPLACEMENT wp = WindowInterop.WINDOWPLACEMENT.Default;
         WindowInterop.RECT rect = new WindowInterop.RECT();
         bool windowAccessible = false;
         bool isNotVisible = false;
         if (WindowInterop.GetWindowInfo(this.hWnd, ref wi)) {
            if (WindowInterop.GetWindowPlacement(hWnd, out wp)) {
               if (wp.ShowCmd == WindowInterop.ShowWindowCommands.Maximize ||
                   wp.ShowCmd == WindowInterop.ShowWindowCommands.Normal ||
                   wp.ShowCmd == WindowInterop.ShowWindowCommands.Show) {
                  if (WindowInterop.GetWindowRect(hWnd, out rect)) {
                     windowAccessible = true;
                  }
               }
               else {
                  isNotVisible = true;
               }
            }
         }
         if (!windowAccessible) {
            string message = string.Format(
               "Specified window (0x{0:X8}) is not accessible. It is either closed, minimized or hidden.",
               hWnd.ToInt32());
            if (isNotVisible) {
               throw new TrackingException(message);
            }
            else {
               throw new TrackingException(message, new Win32Exception());
            }
         }
         Rectangle bounds;
         if (wp.ShowCmd == WindowInterop.ShowWindowCommands.Maximize) {
            rect.left += (int)wi.cxWindowBorders;
            rect.top += (int)wi.cyWindowBorders;
            rect.right -= (int)wi.cxWindowBorders;
            rect.bottom -= (int)wi.cyWindowBorders; ;
            bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
         }
         else {
            bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
         }
         return PreviewBounds(bounds);
      }
      private static Rectangle PreviewBounds(Rectangle bounds) {
         // bounds.Intersect(Screen.PrimaryScreen.Bounds);
         if (bounds.Width % 2 != 0) {
            if (bounds.Width > 1) {
               bounds.Width--;
            }
            else {
               bounds.Width++;
            }
         }
         if (bounds.Height % 2 != 0) {
            if (bounds.Height > 1) {
               bounds.Height--;
            }
            else {
               bounds.Height++;
            }
         }
         if (bounds.Width == 0 || bounds.Height == 0) {
            return Rectangle.Empty;
         }
         return bounds;
      }
      #endregion
   }
}
