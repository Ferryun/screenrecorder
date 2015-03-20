namespace Atf.ScreenRecorder.UI.View {
   partial class SoundDeviceSelector {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.components = new System.ComponentModel.Container();
         this.cmsDevices = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.toolTip = new System.Windows.Forms.ToolTip(this.components);
         this.btnDevices = new Atf.UI.DropDownButton();
         this.SuspendLayout();
         // 
         // cmsDevices
         // 
         this.cmsDevices.Name = "cmsDevices";
         this.cmsDevices.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
         this.cmsDevices.Size = new System.Drawing.Size(61, 4);
         this.cmsDevices.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsDevices_ItemClicked);
         // 
         // btnDevices
         // 
         this.btnDevices.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnDevices.Enabled = false;
         this.btnDevices.Image = global::Atf.ScreenRecorder.Properties.Resources.mute;
         this.btnDevices.Location = new System.Drawing.Point(0, 0);
         this.btnDevices.Menu = this.cmsDevices;
         this.btnDevices.Name = "btnDevices";
         this.btnDevices.Padding = new System.Windows.Forms.Padding(2);
         this.btnDevices.Size = new System.Drawing.Size(51, 35);
         this.btnDevices.Style = Atf.UI.DropDownButtonStyle.Button;
         this.btnDevices.TabIndex = 0;
         this.btnDevices.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // SoundDeviceSelector
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.btnDevices);
         this.Name = "SoundDeviceSelector";
         this.Size = new System.Drawing.Size(51, 35);
         this.ResumeLayout(false);

      }

      #endregion

      private Atf.UI.DropDownButton btnDevices;
      private System.Windows.Forms.ContextMenuStrip cmsDevices;
      private System.Windows.Forms.ToolTip toolTip;
   }
}
