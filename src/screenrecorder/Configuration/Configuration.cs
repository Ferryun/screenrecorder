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
namespace Atf.ScreenRecorder.Configuration {
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.Sound;
   using Atf.ScreenRecorder.Recording;
   using Atf.ScreenRecorder.UI.Presentation;

   using System;
   using System.Drawing;
   using System.Configuration;
   using System.IO;
   using System.Reflection;
   using System.Windows.Forms;

   class Configuration {
      private static readonly string outputDirInMyDocs = "Screen Recorder";
      public GeneralSettings General {
         get;
         set;
      }
      public HotKeySettings HotKeys {
         get;
         set;
      }
      public MouseSettings Mouse {
         get;
         set;
      }
      public SoundSettings Sound {
         get;
         set;
      }
      public TrackingSettings Tracking {
         get;
         set;
      }
      public VideoSettings Video {
         get;
         set;
      }
      public WatermarkSettings Watermark {
         get;
         set;
      }
      private Configuration(Configuration copy) {
         this.General = copy.General.Clone();
         this.HotKeys = copy.HotKeys.Clone();
         this.Mouse = copy.Mouse.Clone();
         this.Sound = copy.Sound.Clone();
         this.Tracking = copy.Tracking.Clone();
         this.Video = copy.Video.Clone();
         this.Watermark = copy.Watermark.Clone();
      }
      private Configuration() {
         var properties = Properties.Settings.Default;
         // Read configuration section elements
         // General
         string outputDir = properties.General_OutputDirectory;
         if (string.IsNullOrEmpty(outputDir)) {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            try {
               outputDir = Path.Combine(myDocuments, outputDirInMyDocs);
            }
            catch (ArgumentException) {
               outputDir = myDocuments;
            }
         }
         GeneralSettings general = new GeneralSettings();
         general.MinimizeOnRecord = properties.General_MinimizeOnRecord;
         general.HideFromTaskbar = properties.General_HideFromTaskbarIfMinimized;
         general.OutputDirectory = outputDir;
         // Hot Keys
         HotKeySettings hotKeys = new HotKeySettings();
         hotKeys.Cancel = properties.HotKeys_Cancel;
         hotKeys.Global = properties.HotKeys_Global;
         hotKeys.Pause = properties.HotKeys_Pause;
         hotKeys.Record = properties.HotKeys_Record;
         hotKeys.Stop = properties.HotKeys_Stop;
         // Mouse
         MouseSettings mouse = new MouseSettings();
         mouse.HighlightCursor = properties.Mouse_HighlightCursor;
         mouse.HighlightCursorColor = properties.Mouse_HighlightCursorColor;
         mouse.HighlightCursorRadious = properties.Mouse_HighlightCursorRadious;
         mouse.RecordCursor = properties.Mouse_RecordCursor;
         // Sound
         SoundSettings sound = new SoundSettings();
         sound.DeviceId = properties.Sound_DeviceId;
         sound.Format = properties.Sound_Format;
         // Tracking
         TrackingSettings tracking = new TrackingSettings();
         tracking.Bounds = properties.Tracking_Bounds;
         tracking.Type = properties.Tracking_Type;         
         // Video
         VideoSettings video = new VideoSettings();
         video.Compressor = properties.Video_Compressor;
         video.Fps = properties.Video_Fps;
         video.Quality = properties.Video_Quality;
         // Watermark
         WatermarkSettings waterMark = new WatermarkSettings();
         waterMark.Alignment = properties.Watermark_Alignment;
         waterMark.Color = properties.Watermark_Color;
         waterMark.Display = properties.Watermark_Display;
         waterMark.Font = properties.Watermark_Font;
         waterMark.Margin = properties.Watermark_Margin;
         waterMark.Outline = properties.Watermark_Outline;
         waterMark.OutlineColor = properties.Watermark_OutlineColor;
         waterMark.RightToLeft = properties.Watermark_RightToLeft;
         waterMark.Text = properties.Watermark_Text;
         // Set properties
         this.General = general;
         this.HotKeys = hotKeys;
         this.Mouse = mouse;
         this.Sound = sound;
         this.Tracking = tracking;
         this.Video = video;
         this.Watermark = waterMark;
      }
      public Configuration Clone() {
         return new Configuration(this);
      }
      public static Configuration Load() {
         return new Configuration();
      }
      public void Save() {
         var properties = Properties.Settings.Default;
         // Write to properties
         // General
         properties.General_OutputDirectory = this.General.OutputDirectory;
         properties.General_MinimizeOnRecord = this.General.MinimizeOnRecord;
         properties.General_HideFromTaskbarIfMinimized = this.General.HideFromTaskbar;
         // Hot Keys
         properties.HotKeys_Cancel = this.HotKeys.Cancel;
         properties.HotKeys_Global = this.HotKeys.Global;
         properties.HotKeys_Record = this.HotKeys.Record;
         properties.HotKeys_Stop = this.HotKeys.Stop;
         // Mouse
         properties.Mouse_HighlightCursor = this.Mouse.HighlightCursor;
         properties.Mouse_HighlightCursorColor = this.Mouse.HighlightCursorColor;
         properties.Mouse_HighlightCursorRadious = this.Mouse.HighlightCursorRadious;
         properties.Mouse_RecordCursor = this.Mouse.RecordCursor;
         // Sound
         properties.Sound_DeviceId = this.Sound.DeviceId;
         properties.Sound_Format = this.Sound.Format;
         // Tracking
         if (this.Tracking.Type != TrackingType.Window) {
            properties.Tracking_Bounds = this.Tracking.Bounds;
            properties.Tracking_Type = this.Tracking.Type;
         }
         else {
            properties.Tracking_Type = TrackingType.Full; // Window Tracking Type doest get saved
         }
         // Video
         properties.Video_Compressor = this.Video.Compressor;
         properties.Video_Fps = this.Video.Fps;
         properties.Video_Quality = this.Video.Quality;
         // Watermark
         properties.Watermark_Alignment = this.Watermark.Alignment;
         properties.Watermark_Color = this.Watermark.Color;
         properties.Watermark_Display = this.Watermark.Display;
         properties.Watermark_Font = this.Watermark.Font;
         properties.Watermark_Margin = this.Watermark.Margin;
         properties.Watermark_Outline = this.Watermark.Outline;
         properties.Watermark_OutlineColor = this.Watermark.OutlineColor;
         properties.Watermark_RightToLeft = this.Watermark.RightToLeft;
         properties.Watermark_Text = this.Watermark.Text;
         // Save
         properties.Save();
      }
   }   
}
