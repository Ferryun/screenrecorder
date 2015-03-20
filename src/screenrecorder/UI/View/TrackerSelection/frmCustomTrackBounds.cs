namespace Atf.ScreenRecorder.UI.View {
   using System;
   using System.Drawing;
   using System.Windows.Forms;
   public partial class frmCustomTrackBounds : Form {
      public event EventHandler CustomBoundsChanged;
      private bool raiseEvent = true;
      public frmCustomTrackBounds() {
         InitializeComponent();
         //
         Rectangle screenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
         this.nudHeight.Maximum = screenBounds.Height;
         this.nudWidth.Maximum = screenBounds.Width;
         this.nudX.Maximum = screenBounds.Width - 2;
         this.nudY.Maximum = screenBounds.Height - 2;
      }
      public Rectangle CustomBounds {
         get {
            return new Rectangle(
               (int)this.nudX.Value,
               (int)this.nudY.Value,
               (int)this.nudWidth.Value,
               (int)this.nudHeight.Value);
         }
         set {
            this.raiseEvent = false;
            this.nudX.Value = value.X;
            this.nudY.Value = value.Y;
            this.nudWidth.Value = value.Width;
            this.nudHeight.Value = value.Height;
            this.raiseEvent = true;
         }
      }
      private void btnCancel_Click(object sender, EventArgs e) {
         this.DialogResult = DialogResult.Cancel;
      }
      private void btnOK_Click(object sender, EventArgs e) {
         this.DialogResult = DialogResult.OK;
      }
      private void OnCustomBoundsChanged(EventArgs e) {
         if (this.raiseEvent && this.CustomBoundsChanged != null) {
            this.CustomBoundsChanged(this, e);
         }
      }
      private void nud_ValueChanged(object sender, EventArgs e) {
         NumericUpDown numericUpDown = (NumericUpDown)sender;
         if (numericUpDown == nudWidth || numericUpDown == nudHeight) {
            decimal value = numericUpDown.Value;
            if (value % 2 == 1) {
               if (value == numericUpDown.Maximum - 1) {
                  value--;
               }
               else {
                  value++;
               }
               numericUpDown.Value = value;
               return;
            }
         }
         this.OnCustomBoundsChanged(EventArgs.Empty);
      }
      private void nud_Enter(object sender, EventArgs e) {
         NumericUpDown numericUpDown = (NumericUpDown)sender;
         numericUpDown.Select(0, numericUpDown.Value.ToString().Length);
      }     
   }
}
