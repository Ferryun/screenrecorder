namespace Atf.ScreenRecorder.UI.View {
   partial class CursorHighlightOptions {
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
         this.picHighlight = new System.Windows.Forms.PictureBox();
         this.tbRadious = new System.Windows.Forms.TrackBar();
         this.btnColor = new System.Windows.Forms.Button();
         this.lblColor = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.picHighlight)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbRadious)).BeginInit();
         this.SuspendLayout();
         // 
         // picHighlight
         // 
         this.picHighlight.Location = new System.Drawing.Point(0, 0);
         this.picHighlight.Name = "picHighlight";
         this.picHighlight.Size = new System.Drawing.Size(107, 109);
         this.picHighlight.TabIndex = 0;
         this.picHighlight.TabStop = false;
         this.picHighlight.Paint += new System.Windows.Forms.PaintEventHandler(this.picHighlight_Paint);
         // 
         // tbRadious
         // 
         this.tbRadious.AutoSize = false;
         this.tbRadious.BackColor = System.Drawing.SystemColors.Control;
         this.tbRadious.LargeChange = 10;
         this.tbRadious.Location = new System.Drawing.Point(0, 113);
         this.tbRadious.Maximum = 50;
         this.tbRadious.Minimum = 1;
         this.tbRadious.Name = "tbRadious";
         this.tbRadious.Size = new System.Drawing.Size(104, 37);
         this.tbRadious.SmallChange = 5;
         this.tbRadious.TabIndex = 0;
         this.tbRadious.TickFrequency = 10;
         this.tbRadious.Value = 1;
         this.tbRadious.Scroll += new System.EventHandler(this.tbRadious_Scroll);
         // 
         // btnColor
         // 
         this.btnColor.Location = new System.Drawing.Point(76, 154);
         this.btnColor.Name = "btnColor";
         this.btnColor.Size = new System.Drawing.Size(28, 23);
         this.btnColor.TabIndex = 1;
         this.btnColor.Text = "...";
         this.btnColor.UseVisualStyleBackColor = true;
         this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
         // 
         // lblColor
         // 
         this.lblColor.AutoEllipsis = true;
         this.lblColor.Location = new System.Drawing.Point(3, 160);
         this.lblColor.Name = "lblColor";
         this.lblColor.Size = new System.Drawing.Size(67, 19);
         this.lblColor.TabIndex = 2;
         this.lblColor.Text = "(Color)";
         // 
         // CursorHighlightOptions
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.Controls.Add(this.lblColor);
         this.Controls.Add(this.btnColor);
         this.Controls.Add(this.tbRadious);
         this.Controls.Add(this.picHighlight);
         this.Name = "CursorHighlightOptions";
         this.Size = new System.Drawing.Size(110, 180);
         ((System.ComponentModel.ISupportInitialize)(this.picHighlight)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbRadious)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.PictureBox picHighlight;
      private System.Windows.Forms.TrackBar tbRadious;
      private System.Windows.Forms.Button btnColor;
      private System.Windows.Forms.Label lblColor;
   }
}
