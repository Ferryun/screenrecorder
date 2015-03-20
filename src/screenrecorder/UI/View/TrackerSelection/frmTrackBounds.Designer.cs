namespace Atf.ScreenRecorder.UI.View {
   partial class frmTrackBounds {
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
         this.components = new System.ComponentModel.Container();
         this.pnlActions = new System.Windows.Forms.Panel();
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnOK = new System.Windows.Forms.Button();
         this.cmsPredefinedSizes = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.tsmiCustom = new System.Windows.Forms.ToolStripMenuItem();
         this.btnPredefinedSizes = new Atf.UI.DropDownButton();
         this.pnlActions.SuspendLayout();
         this.cmsPredefinedSizes.SuspendLayout();
         this.SuspendLayout();
         // 
         // pnlActions
         // 
         this.pnlActions.AutoSize = true;
         this.pnlActions.Controls.Add(this.btnCancel);
         this.pnlActions.Controls.Add(this.btnOK);
         this.pnlActions.Location = new System.Drawing.Point(102, 7);
         this.pnlActions.Name = "pnlActions";
         this.pnlActions.Size = new System.Drawing.Size(165, 28);
         this.pnlActions.TabIndex = 0;
         // 
         // btnCancel
         // 
         this.btnCancel.BackColor = System.Drawing.SystemColors.Control;
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(87, 0);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancel.TabIndex = 1;
         this.btnCancel.Text = "&Cancel";
         this.btnCancel.UseVisualStyleBackColor = false;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnOK
         // 
         this.btnOK.BackColor = System.Drawing.SystemColors.Control;
         this.btnOK.Location = new System.Drawing.Point(3, 0);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 23);
         this.btnOK.TabIndex = 0;
         this.btnOK.Text = "&OK";
         this.btnOK.UseVisualStyleBackColor = false;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // cmsPredefinedSizes
         // 
         this.cmsPredefinedSizes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem1,
            this.toolStripMenuItem6,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripSeparator1,
            this.tsmiCustom});
         this.cmsPredefinedSizes.Name = "cmsPredefinedSizes";
         this.cmsPredefinedSizes.Size = new System.Drawing.Size(127, 208);
         this.cmsPredefinedSizes.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsPredefinedSizes_ItemClicked);
         this.cmsPredefinedSizes.Opening += new System.ComponentModel.CancelEventHandler(this.cmsPredefinedSizes_Opening);
         // 
         // toolStripMenuItem4
         // 
         this.toolStripMenuItem4.Name = "toolStripMenuItem4";
         this.toolStripMenuItem4.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem4.Tag = "320, 240";
         this.toolStripMenuItem4.Text = "320x240";
         // 
         // toolStripMenuItem5
         // 
         this.toolStripMenuItem5.Name = "toolStripMenuItem5";
         this.toolStripMenuItem5.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem5.Tag = "400, 300";
         this.toolStripMenuItem5.Text = "400x300";
         // 
         // toolStripMenuItem1
         // 
         this.toolStripMenuItem1.Name = "toolStripMenuItem1";
         this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem1.Tag = "640, 480";
         this.toolStripMenuItem1.Text = "640x480";
         // 
         // toolStripMenuItem6
         // 
         this.toolStripMenuItem6.Name = "toolStripMenuItem6";
         this.toolStripMenuItem6.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem6.Tag = "720, 480";
         this.toolStripMenuItem6.Text = "720x480";
         // 
         // toolStripMenuItem2
         // 
         this.toolStripMenuItem2.Name = "toolStripMenuItem2";
         this.toolStripMenuItem2.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem2.Tag = "800, 600";
         this.toolStripMenuItem2.Text = "800x600";
         // 
         // toolStripMenuItem3
         // 
         this.toolStripMenuItem3.Name = "toolStripMenuItem3";
         this.toolStripMenuItem3.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem3.Tag = "1024, 768";
         this.toolStripMenuItem3.Text = "1024x768";
         // 
         // toolStripMenuItem7
         // 
         this.toolStripMenuItem7.Name = "toolStripMenuItem7";
         this.toolStripMenuItem7.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem7.Tag = "1280, 720";
         this.toolStripMenuItem7.Text = "1280x720";
         // 
         // toolStripMenuItem8
         // 
         this.toolStripMenuItem8.Name = "toolStripMenuItem8";
         this.toolStripMenuItem8.Size = new System.Drawing.Size(126, 22);
         this.toolStripMenuItem8.Tag = "FullScreen";
         this.toolStripMenuItem8.Text = "Full Screen";
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(123, 6);
         // 
         // tsmiCustom
         // 
         this.tsmiCustom.Name = "tsmiCustom";
         this.tsmiCustom.Size = new System.Drawing.Size(126, 22);
         this.tsmiCustom.Text = "Custom...";
         this.tsmiCustom.Click += new System.EventHandler(this.tsmiCustom_Click);
         // 
         // btnPredefinedSizes
         // 
         this.btnPredefinedSizes.Location = new System.Drawing.Point(12, 7);
         this.btnPredefinedSizes.Menu = this.cmsPredefinedSizes;
         this.btnPredefinedSizes.Name = "btnPredefinedSizes";
         this.btnPredefinedSizes.Padding = new System.Windows.Forms.Padding(2);
         this.btnPredefinedSizes.Size = new System.Drawing.Size(84, 23);
         this.btnPredefinedSizes.Style = Atf.UI.DropDownButtonStyle.Button;
         this.btnPredefinedSizes.TabIndex = 1;
         // 
         // frmTrackBounds
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(272, 48);
         this.Controls.Add(this.btnPredefinedSizes);
         this.Controls.Add(this.pnlActions);
         this.Cursor = System.Windows.Forms.Cursors.Default;
         this.DoubleBuffered = true;
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.MinimumSize = new System.Drawing.Size(272, 48);
         this.Name = "frmTrackBounds";
         this.Opacity = 0.5;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "TrackBounds";
         this.TopMost = true;
         this.pnlActions.ResumeLayout(false);
         this.cmsPredefinedSizes.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Panel pnlActions;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnOK;
      private Atf.UI.DropDownButton btnPredefinedSizes;
      private System.Windows.Forms.ContextMenuStrip cmsPredefinedSizes;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripMenuItem tsmiCustom;

   }
}