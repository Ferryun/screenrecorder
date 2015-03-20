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
namespace Atf.UI {
   using System;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;
   using System.Windows.Forms.VisualStyles;
   class DropDownButton : Control {
      private Rectangle glyphBounds;
      private bool isHovered = false;
      private bool isPressed = false;
      private Image image;
      private Rectangle imageBounds;
      private DropDownButtonImageSizeMode imageSizeMode = DropDownButtonImageSizeMode.Stretch;
      private ToolStripDropDown menu;
      private VisualStyleRenderer renderer;
      private DropDownButtonStyle style;
      private System.Drawing.ContentAlignment textAlign = System.Drawing.ContentAlignment.MiddleCenter;
      private Rectangle textBounds;
      private bool toggleMenu;
      public DropDownButton() {
         this.SuspendLayout();
         // DropDownButton
         this.DoubleBuffered = true;
         this.MeasureBounds();
         this.Padding = new Padding(2);
         this.ResumeLayout();
      }
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [DefaultValue(false)]
      [Browsable(true)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public override bool AutoSize {
         get {
            return base.AutoSize;
         }
         set {
            base.AutoSize = value;
         }
      }     
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [DefaultValue(null)]
      [Browsable(true)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Image Image {
         get {
            return this.image;
         }
         set {
            if (this.image != value) {
               this.image = value;
               // if (this.AutoSize) {
               this.MeasureBounds();
               this.PerformLayout(this, "Image");
               // }
               this.Invalidate();
            }            
         }
      }
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [DefaultValue(DropDownButtonImageSizeMode.Stretch)]
      [Browsable(true)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public DropDownButtonImageSizeMode ImageSizeMode {
         get {
            return this.imageSizeMode;
         }
         set {
            if (this.imageSizeMode != value) {
               this.imageSizeMode = value;
               this.MeasureBounds();
               this.PerformLayout(this, "ImageSizeMode");
               this.Invalidate();
            }
         }
      }
      private bool IsHovered {
         get {
            return this.isHovered;
         }
         set {
            if (this.isHovered != value) {
               this.isHovered = value;
               this.Invalidate();
            }
         }
      }
      private bool IsPressed {
         get {
            return this.isPressed;
         }
         set {
            if (this.isPressed != value) {
               this.isPressed = value;
               this.Invalidate();
            }
         }
      }
      
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [DefaultValue(null)]
      [Browsable(true)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public ToolStripDropDown Menu {
         get {
            return this.menu;
         }
         set {
            if (this.menu != value) {
               if (this.menu != null && !this.menu.IsDisposed) {
                  this.menu.VisibleChanged -= menu_VisibleChanged;
               }
               this.menu = value;
               if (this.menu != null) {
                  this.menu.VisibleChanged += new EventHandler(menu_VisibleChanged);
               }
            }
         }
      }
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [DefaultValue(DropDownButtonStyle.ToolbarButton)]
      [Browsable(true)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public DropDownButtonStyle Style {
         get {
            return this.style;
         }
         set {
            if (this.style != value) {
               this.style = value;
               this.MeasureBounds();
               this.Invalidate();
            }
         }
      }
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [DefaultValue(System.Drawing.ContentAlignment.MiddleCenter)]
      [Browsable(true)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public System.Drawing.ContentAlignment TextAlign {
         get {
            return this.textAlign;
         }
         set {
            if (this.textAlign != value) {
               this.textAlign = value;
               this.Invalidate();
            }
         }
      }      
      private void DrawBorder(PaintEventArgs pevent) {
         if (this.IsPressed) {
            ControlPaint.DrawBorder3D(pevent.Graphics, new Rectangle(0, 0, this.Width - 1, this.Height - 1),
                                      Border3DStyle.SunkenOuter);
         }
         else if (this.IsHovered) {
            ControlPaint.DrawBorder3D(pevent.Graphics, new Rectangle(0, 0, this.Width - 1, this.Height - 1),
                                      Border3DStyle.RaisedInner);
         }
         else {
         }
      }
      private void DrawButton(PaintEventArgs e) {
         PushButtonState state = PushButtonState.Default;
         if (!this.Enabled) {
            state = PushButtonState.Disabled;
         }
         else if (this.IsPressed) {
            state = PushButtonState.Pressed;
         }
         else if (this.IsHovered) {
            state = PushButtonState.Hot;
         }
         else {
            state = PushButtonState.Normal;
         }
         TextFormatFlags format = this.GetTextFormat();
         Rectangle bounds = new Rectangle(0, 0, this.Width, this.Height);
         //if (this.image != null) {
         //   ButtonRenderer.DrawButton(e.Graphics, bounds,/* this.Text, this.Font, format, */this.image,
         //                             /this.imageBounds, this.Focused, state);
         //}
         //else {
            ButtonRenderer.DrawButton(e.Graphics, bounds,/* this.Text, this.Font, format, */ this.Focused, state);
         //}
      }
      private void DrawVisualStyleDropDownButtonBackground(PaintEventArgs e) {
         VisualStyleElement splitButtonElement;
         // VisualStyleElement splitButtonDropDownElement;
         if (!this.Enabled) {
            splitButtonElement = VisualStyleElement.ToolBar.DropDownButton.Disabled;
            // splitButtonDropDownElement = VisualStyleElement.ToolBar.SplitButtonDropDown.Disabled;
         }
         else if (this.IsPressed) {
            splitButtonElement = VisualStyleElement.ToolBar.DropDownButton.Pressed;
            // splitButtonDropDownElement = VisualStyleElement.ToolBar.SplitButtonDropDown.Pressed;
         }
         else if (this.IsHovered) {
            splitButtonElement = VisualStyleElement.ToolBar.DropDownButton.Hot;
            // splitButtonDropDownElement = VisualStyleElement.ToolBar.SplitButtonDropDown.Hot;
         }
         else {
            splitButtonElement = VisualStyleElement.ToolBar.DropDownButton.Normal;
            // splitButtonDropDownElement = VisualStyleElement.ToolBar.SplitButtonDropDown.Normal;
         }
         if (this.SetRenderer(splitButtonElement)) {
            renderer.DrawBackground(e.Graphics, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
         }
         //if (this.SetRenderer(splitButtonDropDownElement)) {
         //    if (this.RightToLeft != RightToLeft.Yes) {
         //        renderer.DrawBackground(e.Graphics, this.glyphBounds);
         //    }
         //}           
      }
      private void DrawDropDownButtonBackGround(PaintEventArgs e) {
         this.DrawBorder(e);
      }
      private void DrawDropDownButton(PaintEventArgs e) {
         if (this.style == DropDownButtonStyle.ToolbarButton) {
            if (System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported &&
                Application.RenderWithVisualStyles) {
               this.DrawVisualStyleDropDownButtonBackground(e);
            }
            else {
               this.DrawDropDownButtonBackGround(e);
            }
            this.DrawImage(e);
            this.DrawText(e);
            if (this.Focused) {
               this.DrawFocusRectangle(e);
            }
         }
         else {
            this.DrawButton(e);
            this.DrawImage(e);
            this.DrawText(e);
         }
         this.DrawGlyph(e);
      }
      private void DrawFocusRectangle(PaintEventArgs e) {
         Rectangle rect = this.ClientRectangle;
         rect.Inflate(-1, -1);
         ControlPaint.DrawFocusRectangle(e.Graphics, rect);
      }
      private void DrawGlyph(PaintEventArgs pevent) {
         using (var image = new Bitmap(glyphBounds.Width, glyphBounds.Height)) {
            using (var graphics = Graphics.FromImage(image)) {
               ControlPaint.DrawMenuGlyph(graphics, 2, 0, glyphBounds.Width, glyphBounds.Height,
                                           MenuGlyph.Arrow, this.ForeColor, Color.Transparent);

            }
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            if (!this.Enabled) {
               ControlPaint.DrawImageDisabled(pevent.Graphics, image, glyphBounds.X, glyphBounds.Y, Color.Transparent);
            }
            else {
               pevent.Graphics.DrawImage(image, glyphBounds);
            }
         }
      }
      private void DrawImage(PaintEventArgs e) {
         if (this.image != null) {
            if (this.Enabled) {               
               e.Graphics.DrawImage(this.image, this.imageBounds);
            }
            else {
               using (Bitmap bitmap = new Bitmap(this.imageBounds.Width, this.imageBounds.Height)) {
                  using (Graphics graphics = Graphics.FromImage(bitmap)) {
                     graphics.DrawImage(this.image, 0, 0, this.imageBounds.Width, this.imageBounds.Height);
                  }
                  ControlPaint.DrawImageDisabled(e.Graphics, bitmap, this.imageBounds.X, this.imageBounds.Y,
                                                 Color.Transparent);
               }
            }
         }
      }
      private void DrawText(PaintEventArgs e) {
         TextFormatFlags format = GetTextFormat();
         // e.Graphics.DrawRectangle(SystemPens.WindowText, this.textBounds);
         TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.textBounds,
                             this.Enabled ? this.ForeColor : SystemColors.ControlDark,
                             format);
      }
      public override Size GetPreferredSize(Size proposedSize) {
         // Size basePreferredSize = base.GetPreferredSize(proposedSize);
         Size textSize;
         TextFormatFlags format = TextFormatFlags.Default;
         if (this.RightToLeft == RightToLeft.Yes) {
            format |= TextFormatFlags.RightToLeft;
         }
         format |= TextFormatFlags.SingleLine;
         using (var graphics = this.CreateGraphics()) {
            textSize = TextRenderer.MeasureText(graphics, this.Text, this.Font, Size.Empty, format);
         }
         if (textSize.Height == 0) {
            textSize.Height = this.Font.Height;
         }
         Size imageSize = Size.Empty;
         if (this.image != null) {
            if (this.imageSizeMode == DropDownButtonImageSizeMode.ActualSize) {
               imageSize = this.image.Size;
            }
            else {
               imageSize = new Size(image.Width, 0);
            }
         }
         Size glyphSize = SystemInformation.MenuCheckSize;
         Size border3DSize = SystemInformation.Border3DSize;
         int prefferedWidth = imageSize.Width + textSize.Width + glyphSize.Width + 2 * border3DSize.Width +
                              this.Padding.Horizontal;
         int prefferedHeight;
         if (this.style == DropDownButtonStyle.ToolbarButton) {
            prefferedHeight = Math.Max(Math.Max(imageSize.Height, textSize.Height), glyphSize.Height)
                                          + 2 * border3DSize.Height + this.Padding.Vertical;
         }
         else {
            prefferedHeight = this.Font.Height + System.Windows.Forms.SystemInformation.BorderSize.Height * 4 + 3 + 2;
            if (this.image != null && this.imageSizeMode == DropDownButtonImageSizeMode.ActualSize) {
               prefferedHeight = Math.Max(prefferedHeight, imageSize.Height);
            }
         }
         return new Size(prefferedWidth, prefferedHeight);
      }
      private TextFormatFlags GetTextFormat() {
         TextFormatFlags format = TextFormatFlags.Default;
         format |= TextFormatFlags.EndEllipsis;
         format |= TextFormatFlags.SingleLine;
         if (this.RightToLeft == RightToLeft.Yes) {
            format |= TextFormatFlags.RightToLeft;
         }
         switch (textAlign) {
            case System.Drawing.ContentAlignment.BottomCenter:
               format |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
               break;
            case System.Drawing.ContentAlignment.BottomLeft:
               format |= TextFormatFlags.Bottom | TextFormatFlags.Left;
               break;
            case System.Drawing.ContentAlignment.BottomRight:
               format |= TextFormatFlags.Bottom | TextFormatFlags.Right;
               break;
            case System.Drawing.ContentAlignment.MiddleCenter:
               format |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
               break;
            case System.Drawing.ContentAlignment.MiddleLeft:
               format |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
               break;
            case System.Drawing.ContentAlignment.MiddleRight:
               format |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
               break;
            case System.Drawing.ContentAlignment.TopCenter:
               format |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
               break;
            case System.Drawing.ContentAlignment.TopLeft:
               format |= TextFormatFlags.Top | TextFormatFlags.Left;
               break;
            case System.Drawing.ContentAlignment.TopRight:
               format |= TextFormatFlags.Top | TextFormatFlags.Right;
               break;
         }
         if (this.RightToLeft == RightToLeft.Yes) {
            if ((format & TextFormatFlags.Left) == TextFormatFlags.Left) {
               format = format ^ TextFormatFlags.Left;
               format |= TextFormatFlags.Right;
            }
            else if ((format & TextFormatFlags.Right) == TextFormatFlags.Right) {
               format = format ^ TextFormatFlags.Right;
               format |= TextFormatFlags.Left;
            }
         }
         return format;
      }
      private void menu_VisibleChanged(object sender, EventArgs e) {
         if (this.menu != null) {
            if (!this.menu.Visible && this.ClientRectangle.Contains(this.PointToClient(Cursor.Position))) {
               toggleMenu = true;
            }
            else {
               toggleMenu = false;
            }
            this.IsPressed = this.menu.Visible;
         }
      }
      protected override void OnClick(System.EventArgs e) {
         if (this.menu != null) {
            this.Toggle();
            try {
               base.OnClick(e);
            }
            finally {
               this.IsPressed = this.menu.Visible;
            }
         }
      }
      protected override void OnFontChanged(EventArgs e) {
         this.MeasureBounds();
         this.Invalidate();
         base.OnFontChanged(e);
      }
      protected override void OnGotFocus(EventArgs e) {
         this.Invalidate();
         base.OnGotFocus(e);
      }
      protected override void OnLostFocus(EventArgs e) {
         this.Invalidate();
         base.OnLostFocus(e);
      }
      protected override void OnKeyDown(KeyEventArgs e) {
         if (e.KeyCode == Keys.Space) {
            this.Toggle();
            e.Handled = true;
         }
         base.OnKeyDown(e);
      }
      protected override void OnRightToLeftChanged(EventArgs e) {
         this.MeasureBounds();
         this.Invalidate();
         base.OnRightToLeftChanged(e);
      }
      protected override void OnMouseDown(MouseEventArgs e) {
         if (this.Style == DropDownButtonStyle.Button && !this.Focused) {
            this.Focus();
         }
         this.IsPressed = !this.IsPressed;
         base.OnMouseDown(e);
      }
      protected override void OnMouseHover(System.EventArgs e) {
         this.IsHovered = true;
         base.OnMouseHover(e);
      }
      protected override void OnMouseMove(MouseEventArgs e) {
         this.IsHovered = true;
         base.OnMouseMove(e);
      }
      protected override void OnMouseLeave(System.EventArgs e) {
         this.IsHovered = false;
         base.OnMouseLeave(e);
      }
      protected override void OnMouseUp(MouseEventArgs e) {
         if (this.menu != null) {
            this.IsPressed = this.menu.Visible;
         }
         base.OnMouseUp(e);
      }
      protected override void OnPaintBackground(PaintEventArgs pevent) {
         base.OnPaintBackground(pevent);
         this.DrawDropDownButton(pevent);
      }
      protected override void OnSizeChanged(EventArgs e) {
         this.MeasureBounds();
         this.Invalidate();
         base.OnSizeChanged(e);
      }
      protected override void OnTextChanged(EventArgs e) {
         this.MeasureBounds();
         this.Invalidate();
         base.OnTextChanged(e);
      }
      private void MeasureBounds() {
         Size border3DSize = SystemInformation.Border3DSize;
         Rectangle clientRectangle = new Rectangle(border3DSize.Width + Padding.Left, border3DSize.Height + Padding.Top,
                                                   this.Width - 2 * border3DSize.Width - Padding.Horizontal,

                                                  this.Height - 2 * border3DSize.Height - Padding.Vertical);
         Size glyphSize;
         //if (VisualStyleRenderer.IsSupported) {
         //    glyphSize = new Size(SystemInformation.MenuCheckSize.Width, this.Height);
         //    glyphBounds = new Rectangle(clientRectangle.Right - glyphSize.Width,
         //                                0,
         //                                glyphSize.Width,
         //                                glyphSize.Height);
         //}
         //else {
         glyphSize = SystemInformation.MenuCheckSize;
         glyphBounds = new Rectangle(clientRectangle.Right - glyphSize.Width,
                                     clientRectangle.Top,
                                     glyphSize.Width,
                                     glyphSize.Height);
         // }

         if (glyphBounds.Height < clientRectangle.Height) {
            glyphBounds.Offset(0, (clientRectangle.Height - glyphBounds.Height) / 2);
         }
         if (this.image != null) {
            int width, height;
            if (this.imageSizeMode == DropDownButtonImageSizeMode.Stretch) {
               int imageHeight = Math.Max(clientRectangle.Height, this.Font.Height);
               if (image.Height > imageHeight) {
                  width = (int)(image.Width * ((double)imageHeight / image.Height));
                  height = imageHeight;
               }
               else {
                  width = image.Width;
                  height = image.Height;
               }
            }
            else {
               width = image.Width;
               height = image.Height;
            }
            if (width > clientRectangle.Width - glyphSize.Width) {
               height = (int)(height * ((double)width / (clientRectangle.Width - glyphSize.Width)));
               width = clientRectangle.Width - glyphSize.Width;
            }
            imageBounds = new Rectangle(0, 0, width, height);
            imageBounds.Offset(clientRectangle.Location);
            if (imageBounds.Height < clientRectangle.Height) {
               imageBounds.Offset(0, (clientRectangle.Height - imageBounds.Height) / 2);
            }
         }
         else {
            imageBounds = Rectangle.Empty;
         }

         textBounds = new Rectangle(clientRectangle.Left + imageBounds.Width,
                                    clientRectangle.Top,
                                    clientRectangle.Width - imageBounds.Width - glyphSize.Width,
                                    clientRectangle.Height);

         if (this.RightToLeft == RightToLeft.Yes) {
            imageBounds.Location = new Point(clientRectangle.Right - imageBounds.Width, imageBounds.Top);
            textBounds.Location = new Point(imageBounds.Left - textBounds.Width, textBounds.Top);
            glyphBounds.Location = new Point(textBounds.Left - glyphBounds.Width, glyphBounds.Top);
         }
      }
      protected override bool ProcessDialogKey(Keys keyData) {
         switch (keyData) {
            case (Keys.Down):
               if (!this.IsPressed) {
                  this.Toggle();
                  return true;
               }
               break;
            case Keys.Up:
               if (this.IsPressed) {
                  this.Toggle();
                  return true;
               }
               break;
         }
         return base.ProcessDialogKey(keyData);
      }
      private bool SetRenderer(VisualStyleElement element) {
         if (!VisualStyleRenderer.IsElementDefined(element)) {
            return false;
         }
         if (renderer == null) {
            renderer = new VisualStyleRenderer(element);
         }
         else {
            renderer.SetParameters(element);
         }
         return true;
      }

      private void Toggle() {
         if (this.menu != null) {
            if (!toggleMenu) {
               ToolStripDropDownDirection direction;
               Point location;
               bool showBelow = true;
               int workingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
               if (this.PointToScreen(this.Location).Y + this.Height + menu.Height > workingAreaHeight) {
                  showBelow = false;
               }
               if (this.RightToLeft != RightToLeft.Yes) {
                  direction = showBelow ? ToolStripDropDownDirection.BelowRight :
                                          ToolStripDropDownDirection.AboveRight;
                  location = this.PointToScreen(new Point(0, showBelow ? this.Height : 0));
               }
               else {
                  direction = showBelow ? ToolStripDropDownDirection.BelowLeft : 
                                          ToolStripDropDownDirection.AboveLeft;
                  location = this.PointToScreen(new Point(this.Width, showBelow ? this.Height : 0));
               }
               this.menu.Show(location, direction);
            }
            else {
               toggleMenu = false;
            }
            this.IsPressed = this.menu.Visible;
         }
      }
   }
}
