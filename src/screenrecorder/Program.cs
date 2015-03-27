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
namespace Atf.ScreenRecorder {
   using Atf.ScreenRecorder.UI.View;
   using Atf.ScreenRecorder.Util;

   using System;
   using System.Diagnostics;
   using System.Windows.Forms;
   class Program {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main() {
         AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

         using (SingleInstanceController siController = new SingleInstanceController()) {
            if (siController.RegisterNewInstance()) {
               frmMain mainForm = new frmMain();
               siController.MainForm = mainForm;
               Application.Run(mainForm);
            }
         }
      }
      static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
         string exceptionAsString = null;
         object exceptionObject = e.ExceptionObject;
         Exception exception = exceptionObject as Exception;
         if (exception != null) {
            exceptionAsString = exception.Message;
            Trace.TraceError(exception.ToString());
         }
         else if (exceptionObject != null) {
            exceptionAsString = exceptionObject.ToString();
            Trace.TraceError(exceptionAsString);
            Trace.Unindent();
         }         
         MessageBox.Show(exceptionAsString, "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
         Environment.Exit(0);    
      }
   }
}
