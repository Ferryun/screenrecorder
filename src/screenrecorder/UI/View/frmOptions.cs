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
   using Atf.ScreenRecorder.Configuration;
   using Atf.ScreenRecorder.UI.Presentation;
   using Atf.ScreenRecorder.Screen;

   using System;
   using System.Drawing;
   using System.Windows.Forms;

   public partial class frmOptions : Form, IOptionsView {
      #region Fields
      private BoundsTracker boundsTracker;
      private Configuration configuration;
      private BoundsTracker prevBoundsTracker;
      private int videoFps;
      private OptionsPresenter presenter;
      private int videoQuality;
      private bool videoQualitySupport;
      private string videoCompressor;
      private WindowFinder windowFinder;
      #endregion

      #region Constructors
      public frmOptions() {
         InitializeComponent();
         this.presenter = new OptionsPresenter(this);
         this.windowFinder = new WindowFinder();
      }
      #endregion

      #region Methods
      private void btnAbout_Click(object sender, EventArgs e) {
         this.OnAboutVideoCompressor(EventArgs.Empty);
      }
      private void btnBrowse_Click(object sender, EventArgs e) {
         FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
         folderBrowserDialog.SelectedPath = this.txtOutputDirectory.Text;
         if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
            this.txtOutputDirectory.Text = folderBrowserDialog.SelectedPath;
         }
      }
      private void btnCancel_Click(object sender, EventArgs e) {
         this.OnCancel(EventArgs.Empty);
      }
      private void btnConfigure_Click(object sender, EventArgs e) {
         this.OnConfigureVideoCompressor(EventArgs.Empty);
      }
      private void btnOK_Click(object sender, EventArgs e) {
         this.OnOK(EventArgs.Empty);
      }
      private void cmbCompressor_SelectedIndexChanged(object sender, EventArgs e) {
         this.OnVideoCompressorChanged(EventArgs.Empty);
      }
      private void cmbFps_TextChanged(object sender, EventArgs e) {
         int.TryParse(this.cmbFps.Text, out this.videoFps);
      }
      private void frmOptions_Load(object sender, EventArgs e) {
         if (this.Owner != null && !this.Owner.Visible) {
            // In case of activated from notify icon
            // this.StartPosition = FormStartPosition.CenterScreen;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((screenBounds.Width - this.Width) / 2, 
                                       (screenBounds.Height - this.Height) / 2);
         }
      }
      private void OnAboutVideoCompressor(EventArgs e) {
         if (this.AboutVideCompressor != null) {
            this.AboutVideCompressor(this, e);
         }
      }
      private void OnBoundsTrackerChanged() {
         if (this.boundsTracker != null) {
            switch (this.boundsTracker.Type) {
               case TrackingType.Fixed:
                  rdoFixed.Checked = true;
                  rdoFull.Checked = false;
                  rdoWindow.Checked = false;
                  break;
               case TrackingType.Full:
                  rdoFixed.Checked = false;
                  rdoFull.Checked = true;
                  rdoWindow.Checked = false;
                  break;
               case TrackingType.Window:
                  rdoFixed.Checked = false;
                  rdoFull.Checked = false;
                  rdoWindow.Checked = true;
                  break;
            }
         }
         else {
            this.BoundsTracker = new BoundsTracker();
         }
      }
      private void OnCancel(EventArgs e) {
         if (this.Cancel != null) {
            this.Cancel(this, e);
         }
      }
      private void OnConfigureVideoCompressor(EventArgs e) {
         if (this.ConfigureVideoCompressor != null) {
            this.ConfigureVideoCompressor(this, e);
         }
      }
      private void OnOK(EventArgs e) {
         if (this.OK != null) {
            this.OK(this, e);
         }
      }
      private void OnVideoCompressorChanged(EventArgs e) {
         if (this.VideoCompressorChanged != null) {
            this.VideoCompressorChanged(this, e);
         }
      }
      private void rdoFixed_Click(object sender, EventArgs e) {
         frmSelectBounds selectBoundsView = new frmSelectBounds();
         if (selectBoundsView.ShowDialog()) {
            this.BoundsTracker = new BoundsTracker(selectBoundsView.SelectedBounds);
         }
      }
      private void rdoFull_Click(object sender, EventArgs e) {
        this.BoundsTracker = new BoundsTracker();
      }
      private void rdoWindow_MouseDown(object sender, MouseEventArgs e) {
         if (!this.windowFinder.IsFinding && e.Button == MouseButtons.Left) {
            this.windowFinder.BeginFind();
            // Keep current tracker in case of cancellation.
            this.prevBoundsTracker = this.boundsTracker;
            // Update radio buttons state
            this.BoundsTracker = new BoundsTracker(IntPtr.Zero);
            // Change cursor
            this.Cursor = Cursors.Cross;
         }
      }
      private void rdoWindow_MouseMove(object sender, MouseEventArgs e) {
         if ((e.Button & MouseButtons.Left) == MouseButtons.Left &&
            this.windowFinder.IsFinding) {
            this.windowFinder.Find();
         }
      }
      private void rdoWindow_MouseUp(object sender, MouseEventArgs e) {
         if (this.windowFinder.IsFinding) {
            this.Cursor = Cursors.Default;
            IntPtr hWnd = this.windowFinder.EndFind();
            if (e.Button == MouseButtons.Left) {
               if (hWnd != IntPtr.Zero) {
                  this.BoundsTracker = new BoundsTracker(hWnd);
               }
               else {
                  this.BoundsTracker = this.prevBoundsTracker;
                  this.prevBoundsTracker = null;
               }
            }
            else {
               this.BoundsTracker = this.prevBoundsTracker;
               this.prevBoundsTracker = null;
            }
         }
      }      
      private void tbQuality_ValueChanged(object sender, EventArgs e) {
         int newValue = tbQuality.Value;
         if (newValue != this.videoQuality) {
            this.videoQuality = tbQuality.Value;
            this.UpdateVideoQualityControls();
         }         
      }
      private void UpdateVideoQualityControls() {
         if (this.videoQualitySupport) {
            this.lblQuality.Text = this.videoQuality.ToString();
            this.tbQuality.Value = this.videoQuality;
         }
         else {
            this.lblQuality.Text = "(Not Supported)";
            //this.tbQuality.Value = 75; // Just a value...
         }
         this.tbQuality.Enabled = this.videoQualitySupport;
      }
      #endregion

      #region IOptionsView Members
      public event EventHandler Cancel;
      public event EventHandler AboutVideCompressor;
      public event EventHandler ConfigureVideoCompressor;
      public event EventHandler OK;
      public event EventHandler VideoCompressorChanged;
      public BoundsTracker BoundsTracker {
         get {
            return this.boundsTracker;
         }
         set {
            if (this.boundsTracker != value) {
               this.boundsTracker = value;
               this.OnBoundsTrackerChanged();
            }
         }
      }
      public Keys CancelHotKey {
         get {
            return this.hkCancel.Value;
         }
         set {
            this.hkCancel.Value = value;
         }
      }
      public Configuration Configuration {
         get {
            return this.configuration;
         }
         set {
            this.configuration = value;
         }
      }
      public bool GlobalHotKeys {
         get {
            return this.chkGlobalHotKeys.Checked;
         }
         set {
            this.chkGlobalHotKeys.Checked = value;
         }
      }
      public bool MinimuzeOnRecord {
         get {
            return this.chkMinimizeOnRecord.Checked;
         }
         set {
            this.chkMinimizeOnRecord.Checked = value;
         }
      }
      public string OutputDirectory {
         get {
            return this.txtOutputDirectory.Text;
         }
         set {
            this.txtOutputDirectory.Text = value;
         }
      }
      public Keys PauseHotKey {
         get {
            return this.hkPause.Value;
         }
         set {
            this.hkPause.Value = value;
         }
      }
      public bool RecordCursor {
         get {
            return this.chkRecordCursor.Checked;
         }
         set {
            this.chkRecordCursor.Checked = value;
         }
      }
      public Keys RecordHotKey {
         get {
            return this.hkRecord.Value;
         }
         set {
            this.hkRecord.Value = value;
         }
      }
      public object SelectedVideoCompressor {
         get {
            return this.cmbCompressor.SelectedItem;
         }
         set {
            this.cmbCompressor.SelectedItem = value;
         }
      }
      public Keys StopHotKey {
         get {
            return this.hkStop.Value;
         }
         set {
            this.hkStop.Value = value;
         }
      }
      public string VideoCompressor {
         get {
            return this.videoCompressor;
         }
         set {
            this.videoCompressor = value;
         }
      }
      public bool VideoCompressorQualitySupport {
         set {
            this.videoQualitySupport = value;
            this.UpdateVideoQualityControls();
         }
      }
      public object[] VideoCompressors {
         set {
            this.cmbCompressor.Items.Clear();
            if (value != null) {
               foreach (var item in value) {
                  this.cmbCompressor.Items.Add(item);
               }
            }
         }
      }
      public int VideoFps {
         get {
            return this.videoFps;
         }
         set {
            if (this.videoFps != value) {
               this.videoFps = value;
               this.cmbFps.Text = this.videoFps.ToString();
            }
         }
      }
      public int VideoQuality {
         get {
            return this.videoQuality;
         }
         set {
            if (this.videoQuality != value) {
               this.videoQuality = value;
               this.UpdateVideoQualityControls();
            }
         }
      }
      #endregion

      #region IView Members
      public bool Result {
         get;
         set;
      }
      public new bool ShowDialog() {
         DialogResult dialogResult = base.ShowDialog();
         return this.Result;         
      }
      public bool ShowDialog(IView owner) {
         DialogResult dialogResult = base.ShowDialog((IWin32Window)owner);
         return this.Result;
      }
      #endregion    
   }
}
