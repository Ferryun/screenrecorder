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
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Drawing;
   using System.Drawing.Drawing2D;
   using System.Drawing.Imaging;
   using System.Runtime.InteropServices;

   public class DisplayProvider : IDisposable {
      #region Fields
      private const int nAlignBytes = 4;
      private const int bpp16 = 16;
      private const int bpp24 = 24;
      private const int bpp32 = 32;

      private Bitmap bitmap;
      private int bitmapBytes;
      private BitmapData bitmapData;
      private bool displayWatermark;
      private bool disposed;
      private IDisplayElement[] displayElements;
      private DisplayFormat format;
      private MouseCursor mouseCursor;
      private bool opened;
      private IntPtr pBitmapBits;
      private bool recordCursor;
      private Screenshot screenshot;
      private BoundsTracker tracker;
      private WaterMark waterMark;
      #endregion

      #region Constructors
      public DisplayProvider() {
         this.mouseCursor = new MouseCursor();
         this.screenshot = new Screenshot();
         this.tracker = new BoundsTracker();
         this.waterMark = new WaterMark();
      }
      #endregion

      #region Properties
      public int BitmapBytes {
         get {
            if (!this.opened) {
               throw new InvalidOperationException();
            }
            return this.bitmapBytes;
         }
      }
      public DisplayFormat Format {
         get {
            if (!this.opened) {
               throw new InvalidOperationException();
            }
            return this.format;
         }
      }
      public MouseSettings MouseSettings {
         get {
            return new MouseSettings() {
               HighlightCursor = this.mouseCursor.Highlight,
               HighlightCursorColor = this.mouseCursor.Color,
               HighlightCursorRadious = this.mouseCursor.Radious,
               RecordCursor = this.recordCursor,
            };
         }
         set {
            if (this.opened || value == null) {
               throw new InvalidOperationException();
            }
            this.recordCursor = value.RecordCursor;
            this.mouseCursor.Color = value.HighlightCursorColor;
            this.mouseCursor.Highlight = value.HighlightCursor;
            this.mouseCursor.Radious = value.HighlightCursorRadious;
         }
      }
      private bool IsLocked {
         get {
            return this.bitmapData != null;
         }
      }
      public TrackingSettings TrackingSettings {
         get {
            if (this.tracker != null) {
               return new TrackingSettings() {
                  Bounds = this.tracker.Bounds,
                  Type = this.tracker.Type,
               };
            }
            return new TrackingSettings() {
               Type = TrackingType.None,
            };
         }
         set {          
            switch (value.Type) {
               case TrackingType.Fixed:
                  this.tracker = new BoundsTracker(value.Bounds);
                  break;
               case TrackingType.Full:
                  this.tracker = new BoundsTracker();
                  break;
               case TrackingType.MouseCursor:
                  this.tracker = new BoundsTracker(value.Bounds, true);
                  break;
               case TrackingType.None:
                  this.tracker = null;
                  break;
               case TrackingType.Window:
                  this.tracker = new BoundsTracker(value.Hwnd);
                  break;
               default:
                  throw new ArgumentException("Invalid tracking settings");
            }
         }
      }
      public WatermarkSettings WatermarkSettings {
         get {
            return new WatermarkSettings() {
               Alignment = this.waterMark.Alignment,
               Color = this.waterMark.Color,
               Display = this.displayWatermark,
               Margin = this.waterMark.Margin,
               Outline = this.waterMark.Outline,
               OutlineColor = this.waterMark.OutlineColor,
               RightToLeft = this.waterMark.RightToLeft,
               Text = this.waterMark.Text,
            };
         }
         set {
            if (this.opened || value == null) {
               throw new InvalidOperationException();
            }
            this.waterMark.Alignment = value.Alignment;
            this.waterMark.Color = value.Color;
            this.waterMark.Font = value.Font;
            this.waterMark.Margin = value.Margin;
            this.waterMark.Outline = value.Outline;
            this.waterMark.OutlineColor = value.OutlineColor;
            this.waterMark.RightToLeft = value.RightToLeft;
            this.waterMark.Text = value.Text;
            this.displayWatermark = value.Display;
         }
      }
      #endregion

      #region Methods
      public void Close() {
         if (this.IsLocked) {
            this.Unlock();
         }
         if (this.screenshot != null) {
            this.screenshot.Close();
         }
         if (this.bitmap != null) {
            this.bitmap.Dispose();
            this.bitmap = null;
         }
         if (this.displayElements != null) {
            foreach (IDisplayElement displayElement in this.displayElements) {
               displayElement.Close();
            }
         }
         if (this.pBitmapBits != IntPtr.Zero) {
            Marshal.FreeHGlobal(this.pBitmapBits);
            this.pBitmapBits = IntPtr.Zero;
         }
         this.displayElements = null;
         this.opened = false;
      }
      private static int GetPrimaryScreenBitDepth() {
         int bitDepth;
         IntPtr hdc = User32Interop.GetDC(IntPtr.Zero);
         try {
            bitDepth = Gdi32Interop.GetDeviceCaps(hdc, Gdi32Interop.DeviceCap.BITSPIXEL);
            bitDepth *= Gdi32Interop.GetDeviceCaps(hdc, Gdi32Interop.DeviceCap.PLANES);
         }
         finally {
            User32Interop.ReleaseDC(IntPtr.Zero, hdc);
         }
         return bitDepth;
      }
      public IntPtr Lock() {
         if (!this.opened || this.IsLocked) {
            throw new InvalidOperationException();
         }
         Rectangle rectangle = new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height);
         this.bitmapData = this.bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, this.bitmap.PixelFormat);
         return this.bitmapData.Scan0;
      }
      public void Open() {
         if (opened) {
            throw new InvalidOperationException();
         }
         if (tracker == null) {
            throw new InvalidOperationException("Tracking is not specified.");
         }
         // Get pixel format of primary screen
         PixelFormat pixelFormat;
         int bpp = GetPrimaryScreenBitDepth();
         switch (bpp) {
            case bpp16:
               pixelFormat = PixelFormat.Format16bppRgb555;
               break;
            case bpp24:
            case bpp32:
               pixelFormat = PixelFormat.Format24bppRgb;
               break;
            default:
               throw new NotSupportedException("PixelFormat is not supported.");
         }

         // Populate display element list
         List<IDisplayElement> displayElementList = new List<IDisplayElement>();
         displayElementList.Add(this.screenshot);
         if (displayWatermark) {
            displayElementList.Add(this.waterMark);
         }
         if (this.recordCursor) {
            displayElementList.Add(this.mouseCursor);
         }         

         // Calc bitmap properties
         Rectangle bounds = this.tracker.Bounds;
         Size size = bounds.Size;
         int bytesPerPixel = Bitmap.GetPixelFormatSize(pixelFormat) / 8;
         int stride = (size.Width * bytesPerPixel);
         int nUnalignedBytes = stride % nAlignBytes;
         if (nUnalignedBytes != 0) {
            stride = stride + nAlignBytes - nUnalignedBytes;
         }
         int nBitmapBytes = stride * size.Height;
         int nExtraBytes = 4;  // NOTE: added + 4 for acc. violation problem with encoding 24bit in XVID codec (?!)

         // Allocate memory for bitmap bits
         this.pBitmapBits = Marshal.AllocHGlobal(nBitmapBytes + nExtraBytes);
         try {            
            // Create bitmap
            this.bitmap = new Bitmap(size.Width, size.Height, stride, pixelFormat, this.pBitmapBits);
            this.bitmapBytes = nBitmapBytes;

            // Open displat elements
            foreach (IDisplayElement displayElement in displayElementList) {
               displayElement.Open(this.bitmap);
            }

            // Set fields
            this.displayElements = displayElementList.ToArray();
            this.format = new DisplayFormat(size, pixelFormat);
            this.opened = true;
         }
         finally {
            if (!this.opened) {
               this.Close();
            }
         }
      }
      public void Render() {
         if (!opened) {
            throw new InvalidOperationException();
         }
         Graphics graphics = null;
         try {
            // Create graphics object
            graphics = Graphics.FromImage(this.bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Bitmap is upside-down, so transform graphics...
            graphics.TranslateTransform(0, this.bitmap.Size.Height - 1);
            graphics.ScaleTransform(1, -1);

            // Notify display elements that a render is going to happen
            foreach (IDisplayElement displayElement in this.displayElements) {
               displayElement.Prepare(graphics);
            }            

            // Render display elements
            RenderingContext context = new RenderingContext(this.bitmap, graphics, this.tracker.Bounds);
            foreach (IDisplayElement displayElement in this.displayElements) {
               displayElement.Render(context);
            }
         }
         finally {
            // Notify display elements that render is done
            foreach (IDisplayElement displayElement in this.displayElements) {
               displayElement.Unprepare();
            }
            if (graphics != null) {
               graphics.Dispose();
            }
         }
      }
      public void Unlock() {
         if (!this.IsLocked) {
            throw new InvalidOperationException();
         }
         this.bitmap.UnlockBits(this.bitmapData);
         this.bitmapData = null;
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
