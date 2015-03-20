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
   using System.Drawing.Drawing2D;
   using System.Drawing.Imaging;
   public class WaterMark : IDisplayElement, IDisposable {
      #region Enums
      enum HorizontalAlignment {
         Left,
         Right,
         Center,
      }
      enum VerticalAlignment {
         Top,
         Bottom,
         Middle,
      }
      #endregion

      #region Fields
      private ContentAlignment alignment;
      private Bitmap bitmap;
      private RectangleF bounds;
      private Color color;
      private bool disposed;
      private Font font;
      private Margins margin;
      private bool outline;
      private Color outlineColor;
      private bool rightToLeft;
      private string text;
      #endregion

      #region Properties
      public ContentAlignment Alignment {
         get {
            return this.alignment;
         }
         set {
            this.alignment = value;
         }
      }
      public Color Color {
         get {
            return this.color;
         }
         set {
            this.color = value;
         }
      }
      public Font Font {
         get {
            return this.font;
         }
         set {
            if (value != null) {
               this.font = value;
            }
            else {
               this.font = SystemFonts.DefaultFont;
            }
         }
      }
      public Margins Margin {
         get {
            return this.margin;
         }
         set {
            this.margin = value;
         }
      }
      public bool Outline {
         get {
            return this.outline;
         }
         set {
            this.outline = value;
         }
      }
      public Color OutlineColor {
         get {
            return this.outlineColor;
         }
         set {
            this.outlineColor = value;
         }
      }
      public bool RightToLeft {
         get {
            return this.rightToLeft;
         }
         set {
            this.rightToLeft = value;
         }
      }
      public string Text {
         get {
            return this.text;
         }
         set {
            this.text = value;
         }
      }
      #endregion

      #region Methods
      private static RectangleF AddMargin(RectangleF bounds, Margins margin) {
         bounds.Width -= margin.Horizontal;
         bounds.X += margin.Left;
         bounds.Height -= margin.Vertical;
         bounds.Y += margin.Top;
         return bounds;
      }
      private static RectangleF RemoveMargin(RectangleF bounds, Margins margin) {
         bounds.Width += margin.Horizontal;
         bounds.X -= margin.Left;
         bounds.Height += margin.Vertical;
         bounds.Y -= margin.Top;
         return bounds;
      }
      private RectangleF DrawWatermark(Graphics graphics, RectangleF bounds, bool calcOnly) {
         StringFormat format = null;
         Brush brush = null;
         Pen outlinePen = null;
         GraphicsPath path = null;
         try {
            HorizontalAlignment halign;
            VerticalAlignment valign;
            GetAlignments(this.alignment, out halign, out valign);
            format = GetStringFormat(halign, valign, this.rightToLeft);
            RectangleF calculatedBounds = bounds;
            if (outline) {
               float outlineWidth = this.GetOutlineWidth();
               outlinePen = new Pen(this.outlineColor, outlineWidth);
               outlinePen.LineJoin = LineJoin.Round;

               path = new GraphicsPath();
               path.AddString(this.text, this.font.FontFamily, (int)this.font.Style, this.font.Height, bounds, format);
               calculatedBounds = path.GetBounds(new Matrix(), outlinePen);
               calculatedBounds.Inflate(2, 2);
            }
            else {
               SizeF textSize = graphics.MeasureString(this.text, this.font, bounds.Size, format);
               switch (halign) {
                  case HorizontalAlignment.Left:
                     break;
                  case HorizontalAlignment.Right:
                     calculatedBounds.X = bounds.Width - textSize.Width;
                     break;
                  case HorizontalAlignment.Center:
                     calculatedBounds.X = (bounds.Width - textSize.Width) / 2.0f;
                     break;
               }
               switch (valign) {
                  case VerticalAlignment.Top:
                     break;
                  case VerticalAlignment.Bottom:
                     calculatedBounds.Y = bounds.Height - textSize.Height;
                     break;
                  case VerticalAlignment.Middle:
                     calculatedBounds.Y = (bounds.Height - textSize.Height) / 2.0f;
                     break;
               }
               calculatedBounds.Size = textSize;
            }
            if (calcOnly) {
               return calculatedBounds;
            }
            brush = new SolidBrush(this.color);
            if (outline) {
               graphics.DrawPath(outlinePen, path);
               graphics.FillPath(brush, path);
            }
            else {
               graphics.DrawString(this.text, this.font, brush, calculatedBounds, format);
            }
            return calculatedBounds;
         }
         finally {
            if (format != null) {
               format.Dispose();
            }
            if (path != null) {
               path.Dispose();
            }
            if (brush != null) {
               brush.Dispose();
            }
            if (outlinePen != null) {
               outlinePen.Dispose();
            }
         }
      }
      private static void GetAlignments(ContentAlignment alignment, out HorizontalAlignment halign,
                                        out VerticalAlignment valign) {
         switch (alignment) {
            case ContentAlignment.BottomCenter:
               halign = HorizontalAlignment.Center;
               valign = VerticalAlignment.Bottom;
               break;
            case ContentAlignment.BottomLeft:
               halign = HorizontalAlignment.Left;
               valign = VerticalAlignment.Bottom;
               break;
            case ContentAlignment.BottomRight:
               halign = HorizontalAlignment.Right;
               valign = VerticalAlignment.Bottom;
               break;
            case ContentAlignment.MiddleCenter:
               halign = HorizontalAlignment.Center;
               valign = VerticalAlignment.Middle;
               break;
            case ContentAlignment.MiddleLeft:
               halign = HorizontalAlignment.Left;
               valign = VerticalAlignment.Middle;
               break;
            case ContentAlignment.MiddleRight:
               halign = HorizontalAlignment.Right;
               valign = VerticalAlignment.Middle;
               break;
            case ContentAlignment.TopCenter:
               halign = HorizontalAlignment.Center;
               valign = VerticalAlignment.Top;
               break;
            case ContentAlignment.TopLeft:
               halign = HorizontalAlignment.Left;
               valign = VerticalAlignment.Top;
               break;
            case ContentAlignment.TopRight:
               halign = HorizontalAlignment.Right;
               valign = VerticalAlignment.Top;
               break;
            default:
               throw new ArgumentException();
         }         
      }
      private float GetOutlineWidth() {
         return this.Font.Height / 7.0f;
      }
      private static StringFormat GetStringFormat(HorizontalAlignment halign, VerticalAlignment valign, 
                                                  bool rightToLeft) {
         StringFormat format = new StringFormat();
         if (rightToLeft) {
            format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
         }
         switch (halign) {
            case HorizontalAlignment.Center:
               format.Alignment = StringAlignment.Center;
               break;
            case HorizontalAlignment.Left:
               format.Alignment = rightToLeft ? StringAlignment.Far : StringAlignment.Near;
               break;
            case HorizontalAlignment.Right:
               format.Alignment = rightToLeft ? StringAlignment.Near : StringAlignment.Far;
               break;
         }
         switch (valign) {
            case VerticalAlignment.Bottom:
               format.LineAlignment = StringAlignment.Far;
               break;
            case VerticalAlignment.Middle:
               format.LineAlignment = StringAlignment.Center;
               break;
            case VerticalAlignment.Top:
               format.LineAlignment = StringAlignment.Near;
               break;
         }
         return format;
      }
      #endregion

      #region IDisplayElement Members
      public void Close() {
         if (this.bitmap != null) {
            this.bitmap.Dispose();
            this.bitmap = null;
         }
      }
      public void Open(Bitmap bitmap) {
         this.Close();
         RectangleF bitmapBounds;
         using (Graphics g = Graphics.FromImage(bitmap)) {
            RectangleF inputBounds = new RectangleF(0, 0, bitmap.Width, bitmap.Height);
            inputBounds = AddMargin(inputBounds, this.margin);
            inputBounds = this.DrawWatermark(g, inputBounds, true);
            bitmapBounds = new RectangleF(0f, 0f, inputBounds.Width, inputBounds.Height);
            this.bounds = inputBounds;
         }
         this.bitmap = new Bitmap((int)Math.Ceiling(bitmapBounds.Width), (int)Math.Ceiling(bitmapBounds.Height));
         using (Graphics graphics = Graphics.FromImage(this.bitmap)) {
            this.DrawWatermark(graphics, bitmapBounds, false);
         }
      }
      public void Prepare(Graphics graphics) {
      }
      public void Render(RenderingContext context) {
         context.Graphics.DrawImage(this.bitmap, this.bounds.X, this.bounds.Y);
      }
      public void RenderSample(Graphics graphics, Size size) {
         RectangleF rect = new RectangleF(0, 0, size.Width, size.Height);
         rect = AddMargin(rect, this.margin);
         this.DrawWatermark(graphics, rect, false);
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
