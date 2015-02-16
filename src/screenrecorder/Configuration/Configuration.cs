namespace Atf.ScreenRecorder.Configuration {
   using Atf.ScreenRecorder.Screen;
   using System;
   using System.Drawing;
   using System.Windows.Forms;
   using System.Configuration;
using System.Reflection;
using System.IO;
   public class Configuration {
      private static readonly string outputDirInMyDocs = "Screen Recorder";
      public GeneralConfig General {
         get;
         private set;
      }
      public HotKeysConfig HotKeys {
         get;
         private set;
      }
      public TrackingConfig Tracking {
         get;
         private set;
      }
      public VideoConfig Video {
         get;
         private set;
      }
      private Configuration(Configuration copy) {
         this.General = copy.General.Clone();
         this.HotKeys = copy.HotKeys.Clone();
         this.Tracking = copy.Tracking.Clone();
         this.Video = copy.Video.Clone();
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
         GeneralConfig general = new GeneralConfig();
         general.MinimizeOnRecord = properties.General_MinimizeOnRecord;
         general.OutputDirectory = outputDir;
         general.RecordCursor = properties.General_RecordCursor;
         // Hot Keys
         HotKeysConfig hotKeys = new HotKeysConfig();
         hotKeys.Cancel = properties.HotKeys_Cancel;
         hotKeys.Global = properties.HotKeys_Global;
         hotKeys.Pause = properties.HotKeys_Pause;
         hotKeys.Record = properties.HotKeys_Record;
         hotKeys.Stop = properties.HotKeys_Stop;
         // Tracking
         TrackingConfig tracking = new TrackingConfig();
         TrackingType trackingType = properties.Tracking_Type;
         switch (trackingType) {
            case TrackingType.Fixed:
               tracking.Tracker = new BoundsTracker(properties.Tracking_Bounds);
               break;
            case TrackingType.Full:
            case TrackingType.Window:  // NOTE: Window Tracking won't be saved.
               tracking.Tracker = new BoundsTracker();
               break;
            default:
               throw new InvalidOperationException();
         }
         // Video
         VideoConfig video = new VideoConfig();
         video.Compressor = properties.Video_Compressor;
         video.Fps = properties.Video_Fps;
         video.Quality = properties.Video_Quality;
         // Set properties
         this.General = general;
         this.HotKeys = hotKeys;
         this.Tracking = tracking;
         this.Video = video;
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
         properties.General_RecordCursor = this.General.RecordCursor;
         // Hot Keys
         properties.HotKeys_Cancel = this.HotKeys.Cancel;
         properties.HotKeys_Global = this.HotKeys.Global;
         properties.HotKeys_Record = this.HotKeys.Record;
         properties.HotKeys_Stop = this.HotKeys.Stop;
         // Tracking
         BoundsTracker tracker = this.Tracking.Tracker;
         if (tracker != null) {
            switch (tracker.Type) {
               case TrackingType.Fixed:
                  properties.Tracking_Bounds = tracker.Bounds;
                  properties.Tracking_Type = tracker.Type;
                  break;
               case TrackingType.Full:
                  properties.Tracking_Bounds = Rectangle.Empty;
                  properties.Tracking_Type = tracker.Type;
                  break;
               case TrackingType.Window:
                  // NOTE: Window Tracking won't be saved in configuration file
                  break;
            }
         }
         // Video
         properties.Video_Compressor = this.Video.Compressor;
         properties.Video_Fps = this.Video.Fps;
         properties.Video_Quality = this.Video.Quality;
         // Save
         properties.Save();
      }
   }
   public class GeneralConfig {
      public bool MinimizeOnRecord {
         get;
         set;
      }
      public string OutputDirectory {
         get;
         set;
      }
      public bool RecordCursor {
         get;
         set;
      }
      public GeneralConfig Clone() {
         return (GeneralConfig)this.MemberwiseClone();
      }
   }
   public class HotKeysConfig {
      public Keys Cancel {
         get;
         set;
      }
      public bool Global {
         get;
         set;
      }
      public Keys Pause {
         get;
         set;
      }
      public Keys Record {
         get;
         set;
      }
      public Keys Stop {
         get;
         set;
      }
      public HotKeysConfig Clone() {
         return (HotKeysConfig)this.MemberwiseClone();
      }
   }
   public class TrackingConfig {
      public BoundsTracker Tracker {
         get;
         set;
      }
      public TrackingConfig Clone() {
         return new TrackingConfig() {
            Tracker = this.Tracker.Clone(),
         };
      }
   }
   public class VideoConfig {
      public string Compressor {
         get;
         set;
      }
      public int Fps {
         get;
         set;
      }
      public int Quality {
         get;
         set;
      }
      public VideoConfig Clone() {
         return (VideoConfig)this.MemberwiseClone();
      }
   }
}
