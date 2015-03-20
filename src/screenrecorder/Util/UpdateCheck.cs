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
   using System.Net;
   using System.Text;
   using System.Text.RegularExpressions;
   static class UpdateCheck {
      #region Fields
      private static readonly string defaultCheckAddress = "http://chehraz.ir/projects/screenrecorder/updateCheck.php";

      private static readonly string osField = "os";
      private static readonly string versionField = "version";
      #endregion

      #region Methods
      public static UpdateInfo Check(string version) {
         return Check(new Uri(defaultCheckAddress), version);
      }
      public static UpdateInfo Check(Uri uri, string version) {
         NameValueCollection data = new NameValueCollection();
         data.Add(osField, Environment.OSVersion.VersionString);
         data.Add(versionField, version);
         byte[] output = null;
         try {          
            WebClientEx webClient = new WebClientEx();
            output = webClient.UploadValues(uri, data);
         }
         catch (WebException e) {
            Trace.TraceWarning(e.ToString());
            return null;
         }
         return UpdateInfo.TryParse(output);
      }
      #endregion
      class WebClientEx : WebClient {
         private static readonly int timeout = 10000;
         protected override WebRequest GetWebRequest(Uri address) {
            WebRequest wr = base.GetWebRequest(address);
            wr.Timeout = timeout;
            return wr;
         }
      }
   }
   public enum UpdateAction {
      None,
      DownloadCopy,
      DownloadInstall,
      NoAction,
      Visit,
   }
   class UpdateInfo {
      #region Fields
      // Example of an update response from web helper:
      // UPDATECHECK:action=Visit|version=1.2.0.0|date=2015/02/05|msg=Do you want to update?|url=[encodedurl]";
      private static readonly string expected = "UPDATECHECK:";
      private static readonly string infoAction = "action";
      private static readonly string infoDate = "date";
      private static readonly string infoMessage = "msg";
      private static readonly string infoUrl = "url";
      private static readonly string infoVersion = "version";
      private static readonly string pattern = @"([^|=]+)=([^|]+)\|?";

      private UpdateAction action;
      private DateTime? date;
      private string message;
      private Uri uri;
      private Version version;
      #endregion

      #region Constructors
      private UpdateInfo() {
      }
      #endregion

      #region Properties
      public UpdateAction Action {
         get {
            return this.action;
         }
      }
      public DateTime? Date {
         get {
            return this.date;
         }
      }
      public string Message {
         get {
            return this.message;
         }
      }
      public Uri Uri {
         get {
            return this.uri;
         }
      }
      public Version Version {
         get {
            return this.version;
         }
      }
      #endregion

      #region Methods
      public static UpdateInfo TryParse(byte[] bytes) {
         if (bytes == null) {
            return null;
         }
         string s = System.Text.Encoding.UTF8.GetString(bytes);
         s = s.Trim();
         if (s.Length <= expected.Length) {
            return null;
         }
         string initial = s.Substring(0, expected.Length);
         if (!string.Equals(initial, expected)) {
            return null;
         }
         string result = s.Substring(initial.Length);
         Regex regex = new Regex(pattern);
         MatchCollection matches = regex.Matches(result);
         if (matches.Count == 0) {
            return null;
         }
         NameValueCollection infoCollection = new NameValueCollection();
         foreach (Match match in matches) {
            infoCollection.Add(match.Groups[1].Value, match.Groups[2].Value);
         }
         string sAction = infoCollection[infoAction];
         if (string.IsNullOrEmpty(sAction)) {
            return null;
         }
         if (!Enum.IsDefined(typeof(UpdateAction), sAction)) {
            return null;
         }
         UpdateInfo updateInfo = new UpdateInfo();
         // Parse action
         updateInfo.action = (UpdateAction)Enum.Parse(typeof(UpdateAction), sAction);
         // Parse date
         DateTime date;
         if (DateTime.TryParse(infoCollection[infoDate], out date)) {
            updateInfo.date = date;
         }
         // Message
         updateInfo.message = infoCollection[infoMessage];
         // Parse url
         string strUrl = infoCollection[infoUrl];
         if (!string.IsNullOrEmpty(strUrl)) {
            strUrl = System.Web.HttpUtility.UrlDecode(strUrl);
            Uri uri;
            if (Uri.TryCreate(strUrl, UriKind.Absolute, out uri)) {
               string scheme = uri.Scheme;
               if (scheme.Equals(Uri.UriSchemeHttp) || scheme.Equals(Uri.UriSchemeHttps) ||
                   scheme.Equals(Uri.UriSchemeFtp)) {
                  updateInfo.uri = uri;
               }
            }
         }
         // Parse version
         try {
            updateInfo.version = new Version(infoCollection[infoVersion]);
         }
         catch (ArgumentException) {
         }
         catch (FormatException) {
         }
         catch (OverflowException) {
         }         
         return updateInfo;
      }
      #endregion
   }
}
