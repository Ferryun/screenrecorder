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
   using Atf.ScreenRecorder.UI.Presentation;

   using System;
   using System.Collections.Generic;
   using System.Drawing;
   using System.Windows.Forms;

   public partial class frmSelectBounds : Form, ISelectBoundsView {
      #region Fields
      private static readonly int frameWidth = 2;
      private Point? point1;
      private Point? point2;      
      private Rectangle selectedBounds;
      private List<Form> visibleForms;
      #endregion

      #region Constructors
      public frmSelectBounds() {
         InitializeComponent();
      }
      #endregion

      #region Methods
      private static Rectangle CreateBounds(Point Point1, Point Point2) {
         int x = Math.Min(Point1.X, Point2.X);
         int width = Math.Max(Point1.X, Point2.X) - x;
         int y = Math.Min(Point1.Y, Point2.Y);
         int height = Math.Max(Point1.Y, Point2.Y) - y;
         return new Rectangle(x, y, width, height);
      }
      private void frmSelectBounds_FormClosing(object sender, FormClosingEventArgs e) {
         foreach (Form form in visibleForms) {
            form.Visible = true;
         }
      }
      private void frmSelectBounds_KeyPress(object sender, KeyPressEventArgs e) {
         this.Close();
      }     
      private void frmSelectBounds_MouseDown(object sender, MouseEventArgs e) {
         if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
            this.selectedBounds = Rectangle.Empty;
            this.point1 = e.Location;
            this.point2 = null;
         }
      }
      private void frmSelectBounds_MouseUp(object sender, MouseEventArgs e) {
         if ((e.Button & MouseButtons.Left) == MouseButtons.Left && point1.HasValue && point2.HasValue) {
            this.selectedBounds = CreateBounds(this.point1.Value, this.point2.Value);
            this.Result = true;
            this.Close();
         }
         else if ((e.Button & MouseButtons.Right) == MouseButtons.Right) {
            if (!this.point1.HasValue) {
               this.Close();
            }
            else {
               point1 = point2 = null;
               this.selectedBounds = Rectangle.Empty;
               this.Invalidate();
            }
         }
      }
      private void frmSelectBounds_MouseMove(object sender, MouseEventArgs e) {         
         if (!((e.Button & MouseButtons.Left) == MouseButtons.Left && this.point1.HasValue)) {
            return;
         }
         Rectangle prevBounds = this.selectedBounds;
         this.point2 = e.Location;
         this.selectedBounds = CreateBounds(this.point1.Value, this.point2.Value);
         if (prevBounds.IsEmpty) {
            this.Invalidate(this.selectedBounds);
         }
         else {
            Region sharedRegion = null;
            Region prevFrameRegion = null;
            Region sharedFrameRegion = null;
            Region invalidRegion = null;
            try {
               // Create shared region
               Rectangle sharedRectangle = prevBounds;
               sharedRectangle.Intersect(this.selectedBounds);
               sharedRectangle.Inflate(-frameWidth, -frameWidth);
               sharedRegion = new Region(sharedRectangle);
               // Create previous bounds frame region
               Rectangle prevInside = prevBounds;
               prevInside.Inflate(-frameWidth, -frameWidth);
               prevFrameRegion = new Region(prevBounds);
               prevFrameRegion.Exclude(prevInside);
               // Create shared bounds frame region
               Rectangle boundsInside = this.selectedBounds;
               boundsInside.Inflate(-frameWidth, -frameWidth);
               sharedFrameRegion = new Region(this.selectedBounds);
               sharedFrameRegion.Exclude(boundsInside);
               sharedFrameRegion.Intersect(prevFrameRegion);
               sharedRegion.Union(sharedFrameRegion);
               // Create invalid region
               invalidRegion = new Region(prevBounds);
               invalidRegion.Union(this.selectedBounds);
               invalidRegion.Exclude(sharedRegion);
               // Invalidate form
               this.Invalidate(invalidRegion);
            }
            finally {
               if (sharedFrameRegion != null) {
                  sharedFrameRegion.Dispose();
               }
               if (prevFrameRegion != null) {
                  prevFrameRegion.Dispose();
               }
               if (sharedRegion != null) {
                  sharedRegion.Dispose();
               }
               if (invalidRegion != null) {
                  invalidRegion.Dispose();
               }
            }
           
         }
      }
      protected override void OnLoad(EventArgs e) {
         base.OnLoad(e);
         // Set form size to screen size
         this.Size = Screen.PrimaryScreen.Bounds.Size;
         // Hide application forms
         this.visibleForms = new List<Form>();
         foreach (Form form in Application.OpenForms) {
            if (form != this && form.Visible) {
               this.visibleForms.Add(form);
               form.Visible = false;
            }
         }
      }
      protected override void OnPaint(PaintEventArgs e) {
         // base.OnPaint(e);
      }
      protected override void OnPaintBackground(PaintEventArgs e) {
         base.OnPaintBackground(e);
         if (!this.selectedBounds.IsEmpty) {
            using (Region region = new Region(this.selectedBounds)) {
               region.Intersect(e.ClipRectangle);
               using (SolidBrush brush = new SolidBrush(this.TransparencyKey)) {
                  e.Graphics.FillRegion(brush, region);
               }
            }
            using (Pen pen = new Pen(SystemColors.WindowFrame, frameWidth)) {
               e.Graphics.DrawRectangle(pen, this.selectedBounds.X + frameWidth / 2,
                                        this.selectedBounds.Y + frameWidth / 2,
                                        this.selectedBounds.Width - frameWidth, this.selectedBounds.Height - frameWidth);
            }
            
         }
      }
      #endregion
      #region IView Members
      public bool Result {
         get;
         set;
      }
      public Rectangle SelectedBounds {
         get {
            return this.selectedBounds;
         }
      }
      public new bool ShowDialog() {
         base.ShowDialog();
         return this.Result;
      }
      public bool ShowDialog(IView owner) {
         base.ShowDialog((IWin32Window)owner);
         return this.Result;
      }
      #endregion
   }
}
