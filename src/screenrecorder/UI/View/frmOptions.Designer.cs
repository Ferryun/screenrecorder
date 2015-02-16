namespace Atf.ScreenRecorder.UI.View {
   partial class frmOptions {
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
         this.btnCancel = new System.Windows.Forms.Button();
         this.btnOK = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.txtOutputDirectory = new System.Windows.Forms.TextBox();
         this.btnBrowse = new System.Windows.Forms.Button();
         this.grpGeneral = new System.Windows.Forms.GroupBox();
         this.chkMinimizeOnRecord = new System.Windows.Forms.CheckBox();
         this.chkRecordCursor = new System.Windows.Forms.CheckBox();
         this.grpVideo = new System.Windows.Forms.GroupBox();
         this.label4 = new System.Windows.Forms.Label();
         this.cmbFps = new System.Windows.Forms.ComboBox();
         this.label13 = new System.Windows.Forms.Label();
         this.lblQuality = new System.Windows.Forms.Label();
         this.label15 = new System.Windows.Forms.Label();
         this.label16 = new System.Windows.Forms.Label();
         this.tbQuality = new System.Windows.Forms.TrackBar();
         this.btnAbout = new System.Windows.Forms.Button();
         this.btnConfigure = new System.Windows.Forms.Button();
         this.cmbCompressor = new System.Windows.Forms.ComboBox();
         this.grpRegion = new System.Windows.Forms.GroupBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label11 = new System.Windows.Forms.Label();
         this.label12 = new System.Windows.Forms.Label();
         this.rdoWindow = new System.Windows.Forms.RadioButton();
         this.rdoFixed = new System.Windows.Forms.RadioButton();
         this.rdoFull = new System.Windows.Forms.RadioButton();
         this.grpHotKeys = new System.Windows.Forms.GroupBox();
         this.label10 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.chkGlobalHotKeys = new System.Windows.Forms.CheckBox();
         this.hkCancel = new Atf.ScreenRecorder.UI.View.HotKeyInput();
         this.hkStop = new Atf.ScreenRecorder.UI.View.HotKeyInput();
         this.hkPause = new Atf.ScreenRecorder.UI.View.HotKeyInput();
         this.hkRecord = new Atf.ScreenRecorder.UI.View.HotKeyInput();
         this.grpGeneral.SuspendLayout();
         this.grpVideo.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
         this.grpRegion.SuspendLayout();
         this.grpHotKeys.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(329, 388);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancel.TabIndex = 5;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnOK
         // 
         this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOK.Location = new System.Drawing.Point(248, 388);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 23);
         this.btnOK.TabIndex = 4;
         this.btnOK.Text = "OK";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 20);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(85, 13);
         this.label1.TabIndex = 19;
         this.label1.Text = "Output directory:";
         // 
         // txtOutputDirectory
         // 
         this.txtOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtOutputDirectory.Location = new System.Drawing.Point(97, 16);
         this.txtOutputDirectory.Name = "txtOutputDirectory";
         this.txtOutputDirectory.ReadOnly = true;
         this.txtOutputDirectory.Size = new System.Drawing.Size(216, 20);
         this.txtOutputDirectory.TabIndex = 0;
         // 
         // btnBrowse
         // 
         this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnBrowse.Location = new System.Drawing.Point(319, 16);
         this.btnBrowse.Name = "btnBrowse";
         this.btnBrowse.Size = new System.Drawing.Size(69, 21);
         this.btnBrowse.TabIndex = 1;
         this.btnBrowse.Text = "Browse...";
         this.btnBrowse.UseVisualStyleBackColor = true;
         this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
         // 
         // grpGeneral
         // 
         this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpGeneral.Controls.Add(this.chkMinimizeOnRecord);
         this.grpGeneral.Controls.Add(this.label1);
         this.grpGeneral.Controls.Add(this.btnBrowse);
         this.grpGeneral.Controls.Add(this.chkRecordCursor);
         this.grpGeneral.Controls.Add(this.txtOutputDirectory);
         this.grpGeneral.Location = new System.Drawing.Point(10, 3);
         this.grpGeneral.Name = "grpGeneral";
         this.grpGeneral.Size = new System.Drawing.Size(395, 87);
         this.grpGeneral.TabIndex = 0;
         this.grpGeneral.TabStop = false;
         this.grpGeneral.Text = "General";
         // 
         // chkMinimizeOnRecord
         // 
         this.chkMinimizeOnRecord.AutoSize = true;
         this.chkMinimizeOnRecord.Location = new System.Drawing.Point(10, 42);
         this.chkMinimizeOnRecord.Name = "chkMinimizeOnRecord";
         this.chkMinimizeOnRecord.Size = new System.Drawing.Size(140, 17);
         this.chkMinimizeOnRecord.TabIndex = 2;
         this.chkMinimizeOnRecord.Text = "Minimize while recording";
         this.chkMinimizeOnRecord.UseVisualStyleBackColor = true;
         // 
         // chkRecordCursor
         // 
         this.chkRecordCursor.AutoSize = true;
         this.chkRecordCursor.Location = new System.Drawing.Point(10, 64);
         this.chkRecordCursor.Name = "chkRecordCursor";
         this.chkRecordCursor.Size = new System.Drawing.Size(127, 17);
         this.chkRecordCursor.TabIndex = 3;
         this.chkRecordCursor.Text = "Record mouse cursor";
         this.chkRecordCursor.UseVisualStyleBackColor = true;
         // 
         // grpVideo
         // 
         this.grpVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpVideo.Controls.Add(this.label4);
         this.grpVideo.Controls.Add(this.cmbFps);
         this.grpVideo.Controls.Add(this.label13);
         this.grpVideo.Controls.Add(this.lblQuality);
         this.grpVideo.Controls.Add(this.label15);
         this.grpVideo.Controls.Add(this.label16);
         this.grpVideo.Controls.Add(this.tbQuality);
         this.grpVideo.Controls.Add(this.btnAbout);
         this.grpVideo.Controls.Add(this.btnConfigure);
         this.grpVideo.Controls.Add(this.cmbCompressor);
         this.grpVideo.Location = new System.Drawing.Point(10, 270);
         this.grpVideo.Name = "grpVideo";
         this.grpVideo.Size = new System.Drawing.Size(395, 112);
         this.grpVideo.TabIndex = 3;
         this.grpVideo.TabStop = false;
         this.grpVideo.Text = "Video";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(131, 55);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(94, 13);
         this.label4.TabIndex = 38;
         this.label4.Text = "frames per second";
         // 
         // cmbFps
         // 
         this.cmbFps.FormattingEnabled = true;
         this.cmbFps.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "15",
            "20",
            "25",
            "30"});
         this.cmbFps.Location = new System.Drawing.Point(78, 50);
         this.cmbFps.MaxLength = 3;
         this.cmbFps.Name = "cmbFps";
         this.cmbFps.Size = new System.Drawing.Size(47, 21);
         this.cmbFps.TabIndex = 1;
         this.cmbFps.TextChanged += new System.EventHandler(this.cmbFps_TextChanged);
         // 
         // label13
         // 
         this.label13.AutoSize = true;
         this.label13.Location = new System.Drawing.Point(7, 55);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(60, 13);
         this.label13.TabIndex = 36;
         this.label13.Text = "Frame rate:";
         // 
         // lblQuality
         // 
         this.lblQuality.AutoSize = true;
         this.lblQuality.Location = new System.Drawing.Point(225, 80);
         this.lblQuality.Name = "lblQuality";
         this.lblQuality.Size = new System.Drawing.Size(45, 13);
         this.lblQuality.TabIndex = 35;
         this.lblQuality.Text = "[Quality]";
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(7, 28);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(65, 13);
         this.label15.TabIndex = 34;
         this.label15.Text = "Compressor:";
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(7, 80);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(42, 13);
         this.label16.TabIndex = 33;
         this.label16.Text = "Quality:";
         // 
         // tbQuality
         // 
         this.tbQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.tbQuality.AutoSize = false;
         this.tbQuality.LargeChange = 10;
         this.tbQuality.Location = new System.Drawing.Point(78, 80);
         this.tbQuality.Maximum = 100;
         this.tbQuality.Name = "tbQuality";
         this.tbQuality.Size = new System.Drawing.Size(148, 16);
         this.tbQuality.SmallChange = 5;
         this.tbQuality.TabIndex = 2;
         this.tbQuality.TickFrequency = 5;
         this.tbQuality.ValueChanged += new System.EventHandler(this.tbQuality_ValueChanged);
         // 
         // btnAbout
         // 
         this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnAbout.Location = new System.Drawing.Point(313, 52);
         this.btnAbout.Name = "btnAbout";
         this.btnAbout.Size = new System.Drawing.Size(75, 23);
         this.btnAbout.TabIndex = 4;
         this.btnAbout.Text = "About";
         this.btnAbout.UseVisualStyleBackColor = true;
         this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
         // 
         // btnConfigure
         // 
         this.btnConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnConfigure.Location = new System.Drawing.Point(313, 23);
         this.btnConfigure.Name = "btnConfigure";
         this.btnConfigure.Size = new System.Drawing.Size(75, 23);
         this.btnConfigure.TabIndex = 3;
         this.btnConfigure.Text = "Configure";
         this.btnConfigure.UseVisualStyleBackColor = true;
         this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
         // 
         // cmbCompressor
         // 
         this.cmbCompressor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.cmbCompressor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbCompressor.FormattingEnabled = true;
         this.cmbCompressor.Location = new System.Drawing.Point(78, 23);
         this.cmbCompressor.Name = "cmbCompressor";
         this.cmbCompressor.Size = new System.Drawing.Size(205, 21);
         this.cmbCompressor.TabIndex = 0;
         this.cmbCompressor.SelectedIndexChanged += new System.EventHandler(this.cmbCompressor_SelectedIndexChanged);
         // 
         // grpRegion
         // 
         this.grpRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpRegion.Controls.Add(this.label6);
         this.grpRegion.Controls.Add(this.label11);
         this.grpRegion.Controls.Add(this.label12);
         this.grpRegion.Controls.Add(this.rdoWindow);
         this.grpRegion.Controls.Add(this.rdoFixed);
         this.grpRegion.Controls.Add(this.rdoFull);
         this.grpRegion.Location = new System.Drawing.Point(10, 201);
         this.grpRegion.Name = "grpRegion";
         this.grpRegion.Size = new System.Drawing.Size(395, 63);
         this.grpRegion.TabIndex = 2;
         this.grpRegion.TabStop = false;
         this.grpRegion.Text = "Capture Region";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(152, 29);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(46, 13);
         this.label6.TabIndex = 24;
         this.label6.Text = "Window";
         // 
         // label11
         // 
         this.label11.AutoSize = true;
         this.label11.Location = new System.Drawing.Point(247, 29);
         this.label11.Name = "label11";
         this.label11.Size = new System.Drawing.Size(32, 13);
         this.label11.TabIndex = 23;
         this.label11.Text = "Fixed";
         // 
         // label12
         // 
         this.label12.AutoSize = true;
         this.label12.Location = new System.Drawing.Point(48, 29);
         this.label12.Name = "label12";
         this.label12.Size = new System.Drawing.Size(60, 13);
         this.label12.TabIndex = 22;
         this.label12.Text = "Full Screen";
         // 
         // rdoWindow
         // 
         this.rdoWindow.Appearance = System.Windows.Forms.Appearance.Button;
         this.rdoWindow.AutoCheck = false;
         this.rdoWindow.Image = global::Atf.ScreenRecorder.Properties.Resources.Window;
         this.rdoWindow.Location = new System.Drawing.Point(114, 19);
         this.rdoWindow.Name = "rdoWindow";
         this.rdoWindow.Size = new System.Drawing.Size(32, 32);
         this.rdoWindow.TabIndex = 1;
         this.rdoWindow.TabStop = true;
         this.rdoWindow.UseVisualStyleBackColor = true;
         this.rdoWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rdoWindow_MouseMove);
         this.rdoWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdoWindow_MouseDown);
         this.rdoWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rdoWindow_MouseUp);
         // 
         // rdoFixed
         // 
         this.rdoFixed.Appearance = System.Windows.Forms.Appearance.Button;
         this.rdoFixed.AutoCheck = false;
         this.rdoFixed.Image = global::Atf.ScreenRecorder.Properties.Resources.Partial;
         this.rdoFixed.Location = new System.Drawing.Point(209, 19);
         this.rdoFixed.Name = "rdoFixed";
         this.rdoFixed.Size = new System.Drawing.Size(32, 32);
         this.rdoFixed.TabIndex = 2;
         this.rdoFixed.TabStop = true;
         this.rdoFixed.UseVisualStyleBackColor = true;
         this.rdoFixed.Click += new System.EventHandler(this.rdoFixed_Click);
         // 
         // rdoFull
         // 
         this.rdoFull.Appearance = System.Windows.Forms.Appearance.Button;
         this.rdoFull.AutoCheck = false;
         this.rdoFull.Image = global::Atf.ScreenRecorder.Properties.Resources.FullScreen;
         this.rdoFull.Location = new System.Drawing.Point(10, 19);
         this.rdoFull.Name = "rdoFull";
         this.rdoFull.Size = new System.Drawing.Size(32, 32);
         this.rdoFull.TabIndex = 0;
         this.rdoFull.TabStop = true;
         this.rdoFull.UseVisualStyleBackColor = true;
         this.rdoFull.Click += new System.EventHandler(this.rdoFull_Click);
         // 
         // grpHotKeys
         // 
         this.grpHotKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpHotKeys.Controls.Add(this.hkCancel);
         this.grpHotKeys.Controls.Add(this.label10);
         this.grpHotKeys.Controls.Add(this.hkStop);
         this.grpHotKeys.Controls.Add(this.hkPause);
         this.grpHotKeys.Controls.Add(this.hkRecord);
         this.grpHotKeys.Controls.Add(this.label9);
         this.grpHotKeys.Controls.Add(this.label8);
         this.grpHotKeys.Controls.Add(this.label7);
         this.grpHotKeys.Controls.Add(this.chkGlobalHotKeys);
         this.grpHotKeys.Location = new System.Drawing.Point(10, 96);
         this.grpHotKeys.Name = "grpHotKeys";
         this.grpHotKeys.Size = new System.Drawing.Size(395, 99);
         this.grpHotKeys.TabIndex = 1;
         this.grpHotKeys.TabStop = false;
         this.grpHotKeys.Text = "Hot Keys";
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(198, 49);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(43, 13);
         this.label10.TabIndex = 30;
         this.label10.Text = "Cancel:";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(7, 49);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(32, 13);
         this.label9.TabIndex = 26;
         this.label9.Text = "Stop:";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(198, 20);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(40, 13);
         this.label8.TabIndex = 25;
         this.label8.Text = "Pause:";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(7, 20);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(45, 13);
         this.label7.TabIndex = 24;
         this.label7.Text = "Record:";
         // 
         // chkGlobalHotKeys
         // 
         this.chkGlobalHotKeys.AutoSize = true;
         this.chkGlobalHotKeys.Location = new System.Drawing.Point(10, 72);
         this.chkGlobalHotKeys.Name = "chkGlobalHotKeys";
         this.chkGlobalHotKeys.Size = new System.Drawing.Size(374, 17);
         this.chkGlobalHotKeys.TabIndex = 23;
         this.chkGlobalHotKeys.Text = "Register hot keys globally to be accessible from anywhere in the Windows\r\n";
         this.chkGlobalHotKeys.UseVisualStyleBackColor = true;
         // 
         // hkCancel
         // 
         this.hkCancel.Location = new System.Drawing.Point(247, 46);
         this.hkCancel.Name = "hkCancel";
         this.hkCancel.Size = new System.Drawing.Size(134, 20);
         this.hkCancel.TabIndex = 3;
         // 
         // hkStop
         // 
         this.hkStop.Location = new System.Drawing.Point(58, 46);
         this.hkStop.Name = "hkStop";
         this.hkStop.Size = new System.Drawing.Size(134, 20);
         this.hkStop.TabIndex = 2;
         // 
         // hkPause
         // 
         this.hkPause.Location = new System.Drawing.Point(247, 17);
         this.hkPause.Name = "hkPause";
         this.hkPause.Size = new System.Drawing.Size(134, 20);
         this.hkPause.TabIndex = 1;
         // 
         // hkRecord
         // 
         this.hkRecord.Location = new System.Drawing.Point(58, 17);
         this.hkRecord.Name = "hkRecord";
         this.hkRecord.Size = new System.Drawing.Size(134, 20);
         this.hkRecord.TabIndex = 0;
         // 
         // frmOptions
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(413, 420);
         this.Controls.Add(this.grpVideo);
         this.Controls.Add(this.grpRegion);
         this.Controls.Add(this.grpHotKeys);
         this.Controls.Add(this.grpGeneral);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmOptions";
         this.Padding = new System.Windows.Forms.Padding(6);
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Options";
         this.Load += new System.EventHandler(this.frmOptions_Load);
         this.grpGeneral.ResumeLayout(false);
         this.grpGeneral.PerformLayout();
         this.grpVideo.ResumeLayout(false);
         this.grpVideo.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
         this.grpRegion.ResumeLayout(false);
         this.grpRegion.PerformLayout();
         this.grpHotKeys.ResumeLayout(false);
         this.grpHotKeys.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Button btnOK;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtOutputDirectory;
      private System.Windows.Forms.Button btnBrowse;
      private System.Windows.Forms.GroupBox grpGeneral;
      private System.Windows.Forms.GroupBox grpVideo;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.ComboBox cmbFps;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label lblQuality;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.TrackBar tbQuality;
      private System.Windows.Forms.Button btnAbout;
      private System.Windows.Forms.Button btnConfigure;
      private System.Windows.Forms.ComboBox cmbCompressor;
      private System.Windows.Forms.GroupBox grpRegion;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label11;
      private System.Windows.Forms.Label label12;
      private System.Windows.Forms.RadioButton rdoWindow;
      private System.Windows.Forms.RadioButton rdoFixed;
      private System.Windows.Forms.RadioButton rdoFull;
      private System.Windows.Forms.CheckBox chkRecordCursor;
      private System.Windows.Forms.GroupBox grpHotKeys;
      private HotKeyInput hkCancel;
      private System.Windows.Forms.Label label10;
      private HotKeyInput hkStop;
      private HotKeyInput hkPause;
      private HotKeyInput hkRecord;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.CheckBox chkGlobalHotKeys;
      private System.Windows.Forms.CheckBox chkMinimizeOnRecord;

   }
}