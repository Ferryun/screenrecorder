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
namespace Atf.ScreenRecorder.UI.View {
   using Atf.ScreenRecorder.Screen;
   using System;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;
   partial class CursorHighlightOptions : UserControl {
      #region Fields
      private const string defaultColorString = "Yellow";
      private const int defaultRadious = 20;
      private static readonly int minRadious = 1;
      private static readonly int maxRadious = 50;
      private Color color;
      private MouseCursor mouseCursor;
      private int radious;
      #endregion

      #region Constructors
      public CursorHighlightOptions() {
         InitializeComponent();
         //
         this.mouseCursor = new MouseCursor();
         //
         this.tbRadious.Minimum = minRadious;
         this.tbRadious.Maximum = maxRadious;
         this.Color = Color.FromName(defaultColorString);
         this.Radious = defaultRadious;
      }
      #endregion

      #region Properties
      [Browsable(true)]
      [DefaultValue(typeof(Color), defaultColorString)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Color Color {
         get {
            return this.color;
         }
         set {
            if (this.color != value) {
               this.color = value;
               this.OnColorChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(defaultRadious)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public int Radious {
         get {
            return this.radious;
         }
         set {
            if (this.radious != value) {
               if (value > maxRadious) {
                  this.radious = maxRadious;
               }
               else if (value > minRadious) {
                  this.radious = value;
               }
               else {
                  this.radious = minRadious;
               }
               this.OnRadiousChanged(EventArgs.Empty);
            }
         }
      }
      #endregion

      #region Methods
      private void btnColor_Click(object sender, EventArgs e) {
         using (ColorDialog colorDialog = new ColorDialog()) {
            colorDialog.Color = this.color;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
               this.Color = colorDialog.Color;
            }
         }
      }
      private void OnColorChanged(EventArgs eventArgs) {
         this.lblColor.Text = ColorTranslator.ToHtml(this.color);
         this.picHighlight.Invalidate();
      }
      protected override void OnGotFocus(EventArgs e) {
         this.tbRadious.Focus();
         // base.OnGotFocus(e);
      }
      private void OnRadiousChanged(EventArgs eventArgs) {
         this.tbRadious.Value = this.radious;
         picHighlight.Invalidate();
      }
      private void picHighlight_Paint(object sender, PaintEventArgs e) {
         Graphics g = e.Graphics;
         g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
         g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
         this.mouseCursor.Color = this.color;
         this.mouseCursor.Radious = this.radious;
         this.mouseCursor.RenderSample(g, this.picHighlight.ClientSize);
      }
      private void tbRadious_Scroll(object sender, EventArgs e) {
         this.Radious = tbRadious.Value;
      }
      #endregion
   }
}
