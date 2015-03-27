/*
 * Copyright (c) 2015 Mehrzad Chehraz (mehrzady@gmail.com)
 * Released under the MIT License
 * http://chehraz.ir/mit_license
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
namespace Atf.ScreenRecorder.UI.View {
   using Atf.ScreenRecorder.Recording;
   using Atf.ScreenRecorder.Sound;
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.UI.Presentation;

   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Reflection;
   using System.Windows.Forms;

   partial class frmMain : Form, IMainView {
      #region Fields
      private static readonly string cancelMessage = "Recording is in progress. Are you sure to want to cancel it?";
      private static readonly string errorMessageTitle = "Error";
      private static readonly string hotKeyWarningMessage = "Failed to register one or more hot keys.";
      private static readonly Icon iconPaused = Properties.Resources.icon_paused;
      private static readonly Icon iconRecord = Properties.Resources.icon_rec;
      private static readonly Icon iconRecord2 = Properties.Resources.icon_16;
      private static readonly Image imagePaused = Properties.Resources.playback_pause;
      private static readonly Image imageRecord = Properties.Resources.primitive_dot_red;
      private static readonly Image imageRecord2 = Properties.Resources.primitive_dot;
      private static readonly int notifyErrorDelay = 20000;
      private static readonly int notifyWarningDelay = 10000;
      private static readonly string noUpdateMessage = "Current version is the lastest available version.";
      private static readonly string nothingToRecord =
         "Cannot start recoding, both display and sound recording are " +
         "disabled. Please enable at least one of them and try again.";
      private static readonly string stopMessage = "Are you sure to want to stop recording?";
      private static readonly string updateCheckErrorMessage = "Failed to check for updates.";
      private static readonly string warningMessageTitle = "Warning";

      private bool hideFromTaskbar;
      private MainPresenter presenter;
      private TimeSpan recordingDuration;
      #endregion

      #region Constructors
      public frmMain() {
         InitializeComponent();
         this.notifyIcon.Text = Application.ProductName;
         this.Text = Application.ProductName;
         this.presenter = new MainPresenter(this);        
      }
      #endregion

      #region Methods
      private void btnCancel_Click(object sender, EventArgs e) {
         this.OnCancel(EventArgs.Empty);
      }
      private void btnOpenFolder_Click(object sender, EventArgs e) {
         this.OnOpenFolder(EventArgs.Empty);
      }
      private void btnPause_Click(object sender, EventArgs e) {
         this.OnPause(EventArgs.Empty);
      }
      private void btnPlay_Click(object sender, EventArgs e) {
         this.OnPlay(EventArgs.Empty);
      }
      private void btnRecord_Click(object sender, EventArgs e) {
         this.OnRecord(EventArgs.Empty);
      }
      private void btnStop_Click(object sender, EventArgs e) {
         this.OnStop(EventArgs.Empty);
      }
      private void ctsmiOpenFolder_Click(object sender, EventArgs e) {
         this.OnOpenFolder(EventArgs.Empty);
      }
      private void ctsmiRestore_Click(object sender, EventArgs e) {
         this.OnRestore();
      }     
      private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
         CancelEventArgs ea = new CancelEventArgs(e.Cancel);
         this.OnViewClosing(ea);
         e.Cancel = ea.Cancel;
      }
      private void frmMain_Resize(object sender, EventArgs e) {
         if (this.WindowState == FormWindowState.Minimized) {
            if (this.hideFromTaskbar) {
               this.Hide();
            }
            this.notifyIcon.Visible = true;
         }
         else {
            this.notifyIcon.Visible = false;
         }
      }
      private void notifyIcon_BalloonTipClicked(object sender, EventArgs e) {
         if (!this.Visible) {
            this.OnRestore();
         }
      }
      private void notifyIcon_DoubleClick(object sender, EventArgs e) {
         this.OnRestore();
      }
      private void OnAbout() {
         frmAboutBox frmAboutBox = new frmAboutBox();
         frmAboutBox.ShowDialog(this);
      }
      public void OnCancel(EventArgs e) {
         if (this.Cancel != null) {
            this.Cancel(this, e);
         }
      }
      private void OnCheckForUpdates(EventArgs e) {
         if (this.CheckForUpdates != null) {
            this.CheckForUpdates(this, e);
         }
      }
      private void OnExit() {
         this.Close();
      }
      private void OnHelpTopics(EventArgs e) {
         if (this.HelpTopics != null) {
            this.HelpTopics(this, e);
         }
      }
      private void OnOpenFolder(EventArgs e) {
         if (this.OpenFolder != null) {
            this.OpenFolder(this, e);
         }
      }
      private void OnOptions() {
         if (this.Options != null) {
            this.Options(this, EventArgs.Empty);
         }
      }
      private void OnPause(EventArgs e) {
         if (this.Pause != null) {
            this.Pause(this, e);
         }
      }
      private void OnPlay(EventArgs e) {
         if (this.Play != null) {
            this.Play(this, e);
         }
      }
      private void OnRecord(EventArgs e) {
         if (this.Record != null) {
            this.Record(this, e);
         }
      }
      private void OnRestore() {
         this.Show();
         this.WindowState = FormWindowState.Normal;
      }
      private void OnSendFeedBack(EventArgs eventArgs) {
         frmFeedback frmFeedback = new frmFeedback();
         frmFeedback.ShowDialog(this);
      }
      private void OnSoundDeviceChanged(EventArgs ea) {         
         if (this.SoundDeviceChanged != null) {
            this.SoundDeviceChanged(this, ea);
         }
      }
      private void OnStop(EventArgs e) {
         if (this.Stop != null) {
            this.Stop(this, e);
         }
      }
      private void OnTrackerChanged(TrackerChangedEventArgs ea) {
         TrackingSettings trackingSettings = ea.TrackingSettings;
         this.TrackingSettings = trackingSettings;
         if (this.TrackerChanged != null) {
            this.TrackerChanged(this, ea);
         }
      }
      private void OnUpdate(EventArgs e) {
         if (this.Update != null) {
            this.Update(this, e);
         }
      }
      private void OnViewClosed(EventArgs e) {
         if (this.ViewClosed != null) {
            this.ViewClosed(this, e);
         }
      }
      private void OnViewClosing(CancelEventArgs ea) {
         if (this.ViewClosing != null) {
            this.ViewClosing(this, ea);
         }
      }
      private void soundDeviceSelector_SoundDeviceChanged(object sender, EventArgs e) {
         this.OnSoundDeviceChanged(e);
      }
      private void trackerSelector_TrackerChanged(object sender, TrackerChangedEventArgs e) {
         this.OnTrackerChanged(e);
      }
      private void tmrUpdate_Tick(object sender, EventArgs e) {
         this.OnUpdate(EventArgs.Empty);
      }
      private void tsmiAbout_Click(object sender, EventArgs e) {
         this.OnAbout();
      }
      private void tsmiCancel_Click(object sender, EventArgs e) {
         this.OnCancel(EventArgs.Empty);
      }
      private void tsmiCheckForUpdates_Click(object sender, EventArgs e) {
         this.OnCheckForUpdates(EventArgs.Empty);
      }
      private void tsmiExit_Click(object sender, EventArgs e) {
         this.OnExit();
      }
      private void tsmiHelpTopics_Click(object sender, EventArgs e) {
         this.OnHelpTopics(EventArgs.Empty);
      }
      private void tsmiOptions_Click(object sender, EventArgs e) {
         this.OnOptions();
      }
      private void tsmiPause_Click(object sender, EventArgs e) {
         this.OnPause(EventArgs.Empty);
      }
      private void tsmiPlay_Click(object sender, EventArgs e) {
         this.OnPlay(EventArgs.Empty);
      }
      private void tsmiRecord_Click(object sender, EventArgs e) {
         this.OnRecord(EventArgs.Empty);
      }
      private void tsmiSendFeedback_Click(object sender, EventArgs e) {
         this.OnSendFeedBack(EventArgs.Empty);
      }
      private void tsmiStop_Click(object sender, EventArgs e) {
         this.OnStop(EventArgs.Empty);
      }   
      #endregion

      #region IMainView Members
      public event EventHandler Cancel;
      public event EventHandler CheckForUpdates;
      public event EventHandler HelpTopics;
      public event EventHandler Options;
      public event EventHandler OpenFolder;
      public event EventHandler Pause;
      public event EventHandler Play;
      public event EventHandler Record;
      public event EventHandler SoundDeviceChanged;
      public event EventHandler Stop;
      public event TrackerChangedEventHandler TrackerChanged;
      public new event EventHandler Update;
      public event EventHandler ViewClosed;
      public event CancelEventHandler ViewClosing;

      public bool AllowCancel {
         set {
            this.btnCancel.Enabled = value;
            this.ctsmiCancel.Enabled = value;
            this.tsmiCancel.Enabled = value;
         }
      }
      public bool AllowOptions {
         set {
            this.ctsmiOptions.Enabled = value;
            this.tsmiOptions.Enabled = value;
         }
      }
      public bool AllowChangeTrackingType {
         set {
            this.trackerSelector.Enabled = value;
         }
      }
      public bool AllowChangeSoundDevice {
         set {
            this.soundDeviceSelector.Enabled = value;
         }
      }
      public bool AllowPause {
         set {
            this.btnPause.Enabled = value;
            this.ctsmiPause.Enabled = value;
            this.tsmiPause.Enabled = value;
         }
      }
      public bool AllowPlay {
         set {
            this.btnPlay.Enabled = value;
            this.ctsmiPlay.Enabled = value;
            this.tsmiPlay.Enabled = value;
         }
      }
      public bool AllowRecord {
         set {
            this.btnRecord.Enabled = value;
            this.ctsmiRecord.Enabled = value;
            this.tsmiRecord.Enabled = value;
         }
      }
      public bool AllowStop {
         set {
            this.btnStop.Enabled = value;
            this.ctsmiStop.Enabled = value;
            this.tsmiStop.Enabled = value;
         }
      }
      public bool AllowUpdate {
         set {
            this.tmrUpdate.Enabled = value;
         }
      }
      public Keys CancelHotKey {
         set {
            this.tsmiCancel.ShortcutKeys = value;
         }
      }
      public bool HideFromTaskbar {
         get {
            return this.hideFromTaskbar;
         }
         set {
            this.hideFromTaskbar = value;
         }
      }
      public bool Minimized {
         get {
            return this.WindowState == FormWindowState.Minimized;
         }
         set {
            if (value) {
               this.WindowState = FormWindowState.Minimized;
            }
            else {
               this.OnRestore();
            }
         }
      }
      public Keys PauseHotKey {
         set {
            this.tsmiPause.ShortcutKeys = value;
         }
      }
      public Keys RecordHotKey {
         set {
            this.tsmiRecord.ShortcutKeys = value;          
         }
      }
      public RecordingState RecordingState {
         set {
            Image statusImage;
            Icon notifyIconIcon;
            switch (value) {
               case RecordingState.Paused:
                  notifyIconIcon = iconPaused;
                  statusImage = imagePaused;
                  break;
               case RecordingState.Recording:
                  bool blink = Math.Ceiling(this.recordingDuration.TotalSeconds) % 2 == 1;
                  if (blink) {
                     notifyIconIcon = iconRecord;
                     statusImage = imageRecord;
                  }
                  else {
                     notifyIconIcon = iconRecord2;
                     statusImage = imageRecord2;
                  }
                  break;
               default:
                  notifyIconIcon = iconRecord2;
                  statusImage = null;
                  break;
            }
            string status = value.ToString();           
            string duration = string.Empty;
            if (value == RecordingState.Recording) {
               duration = string.Format("{0}{1:00}:{2:00}:{3:00}", Environment.NewLine, this.recordingDuration.Hours,
                                        this.recordingDuration.Minutes, this.recordingDuration.Seconds);
            }
            string notifyText = string.Format("{0} ({1}){2}", Application.ProductName, status, duration);
            if (!string.Equals(this.notifyIcon.Text, notifyText)) {
               this.notifyIcon.Text = notifyText;
            }
            this.lblStatus.Image = statusImage;
            this.lblStatus.Text = status;
            this.notifyIcon.Icon = notifyIconIcon;
         }
      }
      public TimeSpan RecordDuration {
         set {
            if (value != TimeSpan.MinValue) {
               this.lblDuration.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
               this.lblDuration.Text = string.Format("{0:00}:{1:00}:{2:00}", value.Hours, value.Minutes, value.Seconds);
            }
            else {
               this.lblDuration.DisplayStyle = ToolStripItemDisplayStyle.None;
               this.lblDuration.Text = string.Empty;
            }
            this.recordingDuration = value;
         }
      }
      public SoundDevice SoundDevice {
         get {
            return this.soundDeviceSelector.SoundDevice;
         }
         set {
            this.soundDeviceSelector.SoundDevice = value;
         }
      }
      public SoundDevice[] SoundDevices {
         get {
            return this.soundDeviceSelector.SoundDevices;
         }
         set {
            this.soundDeviceSelector.SoundDevices = value;
         }
      }
      public Keys StopHotKey {
         set {
            this.tsmiStop.ShortcutKeys = value;                          
         }
      }
      public TrackingSettings TrackingSettings {
         set {
            this.trackerSelector.TrackingSettings = value;
            this.lblCaptureOrigin.Text = string.Format("({0}, {1})", value.Bounds.Left, value.Bounds.Top);
            this.lblCaptureSize.Text = string.Format("{0}x{1}", value.Bounds.Width, value.Bounds.Height);
         }
      }
      public bool ShowCancelMessage() {
         if (!this.Visible) {
            this.OnRestore();
         }
         DialogResult result = MessageBox.Show(this, cancelMessage, Application.ProductName,
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
         return (result == DialogResult.Yes);
      }
      public void ShowError(string message) {
         if (this.InvokeRequired) {
            this.BeginInvoke((MethodInvoker)delegate {
               ShowError(message);
            });
         }
         else {
            if (this.Visible) {
               MessageBox.Show(this, message, errorMessageTitle, MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            }
            else {
               this.notifyIcon.ShowBalloonTip(notifyErrorDelay, errorMessageTitle, message, 
                                              ToolTipIcon.Error);
            }
         }
      }
      public void ShowHotKeyRegisterWarning() {
         if (this.Visible) {
            MessageBox.Show(this, hotKeyWarningMessage, warningMessageTitle, MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
         }
         else {
            this.notifyIcon.ShowBalloonTip(notifyWarningDelay, warningMessageTitle, hotKeyWarningMessage,
                                           ToolTipIcon.Warning);

         }
      }
      public void ShowNoUpdateMessage() {
         if (this.Visible) {
            MessageBox.Show(this, noUpdateMessage, this.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.None);
         }
         else {
            this.notifyIcon.ShowBalloonTip(notifyWarningDelay, this.ProductName, noUpdateMessage,
                                           ToolTipIcon.None);

         }
      }
      public void ShowNothingToRecordMessage() {
         if (this.Visible) {
            MessageBox.Show(this, nothingToRecord, this.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.None);
         }
         else {
            this.notifyIcon.ShowBalloonTip(notifyWarningDelay, this.ProductName, nothingToRecord,
                                           ToolTipIcon.None);

         }
      }
      public bool ShowStopMessage() {
         if (!this.Visible) {
            this.OnRestore();
         }
         DialogResult result = MessageBox.Show(this, stopMessage, Application.ProductName,
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.None, MessageBoxDefaultButton.Button2);
         return (result == DialogResult.Yes);
      }
      public void ShowUpdateCheckError() {
         if (this.Visible) {
            MessageBox.Show(this, updateCheckErrorMessage, this.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.None);
         }
         else {
            this.notifyIcon.ShowBalloonTip(notifyWarningDelay, this.ProductName, updateCheckErrorMessage,
                                           ToolTipIcon.None);

         }
      }
      public bool ShowUpdateMessage(string message) {
          if (!this.Visible) {
            this.OnRestore();
         }
         DialogResult result = MessageBox.Show(this, message, this.ProductName, MessageBoxButtons.YesNo);
         return result == DialogResult.Yes;  
      }
      #endregion

      #region IView Members
      public bool Result {
         get;
         set;
      }
      public new bool ShowDialog() {
         base.ShowDialog();
         return this.Result;
      }
      public bool ShowDialog(IView owner) {
         base.ShowDialog((IWin32Window)owner);
         return this.Result;
      }
      #endregion          
   }
}
