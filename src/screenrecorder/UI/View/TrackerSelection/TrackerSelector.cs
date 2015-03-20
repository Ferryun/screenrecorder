namespace Atf.ScreenRecorder.UI.View {
   using Atf.ScreenRecorder.Screen;
   using Atf.ScreenRecorder.UI.Presentation;
   using Atf.ScreenRecorder.Util;
   using System;
   using System.ComponentModel;
   using System.Drawing;
   using System.Text;
   using System.Windows.Forms;
   using System.Diagnostics;

   partial class TrackerSelector : UserControl {
      #region Events
      public event TrackerChangedEventHandler TrackerChanged;
      #endregion

      #region Fields
      private static readonly string dragToSelect = "Drag over a window";
      //private static readonly string dragToSelectMessage =
      //   "In order to record region of a window, drag the Window menu item over that window.";
      private static readonly int maxWindowTitleLength = 15;
      private static readonly string untitledWindow = "Untitled";
      private static readonly string trackingTypeFixed = "Fixed";
      private static readonly string trackingTypeFullScreen = "Full Screen";
      private static readonly string trackingTypeNone = "(No display recording)";
      private static readonly string trackingTypeTrackMouse = "Track Mouse Cursor";
      private static readonly string trackingTypeWindow = "Window (Drag over a window)";
      private static readonly string window = "Window";
      private bool displayTrackingImage = true;
      private bool displayTrackingName;
      private bool isSelectingTracker;
      private TrackingSettings prevTrackingSettings;
      private string prevTrackingWindowText;
      private Cursor targetCursor;
      private TrackingSettings trackingSettings;
      private WindowFinder windowFinder;
      #endregion

      #region Constructors
      public TrackerSelector() {
         InitializeComponent();
         this.trackingSettings = new TrackingSettings() {
            Type = TrackingType.Full,
         };
         this.windowFinder = new WindowFinder();
         // Item events
         this.tsmiWindow.Click += new EventHandler(tsmiWindow_Click);
         this.tsmiWindow.MouseDown += new MouseEventHandler(tsmiWindow_MouseDown);
         // Subscrible to move event of items in order to update window finder when user moves cursor over them...
         foreach (ToolStripItem item in this.cmsCaptureRegion.Items) {
            item.MouseMove += new MouseEventHandler(cmsCaptureRegion_ItemMouseMove);
         }
         // Load target cursor from .ico file in resources
         System.IO.MemoryStream targetCursorMs = new System.IO.MemoryStream();
         Properties.Resources.target.Save(targetCursorMs);
         targetCursorMs.Position = 0;
         this.targetCursor = new Cursor(targetCursorMs);
      }
      #endregion

      #region Properties
      [Browsable(true)]
      [DefaultValue(true)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Advanced)]
      public bool DisplayTrackingImage {
         get {
            return this.displayTrackingImage;
         }
         set {
            if (this.displayTrackingImage != value) {
               this.displayTrackingImage = value;
               this.UpdateButton();
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
      public TrackingSettings TrackingSettings {
         get {
            return this.trackingSettings;
         }
         set {
            if (!this.isSelectingTracker) {
               if (!value.Equals(this.trackingSettings)) {
                  this.trackingSettings = value;
                  if (this.trackingSettings.Type != TrackingType.Window) {
                     this.tsmiWindow.Text = trackingTypeWindow;
                  }
                  this.UpdateButton();
                  this.UpdateTooltip();
                  this.UpdateItems();
               }
            }
         }
      }
      #endregion

      #region Methods
      private void BeginWindowFinding() {
         Debug.Assert(!this.windowFinder.IsFinding, "Unexpected call of BeginWindowFinding()");
         if (!this.windowFinder.IsFinding) {
            this.windowFinder.BeginFind();
            // Keep current tracking type in case of cancellation.
            this.prevTrackingSettings = this.TrackingSettings.Clone();
            // Update radio buttons state
            this.TrackingSettings = new TrackingSettings() {
               Type = TrackingType.Window,
            };
            // Change cursor
            Cursor.Current = this.targetCursor;
            this.isSelectingTracker = true;
            cmsCaptureRegion.Capture = true;
            this.UpdateWindowFinder();
         }
      }
      private void cmsCaptureRegion_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
         // Get tracking type from item tag
         if (e.ClickedItem is ToolStripSeparator) {
            return;
         }
         else if (!this.windowFinder.IsFinding) {
            TrackingType clickedTrackingType =
               (TrackingType)Enum.Parse(typeof(TrackingType), (string)e.ClickedItem.Tag);
            this.Track(clickedTrackingType);
         }
         else {
            this.EndWindowFinding();
            if (e.ClickedItem == tsmiWindow) {
               cmsCaptureRegion.Close();
               // MessageBox.Show(this, dragToSelectMessage, dragToSelect);
            }
         }
      }
      private void cmsCaptureRegion_ItemMouseMove(object sender, MouseEventArgs e) {
         if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
            this.UpdateWindowFinder();
         }
      }
      private void cmsCaptureRegion_MouseMove(object sender, MouseEventArgs e) {
         if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
            this.UpdateWindowFinder();
         }
      }
      private void cmsCaptureRegion_MouseUp(object sender, MouseEventArgs e) {
         if (this.windowFinder.IsFinding && e.Button == MouseButtons.Left) {
            this.EndWindowFinding();
         }
      }
      private static Image GetTrackingImage(TrackingSettings trackingSettings) {
         switch (trackingSettings.Type) {
            case TrackingType.None:
               return Properties.Resources.nodisplay;
            case TrackingType.Fixed:
               return Properties.Resources.Partial;
            case TrackingType.Full:
               return Properties.Resources.FullScreen;               
            case TrackingType.MouseCursor:
               return Properties.Resources.TrackMouse;               
            case TrackingType.Window:
               return Properties.Resources.Window;               
            default:
               throw new InvalidOperationException();
         }
      }
      private static string GetTrackingString(TrackingSettings trackingSettings) {
         switch (trackingSettings.Type) {
            case TrackingType.Fixed:
               return trackingTypeFixed;
            case TrackingType.Full:
               return trackingTypeFullScreen;
            case TrackingType.MouseCursor:
               return trackingTypeTrackMouse;
            case TrackingType.None:
               return trackingTypeNone;
            case TrackingType.Window:
               return trackingTypeWindow;
            default:
               throw new InvalidOperationException();
         }
      }
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
            this.windowFinder.Dispose();
         }
         base.Dispose(disposing);
      }
      private void EndWindowFinding() {
         Debug.Assert(this.windowFinder.IsFinding, "Unexpected call of EndWindowFinding()");
         if (this.windowFinder.IsFinding) {
            Cursor.Current = Cursors.Default;
            IntPtr hWnd = this.windowFinder.EndFind();
            this.isSelectingTracker = false;
            if (hWnd != IntPtr.Zero) {
               this.prevTrackingWindowText = this.tsmiWindow.Text;
               this.UpdateTooltip();
               TrackingSettings trackerSettings = new TrackingSettings() {
                  Hwnd = hWnd,
                  Type = TrackingType.Window,
               };
               TrackerChangedEventArgs ea = new TrackerChangedEventArgs(trackerSettings);
               this.OnTrackerChanged(ea);
            }
            else {
               if (this.prevTrackingSettings.Type != TrackingType.Window) {
                  this.tsmiWindow.Text = trackingTypeWindow;
               }
               else {
                  this.tsmiWindow.Text = prevTrackingWindowText;
               }
               this.TrackingSettings = this.prevTrackingSettings;
            }
         }
      }
      private void OnTrackerChanged(TrackerChangedEventArgs ea) {
         TrackingSettings trackingSettings = ea.TrackingSettings;
         this.TrackingSettings = trackingSettings;
         if (this.TrackerChanged != null) {
            this.TrackerChanged(this, ea);
         }
      }
      private void Track(TrackingType trackingType) {
         TrackingSettings trackingSettings;
         TrackerChangedEventArgs ea;
         frmTrackBounds selectBoundsView;
         switch (trackingType) {
            case TrackingType.None:
            case TrackingType.Full:
               trackingSettings = new TrackingSettings() {
                  Type = trackingType,
               };
               ea = new TrackerChangedEventArgs(trackingSettings);
               this.OnTrackerChanged(ea);
               break;
            case TrackingType.Fixed:
            case TrackingType.MouseCursor:
               bool trackingMouseCursor = trackingType == TrackingType.MouseCursor;
               selectBoundsView = new frmTrackBounds();
               this.isSelectingTracker = true;
               selectBoundsView.IsFixedAroundCursor = trackingMouseCursor;
               if (this.trackingSettings.Type == trackingType) {
                  selectBoundsView.Bounds = this.TrackingSettings.Bounds;
               }
               else {
                  Rectangle screenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                  selectBoundsView.Bounds = new Rectangle(
                     screenBounds.Width / 4,
                     screenBounds.Height / 4,
                     screenBounds.Width / 2,
                     screenBounds.Height / 2);
               }
               if (selectBoundsView.ShowDialog()) {
                  this.isSelectingTracker = false;
                  trackingSettings = new TrackingSettings() {
                     Bounds = selectBoundsView.Bounds,
                     Type = trackingType,
                  };
                  ea = new TrackerChangedEventArgs(trackingSettings);
                  this.OnTrackerChanged(ea);
               }
               break;
         
            case TrackingType.Window:
               break;
         }
      }
      private void tsmiWindow_Click(object sender, EventArgs e) {
         this.cmsCaptureRegion_MouseUp(this.cmsCaptureRegion, new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0));
      }
      private void tsmiWindow_MouseDown(object sender, MouseEventArgs e) {
         if (e.Button == MouseButtons.Left) {
            this.BeginWindowFinding();
         }
      }
      private void UpdateButton() {
         if (this.displayTrackingName) {
            this.btnTrackingType.Text = GetTrackingString(this.trackingSettings);
         }
         else {
            this.btnTrackingType.Text = string.Empty;
         }
         if (this.displayTrackingImage) {
            this.btnTrackingType.Image = GetTrackingImage(this.trackingSettings);
         }
         else {
            this.btnTrackingType.Image = null;
         }
      }
      private void UpdateItems() {
         foreach (ToolStripItem item in this.cmsCaptureRegion.Items) {
            if (item is ToolStripMenuItem) {
               ToolStripMenuItem menuItem = (ToolStripMenuItem)item;
               TrackingType tag = (TrackingType)Enum.Parse(typeof(TrackingType), (string)item.Tag);
               bool check = tag  == this.trackingSettings.Type;
               menuItem.Checked = check;
            }
         }
      }
      private void UpdateTooltip() {
         this.toolTip.SetToolTip(btnTrackingType, GetTrackingString(this.trackingSettings));              
      }
      private void UpdateWindowFinder() {
         if (this.windowFinder.IsFinding) {
            string text = this.windowFinder.Text;
            if (string.IsNullOrEmpty(text)) {
               if (this.windowFinder.Handle != IntPtr.Zero) {
                  text = untitledWindow;
               }
               else {
                  text = dragToSelect;
               }
            }
            else {
               int textLength = text.Length;
               text = string.Format(@"""{0}{1}""", text.Substring(0, Math.Min(textLength, maxWindowTitleLength)),
                                    textLength > maxWindowTitleLength ? "..." : string.Empty);
            }
            text = string.Format("{0} ({1})", window, text);
            this.tsmiWindow.Text = text;
            this.windowFinder.Find();
         }
      }
      #endregion
   }
}
