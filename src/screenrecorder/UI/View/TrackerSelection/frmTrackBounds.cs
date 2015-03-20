namespace Atf.ScreenRecorder.UI.View {
   using Atf.ScreenRecorder.UI.Presentation;
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;
   using System.Drawing.Drawing2D;
   partial class frmTrackBounds : Form {
      private const int cGrip = 16;      // Grip size
      private const int cCaption = 32;   // Caption bar height;
      private const int HTCAPTION = 2;
      private const int HTLEFT = 10;
      private const int HTRIGHT = 11;
      private const int HTTOP = 12;
      private const int HTTOPLEFT = 13;
      private const int HTTOPRIGHT = 14;
      private const int HTBOTTOM = 15;
      private const int HTBOTTOMLEFT = 16;
      private const int HTBOTTOMRIGHT = 17;
      private const int SC_FIRST = 0xF000;
      private const int SC_MAXIMIZE = SC_FIRST + 0x32;
      private const int WM_NCHITTEST = 0x84;
      private const int WM_SYSCOMMAND = 274;

      private static readonly int frameWidth = 5;
      private static readonly string fullScreenTag = "FullScreen";
      private Rectangle cursorBounds;
      private bool isFixedAroundCursor;
      private Rectangle selectedBounds = Rectangle.Empty;
      private List<Form> visibleForms;
      public frmTrackBounds() {
         InitializeComponent();
      }
      public bool IsFixedAroundCursor {
         get {
            return isFixedAroundCursor;
         }
         set {
            this.isFixedAroundCursor = value;
         }
      }
      private void btnCancel_Click(object sender, EventArgs e) {
         this.OnCancel(EventArgs.Empty);
      }
      private void btnOK_Click(object sender, EventArgs e) {
         this.OnOK(EventArgs.Empty);
      }
      private void cmsPredefinedSizes_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
         if (e.ClickedItem == this.tsmiCustom) {
            return;
         }
         object tag = e.ClickedItem.Tag;
         if (tag is string) {
            Rectangle bounds;
            if (tag.Equals(fullScreenTag)) {
               Size size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
               bounds = new Rectangle(Point.Empty, size);
            }
            else {
               Size size = (Size)new SizeConverter().ConvertFromInvariantString((string)tag);
               bounds = new Rectangle(this.Location, size);
            }
            this.Bounds = bounds;
         }
      }
      private void cmsPredefinedSizes_Opening(object sender, CancelEventArgs e) {
         SizeConverter sizeConverter = new SizeConverter();
         foreach (ToolStripItem item in this.cmsPredefinedSizes.Items) {
            object tag = item.Tag;
            if (item is ToolStripMenuItem && tag is string) {
               ToolStripMenuItem menuItem = ((ToolStripMenuItem)item);
               bool check = false;
               Rectangle bounds = Rectangle.Empty;
               if (tag.Equals(fullScreenTag)) {
                  bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
               }
               else {
                  try {
                     Size size = (Size)sizeConverter.ConvertFromString((string)tag);
                     bounds = new Rectangle(this.Location, size);
                  }
                  catch (NotSupportedException) {
                  }
               }
               if (bounds.Equals(this.Bounds)) {
                  check = true;
               }
               menuItem.Checked = check;
            }
         }
      }
      private void OnCancel(EventArgs eventArgs) {
         this.Close();
      }
      protected override void OnFormClosing(FormClosingEventArgs e) {
         base.OnFormClosing(e);
         foreach (Form form in visibleForms) {
            form.Visible = true;
         }
      }
      protected override void OnKeyPress(KeyPressEventArgs e) {
         base.OnKeyPress(e);
         if (!e.Handled) {
            this.Close();
         }
      }
      protected override void OnLoad(EventArgs e) {
         base.OnLoad(e);
         // Hide application forms
         this.visibleForms = new List<Form>();
         foreach (Form form in Application.OpenForms) {
            if (form != this && form.Visible) {
               this.visibleForms.Add(form);
               form.Visible = false;
            }
         }
      }
      private void OnOK(EventArgs eventArgs) {
         this.Result = true;
         this.Close();
      }
      protected override void OnPaintBackground(PaintEventArgs e) {
         base.OnPaintBackground(e);
         Rectangle clientRectangle = this.ClientRectangle;
         using (Region region = new Region(clientRectangle)) {
            clientRectangle.Inflate(-frameWidth, -frameWidth);
            region.Exclude(clientRectangle);
            region.Intersect(e.ClipRectangle);
            using (var hatchBrush = new HatchBrush(HatchStyle.BackwardDiagonal, SystemColors.WindowText, this.BackColor)) {
               e.Graphics.FillRegion(hatchBrush, region);
            }
         }
         if (isFixedAroundCursor) {
            e.Graphics.DrawRectangle(SystemPens.WindowText, this.cursorBounds.X, this.cursorBounds.Y,
                                     this.cursorBounds.Width - 1, this.cursorBounds.Height - 1);
            Cursors.Default.Draw(e.Graphics, this.cursorBounds);           
         }
      }
      protected override void OnResize(EventArgs e) {
         base.OnResize(e);
         this.SetInvalidRegion();
         this.pnlActions.Location =
            new Point(this.ClientSize.Width - this.pnlActions.Width - 2 * frameWidth, 
                      this.ClientSize.Height - this.pnlActions.Height - 2 * frameWidth);
         this.btnPredefinedSizes.Location = new Point(2 * frameWidth, 2 * frameWidth);
      }
      protected override void OnSizeChanged(EventArgs e) {
         base.OnSizeChanged(e);
         this.SetInvalidRegion();
         this.btnPredefinedSizes.Text = string.Format("{0}x{1}", this.Width, this.Height);
      }
      private void SetInvalidRegion() {
         Rectangle prevBounds = this.selectedBounds;
         this.selectedBounds = this.ClientRectangle;
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
               // Add cursor bounds
               if (this.isFixedAroundCursor) {
                  Cursor cursor = Cursors.Default;
                  int width = this.Width;
                  int height = this.Height;
                  Rectangle rect = new Rectangle((width - cursor.Size.Width) / 2,
                                                 (height - cursor.Size.Height) / 2,
                                                 cursor.Size.Width,
                                                 cursor.Size.Height);
                  rect.Offset(cursor.HotSpot.X, cursor.HotSpot.Y);
                  if (!this.cursorBounds.Equals(rect)) {
                     // Prev. cursor bounds
                     Rectangle oldCursorBounds = this.cursorBounds;
                     this.cursorBounds = rect;
                     invalidRegion.Union(oldCursorBounds);
                     invalidRegion.Union(this.cursorBounds);
                  }
               }
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
      private void tsmiCustom_Click(object sender, EventArgs e) {
         using (frmCustomTrackBounds frmCustomTrackBounds = new frmCustomTrackBounds()) {
            frmCustomTrackBounds.CustomBounds = this.Bounds;
            frmCustomTrackBounds.CustomBoundsChanged += new EventHandler(
               (o, ea) => {
                  this.Bounds = frmCustomTrackBounds.CustomBounds;
               });
            if (DialogResult.OK == frmCustomTrackBounds.ShowDialog(this)) {
               // this.Bounds = frmCustomTrackBounds.CustomBounds;
            }
         }
      }
      protected override void WndProc(ref Message m) {
         if (m.Msg == WM_NCHITTEST) {
            Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
            pos = this.PointToClient(pos);
            Rectangle innerRectangle = this.ClientRectangle;
            innerRectangle.Inflate(-frameWidth, -frameWidth);
            if (pos.X >= innerRectangle.Right && pos.Y >= innerRectangle.Bottom) {
               m.Result = (IntPtr)HTBOTTOMRIGHT;
               return;
            }
            if (pos.X >= innerRectangle.Right && pos.Y <= innerRectangle.Top) {
               m.Result = (IntPtr)HTTOPRIGHT;
               return;
            }
            if (pos.X <= innerRectangle.Left && pos.Y <= innerRectangle.Top) {
               m.Result = (IntPtr)HTTOPLEFT;
               return;
            }
            if (pos.X <= innerRectangle.Left && pos.Y >= innerRectangle.Bottom) {
               m.Result = (IntPtr)HTBOTTOMLEFT;
               return;
            }
            if (pos.X >= innerRectangle.Right) {
               m.Result = (IntPtr)HTRIGHT;
               return;
            }
            if (pos.X <= innerRectangle.Left) {
               m.Result = (IntPtr)HTLEFT;
               return;
            }
            if (pos.Y <= innerRectangle.Top) {
               m.Result = (IntPtr)HTTOP;
               return;
            }
            if (pos.Y >= innerRectangle.Bottom) {
               m.Result = (IntPtr)HTBOTTOM;
               return;
            }
            if (/*!this.isFixedAroundCursor && */this.ClientRectangle.Contains(pos)) {
               m.Result = (IntPtr)HTCAPTION;
               return;
            }
         }
         else if ((m.Msg == WM_SYSCOMMAND) && (m.WParam.ToInt32() == SC_MAXIMIZE)) {          
            return;
         }
         base.WndProc(ref m);
      }
      #region IView Members
      public bool Result {
         get;
         set;
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
