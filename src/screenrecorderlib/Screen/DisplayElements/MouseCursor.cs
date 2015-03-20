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
   public class MouseCursor : IDisplayElement {
      #region Fields
      private static readonly int highlightAlpha = 192;
      private Color color;
      private Point cursorPos;
      private bool highlight;
      private int radious;
      #endregion

      #region Properties
      public Color Color {
         get {
            return this.color;
         }
         set {
            this.color = value;
         }
      }
      public bool Highlight {
         get {
            return this.highlight;
         }
         set {
            this.highlight = value;
         }
      }
      public int Radious {
         get {
            return this.radious;
         }
         set {
            this.radious = value;
         }
      }
      #endregion

      #region Methods
      private static Color AddAlpha(Color color) {
         return Color.FromArgb(highlightAlpha, color.R, color.G, color.B);
      }
      private static void DrawCursor(IntPtr hCursor, Graphics graphics, Point location) {
         location.X += (int)graphics.Transform.OffsetX;
         location.Y += (int)graphics.Transform.OffsetY;
         
         User32Interop.ICONINFO iconInfo;
         if (!User32Interop.GetIconInfo(hCursor, out iconInfo)) {
            return;
         }
         if (iconInfo.hbmColor != IntPtr.Zero) {
            Gdi32Interop.DeleteObject(iconInfo.hbmColor);
         }
         if (iconInfo.hbmMask != IntPtr.Zero) {
            Gdi32Interop.DeleteObject(iconInfo.hbmMask);
         }

         IntPtr hdc = graphics.GetHdc();
         try {
            int x = location.X - iconInfo.xHotspot;
            int y = location.Y - iconInfo.xHotspot;
            Size size = GetCursorSize();
            int width = size.Width;
            int height = size.Height;

            Gdi32Interop.IntersectClipRect(hdc, x, y, x + width, y + height);
            User32Interop.DrawIconEx(hdc, x, y, hCursor, width, height, 0, IntPtr.Zero, Gdi32Interop.DI_NORMAL);
           
         }
         finally {
            graphics.ReleaseHdcInternal(hdc);
         } 
      }
      private static void DrawDefaultCursor(Graphics graphics, Point location) {
         IntPtr hCursor = User32Interop.LoadCursor(IntPtr.Zero, User32Interop.IDC_ARROW);
         try {
            DrawCursor(hCursor, graphics, location);
         }
         finally {
            if (hCursor != IntPtr.Zero) {
               User32Interop.DestroyCursor(hCursor);
            }
         }
      }

      private static Size GetCursorSize() {
         int width = User32Interop.GetSystemMetrics(User32Interop.SystemMetric.SM_CXCURSOR);
         int height = User32Interop.GetSystemMetrics(User32Interop.SystemMetric.SM_CYCURSOR);
         return new Size(width, height);
      }
      #endregion

      #region IDisplayElement Members
      public void Close() {
      }
      public void Open(Bitmap bitmap) {
      }
      public void Prepare(Graphics graphics) {
         if (!User32Interop.GetCursorPos(out this.cursorPos)) {
            this.cursorPos = Point.Empty;
         }
      }
      public void Render(RenderingContext context) { 
         Bitmap bitmap = context.Bitmap;
         Rectangle bounds = context.Bounds;
         if (this.highlight) {
            // Render highlight
            Point position = this.cursorPos;
            Point location = bounds.Location;
            position.Offset(-location.X, -location.Y);            
            Rectangle highlightBounds = new Rectangle(
               position.X - this.radious,
               position.Y - this.radious,
               this.radious * 2,
               this.radious * 2
            );
            Color transparentColor = Color.FromArgb(highlightAlpha, this.color.R, this.color.G, this.color.B);
            using (SolidBrush brush = new SolidBrush(transparentColor)) {
               context.Graphics.FillEllipse(brush, highlightBounds);
            }   
         }
         // Render cursor
         Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
         BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
         try {
            int bpp = Bitmap.GetPixelFormatSize(bitmap.PixelFormat);
            int hr = ScreenshotInterop.SSDrawCursor(bitmapData.Scan0, this.cursorPos.X, this.cursorPos.Y, bounds.X,
                                                    bounds.Y, bitmap.Width, bitmap.Height, bpp);
            // CHECK hr?
         }
         finally {
            bitmap.UnlockBits(bitmapData);
         }
      }
      public void RenderSample(Graphics g, Size size) {
         int height = size.Height;
         int width = size.Width;
         Color transparentColor = AddAlpha(this.color);
         using (SolidBrush brush = new SolidBrush(transparentColor)) {
            Rectangle highlightBounds = new Rectangle(
               (width - this.radious * 2) / 2,
               (height - this.radious * 2) / 2,
               this.radious * 2,
               this.radious * 2
            );
            g.FillEllipse(brush, highlightBounds);
         }
         DrawDefaultCursor(g, new Point( width / 2, height / 2));
       }
      public void Unprepare() {
      }
      #endregion
   }
}
