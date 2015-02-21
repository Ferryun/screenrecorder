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
namespace Atf.ScreenRecorder.UI.View {
   using Atf.ScreenRecorder.UI.Presentation;
   using System;
   using System.Drawing;
   using System.Reflection;
   using System.Windows.Forms;
   public partial class frmFeedback : Form, IFeedbackView {
      #region Fields
      private static readonly string info = "Thank you for using {0}! Please send your feedback in order to " +
                                            "help improve this product.";
      private static readonly string messageEmptyMessage = "Please enter a message for the feedback.";
      private static readonly string subjectEmptyMessage = "Please enter a subject for the feedback.";
      private static readonly string thankyouMessage = "Thank you for sending your feedback!";
      private FeedbackPresenter presenter;
      #endregion
      #region Constructors
      public frmFeedback() {
         InitializeComponent();
         this.presenter = new FeedbackPresenter(this);
         this.lblInfo.Text = string.Format(info, AssemblyProduct);
      }
      #endregion

      #region Properties
      public string AssemblyProduct {
         get {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute),
                                                                                      false);
            if (attributes.Length == 0) {
               return string.Empty;
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;
         }
      }
      #endregion

      #region Methods
      private void btnSend_Click(object sender, EventArgs e) {
         this.OnOK(EventArgs.Empty);
      }
      private void OnOK(EventArgs e) {
         if (this.OK != null) {
            this.OK(this, e);
         }
      }
      private void frmFeedback_Load(object sender, EventArgs e) {
         if (this.Owner != null && !this.Owner.Visible) {
            // In case of activated from notify icon
            // this.StartPosition = FormStartPosition.CenterScreen;
            Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point((screenBounds.Width - this.Width) / 2,
                                       (screenBounds.Height - this.Height) / 2);
         }
      }
      #endregion

      #region IFeedbackView Members
      public event EventHandler OK;

      public string EMail {
         get {
            return this.txtEMail.Text;
         }
      }          
      public string Message {
         get {
            return this.txtMessage.Text;
         }
      }
      public string SenderName {
         get {
            return this.txtName.Text;
         }
      }
      public bool SendReport {
         get {
            return this.chkSendReport.Checked;
         }
      }
      public string Subject {
         get {
            return this.txtSubject.Text;
         }
      }
      public void ShowEmptyMessageError() {
         MessageBox.Show(this, messageEmptyMessage, this.AssemblyProduct, MessageBoxButtons.OK);
         this.txtMessage.Focus();
      }
      public void ShowEmptySubjectError() {
         MessageBox.Show(this, subjectEmptyMessage, this.AssemblyProduct, MessageBoxButtons.OK);
         this.txtSubject.Focus();
      }
      public void ShowThankYou() {
         this.Hide();
         MessageBox.Show(this, thankyouMessage, this.AssemblyProduct, MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      #endregion

      #region IView Members
      public bool Result {
         get;
         set;
      }
      public new bool ShowDialog() {
         base.ShowDialog();
         this.Result = this.DialogResult == DialogResult.OK;
         return this.Result;
      }
      public bool ShowDialog(IView owner) {
         base.ShowDialog((IWin32Window)owner);
         this.Result = this.DialogResult == DialogResult.OK;
         return this.Result;
      }
      #endregion 
   }
}
