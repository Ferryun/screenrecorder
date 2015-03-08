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
   using Atf.ScreenRecorder.Util;
   using System;
   using System.Diagnostics;
   using System.IO;
   using System.IO.Compression;
   using System.Windows.Forms;
   using System.Reflection;
   using System.Security;
   class FeedbackPresenter {
      private static readonly string defaultLogFileName = "application.log"; // Thats enough for now
      private static readonly string feedbackAddress = "http://youraddress/feedback.php";

      private IFeedbackView view;
      public FeedbackPresenter(IFeedbackView view) {
         if (view == null) {
            throw new ArgumentNullException("view");
         }
         this.view = view;
         this.view.OK += new EventHandler(view_OK);
      }
      private string GetEncodedReportFile() {
         string report = null;
         try {
            string exePath = Application.ExecutablePath;
            string appPath = Path.GetDirectoryName(exePath);
            string logFileName = Path.Combine(appPath, defaultLogFileName);
            if (!File.Exists(logFileName)) {
               return null;
            }
            byte[] logFileBytes = File.ReadAllBytes(logFileName);
            using (MemoryStream ms = new MemoryStream()) {
               using (DeflateStream deflateStream = new DeflateStream(ms, CompressionMode.Compress, false)) {
                  deflateStream.Write(logFileBytes, 0, logFileBytes.Length);
                  byte[] compressedBytes = ms.ToArray();
                  report = Convert.ToBase64String(compressedBytes);
               }
            }
         }
         catch (ArgumentException e) {
            Trace.TraceWarning(e.ToString());
         }
         catch (IOException e) {
            Trace.TraceWarning(e.ToString());
         }
         catch (SecurityException e) {
            Trace.TraceWarning(e.ToString());
         }
         catch (UnauthorizedAccessException e) {
            Trace.TraceWarning(e.ToString());
         }
         return report;
      }
      private void Send() {
         bool sendReport = this.view.SendReport;
         string encodedReportFile = null;
         if (sendReport) {
            encodedReportFile = GetEncodedReportFile();
         }
         string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
         Feedback feedback = new Feedback();
         feedback.SendAsync(feedbackAddress, this.view.SenderName, this.view.EMail, this.view.Subject, 
                            this.view.Message, version, encodedReportFile);
      }
      private void view_OK(object sender, EventArgs e) {
         if (string.IsNullOrEmpty(this.view.Subject)) {
            this.view.ShowEmptySubjectError();
         }
         else if (string.IsNullOrEmpty(this.view.Message)) {
            this.view.ShowEmptyMessageError();
         }
         else {
            this.Send();
            this.view.ShowThankYou();
            this.view.Close();
         }
      }
   }
}
