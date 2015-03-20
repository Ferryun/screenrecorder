namespace Atf.ScreenRecorder.UI.View {
   partial class frmAboutBox {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
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
         this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
         this.lblProduct = new System.Windows.Forms.Label();
         this.logoPictureBox = new System.Windows.Forms.PictureBox();
         this.lblCopyright = new System.Windows.Forms.Label();
         this.txtDescription = new System.Windows.Forms.TextBox();
         this.btnOK = new System.Windows.Forms.Button();
         this.tableLayoutPanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
         this.SuspendLayout();
         // 
         // tableLayoutPanel
         // 
         this.tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.tableLayoutPanel.ColumnCount = 2;
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel.Controls.Add(this.lblProduct, 0, 1);
         this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
         this.tableLayoutPanel.Controls.Add(this.lblCopyright, 0, 2);
         this.tableLayoutPanel.Controls.Add(this.txtDescription, 0, 3);
         this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel.Name = "tableLayoutPanel";
         this.tableLayoutPanel.RowCount = 4;
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.42857F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel.Size = new System.Drawing.Size(469, 250);
         this.tableLayoutPanel.TabIndex = 2;
         // 
         // lblProduct
         // 
         this.lblProduct.AutoSize = true;
         this.tableLayoutPanel.SetColumnSpan(this.lblProduct, 2);
         this.lblProduct.Location = new System.Drawing.Point(6, 66);
         this.lblProduct.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
         this.lblProduct.Name = "lblProduct";
         this.lblProduct.Size = new System.Drawing.Size(50, 13);
         this.lblProduct.TabIndex = 15;
         this.lblProduct.Text = "[Product]";
         // 
         // logoPictureBox
         // 
         this.logoPictureBox.BackColor = System.Drawing.Color.White;
         this.tableLayoutPanel.SetColumnSpan(this.logoPictureBox, 2);
         this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.logoPictureBox.Image = global::Atf.ScreenRecorder.Properties.Resources.icon_text;
         this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
         this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
         this.logoPictureBox.Name = "logoPictureBox";
         this.logoPictureBox.Size = new System.Drawing.Size(469, 60);
         this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
         this.logoPictureBox.TabIndex = 13;
         this.logoPictureBox.TabStop = false;
         // 
         // lblCopyright
         // 
         this.lblCopyright.AutoSize = true;
         this.tableLayoutPanel.SetColumnSpan(this.lblCopyright, 2);
         this.lblCopyright.Location = new System.Drawing.Point(6, 90);
         this.lblCopyright.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
         this.lblCopyright.Name = "lblCopyright";
         this.lblCopyright.Size = new System.Drawing.Size(57, 13);
         this.lblCopyright.TabIndex = 14;
         this.lblCopyright.Text = "[Copyright]";
         // 
         // txtDescription
         // 
         this.txtDescription.BackColor = System.Drawing.SystemColors.Window;
         this.tableLayoutPanel.SetColumnSpan(this.txtDescription, 2);
         this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
         this.txtDescription.Location = new System.Drawing.Point(6, 117);
         this.txtDescription.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
         this.txtDescription.Multiline = true;
         this.txtDescription.Name = "txtDescription";
         this.txtDescription.ReadOnly = true;
         this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.txtDescription.Size = new System.Drawing.Size(457, 130);
         this.txtDescription.TabIndex = 0;
         // 
         // btnOK
         // 
         this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnOK.Location = new System.Drawing.Point(388, 253);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 26);
         this.btnOK.TabIndex = 0;
         this.btnOK.Text = "OK";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // frmAboutBox
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnOK;
         this.ClientSize = new System.Drawing.Size(469, 287);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.tableLayoutPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmAboutBox";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "frmAboutBox";
         this.Load += new System.EventHandler(this.frmAboutBox_Load);
         this.tableLayoutPanel.ResumeLayout(false);
         this.tableLayoutPanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
      private System.Windows.Forms.PictureBox logoPictureBox;
      private System.Windows.Forms.Label lblCopyright;
      private System.Windows.Forms.Label lblProduct;
      private System.Windows.Forms.TextBox txtDescription;
      private System.Windows.Forms.Button btnOK;
   }
}
