namespace Atf.ScreenRecorder.UI.View {
   partial class WatermarkOptions {
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.picPreview = new System.Windows.Forms.PictureBox();
         this.cmbAlignment = new System.Windows.Forms.ComboBox();
         this.panel1 = new System.Windows.Forms.Panel();
         this.label4 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.nudHorMargin = new System.Windows.Forms.NumericUpDown();
         this.nudVerMargin = new System.Windows.Forms.NumericUpDown();
         this.label3 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.cmbRightToLeft = new System.Windows.Forms.ComboBox();
         this.btnColor = new System.Windows.Forms.Button();
         this.btnFont = new System.Windows.Forms.Button();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.panel4 = new System.Windows.Forms.Panel();
         this.lblColor = new System.Windows.Forms.Label();
         this.panel3 = new System.Windows.Forms.Panel();
         this.lblFont = new System.Windows.Forms.Label();
         this.panel2 = new System.Windows.Forms.Panel();
         this.lblOutlineColor = new System.Windows.Forms.Label();
         this.btnOutlineColor = new System.Windows.Forms.Button();
         this.chkOutline = new System.Windows.Forms.CheckBox();
         ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudHorMargin)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudVerMargin)).BeginInit();
         this.tableLayoutPanel1.SuspendLayout();
         this.panel4.SuspendLayout();
         this.panel3.SuspendLayout();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // picPreview
         // 
         this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.picPreview.Location = new System.Drawing.Point(6, 98);
         this.picPreview.Name = "picPreview";
         this.picPreview.Size = new System.Drawing.Size(418, 149);
         this.picPreview.TabIndex = 2;
         this.picPreview.TabStop = false;
         this.picPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.picPreview_Paint);
         // 
         // cmbAlignment
         // 
         this.cmbAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbAlignment.FormattingEnabled = true;
         this.cmbAlignment.Location = new System.Drawing.Point(66, 61);
         this.cmbAlignment.Name = "cmbAlignment";
         this.cmbAlignment.Size = new System.Drawing.Size(138, 21);
         this.cmbAlignment.TabIndex = 2;
         this.cmbAlignment.SelectedIndexChanged += new System.EventHandler(this.cmbAlignment_SelectedIndexChanged);
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.label4);
         this.panel1.Controls.Add(this.label2);
         this.panel1.Controls.Add(this.nudHorMargin);
         this.panel1.Controls.Add(this.nudVerMargin);
         this.panel1.Location = new System.Drawing.Point(273, 3);
         this.panel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(142, 22);
         this.panel1.TabIndex = 3;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(72, 2);
         this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(17, 13);
         this.label4.TabIndex = 36;
         this.label4.Text = "V:";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(2, 3);
         this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(18, 13);
         this.label2.TabIndex = 35;
         this.label2.Text = "H:";
         // 
         // nudHorMargin
         // 
         this.nudHorMargin.Location = new System.Drawing.Point(20, 1);
         this.nudHorMargin.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
         this.nudHorMargin.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
         this.nudHorMargin.Name = "nudHorMargin";
         this.nudHorMargin.Size = new System.Drawing.Size(46, 20);
         this.nudHorMargin.TabIndex = 0;
         this.nudHorMargin.ValueChanged += new System.EventHandler(this.nudHorMargin_ValueChanged);
         this.nudHorMargin.Enter += new System.EventHandler(this.nudMargin_Enter);
         // 
         // nudVerMargin
         // 
         this.nudVerMargin.Location = new System.Drawing.Point(92, 0);
         this.nudVerMargin.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
         this.nudVerMargin.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
         this.nudVerMargin.Name = "nudVerMargin";
         this.nudVerMargin.Size = new System.Drawing.Size(47, 20);
         this.nudVerMargin.TabIndex = 1;
         this.nudVerMargin.ValueChanged += new System.EventHandler(this.nudVerMargin_ValueChanged);
         this.nudVerMargin.Enter += new System.EventHandler(this.nudMargin_Enter);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(213, 35);
         this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(52, 13);
         this.label3.TabIndex = 23;
         this.label3.Text = "Direction:";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(213, 6);
         this.label9.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(42, 13);
         this.label9.TabIndex = 28;
         this.label9.Text = "Margin:";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(3, 64);
         this.label5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(56, 13);
         this.label5.TabIndex = 26;
         this.label5.Text = "Alignment:";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(3, 35);
         this.label7.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(34, 13);
         this.label7.TabIndex = 24;
         this.label7.Text = "Color:";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 6);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(31, 13);
         this.label1.TabIndex = 21;
         this.label1.Text = "Font:";
         // 
         // cmbRightToLeft
         // 
         this.cmbRightToLeft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbRightToLeft.FormattingEnabled = true;
         this.cmbRightToLeft.Location = new System.Drawing.Point(276, 32);
         this.cmbRightToLeft.Name = "cmbRightToLeft";
         this.cmbRightToLeft.Size = new System.Drawing.Size(89, 21);
         this.cmbRightToLeft.TabIndex = 4;
         this.cmbRightToLeft.SelectedIndexChanged += new System.EventHandler(this.cmbRightToLeft_SelectedIndexChanged);
         // 
         // btnColor
         // 
         this.btnColor.AutoSize = true;
         this.btnColor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.btnColor.Location = new System.Drawing.Point(112, 0);
         this.btnColor.Name = "btnColor";
         this.btnColor.Size = new System.Drawing.Size(26, 23);
         this.btnColor.TabIndex = 23;
         this.btnColor.Text = "...";
         this.btnColor.UseVisualStyleBackColor = true;
         this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
         // 
         // btnFont
         // 
         this.btnFont.AutoSize = true;
         this.btnFont.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.btnFont.Location = new System.Drawing.Point(112, 0);
         this.btnFont.Name = "btnFont";
         this.btnFont.Size = new System.Drawing.Size(26, 23);
         this.btnFont.TabIndex = 0;
         this.btnFont.Text = "...";
         this.btnFont.UseVisualStyleBackColor = true;
         this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 4;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
         this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.label9, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
         this.tableLayoutPanel1.Controls.Add(this.panel1, 3, 0);
         this.tableLayoutPanel1.Controls.Add(this.cmbAlignment, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 2);
         this.tableLayoutPanel1.Controls.Add(this.cmbRightToLeft, 3, 1);
         this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 3);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(421, 89);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // panel4
         // 
         this.panel4.Controls.Add(this.lblColor);
         this.panel4.Controls.Add(this.btnColor);
         this.panel4.Location = new System.Drawing.Point(66, 32);
         this.panel4.Name = "panel4";
         this.panel4.Size = new System.Drawing.Size(141, 23);
         this.panel4.TabIndex = 1;
         // 
         // lblColor
         // 
         this.lblColor.AutoEllipsis = true;
         this.lblColor.Location = new System.Drawing.Point(3, 3);
         this.lblColor.Name = "lblColor";
         this.lblColor.Size = new System.Drawing.Size(105, 18);
         this.lblColor.TabIndex = 0;
         this.lblColor.Text = "(Color)";
         // 
         // panel3
         // 
         this.panel3.Controls.Add(this.lblFont);
         this.panel3.Controls.Add(this.btnFont);
         this.panel3.Location = new System.Drawing.Point(66, 3);
         this.panel3.Name = "panel3";
         this.panel3.Size = new System.Drawing.Size(141, 23);
         this.panel3.TabIndex = 0;
         // 
         // lblFont
         // 
         this.lblFont.AutoEllipsis = true;
         this.lblFont.Location = new System.Drawing.Point(3, 3);
         this.lblFont.Name = "lblFont";
         this.lblFont.Size = new System.Drawing.Size(103, 18);
         this.lblFont.TabIndex = 1;
         this.lblFont.Text = "(Font)";
         // 
         // panel2
         // 
         this.tableLayoutPanel1.SetColumnSpan(this.panel2, 2);
         this.panel2.Controls.Add(this.lblOutlineColor);
         this.panel2.Controls.Add(this.btnOutlineColor);
         this.panel2.Controls.Add(this.chkOutline);
         this.panel2.Location = new System.Drawing.Point(213, 61);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(205, 25);
         this.panel2.TabIndex = 35;
         // 
         // lblOutlineColor
         // 
         this.lblOutlineColor.AutoEllipsis = true;
         this.lblOutlineColor.Location = new System.Drawing.Point(65, 6);
         this.lblOutlineColor.Name = "lblOutlineColor";
         this.lblOutlineColor.Size = new System.Drawing.Size(105, 16);
         this.lblOutlineColor.TabIndex = 24;
         this.lblOutlineColor.Text = "(Color)";
         // 
         // btnOutlineColor
         // 
         this.btnOutlineColor.AutoSize = true;
         this.btnOutlineColor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.btnOutlineColor.Location = new System.Drawing.Point(176, 1);
         this.btnOutlineColor.Name = "btnOutlineColor";
         this.btnOutlineColor.Size = new System.Drawing.Size(26, 23);
         this.btnOutlineColor.TabIndex = 2;
         this.btnOutlineColor.Text = "...";
         this.btnOutlineColor.UseVisualStyleBackColor = true;
         this.btnOutlineColor.Click += new System.EventHandler(this.btnOutlineColor_Click);
         // 
         // chkOutline
         // 
         this.chkOutline.AutoSize = true;
         this.chkOutline.Dock = System.Windows.Forms.DockStyle.Left;
         this.chkOutline.Location = new System.Drawing.Point(0, 0);
         this.chkOutline.Name = "chkOutline";
         this.chkOutline.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
         this.chkOutline.Size = new System.Drawing.Size(62, 25);
         this.chkOutline.TabIndex = 0;
         this.chkOutline.Text = "Outline";
         this.chkOutline.UseVisualStyleBackColor = true;
         this.chkOutline.CheckedChanged += new System.EventHandler(this.chkOutline_CheckedChanged);
         // 
         // WatermarkOptions
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.Controls.Add(this.picPreview);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "WatermarkOptions";
         this.Size = new System.Drawing.Size(430, 254);
         ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nudHorMargin)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nudVerMargin)).EndInit();
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.panel4.ResumeLayout(false);
         this.panel4.PerformLayout();
         this.panel3.ResumeLayout(false);
         this.panel3.PerformLayout();
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.PictureBox picPreview;
      private System.Windows.Forms.ComboBox cmbAlignment;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.NumericUpDown nudHorMargin;
      private System.Windows.Forms.NumericUpDown nudVerMargin;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox cmbRightToLeft;
      private System.Windows.Forms.Button btnColor;
      private System.Windows.Forms.Button btnFont;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.CheckBox chkOutline;
      private System.Windows.Forms.Button btnOutlineColor;
      private System.Windows.Forms.Panel panel3;
      private System.Windows.Forms.Panel panel4;
      private System.Windows.Forms.Label lblColor;
      private System.Windows.Forms.Label lblFont;
      private System.Windows.Forms.Label lblOutlineColor;

   }
}
