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
         this.chkHideFromTaskbar = new System.Windows.Forms.CheckBox();
         this.chkMinimizeOnRecord = new System.Windows.Forms.CheckBox();
         this.chkRecordCursor = new System.Windows.Forms.CheckBox();
         this.pnlHighlightCursor = new System.Windows.Forms.Panel();
         this.chkHighlighCursor = new System.Windows.Forms.CheckBox();
         this.btnHighlight = new Atf.UI.DropDownButton();
         this.chdHighlight = new Atf.UI.ControlHostDropDown();
         this.txtWatermark = new System.Windows.Forms.TextBox();
         this.grpVideo = new System.Windows.Forms.GroupBox();
         this.lblQuality = new System.Windows.Forms.Label();
         this.cmbFps = new System.Windows.Forms.ComboBox();
         this.label13 = new System.Windows.Forms.Label();
         this.label15 = new System.Windows.Forms.Label();
         this.label16 = new System.Windows.Forms.Label();
         this.tbQuality = new System.Windows.Forms.TrackBar();
         this.cmbCompressor = new System.Windows.Forms.ComboBox();
         this.btnAbout = new System.Windows.Forms.Button();
         this.btnConfigure = new System.Windows.Forms.Button();
         this.grpHotKeys = new System.Windows.Forms.GroupBox();
         this.hkCancel = new Atf.UI.HotKeyInput();
         this.label10 = new System.Windows.Forms.Label();
         this.hkStop = new Atf.UI.HotKeyInput();
         this.hkPause = new Atf.UI.HotKeyInput();
         this.hkRecord = new Atf.UI.HotKeyInput();
         this.label9 = new System.Windows.Forms.Label();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.chkGlobalHotKeys = new System.Windows.Forms.CheckBox();
         this.grpAudio = new System.Windows.Forms.GroupBox();
         this.label6 = new System.Windows.Forms.Label();
         this.cmbSoundFormatTag = new System.Windows.Forms.ComboBox();
         this.label4 = new System.Windows.Forms.Label();
         this.soundDeviceSelector = new Atf.ScreenRecorder.UI.View.SoundDeviceSelector();
         this.cmbSoundFormat = new System.Windows.Forms.ComboBox();
         this.label17 = new System.Windows.Forms.Label();
         this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
         this.grpVisualElements = new System.Windows.Forms.GroupBox();
         this.pnlWatermark = new System.Windows.Forms.Panel();
         this.btnWatermark = new Atf.UI.DropDownButton();
         this.chdWatermark = new Atf.UI.ControlHostDropDown();
         this.chkWatermark = new System.Windows.Forms.CheckBox();
         this.grpGeneral.SuspendLayout();
         this.pnlHighlightCursor.SuspendLayout();
         this.grpVideo.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).BeginInit();
         this.grpHotKeys.SuspendLayout();
         this.grpAudio.SuspendLayout();
         this.grpVisualElements.SuspendLayout();
         this.pnlWatermark.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(335, 557);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 26);
         this.btnCancel.TabIndex = 6;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
         // 
         // btnOK
         // 
         this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOK.Location = new System.Drawing.Point(254, 557);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 26);
         this.btnOK.TabIndex = 5;
         this.btnOK.Text = "OK";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 20);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(91, 13);
         this.label1.TabIndex = 19;
         this.label1.Text = "Output directory:";
         // 
         // txtOutputDirectory
         // 
         this.txtOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtOutputDirectory.Location = new System.Drawing.Point(103, 17);
         this.txtOutputDirectory.Name = "txtOutputDirectory";
         this.txtOutputDirectory.ReadOnly = true;
         this.txtOutputDirectory.Size = new System.Drawing.Size(210, 20);
         this.txtOutputDirectory.TabIndex = 0;
         // 
         // btnBrowse
         // 
         this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnBrowse.Location = new System.Drawing.Point(321, 13);
         this.btnBrowse.Name = "btnBrowse";
         this.btnBrowse.Size = new System.Drawing.Size(69, 26);
         this.btnBrowse.TabIndex = 1;
         this.btnBrowse.Text = "&Browse...";
         this.btnBrowse.UseVisualStyleBackColor = true;
         this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
         // 
         // grpGeneral
         // 
         this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpGeneral.Controls.Add(this.chkHideFromTaskbar);
         this.grpGeneral.Controls.Add(this.chkMinimizeOnRecord);
         this.grpGeneral.Controls.Add(this.label1);
         this.grpGeneral.Controls.Add(this.btnBrowse);
         this.grpGeneral.Controls.Add(this.txtOutputDirectory);
         this.grpGeneral.Location = new System.Drawing.Point(10, 3);
         this.grpGeneral.Name = "grpGeneral";
         this.grpGeneral.Size = new System.Drawing.Size(401, 88);
         this.grpGeneral.TabIndex = 0;
         this.grpGeneral.TabStop = false;
         this.grpGeneral.Text = "General";
         // 
         // chkHideFromTaskbar
         // 
         this.chkHideFromTaskbar.AutoSize = true;
         this.chkHideFromTaskbar.Location = new System.Drawing.Point(10, 66);
         this.chkHideFromTaskbar.Name = "chkHideFromTaskbar";
         this.chkHideFromTaskbar.Size = new System.Drawing.Size(111, 17);
         this.chkHideFromTaskbar.TabIndex = 3;
         this.chkHideFromTaskbar.Text = "Hide from taskbar";
         this.chkHideFromTaskbar.UseVisualStyleBackColor = true;
         // 
         // chkMinimizeOnRecord
         // 
         this.chkMinimizeOnRecord.AutoSize = true;
         this.chkMinimizeOnRecord.Location = new System.Drawing.Point(10, 43);
         this.chkMinimizeOnRecord.Name = "chkMinimizeOnRecord";
         this.chkMinimizeOnRecord.Size = new System.Drawing.Size(173, 17);
         this.chkMinimizeOnRecord.TabIndex = 2;
         this.chkMinimizeOnRecord.Text = "Minimize when recording starts";
         this.chkMinimizeOnRecord.UseVisualStyleBackColor = true;
         // 
         // chkRecordCursor
         // 
         this.chkRecordCursor.AutoSize = true;
         this.chkRecordCursor.Location = new System.Drawing.Point(10, 22);
         this.chkRecordCursor.Name = "chkRecordCursor";
         this.chkRecordCursor.Size = new System.Drawing.Size(90, 17);
         this.chkRecordCursor.TabIndex = 0;
         this.chkRecordCursor.Text = "Mouse cursor";
         this.chkRecordCursor.UseVisualStyleBackColor = true;
         this.chkRecordCursor.CheckedChanged += new System.EventHandler(this.chkRecordCursor_CheckedChanged);
         // 
         // pnlHighlightCursor
         // 
         this.pnlHighlightCursor.AutoSize = true;
         this.pnlHighlightCursor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.pnlHighlightCursor.Controls.Add(this.chkHighlighCursor);
         this.pnlHighlightCursor.Controls.Add(this.btnHighlight);
         this.pnlHighlightCursor.Enabled = false;
         this.pnlHighlightCursor.Location = new System.Drawing.Point(10, 45);
         this.pnlHighlightCursor.Name = "pnlHighlightCursor";
         this.pnlHighlightCursor.Size = new System.Drawing.Size(187, 26);
         this.pnlHighlightCursor.TabIndex = 1;
         // 
         // chkHighlighCursor
         // 
         this.chkHighlighCursor.AutoSize = true;
         this.chkHighlighCursor.Location = new System.Drawing.Point(0, 0);
         this.chkHighlighCursor.Name = "chkHighlighCursor";
         this.chkHighlighCursor.Size = new System.Drawing.Size(133, 17);
         this.chkHighlighCursor.TabIndex = 0;
         this.chkHighlighCursor.Text = "Mouse cursor highlight";
         this.chkHighlighCursor.UseVisualStyleBackColor = true;
         this.chkHighlighCursor.CheckedChanged += new System.EventHandler(this.chkHighlighCursor_CheckedChanged);
         // 
         // btnHighlight
         // 
         this.btnHighlight.Enabled = false;
         this.btnHighlight.Location = new System.Drawing.Point(139, 0);
         this.btnHighlight.Menu = this.chdHighlight;
         this.btnHighlight.Name = "btnHighlight";
         this.btnHighlight.Padding = new System.Windows.Forms.Padding(2);
         this.btnHighlight.Size = new System.Drawing.Size(45, 23);
         this.btnHighlight.Style = Atf.UI.DropDownButtonStyle.Button;
         this.btnHighlight.TabIndex = 1;
         this.btnHighlight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // chdHighlight
         // 
         this.chdHighlight.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
         this.chdHighlight.Name = "chdHighlight";
         this.chdHighlight.Padding = new System.Windows.Forms.Padding(0);
         this.chdHighlight.Size = new System.Drawing.Size(0, 0);
         this.chdHighlight.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.chdHighlight_Closed);
         // 
         // txtWatermark
         // 
         this.txtWatermark.AcceptsReturn = true;
         this.txtWatermark.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.txtWatermark.Location = new System.Drawing.Point(0, 0);
         this.txtWatermark.Multiline = true;
         this.txtWatermark.Name = "txtWatermark";
         this.txtWatermark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.txtWatermark.Size = new System.Drawing.Size(287, 48);
         this.txtWatermark.TabIndex = 0;
         this.txtWatermark.Text = "A\r\nB";
         this.txtWatermark.TextChanged += new System.EventHandler(this.txtWatermark_TextChanged);
         // 
         // grpVideo
         // 
         this.grpVideo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpVideo.Controls.Add(this.lblQuality);
         this.grpVideo.Controls.Add(this.cmbFps);
         this.grpVideo.Controls.Add(this.label13);
         this.grpVideo.Controls.Add(this.label15);
         this.grpVideo.Controls.Add(this.label16);
         this.grpVideo.Controls.Add(this.tbQuality);
         this.grpVideo.Controls.Add(this.cmbCompressor);
         this.grpVideo.Controls.Add(this.btnAbout);
         this.grpVideo.Controls.Add(this.btnConfigure);
         this.grpVideo.Location = new System.Drawing.Point(10, 442);
         this.grpVideo.Name = "grpVideo";
         this.grpVideo.Size = new System.Drawing.Size(401, 109);
         this.grpVideo.TabIndex = 4;
         this.grpVideo.TabStop = false;
         this.grpVideo.Text = "Video";
         // 
         // lblQuality
         // 
         this.lblQuality.AutoSize = true;
         this.lblQuality.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblQuality.Location = new System.Drawing.Point(239, 81);
         this.lblQuality.Name = "lblQuality";
         this.lblQuality.Size = new System.Drawing.Size(29, 12);
         this.lblQuality.TabIndex = 37;
         this.lblQuality.Text = "label4";
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
         this.label13.Location = new System.Drawing.Point(7, 53);
         this.label13.Name = "label13";
         this.label13.Size = new System.Drawing.Size(64, 13);
         this.label13.TabIndex = 36;
         this.label13.Text = "Frame rate:";
         // 
         // label15
         // 
         this.label15.AutoSize = true;
         this.label15.Location = new System.Drawing.Point(7, 22);
         this.label15.Name = "label15";
         this.label15.Size = new System.Drawing.Size(68, 13);
         this.label15.TabIndex = 34;
         this.label15.Text = "Compressor:";
         // 
         // label16
         // 
         this.label16.AutoSize = true;
         this.label16.Location = new System.Drawing.Point(7, 84);
         this.label16.Name = "label16";
         this.label16.Size = new System.Drawing.Size(45, 13);
         this.label16.TabIndex = 33;
         this.label16.Text = "Quality:";
         // 
         // tbQuality
         // 
         this.tbQuality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.tbQuality.AutoSize = false;
         this.tbQuality.LargeChange = 10;
         this.tbQuality.Location = new System.Drawing.Point(78, 81);
         this.tbQuality.Maximum = 100;
         this.tbQuality.Name = "tbQuality";
         this.tbQuality.Size = new System.Drawing.Size(127, 16);
         this.tbQuality.SmallChange = 5;
         this.tbQuality.TabIndex = 2;
         this.tbQuality.TickFrequency = 5;
         this.tbQuality.Scroll += new System.EventHandler(this.tbQuality_Scroll);
         // 
         // cmbCompressor
         // 
         this.cmbCompressor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.cmbCompressor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbCompressor.DropDownWidth = 150;
         this.cmbCompressor.FormattingEnabled = true;
         this.cmbCompressor.Location = new System.Drawing.Point(78, 18);
         this.cmbCompressor.Name = "cmbCompressor";
         this.cmbCompressor.Size = new System.Drawing.Size(235, 21);
         this.cmbCompressor.TabIndex = 0;
         this.cmbCompressor.SelectedIndexChanged += new System.EventHandler(this.cmbCompressor_SelectedIndexChanged);
         // 
         // btnAbout
         // 
         this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnAbout.Location = new System.Drawing.Point(321, 18);
         this.btnAbout.Name = "btnAbout";
         this.btnAbout.Size = new System.Drawing.Size(69, 26);
         this.btnAbout.TabIndex = 3;
         this.btnAbout.Text = "&About";
         this.btnAbout.UseVisualStyleBackColor = true;
         this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
         // 
         // btnConfigure
         // 
         this.btnConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnConfigure.Location = new System.Drawing.Point(321, 50);
         this.btnConfigure.Name = "btnConfigure";
         this.btnConfigure.Size = new System.Drawing.Size(69, 26);
         this.btnConfigure.TabIndex = 4;
         this.btnConfigure.Text = "&Configure";
         this.btnConfigure.UseVisualStyleBackColor = true;
         this.btnConfigure.Click += new System.EventHandler(this.btnConfigure_Click);
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
         this.grpHotKeys.Location = new System.Drawing.Point(9, 251);
         this.grpHotKeys.Name = "grpHotKeys";
         this.grpHotKeys.Size = new System.Drawing.Size(401, 97);
         this.grpHotKeys.TabIndex = 2;
         this.grpHotKeys.TabStop = false;
         this.grpHotKeys.Text = "Hot Keys";
         // 
         // hkCancel
         // 
         this.hkCancel.Location = new System.Drawing.Point(220, 46);
         this.hkCancel.Name = "hkCancel";
         this.hkCancel.Size = new System.Drawing.Size(93, 20);
         this.hkCancel.TabIndex = 3;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(171, 49);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(43, 13);
         this.label10.TabIndex = 30;
         this.label10.Text = "Cancel:";
         // 
         // hkStop
         // 
         this.hkStop.Location = new System.Drawing.Point(58, 46);
         this.hkStop.Name = "hkStop";
         this.hkStop.Size = new System.Drawing.Size(93, 20);
         this.hkStop.TabIndex = 2;
         // 
         // hkPause
         // 
         this.hkPause.Location = new System.Drawing.Point(220, 17);
         this.hkPause.Name = "hkPause";
         this.hkPause.Size = new System.Drawing.Size(93, 20);
         this.hkPause.TabIndex = 1;
         // 
         // hkRecord
         // 
         this.hkRecord.Location = new System.Drawing.Point(58, 17);
         this.hkRecord.Name = "hkRecord";
         this.hkRecord.Size = new System.Drawing.Size(93, 20);
         this.hkRecord.TabIndex = 0;
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(7, 49);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(33, 13);
         this.label9.TabIndex = 26;
         this.label9.Text = "Stop:";
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(171, 20);
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
         this.chkGlobalHotKeys.Size = new System.Drawing.Size(125, 17);
         this.chkGlobalHotKeys.TabIndex = 4;
         this.chkGlobalHotKeys.Text = "Register to Windows";
         this.chkGlobalHotKeys.UseVisualStyleBackColor = true;
         // 
         // grpAudio
         // 
         this.grpAudio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpAudio.Controls.Add(this.label6);
         this.grpAudio.Controls.Add(this.cmbSoundFormatTag);
         this.grpAudio.Controls.Add(this.label4);
         this.grpAudio.Controls.Add(this.soundDeviceSelector);
         this.grpAudio.Controls.Add(this.cmbSoundFormat);
         this.grpAudio.Controls.Add(this.label17);
         this.grpAudio.Location = new System.Drawing.Point(10, 354);
         this.grpAudio.Name = "grpAudio";
         this.grpAudio.Size = new System.Drawing.Size(401, 82);
         this.grpAudio.TabIndex = 3;
         this.grpAudio.TabStop = false;
         this.grpAudio.Text = "Audio";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(7, 52);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(45, 13);
         this.label6.TabIndex = 46;
         this.label6.Text = "Format:";
         // 
         // cmbSoundFormatTag
         // 
         this.cmbSoundFormatTag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbSoundFormatTag.DropDownWidth = 128;
         this.cmbSoundFormatTag.Enabled = false;
         this.cmbSoundFormatTag.FormattingEnabled = true;
         this.cmbSoundFormatTag.Location = new System.Drawing.Point(321, 49);
         this.cmbSoundFormatTag.Name = "cmbSoundFormatTag";
         this.cmbSoundFormatTag.Size = new System.Drawing.Size(69, 21);
         this.cmbSoundFormatTag.TabIndex = 1;
         this.cmbSoundFormatTag.Visible = false;
         this.cmbSoundFormatTag.SelectedIndexChanged += new System.EventHandler(this.cmdSoundFormatTag_SelectedIndexChanged);
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(7, 22);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(44, 13);
         this.label4.TabIndex = 44;
         this.label4.Text = "Source:";
         // 
         // soundDeviceSelector
         // 
         this.soundDeviceSelector.DisplayProperty = Atf.ScreenRecorder.UI.View.SoundDeviceDisplayProperty.Name;
         this.soundDeviceSelector.DisplayTrackingName = true;
         this.soundDeviceSelector.Location = new System.Drawing.Point(57, 16);
         this.soundDeviceSelector.Name = "soundDeviceSelector";
         this.soundDeviceSelector.Size = new System.Drawing.Size(256, 23);
         this.soundDeviceSelector.TabIndex = 0;
         this.soundDeviceSelector.SoundDeviceChanged += new System.EventHandler(this.soundDeviceSelector_SoundDeviceChanged);
         // 
         // cmbSoundFormat
         // 
         this.cmbSoundFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.cmbSoundFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cmbSoundFormat.DropDownWidth = 231;
         this.cmbSoundFormat.Enabled = false;
         this.cmbSoundFormat.FormattingEnabled = true;
         this.cmbSoundFormat.Location = new System.Drawing.Point(58, 49);
         this.cmbSoundFormat.Name = "cmbSoundFormat";
         this.cmbSoundFormat.Size = new System.Drawing.Size(255, 21);
         this.cmbSoundFormat.TabIndex = 2;
         this.cmbSoundFormat.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.cmbSoundFormat_Format);
         // 
         // label17
         // 
         this.label17.AutoSize = true;
         this.label17.Location = new System.Drawing.Point(7, 80);
         this.label17.Name = "label17";
         this.label17.Size = new System.Drawing.Size(45, 13);
         this.label17.TabIndex = 2;
         this.label17.Text = "Quality:";
         // 
         // folderBrowserDialog
         // 
         this.folderBrowserDialog.Description = "Select a directory for saving output files.";
         // 
         // grpVisualElements
         // 
         this.grpVisualElements.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpVisualElements.Controls.Add(this.pnlWatermark);
         this.grpVisualElements.Controls.Add(this.chkWatermark);
         this.grpVisualElements.Controls.Add(this.pnlHighlightCursor);
         this.grpVisualElements.Controls.Add(this.chkRecordCursor);
         this.grpVisualElements.Location = new System.Drawing.Point(10, 97);
         this.grpVisualElements.Name = "grpVisualElements";
         this.grpVisualElements.Size = new System.Drawing.Size(401, 148);
         this.grpVisualElements.TabIndex = 1;
         this.grpVisualElements.TabStop = false;
         this.grpVisualElements.Text = "Visual Elements";
         // 
         // pnlWatermark
         // 
         this.pnlWatermark.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.pnlWatermark.Controls.Add(this.txtWatermark);
         this.pnlWatermark.Controls.Add(this.btnWatermark);
         this.pnlWatermark.Enabled = false;
         this.pnlWatermark.Location = new System.Drawing.Point(26, 91);
         this.pnlWatermark.Name = "pnlWatermark";
         this.pnlWatermark.Size = new System.Drawing.Size(364, 51);
         this.pnlWatermark.TabIndex = 3;
         // 
         // btnWatermark
         // 
         this.btnWatermark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnWatermark.Location = new System.Drawing.Point(295, 0);
         this.btnWatermark.Menu = this.chdWatermark;
         this.btnWatermark.Name = "btnWatermark";
         this.btnWatermark.Padding = new System.Windows.Forms.Padding(2);
         this.btnWatermark.Size = new System.Drawing.Size(69, 23);
         this.btnWatermark.Style = Atf.UI.DropDownButtonStyle.Button;
         this.btnWatermark.TabIndex = 1;
         this.btnWatermark.Text = "Style";
         this.btnWatermark.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // chdWatermark
         // 
         this.chdWatermark.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
         this.chdWatermark.Name = "controlHostDropDown1";
         this.chdWatermark.Size = new System.Drawing.Size(2, 4);
         // 
         // chkWatermark
         // 
         this.chkWatermark.AutoSize = true;
         this.chkWatermark.Location = new System.Drawing.Point(10, 68);
         this.chkWatermark.Name = "chkWatermark";
         this.chkWatermark.Size = new System.Drawing.Size(79, 17);
         this.chkWatermark.TabIndex = 2;
         this.chkWatermark.Text = "Watermark";
         this.chkWatermark.UseVisualStyleBackColor = true;
         this.chkWatermark.CheckedChanged += new System.EventHandler(this.chkWatermark_CheckedChanged);
         // 
         // frmOptions
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(419, 592);
         this.Controls.Add(this.grpGeneral);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.grpVisualElements);
         this.Controls.Add(this.grpHotKeys);
         this.Controls.Add(this.grpAudio);
         this.Controls.Add(this.grpVideo);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmOptions";
         this.Padding = new System.Windows.Forms.Padding(6);
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Options";
         this.Load += new System.EventHandler(this.frmOptions_Load);
         this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.frmOptions_HelpButtonClicked);
         this.grpGeneral.ResumeLayout(false);
         this.grpGeneral.PerformLayout();
         this.pnlHighlightCursor.ResumeLayout(false);
         this.pnlHighlightCursor.PerformLayout();
         this.grpVideo.ResumeLayout(false);
         this.grpVideo.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbQuality)).EndInit();
         this.grpHotKeys.ResumeLayout(false);
         this.grpHotKeys.PerformLayout();
         this.grpAudio.ResumeLayout(false);
         this.grpAudio.PerformLayout();
         this.grpVisualElements.ResumeLayout(false);
         this.grpVisualElements.PerformLayout();
         this.pnlWatermark.ResumeLayout(false);
         this.pnlWatermark.PerformLayout();
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
      private System.Windows.Forms.ComboBox cmbFps;
      private System.Windows.Forms.Label label13;
      private System.Windows.Forms.Label label15;
      private System.Windows.Forms.Label label16;
      private System.Windows.Forms.TrackBar tbQuality;
      private System.Windows.Forms.Button btnAbout;
      private System.Windows.Forms.Button btnConfigure;
      private System.Windows.Forms.ComboBox cmbCompressor;
      private System.Windows.Forms.CheckBox chkRecordCursor;
      private System.Windows.Forms.GroupBox grpHotKeys;
      private Atf.UI.HotKeyInput hkCancel;
      private System.Windows.Forms.Label label10;
      private Atf.UI.HotKeyInput hkStop;
      private Atf.UI.HotKeyInput hkPause;
      private Atf.UI.HotKeyInput hkRecord;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.CheckBox chkGlobalHotKeys;
      private System.Windows.Forms.CheckBox chkMinimizeOnRecord;
      private System.Windows.Forms.CheckBox chkHighlighCursor;
      private System.Windows.Forms.CheckBox chkHideFromTaskbar;
      private System.Windows.Forms.GroupBox grpAudio;
      private System.Windows.Forms.Label label17;
      private System.Windows.Forms.ComboBox cmbSoundFormat;
      private System.Windows.Forms.TextBox txtWatermark;
      private System.Windows.Forms.Label lblQuality;
      private Atf.UI.DropDownButton btnHighlight;
      private System.Windows.Forms.Panel pnlHighlightCursor;
      private Atf.UI.ControlHostDropDown chdHighlight;
      private Atf.UI.DropDownButton btnWatermark;
      private Atf.UI.ControlHostDropDown chdWatermark;
      private SoundDeviceSelector soundDeviceSelector;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.ComboBox cmbSoundFormatTag;
      private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
      private System.Windows.Forms.GroupBox grpVisualElements;
      private System.Windows.Forms.CheckBox chkWatermark;
      private System.Windows.Forms.Panel pnlWatermark;

   }
}