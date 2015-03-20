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
namespace Atf.ScreenRecorder.UI.Presentation {
   using Atf.ScreenRecorder.Configuration;
   using Atf.ScreenRecorder.Recording;
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.Sound;
   using Atf.ScreenRecorder.Util;

   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Drawing;
   using System.Reflection;
   using System.Windows.Forms;
   class MainPresenter {
      #region enums
      enum HotKeyType {
         Cancel,
         Pause,
         Record,
         Stop,
      }
      #endregion

      #region Fields
      private bool anyRecord = false;
      private bool autoMinimized;
      private Configuration configuration;
      private DisplayProvider displayProvider;
      private DisplaySettings displaySettings;
      private HotKeyManager hotKeyManager;
      private Recorder recorder;
      private SoundProvider soundProvider;
      private SoundSettings soundSettings;
      private IMainView view;
      #endregion

      #region Constructors
      public MainPresenter(IMainView view) {
         if (view == null) {
            throw new ArgumentNullException("view");
         }
         // Load configuration
         this.configuration = Configuration.Load();
         // Initialize recorder
         this.displayProvider = new DisplayProvider();
         this.displaySettings = new DisplaySettings() {
            Mouse = this.displayProvider.MouseSettings,
            Tracking = this.displayProvider.TrackingSettings,
            Watermark = this.displayProvider.WatermarkSettings,
         };
         this.soundProvider = new SoundProvider();
         this.soundSettings = new SoundSettings() {
            DeviceId = this.soundProvider.DeviceId,
            Format = this.soundProvider.Format,
         };
         this.recorder = new Recorder();
         this.recorder.Error += new RecordErrorEventHandler(recorder_Error);

         // Initialize view
         this.view = view;
         this.InitializeView();

         // Apply configuration
         this.ApplyConfiguration(null);

         // Update View
         this.view.AllowUpdate = true;
         // this.UpdateView();
      }
      #endregion

      #region Properties
      private DisplaySettings DisplaySettings {
         get {
            return this.displaySettings;
         }
         set {
            this.displaySettings = value;
            this.displayProvider.MouseSettings = value.Mouse;
            this.displayProvider.TrackingSettings = value.Tracking;
            this.displayProvider.WatermarkSettings = value.Watermark;
         }
      }
      private SoundSettings SoundSettings {
         get {
            return this.soundSettings;
         }
         set {
            this.soundSettings = value;
            this.soundProvider.DeviceId = value.DeviceId;
            this.soundProvider.Format = value.Format;
         }
      }
      #endregion

