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
   using Atf.ScreenRecorder.Avi;
   using Atf.ScreenRecorder.Configuration;
   using System;
   class OptionsPresenter {
      private IOptionsView view;
      public OptionsPresenter(IOptionsView view) {
         if (view == null) {
            throw new ArgumentNullException("view");
         }
         this.view = view;
         this.view.AboutVideCompressor += new EventHandler(view_AboutVideCompressor);
         this.view.Cancel += new EventHandler(view_Cancel);
         this.view.ConfigureVideoCompressor += new EventHandler(view_ConfigureVideoCompressor);
         this.view.Load += new EventHandler(view_Load);
         this.view.OK += new EventHandler(view_OK);
         this.view.VideoCompressorChanged += new EventHandler(view_VideoCompressorChanged);
      }
      private void view_AboutVideCompressor(object sender, EventArgs e) {
         AviCompressor compressor = this.view.SelectedVideoCompressor as AviCompressor;
         if (compressor != null) {
            compressor.About(this.view.Handle);
         }
      }
      private void view_Cancel(object sender, EventArgs e) {
         this.view.Result = false;
         this.view.Close();
      }
      private void view_ConfigureVideoCompressor(object sender, EventArgs e) {
         AviCompressor compressor = this.view.SelectedVideoCompressor as AviCompressor;
         if (compressor != null) {
            compressor.Configure(this.view.Handle);
         }
      }
      private void view_Load(object sender, EventArgs e) {
         // Read config
         Configuration config = this.view.Configuration;
         if (config == null) {
            return;
         }
         GeneralConfig generalConfig = config.General; 
         HotKeysConfig hotKeysConfig = config.HotKeys;
         TrackingConfig trackingConfig = config.Tracking;
         VideoConfig videoConfig = config.Video;
         this.view.BoundsTracker = trackingConfig.Tracker;
         this.view.CancelHotKey = hotKeysConfig.Cancel;
         this.view.GlobalHotKeys = hotKeysConfig.Global;
         this.view.MinimuzeOnRecord = generalConfig.MinimizeOnRecord;
         this.view.OutputDirectory = generalConfig.OutputDirectory;
         this.view.PauseHotKey = hotKeysConfig.Pause;
         this.view.RecordCursor = generalConfig.RecordCursor;
         this.view.RecordHotKey = hotKeysConfig.Record;
         this.view.StopHotKey = hotKeysConfig.Stop;
         this.view.VideoCompressor = videoConfig.Compressor;
         this.view.VideoFps = videoConfig.Fps;
         this.view.VideoQuality = videoConfig.Quality;

         AviCompressor[] compressors = AviCompressor.GetAll();
         this.view.VideoCompressors = compressors;
         string compressorFCC = this.view.VideoCompressor;
         // Find video compressor using it's ffc handler string
         if (!string.IsNullOrEmpty(compressorFCC)) {
            foreach (var compressor in compressors) {
               uint fccHandler = AviCompressor.FccHandlerFromString(compressorFCC);
               if (compressor.FccHandler == fccHandler) {
                  this.view.SelectedVideoCompressor = compressor;
                  break;
               }
            }
         }
      }
      private void view_OK(object sender, EventArgs e) {
         Configuration config = this.view.Configuration;
         if (config == null) {
            return;
         }
         GeneralConfig generalConfig = config.General;
         HotKeysConfig hotKeysConfig = config.HotKeys;
         TrackingConfig trackingConfig = config.Tracking;
         VideoConfig videoConfig = config.Video;
         generalConfig.MinimizeOnRecord = this.view.MinimuzeOnRecord;
         generalConfig.OutputDirectory = this.view.OutputDirectory;
         generalConfig.RecordCursor = this.view.RecordCursor;
         hotKeysConfig.Cancel = this.view.CancelHotKey;
         hotKeysConfig.Global = this.view.GlobalHotKeys;
         hotKeysConfig.Pause = this.view.PauseHotKey;
         hotKeysConfig.Record = this.view.RecordHotKey;
         hotKeysConfig.Stop = this.view.StopHotKey;
         trackingConfig.Tracker = this.view.BoundsTracker;
         videoConfig.Compressor = this.view.VideoCompressor;
         videoConfig.Fps = this.view.VideoFps;
         videoConfig.Quality = this.view.VideoQuality;  
         this.view.Result = true;
         this.view.Close();
      }
      private void view_VideoCompressorChanged(object sender, EventArgs e) {
         AviCompressor compressor = this.view.SelectedVideoCompressor as AviCompressor;
         if (compressor != null) {
            this.view.VideoCompressorQualitySupport = compressor.SupportsQuality;
            this.view.VideoCompressor = compressor.FccHandlerString;
         }
      }
   }
}
