namespace Atf.ScreenRecorder.UI.View {
   partial class TrackerSelector {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         this.cmsCaptureRegion = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.tsmiFullScreen = new System.Windows.Forms.ToolStripMenuItem();
         this.tsmiWindow = new System.Windows.Forms.ToolStripMenuItem();
         this.tsmiFixed = new System.Windows.Forms.ToolStripMenuItem();
         this.tsmiTrackMouse = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.tsmiDoNotRecordDisplay = new System.Windows.Forms.ToolStripMenuItem();
         this.toolTip = new System.Windows.Forms.ToolTip(this.components);
         this.btnTrackingType = new Atf.UI.DropDownButton();
         this.cmsCaptureRegion.SuspendLayout();
         this.SuspendLayout();
         // 
         // cmsCaptureRegion
         // 
         this.cmsCaptureRegion.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFullScreen,
            this.tsmiWindow,
            this.tsmiFixed,
            this.tsmiTrackMouse,
            this.toolStripSeparator1,
            this.tsmiDoNotRecordDisplay});
         this.cmsCaptureRegion.Name = "cmsCaptureRegion";
         this.cmsCaptureRegion.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
         this.cmsCaptureRegion.Size = new System.Drawing.Size(228, 182);
         this.cmsCaptureRegion.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cmsCaptureRegion_MouseUp);
         this.cmsCaptureRegion.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsCaptureRegion_ItemClicked);
         this.cmsCaptureRegion.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cmsCaptureRegion_MouseMove);
         // 
         // tsmiFullScreen
         // 
         this.tsmiFullScreen.Image = global::Atf.ScreenRecorder.Properties.Resources.FullScreen;
         this.tsmiFullScreen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
         this.tsmiFullScreen.Name = "tsmiFullScreen";
         this.tsmiFullScreen.Size = new System.Drawing.Size(227, 30);
         this.tsmiFullScreen.Tag = "Full";
         this.tsmiFullScreen.Text = "Full Screen";
         // 
         // tsmiWindow
         // 
         this.tsmiWindow.Image = global::Atf.ScreenRecorder.Properties.Resources.Window;
         this.tsmiWindow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
         this.tsmiWindow.Name = "tsmiWindow";
         this.tsmiWindow.Size = new System.Drawing.Size(227, 30);
         this.tsmiWindow.Tag = "Window";
         this.tsmiWindow.Text = "Window (Drag over a window)";
         // 
         // tsmiFixed
         // 
         this.tsmiFixed.Image = global::Atf.ScreenRecorder.Properties.Resources.Partial;
         this.tsmiFixed.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
         this.tsmiFixed.Name = "tsmiFixed";
         this.tsmiFixed.Size = new System.Drawing.Size(227, 30);
         this.tsmiFixed.Tag = "Fixed";
         this.tsmiFixed.Text = "Fixed";
         // 
         // tsmiTrackMouse
         // 
         this.tsmiTrackMouse.Image = global::Atf.ScreenRecorder.Properties.Resources.TrackMouse;
         this.tsmiTrackMouse.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
         this.tsmiTrackMouse.Name = "tsmiTrackMouse";
         this.tsmiTrackMouse.Size = new System.Drawing.Size(227, 30);
         this.tsmiTrackMouse.Tag = "MouseCursor";
         this.tsmiTrackMouse.Text = "Track Mouse Cursor";
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(224, 6);
         // 
         // tsmiDoNotRecordDisplay
         // 
         this.tsmiDoNotRecordDisplay.Image = global::Atf.ScreenRecorder.Properties.Resources.nodisplay;
         this.tsmiDoNotRecordDisplay.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
         this.tsmiDoNotRecordDisplay.Name = "tsmiDoNotRecordDisplay";
         this.tsmiDoNotRecordDisplay.Size = new System.Drawing.Size(227, 30);
         this.tsmiDoNotRecordDisplay.Tag = "None";
         this.tsmiDoNotRecordDisplay.Text = "(No display recording)";
         // 
         // btnTrackingType
         // 
         this.btnTrackingType.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnTrackingType.Image = global::Atf.ScreenRecorder.Properties.Resources.FullScreen;
         this.btnTrackingType.Location = new System.Drawing.Point(0, 0);
         this.btnTrackingType.Menu = this.cmsCaptureRegion;
         this.btnTrackingType.Name = "btnTrackingType";
         this.btnTrackingType.Padding = new System.Windows.Forms.Padding(2);
         this.btnTrackingType.Size = new System.Drawing.Size(51, 35);
         this.btnTrackingType.Style = Atf.UI.DropDownButtonStyle.Button;
         this.btnTrackingType.TabIndex = 0;
         this.btnTrackingType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // TrackerSelector
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.btnTrackingType);
         this.Name = "TrackerSelector";
         this.Size = new System.Drawing.Size(51, 35);
         this.cmsCaptureRegion.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Atf.UI.DropDownButton btnTrackingType;
      private System.Windows.Forms.ContextMenuStrip cmsCaptureRegion;
      private System.Windows.Forms.ToolStripMenuItem tsmiFullScreen;
      private System.Windows.Forms.ToolStripMenuItem tsmiWindow;
      private System.Windows.Forms.ToolStripMenuItem tsmiFixed;
      private System.Windows.Forms.ToolStripMenuItem tsmiTrackMouse;
      private System.Windows.Forms.ToolTip toolTip;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripMenuItem tsmiDoNotRecordDisplay;
   }
}
