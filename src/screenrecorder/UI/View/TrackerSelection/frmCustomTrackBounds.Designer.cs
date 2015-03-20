namespace Atf.ScreenRecorder.UI.View {
   partial class frmCustomTrackBounds {
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.nudHeight = new System.Windows.Forms.NumericUpDown();
         this.nudWidth = new System.Windows.Forms.NumericUpDown();
         this.nudX = new System.Windows.Forms.NumericUpDown();
         this.label8 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.nudY = new System.Windows.Forms.NumericUpDown();
         this.panel1 = new System.Windows.Forms.Panel();
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnOK = new System.Windows.Forms.Button();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 4;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.83333F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.16666F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
         this.tableLayoutPanel1.Controls.Add(this.nudHeight, 3, 1);
         this.tableLayoutPanel1.Controls.Add(this.nudX, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label8, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
         this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.nudY, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.nudWidth, 3, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(224, 52);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // nudHeight
         // 
         this.nudHeight.Location = new System.Drawing.Point(170, 29);
         this.nudHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
         this.nudHeight.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
         this.nudHeight.Name = "nudHeight";
         this.nudHeight.Size = new System.Drawing.Size(51, 20);
         this.nudHeight.TabIndex = 7;
         this.nudHeight.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
         this.nudHeight.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
         this.nudHeight.Enter += new System.EventHandler(this.nud_Enter);
         // 
         // nudWidth
         // 
         this.nudWidth.Location = new System.Drawing.Point(170, 3);
         this.nudWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
         this.nudWidth.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
         this.nudWidth.Name = "nudWidth";
         this.nudWidth.Size = new System.Drawing.Size(51, 20);
         this.nudWidth.TabIndex = 5;
         this.nudWidth.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
         this.nudWidth.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
         this.nudWidth.Enter += new System.EventHandler(this.nud_Enter);
         // 
         // nudX
         // 
         this.nudX.Location = new System.Drawing.Point(28, 3);
         this.nudX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
         this.nudX.Name = "nudX";
         this.nudX.Size = new System.Drawing.Size(51, 20);
         this.nudX.TabIndex = 1;
         this.nudX.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
         this.nudX.Enter += new System.EventHandler(this.nud_Enter);
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(3, 26);
         this.label8.Name = "label8";
         this.label8.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
         this.label8.Size = new System.Drawing.Size(17, 25);
         this.label8.TabIndex = 2;
         this.label8.Text = "Y:";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 0);
         this.label1.Name = "label1";
         this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
         this.label1.Size = new System.Drawing.Size(17, 25);
         this.label1.TabIndex = 0;
         this.label1.Text = "X:";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(123, 26);
         this.label3.Name = "label3";
         this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
         this.label3.Size = new System.Drawing.Size(41, 25);
         this.label3.TabIndex = 6;
         this.label3.Text = "Height:";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(123, 0);
         this.label4.Name = "label4";
         this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
         this.label4.Size = new System.Drawing.Size(38, 25);
         this.label4.TabIndex = 4;
         this.label4.Text = "Width:";
         // 
         // nudY
         // 
         this.nudY.Location = new System.Drawing.Point(28, 29);
         this.nudY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
         this.nudY.Name = "nudY";
         this.nudY.Size = new System.Drawing.Size(51, 20);
         this.nudY.TabIndex = 3;
         this.nudY.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
         this.nudY.Enter += new System.EventHandler(this.nud_Enter);
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.btnCancel);
         this.panel1.Controls.Add(this.btnOK);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panel1.Location = new System.Drawing.Point(6, 58);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(224, 39);
         this.panel1.TabIndex = 1;
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(146, 13);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 26);
         this.btnCancel.TabIndex = 1;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnOK
         // 
         this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOK.Location = new System.Drawing.Point(65, 13);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 26);
         this.btnOK.TabIndex = 0;
         this.btnOK.Text = "OK";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // frmCustomTrackBounds
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(236, 103);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Controls.Add(this.panel1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmCustomTrackBounds";
         this.Padding = new System.Windows.Forms.Padding(6);
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Custom Region";
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
         this.panel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.NumericUpDown nudHeight;
      private System.Windows.Forms.NumericUpDown nudWidth;
      private System.Windows.Forms.NumericUpDown nudX;
      private System.Windows.Forms.NumericUpDown nudY;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnOK;
   }
}