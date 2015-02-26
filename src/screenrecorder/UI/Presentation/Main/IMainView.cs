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
   using Atf.ScreenRecorder.Recording;
   using Atf.ScreenRecorder.Screen;
   using System;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;
   public interface IMainView : IView {
      event EventHandler Cancel;
      event EventHandler CheckForUpdates;
      event EventHandler HelpTopics;
      event EventHandler OpenFolder;
      event EventHandler Options;
      event EventHandler Pause;
      event EventHandler Play;
      event EventHandler Record;
      event EventHandler Stop;
      event TrackerChangedEventHandler TrackerChanged;
      event EventHandler Update;
      event CancelEventHandler ViewClosing;
      bool AllowCancel {
         set;
      }
      bool AllowChangeTrackingType {
         set;
      }
      bool AllowOptions {
         set;
      }
      bool AllowPause {
         set;
      }
      bool AllowPlay {
         set;
      }
      bool AllowRecord {
         set;
      }
      bool AllowStop {
         set;
      }
      bool AllowUpdate {
         set;
      }
      Keys CancelHotKey {
         set;
      }
      bool Minimized {
         get;
         set;
      }
      Keys RecordHotKey {
         set;
      }
      Keys PauseHotKey {
         set;
      }
      TimeSpan RecordDuration {
         set;
      }
      RecordingState RecordingState {
         set;
      }
      Keys StopHotKey {
         set;
      }
      Rectangle TrackingBounds {
         set;
      }
      TrackingType TrackingType {
         set;
      }
      bool ShowCancelMessage();
      void ShowError(string message);
      void ShowHotKeyRegisterWarning();
      void ShowNoUpdate();
      bool ShowStopMessage();
      void ShowUpdateCheckError();
      bool ShowUpdateMessage(string p);
   }
}
