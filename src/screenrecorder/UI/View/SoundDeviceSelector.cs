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
   using Atf.ScreenRecorder.Sound;
   using Atf.ScreenRecorder.UI.Presentation;
   using System;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;
   partial class SoundDeviceSelector : UserControl {
      #region Events
      public event EventHandler SoundDeviceChanged;
      #endregion

      #region Fields
      private static readonly string noSoundRecording = "(No sound recording)";
      private static readonly string noAudioDevice = "(No input source was found)";

      private SoundDevice device;
      private SoundDevice[] devices;
      private SoundDeviceDisplayProperty displayProperty = SoundDeviceDisplayProperty.Description;
      private bool displayTrackingName;
      #endregion

      #region Constructors
      public SoundDeviceSelector() {
         InitializeComponent();
         this.UpdateTooltip();
      }
      #endregion

      #region Properties
      [Browsable(true)]
      [DefaultValue(SoundDeviceDisplayProperty.Description)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public SoundDeviceDisplayProperty DisplayProperty {
         get {
            return this.displayProperty;
         }
         set {
            if (this.displayProperty != value) {
               this.displayProperty = value;
               this.UpdateItems();
               this.UpdateButton();
               this.UpdateTooltip();
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Advanced)]
      public bool DisplayTrackingName {
         get {
            return this.displayTrackingName;
         }
         set {
            if (this.displayTrackingName != value) {
               this.displayTrackingName = value;
               this.UpdateButton();
            }
         }
      }
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public SoundDevice SoundDevice {
         get {
            return this.device;
         }
         set {
            if (this.device != value) {
               this.device = value;
               this.OnSoundDeviceChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public SoundDevice[] SoundDevices {
         get {
            return this.devices;
         }
         set {
            if (this.devices != value) {
               this.devices = value;
               this.UpdateItems();
               this.UpdateButton();
               this.UpdateDeviceItem();
               this.UpdateTooltip();
            }
         }
      }

      #endregion

      #region Methods
      private void cmsDevices_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
         this.SoundDevice = e.ClickedItem.Tag as SoundDevice;
      }
      public string GetDeviceDisplayValue() {
         if (this.devices == null || this.devices.Length == 0) {
            return noAudioDevice;
         }
         return GetDeviceDisplayText(this.device);
      }
      public string GetDeviceDisplayText(SoundDevice device) {         
         if (device == null || string.IsNullOrEmpty(device.Id)) {
            return noSoundRecording;
         }
         switch (this.displayProperty) {
            case SoundDeviceDisplayProperty.Description:
               return device.Description;
            case SoundDeviceDisplayProperty.Name:
               return device.Name;
            default:
               throw new InvalidOperationException();
         }
      }
      private void OnSoundDeviceChanged(EventArgs ea) {
         if (this.SoundDeviceChanged != null) {
            this.SoundDeviceChanged(this, ea);
         }
         this.UpdateButton();
         this.UpdateDeviceItem();
         this.UpdateTooltip();
      }
      private void UpdateDeviceItem() {
         Image deviceImage = Properties.Resources.mute;
         string deviceName = string.Empty;
         foreach (ToolStripItem item in this.cmsDevices.Items) {
            if (item is ToolStripMenuItem) {
               ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
               bool check = object.Equals(this.device, menuItem.Tag);
               menuItem.Checked = check;
               if (check) {
                  deviceImage = menuItem.Image;
                  deviceName = menuItem.Text;
               }
            }
         }
         this.btnDevices.Image = deviceImage;
      }
      private void UpdateItems() {
         this.cmsDevices.Items.Clear();
         if (this.devices != null) {
            foreach (SoundDevice device in this.devices) {
               string displayText = this.GetDeviceDisplayText(device);
               ToolStripMenuItem item = new ToolStripMenuItem(displayText);
               item.Image = device.IsLoopback ? Properties.Resources.unmute : Properties.Resources.microphone;
               item.ImageScaling = ToolStripItemImageScaling.None;
               item.Tag = device;
               this.cmsDevices.Items.Add(item);
            }
         }
         ToolStripMenuItem noSoundItem = new ToolStripMenuItem(noSoundRecording, Properties.Resources.mute);
         noSoundItem.ImageScaling = ToolStripItemImageScaling.None;
         this.cmsDevices.Items.Add(new ToolStripSeparator());
         this.cmsDevices.Items.Add(noSoundItem);
      }
      private void UpdateTooltip() {
         this.toolTip.SetToolTip(this.btnDevices, this.GetDeviceDisplayValue());
      }
      private void UpdateButton() {
         this.btnDevices.Enabled = (this.devices != null && this.devices.Length > 0);
         if (this.displayTrackingName) {
            this.btnDevices.Text = this.GetDeviceDisplayValue();
         }
         else {
            this.btnDevices.Text = string.Empty;
         }
      }
      #endregion      
   }

   public enum SoundDeviceDisplayProperty {
      Name,
      Description,
   }
}
