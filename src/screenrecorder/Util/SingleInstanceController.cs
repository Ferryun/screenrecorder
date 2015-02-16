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
   using System.Threading;
   using System.Windows.Forms;
   class SingleInstanceController {
      #region Fields
      private static readonly string mutexName = "__ScreenRecorder__";
      private static readonly string notifyMessageName = "WM_NOTIFYINSTANCE";
      private Form mainForm;
      private static int notifyInstanceMessage;
      private Window siWindow;
      #endregion

      #region Properties
      public Form MainForm {
         get {
            return this.mainForm;
         }
         set {
            this.mainForm = value;
         }
      }
      #endregion

      #region Methods
      public bool RegisterNewInstance() {
         Mutex mutex = null;
         try {
            mutex = Mutex.OpenExisting(mutexName);
         }
         catch (WaitHandleCannotBeOpenedException) {
         }
         if (notifyInstanceMessage == 0) {
            notifyInstanceMessage = SingleInstanceInterop.RegisterWindowMessage(notifyMessageName);
         }
         if (mutex != null) {
            if (notifyInstanceMessage != 0) {
               SingleInstanceInterop.PostMessage((IntPtr)SingleInstanceInterop.HWND_BROADCAST, notifyInstanceMessage,
                                                 0, 0);
            }
            mutex.Close();
            return false;
         }
         mutex = new Mutex(true, mutexName);
         if (this.siWindow == null) {
            this.siWindow = new SIWindow(this);
         }
         return true;
      }
      #endregion

      class SIWindow : Window {
         private SingleInstanceController controller;
         public SIWindow(SingleInstanceController controller) {
            this.controller = controller;
         }
         protected override void WndProc(ref Message m) {
            if (m.Msg == notifyInstanceMessage) {
               Form mainForm = this.controller.mainForm;
               if (mainForm != null) {
                  if (mainForm.WindowState == FormWindowState.Minimized) {
                     mainForm.Show();
                     mainForm.WindowState = FormWindowState.Normal;                     
                  }
                  bool oldTopMost = mainForm.TopMost;
                  mainForm.TopMost = false;
                  mainForm.TopMost = true;
                  mainForm.TopMost = oldTopMost;
                  // SingleInstanceInterop.SetForegroundWindow(mainForm.Handle);
               }
            }
            base.WndProc(ref m);
         }
      }
   }
}
