﻿namespace Atf.ScreenRecorder.UI.View {
    partial class frmMain {
       /// <summary>
       /// Required designer variable.
       /// </summary>
       private System.ComponentModel.IContainer components = null;
       #region Windows Form Designer generated code
       /// <summary>
       /// Required method for Designer support - do not modify
       /// the contents of this method with the code editor.
       /// </summary>
       private void InitializeComponent() {
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
          this.menuStrip = new System.Windows.Forms.MenuStrip();
          this.recordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiRecord = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiPause = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiStop = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiCancel = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiPlay = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
          this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
          this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
          this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiHelpTopics = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
          this.tsmiCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
          this.tsmiSendFeedback = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
          this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
          this.pnlTrackingType = new System.Windows.Forms.Panel();
          this.rdoWindow = new System.Windows.Forms.RadioButton();
          this.rdoPartial = new System.Windows.Forms.RadioButton();
          this.rdoFull = new System.Windows.Forms.RadioButton();
          this.statusStrip = new System.Windows.Forms.StatusStrip();
          this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
          this.lblDuration = new System.Windows.Forms.ToolStripStatusLabel();
          this.lblCaptureOrigin = new System.Windows.Forms.ToolStripStatusLabel();
          this.lblCaptureSize = new System.Windows.Forms.ToolStripStatusLabel();
          this.picCaptureOrigin = new System.Windows.Forms.ToolStripStatusLabel();
          this.picCaptureSize = new System.Windows.Forms.ToolStripStatusLabel();
          this.tspProgress = new System.Windows.Forms.ToolStripProgressBar();
          this.lblProgressPercent = new System.Windows.Forms.ToolStripStatusLabel();
          this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
          this.toolTip = new System.Windows.Forms.ToolTip(this.components);
          this.btnOpenFolder = new System.Windows.Forms.Button();
          this.btnPlay = new System.Windows.Forms.Button();
          this.btnCancel = new System.Windows.Forms.Button();
          this.btnStop = new System.Windows.Forms.Button();
          this.btnPause = new System.Windows.Forms.Button();
          this.btnRecord = new System.Windows.Forms.Button();
          this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
          this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.ctsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
          this.ctmsiHelpTopics = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiSendFeedback = new System.Windows.Forms.ToolStripMenuItem();
          this.ctssRestore = new System.Windows.Forms.ToolStripSeparator();
          this.ctsmiRestore = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiRecord = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiPause = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiStop = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiCancel = new System.Windows.Forms.ToolStripMenuItem();
          this.ctsmiPlay = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.ctsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
          this.ctsmiExit = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
          this.menuStrip.SuspendLayout();
          this.pnlTrackingType.SuspendLayout();
          this.statusStrip.SuspendLayout();
          this.contextMenuStrip.SuspendLayout();
          this.SuspendLayout();
          // 
          // menuStrip
          // 
          this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordingToolStripMenuItem,
            this.helpToolStripMenuItem});
          this.menuStrip.Location = new System.Drawing.Point(0, 0);
          this.menuStrip.Name = "menuStrip";
          this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
          this.menuStrip.Size = new System.Drawing.Size(454, 24);
          this.menuStrip.TabIndex = 0;
          this.menuStrip.Text = "menuStrip1";
          // 
          // recordingToolStripMenuItem
          // 
          this.recordingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRecord,
            this.tsmiPause,
            this.tsmiStop,
            this.tsmiCancel,
            this.tsmiPlay,
            this.toolStripMenuItem2,
            this.tsmiOptions,
            this.toolStripMenuItem1,
            this.tsmiExit});
          this.recordingToolStripMenuItem.Name = "recordingToolStripMenuItem";
          this.recordingToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
          this.recordingToolStripMenuItem.Text = "&Recording";
          // 
          // tsmiRecord
          // 
          this.tsmiRecord.Image = global::Atf.ScreenRecorder.Properties.Resources.primitive_dot;
          this.tsmiRecord.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
          this.tsmiRecord.Name = "tsmiRecord";
          this.tsmiRecord.Size = new System.Drawing.Size(116, 22);
          this.tsmiRecord.Text = "&Record";
          this.tsmiRecord.Click += new System.EventHandler(this.tsmiRecord_Click);
          // 
          // tsmiPause
          // 
          this.tsmiPause.Image = global::Atf.ScreenRecorder.Properties.Resources.playback_pause;
          this.tsmiPause.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
          this.tsmiPause.Name = "tsmiPause";
          this.tsmiPause.Size = new System.Drawing.Size(116, 22);
          this.tsmiPause.Text = "&Pause";
          this.tsmiPause.Click += new System.EventHandler(this.tsmiPause_Click);
          // 
          // tsmiStop
          // 
          this.tsmiStop.Image = global::Atf.ScreenRecorder.Properties.Resources.primitive_square;
          this.tsmiStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
          this.tsmiStop.Name = "tsmiStop";
          this.tsmiStop.Size = new System.Drawing.Size(116, 22);
          this.tsmiStop.Text = "&Stop";
          this.tsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
          // 
          // tsmiCancel
          // 
          this.tsmiCancel.Image = global::Atf.ScreenRecorder.Properties.Resources.x;
          this.tsmiCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
          this.tsmiCancel.Name = "tsmiCancel";
          this.tsmiCancel.Size = new System.Drawing.Size(116, 22);
          this.tsmiCancel.Text = "&Cancel";
          this.tsmiCancel.Click += new System.EventHandler(this.tsmiCancel_Click);
          // 
          // tsmiPlay
          // 
          this.tsmiPlay.Image = global::Atf.ScreenRecorder.Properties.Resources.playback_play;
          this.tsmiPlay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
          this.tsmiPlay.Name = "tsmiPlay";
          this.tsmiPlay.Size = new System.Drawing.Size(116, 22);
          this.tsmiPlay.Text = "Play";
          this.tsmiPlay.Click += new System.EventHandler(this.tsmiPlay_Click);
          // 
          // toolStripMenuItem2
          // 
          this.toolStripMenuItem2.Name = "toolStripMenuItem2";
          this.toolStripMenuItem2.Size = new System.Drawing.Size(113, 6);
          // 
          // tsmiOptions
          // 
          this.tsmiOptions.Name = "tsmiOptions";
          this.tsmiOptions.Size = new System.Drawing.Size(116, 22);
          this.tsmiOptions.Text = "&Options";
          this.tsmiOptions.Click += new System.EventHandler(this.tsmiOptions_Click);
          // 
          // toolStripMenuItem1
          // 
          this.toolStripMenuItem1.Name = "toolStripMenuItem1";
          this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 6);
          // 
          // tsmiExit
          // 
          this.tsmiExit.Name = "tsmiExit";
          this.tsmiExit.Size = new System.Drawing.Size(116, 22);
          this.tsmiExit.Text = "E&xit";
          this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
          // 
          // helpToolStripMenuItem
          // 
          this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelpTopics,
            this.toolStripSeparator4,
            this.tsmiCheckForUpdates,
            this.tsmiSendFeedback,
            this.toolStripMenuItem3,
            this.tsmiAbout});
          this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
          this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
          this.helpToolStripMenuItem.Text = "&Help";
          // 
          // tsmiHelpTopics
          // 
          this.tsmiHelpTopics.Name = "tsmiHelpTopics";
          this.tsmiHelpTopics.ShortcutKeys = System.Windows.Forms.Keys.F1;
          this.tsmiHelpTopics.Size = new System.Drawing.Size(179, 22);
          this.tsmiHelpTopics.Text = "&Help Topics";
          this.tsmiHelpTopics.Click += new System.EventHandler(this.tsmiHelpTopics_Click);
          // 
          // toolStripSeparator4
          // 
          this.toolStripSeparator4.Name = "toolStripSeparator4";
          this.toolStripSeparator4.Size = new System.Drawing.Size(176, 6);
          // 
          // tsmiCheckForUpdates
          // 
          this.tsmiCheckForUpdates.Name = "tsmiCheckForUpdates";
          this.tsmiCheckForUpdates.Size = new System.Drawing.Size(179, 22);
          this.tsmiCheckForUpdates.Text = "Check for &updates...";
          this.tsmiCheckForUpdates.Click += new System.EventHandler(this.tsmiCheckForUpdates_Click);
          // 
          // tsmiSendFeedback
          // 
          this.tsmiSendFeedback.Name = "tsmiSendFeedback";
          this.tsmiSendFeedback.Size = new System.Drawing.Size(179, 22);
          this.tsmiSendFeedback.Text = "Send &feedback...";
          this.tsmiSendFeedback.Click += new System.EventHandler(this.tsmiSendFeedback_Click);
          // 
          // toolStripMenuItem3
          // 
          this.toolStripMenuItem3.Name = "toolStripMenuItem3";
          this.toolStripMenuItem3.Size = new System.Drawing.Size(176, 6);
          // 
          // tsmiAbout
          // 
          this.tsmiAbout.Name = "tsmiAbout";
          this.tsmiAbout.Size = new System.Drawing.Size(179, 22);
          this.tsmiAbout.Text = "&About";
          this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
          // 
          // pnlTrackingType
          // 
          this.pnlTrackingType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.pnlTrackingType.AutoSize = true;
          this.pnlTrackingType.Controls.Add(this.rdoWindow);
          this.pnlTrackingType.Controls.Add(this.rdoPartial);
          this.pnlTrackingType.Controls.Add(this.rdoFull);
          this.pnlTrackingType.Location = new System.Drawing.Point(330, 37);
          this.pnlTrackingType.Margin = new System.Windows.Forms.Padding(0);
          this.pnlTrackingType.Name = "pnlTrackingType";
          this.pnlTrackingType.Size = new System.Drawing.Size(115, 35);
          this.pnlTrackingType.TabIndex = 6;
          // 
          // rdoWindow
          // 
          this.rdoWindow.Appearance = System.Windows.Forms.Appearance.Button;
          this.rdoWindow.AutoCheck = false;
          this.rdoWindow.Image = global::Atf.ScreenRecorder.Properties.Resources.Window;
          this.rdoWindow.Location = new System.Drawing.Point(41, 0);
          this.rdoWindow.Name = "rdoWindow";
          this.rdoWindow.Size = new System.Drawing.Size(32, 32);
          this.rdoWindow.TabIndex = 1;
          this.rdoWindow.TabStop = true;
          this.toolTip.SetToolTip(this.rdoWindow, "Window");
          this.rdoWindow.UseVisualStyleBackColor = true;
          this.rdoWindow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rdoWindow_MouseMove);
          this.rdoWindow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rdoWindow_MouseDown);
          this.rdoWindow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rdoWindow_MouseUp);
          // 
          // rdoPartial
          // 
          this.rdoPartial.Appearance = System.Windows.Forms.Appearance.Button;
          this.rdoPartial.AutoCheck = false;
          this.rdoPartial.Image = global::Atf.ScreenRecorder.Properties.Resources.Partial;
          this.rdoPartial.Location = new System.Drawing.Point(79, 0);
          this.rdoPartial.Name = "rdoPartial";
          this.rdoPartial.Size = new System.Drawing.Size(32, 32);
          this.rdoPartial.TabIndex = 2;
          this.rdoPartial.TabStop = true;
          this.toolTip.SetToolTip(this.rdoPartial, "Fixed");
          this.rdoPartial.UseVisualStyleBackColor = true;
          this.rdoPartial.Click += new System.EventHandler(this.rdoFixed_Click);
          // 
          // rdoFull
          // 
          this.rdoFull.Appearance = System.Windows.Forms.Appearance.Button;
          this.rdoFull.AutoCheck = false;
          this.rdoFull.Image = global::Atf.ScreenRecorder.Properties.Resources.FullScreen;
          this.rdoFull.Location = new System.Drawing.Point(3, 0);
          this.rdoFull.Name = "rdoFull";
          this.rdoFull.Size = new System.Drawing.Size(32, 32);
          this.rdoFull.TabIndex = 0;
          this.rdoFull.TabStop = true;
          this.toolTip.SetToolTip(this.rdoFull, "Full Screen");
          this.rdoFull.UseVisualStyleBackColor = true;
          this.rdoFull.Click += new System.EventHandler(this.rdoFull_Click);
          // 
          // statusStrip
          // 
          this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblDuration,
            this.lblCaptureOrigin,
            this.lblCaptureSize});
          this.statusStrip.Location = new System.Drawing.Point(0, 82);
          this.statusStrip.Name = "statusStrip";
          this.statusStrip.ShowItemToolTips = true;
          this.statusStrip.Size = new System.Drawing.Size(454, 22);
          this.statusStrip.SizingGrip = false;
          this.statusStrip.TabIndex = 7;
          this.statusStrip.Text = "statusStrip1";
          // 
          // lblStatus
          // 
          this.lblStatus.AutoSize = false;
          this.lblStatus.AutoToolTip = true;
          this.lblStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
          this.lblStatus.Name = "lblStatus";
          this.lblStatus.Size = new System.Drawing.Size(150, 17);
          this.lblStatus.Text = "[Status]";
          this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
          // 
          // lblDuration
          // 
          this.lblDuration.AutoSize = false;
          this.lblDuration.AutoToolTip = true;
          this.lblDuration.Image = global::Atf.ScreenRecorder.Properties.Resources.Timer;
          this.lblDuration.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
          this.lblDuration.Name = "lblDuration";
          this.lblDuration.Size = new System.Drawing.Size(90, 17);
          this.lblDuration.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
          this.lblDuration.ToolTipText = "Duration";
          // 
          // lblCaptureOrigin
          // 
          this.lblCaptureOrigin.AutoSize = false;
          this.lblCaptureOrigin.AutoToolTip = true;
          this.lblCaptureOrigin.Image = global::Atf.ScreenRecorder.Properties.Resources.Origin;
          this.lblCaptureOrigin.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
          this.lblCaptureOrigin.Name = "lblCaptureOrigin";
          this.lblCaptureOrigin.Size = new System.Drawing.Size(100, 17);
          this.lblCaptureOrigin.Text = "[Origin]";
          this.lblCaptureOrigin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
          this.lblCaptureOrigin.ToolTipText = "Origin";
          // 
          // lblCaptureSize
          // 
          this.lblCaptureSize.AutoSize = false;
          this.lblCaptureSize.AutoToolTip = true;
          this.lblCaptureSize.Image = global::Atf.ScreenRecorder.Properties.Resources.Dimensions;
          this.lblCaptureSize.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
          this.lblCaptureSize.Name = "lblCaptureSize";
          this.lblCaptureSize.Size = new System.Drawing.Size(110, 17);
          this.lblCaptureSize.Text = "[Size]";
          this.lblCaptureSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
          this.lblCaptureSize.ToolTipText = "Dimensions";
          // 
          // picCaptureOrigin
          // 
          this.picCaptureOrigin.Name = "picCaptureOrigin";
          this.picCaptureOrigin.Size = new System.Drawing.Size(0, 17);
          // 
          // picCaptureSize
          // 
          this.picCaptureSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
          this.picCaptureSize.Name = "picCaptureSize";
          this.picCaptureSize.Size = new System.Drawing.Size(0, 17);
          // 
          // tspProgress
          // 
          this.tspProgress.Name = "tspProgress";
          this.tspProgress.Size = new System.Drawing.Size(130, 16);
          this.tspProgress.Visible = false;
          // 
          // lblProgressPercent
          // 
          this.lblProgressPercent.Name = "lblProgressPercent";
          this.lblProgressPercent.Size = new System.Drawing.Size(0, 17);
          // 
          // tmrUpdate
          // 
          this.tmrUpdate.Interval = 200;
          this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
          // 
          // btnOpenFolder
          // 
          this.btnOpenFolder.Image = global::Atf.ScreenRecorder.Properties.Resources.file_directory;
          this.btnOpenFolder.Location = new System.Drawing.Point(202, 37);
          this.btnOpenFolder.Name = "btnOpenFolder";
          this.btnOpenFolder.Size = new System.Drawing.Size(32, 32);
          this.btnOpenFolder.TabIndex = 5;
          this.toolTip.SetToolTip(this.btnOpenFolder, "Open containing folder");
          this.btnOpenFolder.UseVisualStyleBackColor = true;
          this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
          // 
          // btnPlay
          // 
          this.btnPlay.Image = global::Atf.ScreenRecorder.Properties.Resources.playback_play;
          this.btnPlay.Location = new System.Drawing.Point(164, 37);
          this.btnPlay.Name = "btnPlay";
          this.btnPlay.Size = new System.Drawing.Size(32, 32);
          this.btnPlay.TabIndex = 4;
          this.toolTip.SetToolTip(this.btnPlay, "Play");
          this.btnPlay.UseVisualStyleBackColor = true;
          this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
          // 
          // btnCancel
          // 
          this.btnCancel.Image = global::Atf.ScreenRecorder.Properties.Resources.x;
          this.btnCancel.Location = new System.Drawing.Point(126, 37);
          this.btnCancel.Name = "btnCancel";
          this.btnCancel.Size = new System.Drawing.Size(32, 32);
          this.btnCancel.TabIndex = 3;
          this.toolTip.SetToolTip(this.btnCancel, "Cancel");
          this.btnCancel.UseVisualStyleBackColor = true;
          this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
          // 
          // btnStop
          // 
          this.btnStop.Image = global::Atf.ScreenRecorder.Properties.Resources.primitive_square;
          this.btnStop.Location = new System.Drawing.Point(88, 37);
          this.btnStop.Name = "btnStop";
          this.btnStop.Size = new System.Drawing.Size(32, 32);
          this.btnStop.TabIndex = 2;
          this.toolTip.SetToolTip(this.btnStop, "Stop");
          this.btnStop.UseVisualStyleBackColor = true;
          this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
          // 
          // btnPause
          // 
          this.btnPause.Image = global::Atf.ScreenRecorder.Properties.Resources.playback_pause;
          this.btnPause.Location = new System.Drawing.Point(50, 37);
          this.btnPause.Name = "btnPause";
          this.btnPause.Size = new System.Drawing.Size(32, 32);
          this.btnPause.TabIndex = 1;
          this.toolTip.SetToolTip(this.btnPause, "Pause");
          this.btnPause.UseVisualStyleBackColor = true;
          this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
          // 
          // btnRecord
          // 
          this.btnRecord.Image = global::Atf.ScreenRecorder.Properties.Resources.primitive_dot;
          this.btnRecord.Location = new System.Drawing.Point(12, 37);
          this.btnRecord.Name = "btnRecord";
          this.btnRecord.Size = new System.Drawing.Size(32, 32);
          this.btnRecord.TabIndex = 0;
          this.toolTip.SetToolTip(this.btnRecord, "Record");
          this.btnRecord.UseVisualStyleBackColor = true;
          this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
          // 
          // notifyIcon
          // 
          this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
          this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
          this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
          this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
          // 
          // contextMenuStrip
          // 
          this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctsmiAbout,
            this.toolStripSeparator3,
            this.ctmsiHelpTopics,
            this.ctsmiCheckForUpdates,
            this.ctsmiSendFeedback,
            this.ctssRestore,
            this.ctsmiRestore,
            this.ctsmiRecord,
            this.ctsmiPause,
            this.ctsmiStop,
            this.ctsmiCancel,
            this.ctsmiPlay,
            this.toolStripSeparator1,
            this.ctsmiOptions,
            this.toolStripSeparator2,
            this.ctsmiExit});
          this.contextMenuStrip.Name = "contextMenuStrip";
          this.contextMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
          this.contextMenuStrip.Size = new System.Drawing.Size(180, 320);
          // 
          // ctsmiAbout
          // 
          this.ctsmiAbout.Name = "ctsmiAbout";
          this.ctsmiAbout.Size = new System.Drawing.Size(179, 22);
          this.ctsmiAbout.Text = "&About";
          this.ctsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
          // 
          // toolStripSeparator3
          // 
          this.toolStripSeparator3.Name = "toolStripSeparator3";
          this.toolStripSeparator3.Size = new System.Drawing.Size(176, 6);
          // 
          // ctmsiHelpTopics
          // 
          this.ctmsiHelpTopics.Name = "ctmsiHelpTopics";
          this.ctmsiHelpTopics.Size = new System.Drawing.Size(179, 22);
          this.ctmsiHelpTopics.Text = "&Help Topics";
          this.ctmsiHelpTopics.Click += new System.EventHandler(this.tsmiHelpTopics_Click);
          // 
          // ctsmiCheckForUpdates
          // 
          this.ctsmiCheckForUpdates.Name = "ctsmiCheckForUpdates";
          this.ctsmiCheckForUpdates.Size = new System.Drawing.Size(179, 22);
          this.ctsmiCheckForUpdates.Text = "Check for &updates...";
          this.ctsmiCheckForUpdates.Click += new System.EventHandler(this.tsmiCheckForUpdates_Click);
          // 
          // ctsmiSendFeedback
          // 
          this.ctsmiSendFeedback.Name = "ctsmiSendFeedback";
          this.ctsmiSendFeedback.Size = new System.Drawing.Size(179, 22);
          this.ctsmiSendFeedback.Text = "&Send feedback...";
          this.ctsmiSendFeedback.Click += new System.EventHandler(this.tsmiSendFeedback_Click);
          // 
          // ctssRestore
          // 
          this.ctssRestore.Name = "ctssRestore";
          this.ctssRestore.Size = new System.Drawing.Size(176, 6);
          // 
          // ctsmiRestore
          // 
          this.ctsmiRestore.Image = global::Atf.ScreenRecorder.Properties.Resources.restore;
          this.ctsmiRestore.Name = "ctsmiRestore";
          this.ctsmiRestore.Size = new System.Drawing.Size(179, 22);
          this.ctsmiRestore.Text = "Res&tore";
          this.ctsmiRestore.Click += new System.EventHandler(this.ctsmiRestore_Click);
          // 
          // ctsmiRecord
          // 
          this.ctsmiRecord.Image = global::Atf.ScreenRecorder.Properties.Resources.primitive_dot;
          this.ctsmiRecord.Name = "ctsmiRecord";
          this.ctsmiRecord.Size = new System.Drawing.Size(179, 22);
          this.ctsmiRecord.Text = "&Record";
          this.ctsmiRecord.Click += new System.EventHandler(this.tsmiRecord_Click);
          // 
          // ctsmiPause
          // 
          this.ctsmiPause.Image = global::Atf.ScreenRecorder.Properties.Resources.playback_pause;
          this.ctsmiPause.Name = "ctsmiPause";
          this.ctsmiPause.Size = new System.Drawing.Size(179, 22);
          this.ctsmiPause.Text = "&Pause";
          this.ctsmiPause.Click += new System.EventHandler(this.tsmiPause_Click);
          // 
          // ctsmiStop
          // 
          this.ctsmiStop.Image = global::Atf.ScreenRecorder.Properties.Resources.primitive_square;
          this.ctsmiStop.Name = "ctsmiStop";
          this.ctsmiStop.Size = new System.Drawing.Size(179, 22);
          this.ctsmiStop.Text = "&Stop";
          this.ctsmiStop.Click += new System.EventHandler(this.tsmiStop_Click);
          // 
          // ctsmiCancel
          // 
          this.ctsmiCancel.Image = global::Atf.ScreenRecorder.Properties.Resources.x;
          this.ctsmiCancel.Name = "ctsmiCancel";
          this.ctsmiCancel.Size = new System.Drawing.Size(179, 22);
          this.ctsmiCancel.Text = "&Cancel";
          this.ctsmiCancel.Click += new System.EventHandler(this.tsmiCancel_Click);
          // 
          // ctsmiPlay
          // 
          this.ctsmiPlay.Image = global::Atf.ScreenRecorder.Properties.Resources.playback_play;
          this.ctsmiPlay.Name = "ctsmiPlay";
          this.ctsmiPlay.Size = new System.Drawing.Size(179, 22);
          this.ctsmiPlay.Text = "Play";
          this.ctsmiPlay.Click += new System.EventHandler(this.tsmiPlay_Click);
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
          // 
          // ctsmiOptions
          // 
          this.ctsmiOptions.Name = "ctsmiOptions";
          this.ctsmiOptions.Size = new System.Drawing.Size(179, 22);
          this.ctsmiOptions.Text = "Options";
          this.ctsmiOptions.Click += new System.EventHandler(this.tsmiOptions_Click);
          // 
          // toolStripSeparator2
          // 
          this.toolStripSeparator2.Name = "toolStripSeparator2";
          this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
          // 
          // ctsmiExit
          // 
          this.ctsmiExit.Name = "ctsmiExit";
          this.ctsmiExit.Size = new System.Drawing.Size(179, 22);
          this.ctsmiExit.Text = "E&xit";
          this.ctsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
          // 
          // toolStripMenuItem4
          // 
          this.toolStripMenuItem4.Name = "toolStripMenuItem4";
          this.toolStripMenuItem4.Size = new System.Drawing.Size(176, 6);
          // 
          // frmMain
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(454, 104);
          this.Controls.Add(this.btnOpenFolder);
          this.Controls.Add(this.statusStrip);
          this.Controls.Add(this.btnPlay);
          this.Controls.Add(this.pnlTrackingType);
          this.Controls.Add(this.btnCancel);
          this.Controls.Add(this.btnStop);
          this.Controls.Add(this.btnPause);
          this.Controls.Add(this.btnRecord);
          this.Controls.Add(this.menuStrip);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.MainMenuStrip = this.menuStrip;
          this.MaximizeBox = false;
          this.Name = "frmMain";
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
          this.Resize += new System.EventHandler(this.frmMain_Resize);
          this.menuStrip.ResumeLayout(false);
          this.menuStrip.PerformLayout();
          this.pnlTrackingType.ResumeLayout(false);
          this.statusStrip.ResumeLayout(false);
          this.statusStrip.PerformLayout();
          this.contextMenuStrip.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

       }
       #endregion
       private System.Windows.Forms.MenuStrip menuStrip;
       private System.Windows.Forms.ToolStripMenuItem recordingToolStripMenuItem;
       private System.Windows.Forms.ToolStripMenuItem tsmiRecord;
       private System.Windows.Forms.ToolStripMenuItem tsmiPause;
       private System.Windows.Forms.ToolStripMenuItem tsmiStop;
       private System.Windows.Forms.ToolStripMenuItem tsmiCancel;
       private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
       private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
       private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
       private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
       private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
       private System.Windows.Forms.ToolStripMenuItem tsmiExit;
       private System.Windows.Forms.Button btnRecord;
       private System.Windows.Forms.Button btnPause;
       private System.Windows.Forms.Button btnStop;
       private System.Windows.Forms.Button btnCancel;
       private System.Windows.Forms.Button btnPlay;
       private System.Windows.Forms.Panel pnlTrackingType;
       private System.Windows.Forms.RadioButton rdoFull;
       private System.Windows.Forms.RadioButton rdoWindow;
       private System.Windows.Forms.RadioButton rdoPartial;
       private System.Windows.Forms.ToolStripMenuItem tsmiPlay;
       private System.Windows.Forms.StatusStrip statusStrip;
       private System.Windows.Forms.ToolStripStatusLabel lblStatus;
       private System.Windows.Forms.ToolStripStatusLabel lblDuration;
       private System.Windows.Forms.ToolStripStatusLabel picCaptureOrigin;
       private System.Windows.Forms.ToolStripStatusLabel lblCaptureOrigin;
       private System.Windows.Forms.ToolStripStatusLabel picCaptureSize;
       private System.Windows.Forms.ToolStripStatusLabel lblCaptureSize;
       private System.Windows.Forms.ToolStripProgressBar tspProgress;
       private System.Windows.Forms.ToolStripStatusLabel lblProgressPercent;
       private System.Windows.Forms.Timer tmrUpdate;
       private System.Windows.Forms.ToolTip toolTip;
       private System.Windows.Forms.Button btnOpenFolder;
       private System.Windows.Forms.NotifyIcon notifyIcon;
       private System.Windows.Forms.ToolStripMenuItem tsmiCheckForUpdates;
       private System.Windows.Forms.ToolStripMenuItem tsmiSendFeedback;
       private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
       private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
       private System.Windows.Forms.ToolStripMenuItem ctsmiAbout;
       private System.Windows.Forms.ToolStripMenuItem ctsmiCheckForUpdates;
       private System.Windows.Forms.ToolStripMenuItem ctsmiSendFeedback;
       private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
       private System.Windows.Forms.ToolStripMenuItem ctsmiRestore;
       private System.Windows.Forms.ToolStripSeparator ctssRestore;
       private System.Windows.Forms.ToolStripMenuItem ctsmiRecord;
       private System.Windows.Forms.ToolStripMenuItem ctsmiPause;
       private System.Windows.Forms.ToolStripMenuItem ctsmiStop;
       private System.Windows.Forms.ToolStripMenuItem ctsmiPlay;
       private System.Windows.Forms.ToolStripMenuItem ctsmiCancel;
       private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
       private System.Windows.Forms.ToolStripMenuItem ctsmiOptions;
       private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
       private System.Windows.Forms.ToolStripMenuItem ctsmiExit;
       private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
       private System.Windows.Forms.ToolStripMenuItem tsmiHelpTopics;
       private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
       private System.Windows.Forms.ToolStripMenuItem ctmsiHelpTopics;
    }
 }