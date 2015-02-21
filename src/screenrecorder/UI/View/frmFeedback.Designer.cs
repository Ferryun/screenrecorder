namespace Atf.ScreenRecorder.UI.View {
   partial class frmFeedback {
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
         this.lblInfo = new System.Windows.Forms.Label();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.txtName = new System.Windows.Forms.TextBox();
         this.chkSendReport = new System.Windows.Forms.CheckBox();
         this.txtMessage = new System.Windows.Forms.TextBox();
         this.txtSubject = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.txtEMail = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnSend = new System.Windows.Forms.Button();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // lblInfo
         // 
         this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.lblInfo.Location = new System.Drawing.Point(12, 9);
         this.lblInfo.Name = "lblInfo";
         this.lblInfo.Size = new System.Drawing.Size(379, 28);
         this.lblInfo.TabIndex = 0;
         this.lblInfo.Text = "[Info]";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.0101F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.9899F));
         this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.chkSendReport, 1, 4);
         this.tableLayoutPanel1.Controls.Add(this.txtMessage, 1, 3);
         this.tableLayoutPanel1.Controls.Add(this.txtSubject, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.txtEMail, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
         this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 40);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 5;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(376, 215);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(3, 6);
         this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(81, 13);
         this.label2.TabIndex = 0;
         this.label2.Text = "Name (optional)";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(3, 31);
         this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(82, 13);
         this.label3.TabIndex = 1;
         this.label3.Text = "E-Mail (optional)";
         // 
         // txtName
         // 
         this.txtName.Location = new System.Drawing.Point(100, 3);
         this.txtName.MaxLength = 64;
         this.txtName.Name = "txtName";
         this.txtName.Size = new System.Drawing.Size(216, 20);
         this.txtName.TabIndex = 2;
         // 
         // chkSendReport
         // 
         this.chkSendReport.AutoSize = true;
         this.chkSendReport.Location = new System.Drawing.Point(100, 192);
         this.chkSendReport.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.chkSendReport.Name = "chkSendReport";
         this.chkSendReport.Size = new System.Drawing.Size(105, 17);
         this.chkSendReport.TabIndex = 1;
         this.chkSendReport.Text = "Send error report";
         this.chkSendReport.UseVisualStyleBackColor = true;
         // 
         // txtMessage
         // 
         this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
         this.txtMessage.Location = new System.Drawing.Point(100, 78);
         this.txtMessage.MaxLength = 2048;
         this.txtMessage.Multiline = true;
         this.txtMessage.Name = "txtMessage";
         this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.txtMessage.Size = new System.Drawing.Size(273, 105);
         this.txtMessage.TabIndex = 0;
         // 
         // txtSubject
         // 
         this.txtSubject.Dock = System.Windows.Forms.DockStyle.Fill;
         this.txtSubject.Location = new System.Drawing.Point(100, 53);
         this.txtSubject.MaxLength = 128;
         this.txtSubject.Name = "txtSubject";
         this.txtSubject.Size = new System.Drawing.Size(273, 20);
         this.txtSubject.TabIndex = 4;
         this.txtSubject.Text = "Feedback";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(3, 56);
         this.label5.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(43, 13);
         this.label5.TabIndex = 7;
         this.label5.Text = "Subject";
         // 
         // txtEMail
         // 
         this.txtEMail.Location = new System.Drawing.Point(100, 28);
         this.txtEMail.MaxLength = 64;
         this.txtEMail.Name = "txtEMail";
         this.txtEMail.Size = new System.Drawing.Size(216, 20);
         this.txtEMail.TabIndex = 3;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(3, 81);
         this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(50, 13);
         this.label4.TabIndex = 2;
         this.label4.Text = "Message";
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(316, 265);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 26);
         this.btnCancel.TabIndex = 2;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         // 
         // btnSend
         // 
         this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnSend.Location = new System.Drawing.Point(235, 265);
         this.btnSend.Name = "btnSend";
         this.btnSend.Size = new System.Drawing.Size(75, 26);
         this.btnSend.TabIndex = 1;
         this.btnSend.Text = "Send";
         this.btnSend.UseVisualStyleBackColor = true;
         this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
         // 
         // frmFeedback
         // 
         this.AcceptButton = this.btnSend;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(403, 300);
         this.Controls.Add(this.btnSend);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.tableLayoutPanel1);
         this.Controls.Add(this.lblInfo);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmFeedback";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Send your feedback";
         this.Load += new System.EventHandler(this.frmFeedback_Load);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label lblInfo;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox txtName;
      private System.Windows.Forms.CheckBox chkSendReport;
      private System.Windows.Forms.TextBox txtMessage;
      private System.Windows.Forms.TextBox txtSubject;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnSend;
      private System.Windows.Forms.TextBox txtEMail;
   }
}