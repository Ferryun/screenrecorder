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
   using Atf.ScreenRecorder.Recording;
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.Sound;

   using System;
   using System.Collections.Generic;
   class OptionsPresenter {
      #region Fields
      private static readonly string helpContextId = "106";

      private List<SoundFormat> formatList;
      private IOptionsView view;
      #endregion

      #region Constructors
      public OptionsPresenter(IOptionsView view) {
         if (view == null) {
            throw new ArgumentNullException("view");
         }
         this.view = view;
         this.view.AboutVideCompressor += new EventHandler(view_AboutVideCompressor);
         this.view.Cancel += new EventHandler(view_Cancel);
         this.view.CompressorChanged += new EventHandler(view_CompressorChanged);
         this.view.ConfigureVideoCompressor += new EventHandler(view_ConfigureVideoCompressor);
         this.view.HelpRequest += new EventHandler(view_HelpRequested);
         this.view.Load += new EventHandler(view_Load);
         this.view.OK += new EventHandler(view_OK);
         this.view.SoundDeviceChanged += new EventHandler(view_SoundDeviceChanged);
         this.view.SoundFormatTagChanged += new EventHandler(view_SoundFormatTagChanged);
      }
      #endregion

      #region Methods
      private static bool FormatListContainsFormatTag(IList<SoundFormat> formats, SoundFormatTag tag) {
         foreach (var format in formats) {
            if (format.Tag == tag) {
               return true;
            }
         }
         return false;
      }
      private static int SoundFormatComparer(SoundFormat a, SoundFormat b) {
         return a.AverageBytesPerSecond.CompareTo(b.AverageBytesPerSecond);
      }
      private void view_AboutVideCompressor(object sender, EventArgs e) {
         VideoCompressor compressor = this.view.VideoCompressor as VideoCompressor;
         if (compressor != null) {
            compressor.About(this.view.Handle);
         }
      }
      private void view_Cancel(object sender, EventArgs e) {
         this.view.Result = false;
         this.view.Close();
      }
      private void view_CompressorChanged(object sender, EventArgs e) {
         VideoCompressor comressor = this.view.VideoCompressor;
         if (comressor != null) {
            this.view.VideoCompressorQualitySupport = comressor.SupportsQuality;
            if (this.view.VideoQuality <= 0) {
               this.view.VideoQuality = (int)comressor.Quality / 100;
            }
         }
         else {
            this.view.VideoCompressorQualitySupport = false;
            this.view.VideoQuality = 0;
         }
      }
      private void view_ConfigureVideoCompressor(object sender, EventArgs e) {
         VideoCompressor compressor = this.view.VideoCompressor as VideoCompressor;
         if (compressor != null) {
            compressor.Configure(this.view.Handle);
         }
      }
      private void view_HelpRequested(object sender, EventArgs e) {
         ScreenRecorder.Util.HelpUtil.ShowHelpTopic((System.Windows.Forms.Control)this.view, helpContextId);
      }
      private void view_Load(object sender, EventArgs e) {
         // TODO: Display screen bit depth (16, 24 and 32 is currently supported)
         // Read config
         Configuration config = this.view.Configuration;
         if (config == null) {
            return;
         }   
         // Get list of sound devices
         SoundDevice[] soundDevices = SoundProvider.GetDevices();
         this.view.SoundDevices = soundDevices;
         string soundDeviceId = config.Sound.DeviceId;
         if (!string.IsNullOrEmpty(soundDeviceId)) {
            foreach (var soundDevice in soundDevices) {
               if (soundDevice.Id.Equals(soundDeviceId)) {
                  this.view.SoundDevice = soundDevice;
                  break;
               }
            }
         }
         // Get list of video compressors
         var compressors = new List<VideoCompressor>(VideoCompressor.GetAll());
         compressors.Add(VideoCompressor.None);
         this.view.VideoCompressors = compressors.ToArray();
         string compressorFCC = this.view.Configuration.Video.Compressor;
         // Find video compressor using it's ffc handler string
         if (!string.IsNullOrEmpty(compressorFCC)) {
            foreach (var compressor in compressors) {
               uint fccHandler = VideoCompressor.FccHandlerFromString(compressorFCC);
               if (compressor.FccHandler == fccHandler) {
                  this.view.VideoCompressor = compressor;
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
         this.view.Result = true;
         this.view.Close();
      }
      private void view_SoundDeviceChanged(object sender, EventArgs e) {
         SoundDevice device = this.view.SoundDevice;
         if (device != null && !string.IsNullOrEmpty(device.Id)) {            
            SoundFormat[] formats = null;
            try {
               formats = SoundProvider.GetFormats(device.Id, true);
            }
            catch (SoundException) {
               this.view.SoundDevice = null;
               this.view.SoundFormats = null;
               this.view.SoundFormatTags = null;
               this.view.AllowSelectSoundFormat = false;
               this.view.AllowSelectSoundFormatTag = false;
               return;
            }
            this.view.AllowSelectSoundFormat = true;
            this.view.AllowSelectSoundFormatTag = true;

            this.formatList = new List<SoundFormat>(formats);
            this.formatList.Sort(SoundFormatComparer);
            List<SoundFormatTag> distinctTags = new List<SoundFormatTag>(formatList.Count);
            foreach (var format in formats) {
               if (!distinctTags.Contains(format.Tag)) {
                  distinctTags.Add(format.Tag);
               }
            }
            SoundFormat currentFormat = this.view.SoundFormat;
            this.view.SoundFormatTags = distinctTags.ToArray();
            SoundFormatTag? preferredTag = null;
            int? preferredChannels = null;
            int? preferredSampleRate = null;
            if (currentFormat != null) {
               preferredTag = currentFormat.Tag;
               preferredSampleRate = currentFormat.SamplesPerSecond;
               preferredChannels = currentFormat.Channels;
            }
            SoundFormat suggestedFormat = SoundProvider.SuggestFormat(device.Id, preferredTag, preferredSampleRate,
                                                                       preferredChannels);
            this.view.SoundFormatTag = suggestedFormat.Tag;
            this.view.SoundFormat = suggestedFormat;
         }
         else {
            this.view.AllowSelectSoundFormat = false;
            this.view.AllowSelectSoundFormatTag = false;
         }
      }
      private void view_SoundFormatTagChanged(object sender, EventArgs e) {
         SoundFormatTag? viewFormatTag = this.view.SoundFormatTag;
         if (viewFormatTag.HasValue) {
            List<SoundFormat> soundFormatList = new List<SoundFormat>();
            foreach (var format in this.formatList) {
               if (format.Tag == viewFormatTag) {
                  soundFormatList.Add(format);
               }
            }
            SoundFormat currentFormat = this.view.SoundFormat;
            SoundFormatTag? preferredTag = viewFormatTag;
            int? preferredSampleRate = null;
            int? preferredChannels = null;
            if (currentFormat != null) {
               preferredSampleRate = currentFormat.SamplesPerSecond;
               preferredChannels = currentFormat.Channels;
            }
            this.view.SoundFormats = soundFormatList.ToArray();
            SoundFormat suggestedFormat = SoundProvider.SuggestFormat(this.view.SoundDevice.Id, viewFormatTag,
                                                                      preferredSampleRate, preferredChannels);
             this.view.SoundFormat = suggestedFormat;
         }
         else {
            this.view.SoundFormats = null;
         }
      }
      #endregion
   }
}