      #region Methods
      private void ApplyConfiguration(Configuration oldConfiguration) {
         // General
         GeneralSettings general = this.configuration.General;
         this.view.HideFromTaskbar = general.HideFromTaskbar;
         // Display (Mouse, Tracking and Watermark)
         this.DisplaySettings = new DisplaySettings() {
            Mouse = configuration.Mouse,
            Tracking = configuration.Tracking,
            Watermark = configuration.Watermark,
         };
         this.view.TrackingSettings = configuration.Tracking;
         // Hot Keys
         HotKeySettings hotKey = this.configuration.HotKeys;
         this.view.CancelHotKey = hotKey.Cancel;
         this.view.PauseHotKey = hotKey.Pause;
         this.view.RecordHotKey = hotKey.Record;
         this.view.StopHotKey = hotKey.Stop;
         this.UpdateHotKeys(oldConfiguration != null ? oldConfiguration.HotKeys : null);
         // Sound
         SoundSettings sound = this.configuration.Sound;
         SoundDevice[] soundDevices = SoundProvider.GetDevices();
         this.view.SoundDevices = soundDevices;
         string soundDeviceId = sound.DeviceId;
         SoundDevice soundDevice = null;
         if (!string.IsNullOrEmpty(soundDeviceId)) {
            soundDevice = SoundProvider.GetDeviceOrDefault(soundDeviceId);
            if (soundDevice != null) {
               // Update configuration if device id is invalid
               sound.DeviceId = soundDevice.Id;
            }
         }
         this.view.SoundDevice = soundDevice;
         this.SoundSettings = this.configuration.Sound;
         // Get updated (valid) configuration from recorder
         this.configuration.Sound = this.SoundSettings;
         // Video
         this.recorder.VideoSettings = this.configuration.Video;
         // Get updated (valid) configuration from recorder
         this.configuration.Video = this.recorder.VideoSettings;
         this.UpdateView();
      }   
      private void Cancel() {
         // Check recording state
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Recording || state == RecordingState.Paused);
         if (state == RecordingState.Recording || state == RecordingState.Paused) {
            if (!this.view.ShowCancelMessage()) { // Ask user
               return;
            }
            this.recorder.Stop();
            IOUtil.DeleteFile(this.recorder.FileName);
            this.UpdateView();
         }
      }
      private void ChangeSoundDevice(SoundDevice soundDevice) {
         // Check recording state
         RecordingState state = this.recorder.State;
         // Debug.Assert(state == RecordingState.Idle);
         if (state != RecordingState.Idle) {
            return;
         }
         SoundSettings soundSettings = this.SoundSettings;
         if (soundDevice != null) {
            soundSettings.DeviceId = soundDevice.Id;
            if (soundSettings.Format == null) {
               soundSettings.Format = SoundProvider.SuggestFormat(soundDevice.Id, null, null, null);
               this.configuration.Sound.DeviceId = soundDevice.Id;
            }
         }
         else {
            soundSettings.DeviceId = null;
            this.configuration.Sound = soundSettings;
         }
         this.SoundSettings = soundSettings;
      }
      private void ChangeTracker(TrackingSettings trackerSettings) {
         // Check recording state
         RecordingState state = this.recorder.State;
         // Debug.Assert(state == RecordingState.Idle);
         if (state != RecordingState.Idle) {
            return;
         }
         DisplaySettings displaySettings = this.DisplaySettings;
         displaySettings.Tracking = trackerSettings;
         this.DisplaySettings = displaySettings;
         this.configuration.Tracking = trackerSettings;
      }
      private void CheckForUpdates() {
         Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
         UpdateInfo updateInfo = UpdateCheck.Check(currentVersion.ToString());
         if (updateInfo != null) {
            switch (updateInfo.Action) {
               case UpdateAction.NoAction:
                  this.view.ShowNoUpdateMessage();
                  break;
               case UpdateAction.DownloadCopy:
               case UpdateAction.DownloadInstall:
               case UpdateAction.Visit:
                  bool update = this.view.ShowUpdateMessage(updateInfo.Message);
                  if (update) {
                     IOUtil.LaunchUrl(updateInfo.Uri.ToString());
                  }
                  break;
               default:
                  this.view.ShowUpdateCheckError();
                  break;
            }
         }
         else {
            this.view.ShowUpdateCheckError();
         }
      }
      private bool Close() {
         RecordingState state = this.recorder.State;
         bool result = false;
         switch (state) {
            case RecordingState.Idle:
               result = true;
               break;
            case RecordingState.Preparing:
               break;
            case RecordingState.Paused:
            case RecordingState.Recording:
               result = this.view.ShowStopMessage();
               if (result) {
                  this.recorder.Stop();                 
               }
               break;
         }
         if (result) {
            this.view.AllowUpdate = false;
            if (this.hotKeyManager != null) {
               this.hotKeyManager.Dispose();
               this.hotKeyManager = null;
            }
            try {
               this.configuration.Save();
            }
            catch (ConfigurationException ce) {
               Trace.TraceError(ce.ToString());
            }
         }
         return result;
      }
      private static Keys GetHotKeyFromConfig(HotKeySettings hotKeysConfig, HotKeyType type) {
         switch (type) {
            case HotKeyType.Cancel:
               return hotKeysConfig.Cancel;
            case HotKeyType.Pause:
               return hotKeysConfig.Pause;
            case HotKeyType.Record:
               return hotKeysConfig.Record;
            case HotKeyType.Stop:
               return hotKeysConfig.Stop;
            default:
               throw new InvalidOperationException();
         }
      }
      private void HelpTopics() {
         HelpUtil.ShowHelp(this.view.Minimized ? null : (Control)this.view);
      }
      private void hotKeyManager_HotKey(object sender, KeyEventArgs e) {
         Keys keyValue = e.KeyData;
         HotKeySettings hotKeysConfig = this.configuration.HotKeys;
         Debug.Assert(hotKeysConfig.Global);
         if (keyValue == hotKeysConfig.Cancel) {
            this.Cancel();
         }
         else if (keyValue == hotKeysConfig.Pause) {
            this.Pause();
         }
         else if (keyValue == hotKeysConfig.Record) {
            this.Record();
         }
         else if (keyValue == hotKeysConfig.Stop) {
            this.Stop();
         }
      } 
      private void InitializeView() {
         // Event Handlers
         this.view.Cancel += new EventHandler(view_Cancel);
         this.view.CheckForUpdates += new EventHandler(view_CheckForUpdates);
         this.view.HelpTopics += new EventHandler(view_HelpTopics);
         this.view.OpenFolder += new EventHandler(view_OpenFolder);
         this.view.Options += new EventHandler(view_Options);
         this.view.Pause += new EventHandler(view_Pause);
         this.view.Play += new EventHandler(view_Play);
         this.view.Record += new EventHandler(view_Record);
         this.view.SoundDeviceChanged += new EventHandler(view_SoundDeviceChanged);
         this.view.Stop += new EventHandler(view_Stop);
         this.view.TrackerChanged += new TrackerChangedEventHandler(view_TrackerChanged);
         this.view.Update += new EventHandler(view_Update);
         this.view.ViewClosing += new System.ComponentModel.CancelEventHandler(view_ViewClosing);    
      }
      private void OpenFolder() {
         if (anyRecord) {
            string fileName = this.recorder.FileName;
            if (!IOUtil.SelectFile(fileName)) {
               string directory = System.IO.Path.GetDirectoryName(fileName);
               IOUtil.OpenFolder(directory);
            }
         }
         else {
            IOUtil.OpenFolder(configuration.General.OutputDirectory);
            // Check if output directory is empty!
         }
      }
      private void Options() {
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Idle);
         if (state != RecordingState.Idle) {
            return;
         }
         IOptionsView optionsView = ViewFactory.Create<IOptionsView>();
         optionsView.Configuration = this.configuration.Clone();
         if (optionsView.ShowDialog(this.view)) {
            Configuration old = this.configuration;
            this.configuration = optionsView.Configuration;
            this.ApplyConfiguration(old);
         }
      }
      private void Pause() {
         // Check recording state
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Recording);
         if (state == RecordingState.Recording) {
            this.recorder.Pause();
            this.UpdateView();
         }
      }
      private void Play() {
         // Check recording state
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Idle && anyRecord);
         if (state == RecordingState.Idle && anyRecord) {
            IOUtil.LaunchFile(this.recorder.FileName);
         }
      }
      private void ResetTracker() {
         TrackingSettings trackingSettings = new TrackingSettings() {
            Type = TrackingType.Full,
         };
         this.configuration.Tracking = trackingSettings;
         DisplaySettings displaySettings = this.DisplaySettings;
         displaySettings.Tracking = trackingSettings;
         this.DisplaySettings = displaySettings;
         this.configuration.Tracking = trackingSettings;
      }
      private void Record() {
         // Check recording state
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Idle || state == RecordingState.Paused);
         // Record display if tracking type is set
         bool recordDisplay = this.configuration.Tracking.Type != TrackingType.None;
         // Record sound if device id is set
         bool recordSound = !string.IsNullOrEmpty(this.soundProvider.DeviceId);
         if (!recordDisplay && !recordSound) {
            this.view.ShowNothingToRecordMessage();
            return;
         }
         if (state == RecordingState.Idle) {
            try {
               this.recorder.FileName = ScreenRecorder.Util.IOUtil.CreateNewFile(configuration.General.OutputDirectory,
                                                                                 "avi");
            }
            catch (InvalidOperationException e) {
               Trace.TraceError(e.ToString());
               this.view.ShowError(e.Message);
               return;
            }
            this.anyRecord = false;
            bool minimize = this.configuration.General.MinimizeOnRecord;
            if (minimize && !this.view.Minimized) {
               this.autoMinimized = true;
               this.view.Minimized = true;
            }
            
            this.recorder.Record(recordDisplay ? this.displayProvider : null, recordSound ? this.soundProvider : null);
            this.UpdateView();
         }
         else if (state == RecordingState.Paused) {
            this.recorder.Resume();
            this.UpdateView();
         }
      }
      private void recorder_Error(object sender, RecordErrorEventArgs e) {
         Trace.TraceError(e.Exception.ToString());
         Exception inneException = e.Exception.InnerException;
         if (inneException is TrackingException) { // This usually happens if specified window get closed
            this.ResetTracker();
         }
         this.view.ShowError(e.Exception.Message);
         this.anyRecord = IOUtil.FileExists(this.recorder.FileName);
      }
      private void Stop() {
         // Check recording state
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Recording || state == RecordingState.Paused);
         if (state == RecordingState.Recording || state == RecordingState.Paused) {
            this.recorder.Stop();
            if (this.autoMinimized) {
               this.view.Minimized = false;
               this.autoMinimized = false;
            }
            this.anyRecord = true;
            this.UpdateView();
         }
      }
      private void UpdateHotKeys(HotKeySettings oldConfig) {
         HotKeySettings hotKeysConfig = this.configuration.HotKeys;
         if (hotKeysConfig.Global) {
            List<HotKeyType> hotKeyTypesToRegister = new List<HotKeyType>(new HotKeyType[] {
               HotKeyType.Cancel,
               HotKeyType.Pause,
               HotKeyType.Record,
               HotKeyType.Stop,
            });
            if (this.hotKeyManager == null) {
               this.hotKeyManager = new HotKeyManager();
               this.hotKeyManager.HotKey += new KeyEventHandler(hotKeyManager_HotKey);
            }
            else if (oldConfig != null && oldConfig.Global) {
               // Unregister old hot keys
               Array hotKeyTypes = Enum.GetValues(typeof(HotKeyType));
               foreach (object hotKeyType in hotKeyTypes) {
                  Keys oldHotKey = GetHotKeyFromConfig(oldConfig, (HotKeyType)hotKeyType);
                  Keys newHotKey = GetHotKeyFromConfig(hotKeysConfig, (HotKeyType)hotKeyType);
                  if (oldHotKey == newHotKey) {
                     hotKeyTypesToRegister.Remove((HotKeyType)hotKeyType);
                  }
                  else if (oldHotKey != Keys.None) {
                     try {
                        this.hotKeyManager.UnregisterHotKey(oldHotKey);
                     }
                     catch (InvalidOperationException e) {
                        Trace.TraceWarning(e.ToString());
                     }
                  }
               }               
            }
            // Register new hot keys
            bool allSucceed = true;
            foreach (HotKeyType hotKeyType in hotKeyTypesToRegister) {
               Keys newHotKey = GetHotKeyFromConfig(hotKeysConfig, hotKeyType);
               try {
                  this.hotKeyManager.RegisterHotKey(newHotKey);
               }
               catch (ArgumentException e) {
                  Trace.TraceWarning(e.ToString());
                  allSucceed = false;
               }
               catch (InvalidOperationException e) {
                  Trace.TraceWarning(e.ToString());
                  allSucceed = false;
               }
            }
            if (!allSucceed) {
               this.view.ShowHotKeyRegisterWarning();
            }
         }
         else if (this.hotKeyManager != null) {
            this.hotKeyManager.HotKey -= hotKeyManager_HotKey;
            this.hotKeyManager.Dispose();
            this.hotKeyManager = null;
         }
      }
      private void UpdateView() {
         RecordingState state = this.recorder.State;
         // Update status
         this.view.RecordingState = this.recorder.State;
         // Update tracking info
         this.view.AllowChangeTrackingType = state == RecordingState.Idle;
         // Update sound recording info
         this.view.AllowChangeSoundDevice = state == RecordingState.Idle && this.view.SoundDevices.Length > 0;
                
         try {
            this.view.TrackingSettings = this.displayProvider.TrackingSettings;
         }
         catch (TrackingException e) { // This usually happens if specified window get closed
            Trace.TraceWarning(e.ToString());
            if (state == RecordingState.Idle) {
               this.ResetTracker();
               // this.view.ShowWindowInaccessibleWarning();
            }           
         }
         // Update duration
         if (state == RecordingState.Recording || state == RecordingState.Paused || this.anyRecord) {
            this.view.RecordDuration = this.recorder.Duration;
         }
         else {
            this.view.RecordDuration = TimeSpan.MinValue;
         }
         // Update actions availability
         switch (state) {
            case RecordingState.Idle:
               this.view.AllowCancel = false;
               this.view.AllowOptions = true;
               this.view.AllowPause = false;
               this.view.AllowPlay = this.anyRecord;
               this.view.AllowRecord = true;
               this.view.AllowStop = false;
               break;
            case RecordingState.Preparing:
               this.view.AllowCancel = false;
               this.view.AllowOptions = false;
               this.view.AllowPause = false;
               this.view.AllowPlay = false;
               this.view.AllowRecord = false;
               this.view.AllowStop = false;
               break;
            case RecordingState.Paused:
               this.view.AllowCancel = true;
               this.view.AllowOptions = false;
               this.view.AllowPause = false;
               this.view.AllowPlay = false;
               this.view.AllowRecord = true;
               this.view.AllowStop = true;
               break;
            case RecordingState.Recording:
               this.view.AllowCancel = true;
               this.view.AllowOptions = false;
               this.view.AllowPause = true;
               this.view.AllowPlay = false;
               this.view.AllowRecord = false;
               this.view.AllowStop = true;
               break;
         }     
      }
      private void view_Cancel(object sender, EventArgs e) {
         this.Cancel();
      }
      private void view_CheckForUpdates(object sender, EventArgs e) {
         this.CheckForUpdates();
      }
      private void view_HelpTopics(object sender, EventArgs e) {
         this.HelpTopics();
      }
      private void view_OpenFolder(object sender, EventArgs e) {
         this.OpenFolder();
      }
      private void view_Options(object sender, EventArgs e) {
         this.Options();
      }
      private void view_Pause(object sender, EventArgs e) {
         this.Pause();
      }
      private void view_Play(object sender, EventArgs e) {
         this.Play();
      }
      private void view_Record(object sender, EventArgs e) {
         this.Record();
      }
      private void view_SoundDeviceChanged(object sender, EventArgs e) {
         this.ChangeSoundDevice(this.view.SoundDevice);
      }
      private void view_Stop(object sender, EventArgs e) {
         this.Stop();
      }
      private void view_TrackerChanged(object sender, TrackerChangedEventArgs e) {
         this.ChangeTracker(e.TrackingSettings);
      }  
      private void view_Update(object sender, EventArgs e) {
         this.UpdateView();
      }
      private void view_ViewClosing(object sender, System.ComponentModel.CancelEventArgs e) {
         e.Cancel = !this.Close();
      }
      #endregion
   }
}
