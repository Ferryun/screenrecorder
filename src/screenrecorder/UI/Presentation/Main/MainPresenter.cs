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
   using Atf.ScreenRecorder.Util;

   using System;
   using System.Collections.Generic;
   using System.Diagnostics;
   using System.Drawing;
   using System.Reflection;
   using System.Windows.Forms;
   public class MainPresenter {
      #region enums
      enum HotKeyType {
         Cancel,
         Pause,
         Record,
         Stop,
      }
      #endregion

      #region Fields
      private static readonly string helpFileName = "Help.chm";
      private static readonly string updateCheckAddress = "http://youraddress/updateCheck.php";
      private bool anyRecord = false;
      private bool autoMinimized;
      private Configuration configuration;
      private HotKeyManager hotKeyManager;
      private Recorder recorder;
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

      #region Methods
      private void ApplyConfiguration(Configuration oldConfiguration) {
         // General
         GeneralConfig generalConfig = this.configuration.General;
         this.recorder.RecordCursor = generalConfig.RecordCursor;
         // Hot Keys
         HotKeysConfig hotKeyConfig = this.configuration.HotKeys;
         this.view.CancelHotKey = hotKeyConfig.Cancel;
         this.view.PauseHotKey = hotKeyConfig.Pause;
         this.view.RecordHotKey = hotKeyConfig.Record;
         this.view.StopHotKey = hotKeyConfig.Stop;
         this.UpdateHotKeys(oldConfiguration != null ? oldConfiguration.HotKeys : null);
         // Tracking
         TrackingConfig trackingConfig = this.configuration.Tracking;
         BoundsTracker tracker = trackingConfig.Tracker;
         this.recorder.Tracker = tracker;
         this.view.TrackingBounds = tracker.Bounds;
         this.view.TrackingType = tracker.Type;
         // Video
         VideoConfig videoConfig = this.configuration.Video;
         // Create AVI compressor
         string compressorFcc = videoConfig.Compressor;
         uint compressorQuality = (uint)videoConfig.Quality * 100;
         // Try create specified compressor
         Avi.AviCompressor aviCompressor = Avi.AviCompressor.CreateOrDefault(compressorFcc);
         if (aviCompressor != null) {
            aviCompressor.Quality = compressorQuality;
            videoConfig.Compressor = aviCompressor.FccHandlerString;
         }
         else {
            videoConfig.Compressor = string.Empty;
         }
         recorder.Compressor = aviCompressor;
         // Fps
         int fps = videoConfig.Fps;
         if (fps > 0) {
            this.recorder.Fps = fps;
         }
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
      private void ChangeTracker(BoundsTracker tracker) {
         // Check recording state
         RecordingState state = this.recorder.State;
         // Debug.Assert(state == RecordingState.Idle);
         if (state != RecordingState.Idle) {
            return;
         }
         this.recorder.Tracker = tracker;
         this.configuration.Tracking.Tracker = tracker;
      }
      private void CheckForUpdates() {
         Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
         UpdateInfo updateInfo = UpdateCheck.Check(new Uri(updateCheckAddress), currentVersion.ToString());
         if (updateInfo != null) {
            switch (updateInfo.Action) {
               case UpdateAction.NoAction:
                  this.view.ShowNoUpdate();
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
            case RecordingState.Initializing:
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
      private static Keys GetHotKeyFromConfig(HotKeysConfig hotKeysConfig, HotKeyType type) {
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
         Help.ShowHelp(this.view.Minimized ? null : (Control)this.view, helpFileName);
      }
      private void hotKeyManager_HotKey(object sender, KeyEventArgs e) {
         Keys keyValue = e.KeyData;
         HotKeysConfig hotKeysConfig = this.configuration.HotKeys;
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
         BoundsTracker newTracker = new BoundsTracker();
         this.configuration.Tracking.Tracker = newTracker;
         this.recorder.Tracker = newTracker;         
      }
      private void Record() {
         // Check recording state
         RecordingState state = this.recorder.State;
         Debug.Assert(state == RecordingState.Idle || state == RecordingState.Paused);
         if (state == RecordingState.Idle) {
            try {
               this.recorder.FileName = Util.IOUtil.CreateNewFile(configuration.General.OutputDirectory, "avi");
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
            this.recorder.Record();
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
      private void UpdateHotKeys(HotKeysConfig oldConfig) {
         HotKeysConfig hotKeysConfig = this.configuration.HotKeys;
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
         BoundsTracker tracker = (BoundsTracker)this.recorder.Tracker;
         try {           
            this.view.TrackingBounds = tracker.Bounds;
         }
         catch (TrackingException e) { // This usually happens if specified window get closed
            Trace.TraceWarning(e.ToString());
            if (state == RecordingState.Idle) {
               this.ResetTracker();
               // this.view.ShowWindowInaccessibleWarning();
            }           
         }
         this.view.TrackingType = tracker.Type;
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
            case RecordingState.Initializing:
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
      private void view_Stop(object sender, EventArgs e) {
         this.Stop();
      }
      private void view_TrackerChanged(object sender, TrackerChangedEventArgs e) {
         this.ChangeTracker(e.BoundsTracker);
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
