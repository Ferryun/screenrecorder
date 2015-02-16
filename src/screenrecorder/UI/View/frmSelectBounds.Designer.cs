namespace Atf.ScreenRecorder.UI.View {
   partial class frmSelectBounds {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.SuspendLayout();
         // 
         // frmSelectBounds
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(284, 262);
         this.Cursor = System.Windows.Forms.Cursors.Cross;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "frmSelectBounds";
         this.Opacity = 0.4;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "frmSelectBounds";
         this.TopMost = true;
         this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
         this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmSelectBounds_MouseUp);
         this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmSelectBounds_MouseDown);
         this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmSelectBounds_KeyPress);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSelectBounds_FormClosing);
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmSelectBounds_MouseMove);
         this.ResumeLayout(false);

      }

      #endregion
   }
}