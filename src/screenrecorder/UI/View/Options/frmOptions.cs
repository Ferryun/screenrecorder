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
   using Atf.ScreenRecorder.Avi;
   using Atf.ScreenRecorder.Configuration;
   using Atf.ScreenRecorder.Recording;
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.Sound;
   using Atf.ScreenRecorder.UI.Presentation;

   using System;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;

   partial class frmOptions : Form, IOptionsView {
      #region Fields
      private Configuration configuration;
      private CursorHighlightOptions cursorHighlightOptions;
      private int videoFps;
      private OptionsPresenter presenter;
      private int videoQuality;
      private bool videoQualitySupport;
      // private string videoCompressor;
      private WatermarkOptions watermarkOptions;
      #endregion

      #region Constructors
      public frmOptions() {         
         InitializeComponent();
         this.presenter = new OptionsPresenter(this);
         this.cursorHighlightOptions = new CursorHighlightOptions();
         this.chdHighlight.Control = this.cursorHighlightOptions;
         this.watermarkOptions = new WatermarkOptions();
         this.chdWatermark.Control = this.watermarkOptions;
      }
      #endregion

      #region Methods
      private void btnAbout_Click(object sender, EventArgs e) {
         this.OnAboutVideoCompressor(EventArgs.Empty);
      }
      private void btnBrowse_Click(object sender, EventArgs e) {        
         this.folderBrowserDialog.SelectedPath = this.txtOutputDirectory.Text;
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
      private void chdHighlight_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
         UpdateHighlightImage();
      }
      private void chkHighlighCursor_CheckedChanged(object sender, EventArgs e) {
         this.btnHighlight.Enabled = this.chkHighlighCursor.Checked;
      }
      private void chkRecordCursor_CheckedChanged(object sender, EventArgs e) {
         this.pnlHighlightCursor.Enabled = this.chkRecordCursor.Checked;
      }
      private void chkWatermark_CheckedChanged(object sender, EventArgs e) {
         this.pnlWatermark.Enabled = this.chkWatermark.Checked;
      }
      private void cmbCompressor_SelectedIndexChanged(object sender, EventArgs e) {
         this.OnCompressorChanged(EventArgs.Empty);
      }
      private void cmbFps_TextChanged(object sender, EventArgs e) {
         int videoFps;
         if (int.TryParse(this.cmbFps.Text, out videoFps)) {
            this.videoFps = videoFps;
         }
         else {
            this.cmbFps.Text = this.videoFps.ToString();
         }
      }
      private void cmbSoundFormat_Format(object sender, ListControlConvertEventArgs e) {
         SoundFormat format = e.Value as SoundFormat;
         if (format != null) {
            e.Value = string.Format("{0,4} kbps, {1,7} Hz, {2} channels, {3,2} bit", 
                                   (format.AverageBytesPerSecond * 8) / 1000,
                                   (float)format.SamplesPerSecond, format.Channels, format.BitsPerSample);
         }
      }
      private void cmdSoundFormatTag_SelectedIndexChanged(object sender, EventArgs e) {
         if (this.SoundFormatTagChanged != null) {
            this.SoundFormatTagChanged(this, EventArgs.Empty);
         }
      }
      private void frmOptions_HelpButtonClicked(object sender, CancelEventArgs e) {
         e.Cancel = true;
         this.OnHelpRequest(EventArgs.Empty);
      } 
      private void frmOptions_Load(object sender, EventArgs e) {
         if (this.Owner != null && !this.Owner.Visible) {
            // In case of opening from notify icon
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
      private void OnCancel(EventArgs e) {
         if (this.Cancel != null) {
            this.Cancel(this, e);
         }
      }
      private void OnCompressorChanged(EventArgs eventArgs) {
         this.UpdateVideoQualityControls();
         if (this.CompressorChanged != null) {
            this.CompressorChanged(this, eventArgs);
         }
      }
      private void OnConfigurationChanged() {
         // General
         GeneralSettings generalSettings = this.configuration.General;
         this.chkMinimizeOnRecord.Checked = generalSettings.MinimizeOnRecord;
         this.chkHideFromTaskbar.Checked = generalSettings.HideFromTaskbar;
         this.txtOutputDirectory.Text = generalSettings.OutputDirectory;
         // Hotkeys
         HotKeySettings hotKeys = this.configuration.HotKeys;
         this.chkGlobalHotKeys.Checked = hotKeys.Global;
         this.hkCancel.Value = hotKeys.Cancel;
         this.hkPause.Value = hotKeys.Pause;
         this.hkRecord.Value = hotKeys.Record;
         this.hkStop.Value = hotKeys.Stop;
         // Mouse
         MouseSettings mouse = this.configuration.Mouse;
         this.chkRecordCursor.Checked = mouse.RecordCursor;
         this.chkHighlighCursor.Checked = mouse.HighlightCursor;
         this.cursorHighlightOptions.Color = mouse.HighlightCursorColor;
         this.cursorHighlightOptions.Radious = mouse.HighlightCursorRadious;
         // Sound
         SoundSettings sound = this.configuration.Sound;
         SoundDevice soundDevice = null;
         if (!string.IsNullOrEmpty(sound.DeviceId)) {
            soundDevice = new SoundDevice(sound.DeviceId);
         }
         this.soundDeviceSelector.SoundDevice = soundDevice;
         SoundFormat soundFormat = sound.Format;
         if (soundFormat != null) {
            this.cmbSoundFormatTag.SelectedItem = soundFormat.Tag;
            if (soundFormat != null && !this.cmbSoundFormat.Items.Contains(soundFormat)) {
               this.cmbSoundFormat.Items.Add(soundFormat);
            }
            this.cmbSoundFormat.SelectedItem = soundFormat;
         }
         // Tracking
         // this.trackerSelector.TrackingSettings = this.configuration.Tracking;
         // Video
         VideoSettings video = this.configuration.Video;
         this.cmbSoundFormat.Text = video.Fps.ToString();
         this.videoQuality = video.Quality;
         this.videoFps = video.Fps;
         this.cmbFps.Text = this.videoFps.ToString();
         this.tbQuality.Value = this.videoQuality;
         // Watermark
         WatermarkSettings watermark = this.configuration.Watermark;
         this.chkWatermark.Checked = watermark.Display;
         this.txtWatermark.Text = watermark.Text;
         this.watermarkOptions.WatermarkAlignment = watermark.Alignment;
         this.watermarkOptions.WatermarkColor = watermark.Color;
         this.watermarkOptions.WatermarkFont = watermark.Font;
         this.watermarkOptions.WatermarkMargin = watermark.Margin;
         this.watermarkOptions.WatermarkOutline = watermark.Outline;
         this.watermarkOptions.WatermarkOutlineColor = watermark.OutlineColor;
         this.watermarkOptions.WatermarkRightToLeft = watermark.RightToLeft;
         this.watermarkOptions.WatermarkText = watermark.Text;
         //
         this.UpdateHighlightImage();
         this.UpdateVideoQualityControls();
      }
      private void OnConfigureVideoCompressor(EventArgs e) {
         if (this.ConfigureVideoCompressor != null) {
            this.ConfigureVideoCompressor(this, e);
         }
      }
      private void OnHelpRequest(EventArgs e) {
         if (this.HelpRequest != null) {
            this.HelpRequest(this, e);
         }
      }
      private void OnOK(EventArgs e) {
         // General
         GeneralSettings general = this.configuration.General;
         general.MinimizeOnRecord = this.chkMinimizeOnRecord.Checked;
         general.HideFromTaskbar = this.chkHideFromTaskbar.Checked;
         general.OutputDirectory = this.txtOutputDirectory.Text;
         // Keys
         HotKeySettings hotKeys = this.configuration.HotKeys;
         hotKeys.Cancel = this.hkCancel.Value;
         hotKeys.Global = this.chkGlobalHotKeys.Checked;
         hotKeys.Pause = this.hkPause.Value;
         hotKeys.Record = this.hkRecord.Value;
         hotKeys.Stop = this.hkStop.Value;
         // Mouse
         MouseSettings mouse = this.configuration.Mouse;
         mouse.HighlightCursor = this.chkHighlighCursor.Checked;
         mouse.HighlightCursorColor = this.cursorHighlightOptions.Color;
         mouse.HighlightCursorRadious = this.cursorHighlightOptions.Radious;
         // Sound
         SoundSettings sound = this.configuration.Sound;
         SoundDevice selectedSoundDevice = this.soundDeviceSelector.SoundDevice;
         sound.DeviceId = selectedSoundDevice != null ? selectedSoundDevice.Id : null;
         sound.Format = this.cmbSoundFormat.SelectedItem as SoundFormat;
         // Tracking
         // this.configuration.Tracking = this.trackerSelector.TrackingSettings;
         // Video
         VideoSettings video = this.configuration.Video;
         video.Compressor = (this.cmbCompressor.SelectedItem as Avi.VideoCompressor).FccHandlerString;
         video.Fps = this.videoFps;
         video.Quality = this.videoQuality;
         // Watermark
         WatermarkSettings watermark = this.configuration.Watermark;
         watermark.Alignment = this.watermarkOptions.WatermarkAlignment;
         watermark.Color = this.watermarkOptions.WatermarkColor;
         watermark.Display = this.chkWatermark.Checked;
         watermark.Font = this.watermarkOptions.WatermarkFont;
         watermark.Margin = this.watermarkOptions.WatermarkMargin;
         watermark.Outline = this.watermarkOptions.WatermarkOutline;
         watermark.OutlineColor = this.watermarkOptions.WatermarkOutlineColor;
         watermark.RightToLeft = this.watermarkOptions.WatermarkRightToLeft;
         watermark.Text = this.watermarkOptions.WatermarkText;
         if (this.OK != null) {
            this.OK(this, e);
         }
      }
      private void OnSoundDeviceChanged(EventArgs e) {
         if (this.SoundDeviceChanged != null) {
            this.SoundDeviceChanged(this, e);
         }
      }
      private void OnSoundFormatTagChanged(EventArgs e) {
         if (this.SoundFormatTagChanged != null) {
            this.SoundFormatTagChanged(this, e);
         }
      }
      private void soundDeviceSelector_SoundDeviceChanged(object sender,EventArgs e) {
         this.OnSoundDeviceChanged(e);
      }
      private void tbQuality_Scroll(object sender, EventArgs e) {
         int newValue = tbQuality.Value;
         if (newValue != this.videoQuality) {
            this.VideoQuality = newValue;
         }
      }
      private void txtWatermark_TextChanged(object sender, EventArgs e) {
         this.watermarkOptions.WatermarkText = txtWatermark.Text;
      }
      private void UpdateHighlightImage() {
         Image oldHighlightImage = this.btnHighlight.Image;
         if (oldHighlightImage != null) {
            oldHighlightImage.Dispose();
         }
         int size = Math.Min(2 * cursorHighlightOptions.Radious, this.btnHighlight.Height);
         Color color = cursorHighlightOptions.Color;
         Bitmap bitmap = new Bitmap(size, size);
         using (Graphics graphics = Graphics.FromImage(bitmap)) {
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (SolidBrush brush = new SolidBrush(color)) {
               Rectangle highlightBounds = new Rectangle(0, 0, size - 1, size - 1);
               graphics.FillEllipse(brush, highlightBounds);
            }
         }
         this.btnHighlight.Image = bitmap;
      }
      private void UpdateVideoQualityControls() {
         if (this.videoQualitySupport) {
            this.tbQuality.Value = this.videoQuality;
            this.lblQuality.Text = this.videoQuality.ToString();
         }
         else {
            this.lblQuality.Text = "(Not Supported)";
         }
         this.tbQuality.Enabled = this.videoQualitySupport;
      }
      #endregion

      #region IOptionsView Members
      public event EventHandler Cancel;
      public event EventHandler AboutVideCompressor;
      public event EventHandler CompressorChanged;
      public event EventHandler ConfigureVideoCompressor;
      public event EventHandler HelpRequest;
      public event EventHandler OK;
      public event EventHandler SoundDeviceChanged;
      public event EventHandler SoundFormatTagChanged;
      public bool AllowSelectSoundFormat {
         set {
            this.cmbSoundFormat.Enabled = value;
         }
      }
      public bool AllowSelectSoundFormatTag {
         set {
            this.cmbSoundFormatTag.Enabled = value;
         }
      }
      public Configuration Configuration {
         get {
            return this.configuration;
         }
         set {
            if (this.configuration != value) {
               this.configuration = value;
               this.OnConfigurationChanged();
            }            
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
         set {
            this.soundDeviceSelector.SoundDevices = value;
         }
      }
      public SoundFormat SoundFormat {
         get {
            return this.cmbSoundFormat.SelectedItem as SoundFormat;
         }
         set {
            if (value != null && !this.cmbSoundFormat.Items.Contains(value)) {
               this.cmbSoundFormat.Items.Add(value);
            }
            this.cmbSoundFormat.SelectedItem = value;
         }
      }
      public SoundFormatTag? SoundFormatTag {
         get {
            object selectedItem = this.cmbSoundFormatTag.SelectedItem;
            if (selectedItem != null) {
               return (Sound.SoundFormatTag)selectedItem;
            }
            return null;
         }
         set {
            this.cmbSoundFormatTag.SelectedItem = value;
         }
      }
      public SoundFormatTag[] SoundFormatTags {
         set {
            this.cmbSoundFormatTag.Items.Clear();
            if (value != null) {
               foreach (var item in value) {
                  this.cmbSoundFormatTag.Items.Add(item);
               }
            }
         }
      }
      public SoundFormat[] SoundFormats {
         set {
            this.cmbSoundFormat.Items.Clear();
            if (value != null) {
               foreach (var item in value) {
                  this.cmbSoundFormat.Items.Add(item);
               }
            }
         }
      }      
      public VideoCompressor VideoCompressor {
         get {
            return this.cmbCompressor.SelectedItem as VideoCompressor;
         }
         set {
            this.cmbCompressor.SelectedItem = value;
         }
      }
      public VideoCompressor[] VideoCompressors {
         set {
            this.cmbCompressor.Items.Clear();
            if (value != null) {
               foreach (var item in value) {
                  this.cmbCompressor.Items.Add(item);
               }
            }
         }
      }
      public int VideoQuality {
         get {
            return this.videoQuality;
         }
         set {
            this.videoQuality = value;
            this.UpdateVideoQualityControls();
         }
      }
      public bool VideoCompressorQualitySupport {
         set {
            this.videoQualitySupport = value;
            this.UpdateVideoQualityControls();
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
