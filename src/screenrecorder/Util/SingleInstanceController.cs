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
   using System.Reflection;
   using System.Runtime.InteropServices;
   class SingleInstanceController : IDisposable {
      #region Fields
      private static readonly string mutexName = "__ScreenRecorder__";
      private static readonly string notifyMessageName = "WM_NOTIFYINSTANCE";
      private bool disposed;
      private Form mainForm;
      private Mutex mutex = null;
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
         // Get guid of the executing assembly 
         Assembly assembly = Assembly.GetExecutingAssembly();
         GuidAttribute guidAttribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
         string guid = guidAttribute.Value;

         // Create mutext
         this.mutex = new Mutex(false, guid);

         // Register notify message
         if (notifyInstanceMessage == 0) {
            notifyInstanceMessage = User32Interop.RegisterWindowMessage(notifyMessageName);
         }

         // Wait on the mutext, false return value means that another instance is running   
         if (!this.mutex.WaitOne(0, false)) {
            if (notifyInstanceMessage != 0) {
               User32Interop.PostMessage((IntPtr)User32Interop.HWND_BROADCAST, notifyInstanceMessage, 0, 0);
            }
            return false;
         }

         // Create a window to receive notifications
         if (this.siWindow == null) {
            this.siWindow = new SIWindow(this);
         }
         return true;
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
      protected virtual void Dispose(bool disposing) {
         if (!this.disposed) {
            if (this.mutex != null) {
               this.mutex.Close();
               this.mutex = null;
            }
            disposed = true;
         }
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
               }
            }
            base.WndProc(ref m);
         }
      }
   }
}
