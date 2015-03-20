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
namespace Atf.UI {
   using System;
   using System.ComponentModel;
   using System.Windows.Forms;
   class ControlHostDropDown : ToolStripDropDown {
      #region Fields
      private Control control;
      private ToolStripControlHost host;
      #endregion

      #region Properties
      [Browsable(true)]
      [DefaultValue(null)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Control Control {
         get {
            return this.control;
         }
         set {
            if (this.control != value) {
               this.control = value;
               this.OnControlChanged(EventArgs.Empty);
            }
         }
      } 
      #endregion

      #region Methods
      private void OnControlChanged(EventArgs e) {
         if (this.host != null) {
            this.host.Dispose();
            this.host = null;
         }
         if (this.control != null) {
            this.AutoSize = true;
            //
            this.host = new ToolStripControlHost(this.control);
            this.SuspendLayout();
            // host
            // this.host.AutoSize = true;
            this.host.Margin = new Padding(0);
            this.host.Padding = new Padding(SystemInformation.Border3DSize.Width);
            // DateTimePickerPopup
            this.AutoSize = true;
            // this.AutoClose = false;
            base.Items.Add(this.host);
            this.Padding = Padding.Empty;
            this.TabStop = false;
            // Force a rescale?! Removing these lines causes containing control doesnt scale well!
            // (Is there any better way to do that?)
            System.Drawing.Font font = this.Font;
            this.Font = System.Drawing.SystemFonts.DefaultFont;
            this.Font = font;
            this.ResumeLayout(true);

         }
      }
      #endregion
   }

}
