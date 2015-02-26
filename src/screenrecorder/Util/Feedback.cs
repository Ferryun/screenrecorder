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
namespace Atf.ScreenRecorder.Util {
   using System;
   using System.Collections.Specialized;
   using System.Diagnostics;
   using System.IO;
   using System.Net;
   class Feedback {
      private static readonly string feedbackError = "Feedback Error:";
      private static readonly string nameField = "name";
      private static readonly string emailField = "email";
      private static readonly string messageField = "message";
      private static readonly string osField = "os";
      private static readonly string reportField = "report";
      private static readonly string submitField = "submit";
      private static readonly string submitValue = "1";
      private static readonly string successMessage = "Succeed";
      private static readonly string subjectField = "subject";
      private static readonly string versionField = "version";

      public void SendAsync(string feedbackAddress, string name, string email, string subject, string message,
                            string version, string report) {
         WebClient webClient = new WebClient();
         NameValueCollection data = new NameValueCollection();
         data.Add(nameField, name);
         data.Add(emailField, email);
         data.Add(osField, Environment.OSVersion.VersionString);
         data.Add(submitField, submitValue);
         data.Add(subjectField, subject);
         data.Add(messageField, message);
         data.Add(versionField, version);
         if (!string.IsNullOrEmpty(report)) {
            data.Add(reportField, report);
         }
         webClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(webClient_UploadValuesCompleted);
         Uri feedbackUri = new Uri(feedbackAddress);
         webClient.UploadValuesAsync(feedbackUri, data);
      }
      private void webClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e) {
         if (e.Error != null) {
            Trace.TraceWarning(e.Error.ToString());
         }
         else {
            string result = System.Text.Encoding.UTF8.GetString(e.Result);
            if (string.Compare(result, successMessage) != 0) {
               Trace.TraceWarning(feedbackError + result);
            }
         }
      }

   }
}
