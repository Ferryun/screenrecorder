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
namespace Atf.ScreenRecorder.Recording {
   using Atf.ScreenRecorder.Avi;
   using Atf.ScreenRecorder.Screen;

   using System;
   using System.ComponentModel;
   using System.Diagnostics;
   using System.Drawing;
   using System.Threading;

   class Recorder {
      #region Events
      public event RecordErrorEventHandler Error;
      #endregion

      #region Delegates
      private delegate void RecordDelegate();
      #endregion

      #region Fields
      private static readonly int defaultFps = 15;
      private static readonly string errorMessage = "Recording interrupted: {0}";
      private static readonly int maxFps = 120;
      private static readonly int pauseDelay = 10; // ms
      private static readonly object syncRoot = new object();
      private AviCompressor compressor;
      private TimeSpan duration;
      private string fileName;
      private int fps = defaultFps;
      private RecordDelegate record;
      private bool recordCursor;
      private RecordingState state;
      private AutoResetEvent stateTransition = new AutoResetEvent(false);
      private IBoundsTracker tracker;
      #endregion

      #region Properties
      public AviCompressor Compressor {
         get {
            return this.compressor;
         }
         set {
            if (this.compressor != value) {
               if (this.state != RecordingState.Idle) {
                  throw new InvalidOperationException();
               }
               this.compressor = value;
            }
         }
      }
      public TimeSpan Duration {
         get {
            return this.duration;
         }
      }
      public string FileName {
         get {
            return this.fileName;
         }
         set {
            if (this.fileName != value) {
               if (this.state != RecordingState.Idle) {
                  throw new InvalidOperationException();
               }
               this.fileName = value;
            }
         }
      }
      public int Fps {
         get {
            return this.fps;
         }
         set {
            if (this.fps != value) {
               if (value <= 0 || value > maxFps) {
                  throw new ArgumentOutOfRangeException("Fps", string.Format("Fps must be between {0} and {1}", 
                                                                              0, maxFps));
               }
               if (this.state != RecordingState.Idle) {
                  throw new InvalidOperationException();
               }
               this.fps = value;
            }
         }
      }
      public RecordingState State {
         get {
            return this.state;
         }
      }
      public bool RecordCursor {
         get {
            return this.recordCursor;
         }
         set {
            if (this.recordCursor != value) {
               if (this.state != RecordingState.Idle) {
                  throw new InvalidOperationException();
               }
               this.recordCursor = value;
            }
         }
      }
      public IBoundsTracker Tracker {
         get {
            return this.tracker;
         }
         set {
            if (this.tracker != value) {
               if (this.state != RecordingState.Idle) {
                  throw new InvalidOperationException();
               }
               this.tracker = value;
            }
         }
      } 
      #endregion

      #region Constructors
      public Recorder() {
      }
      #endregion

      #region Methods      
      private void OnError(RecordErrorEventArgs e) {
         if (this.Error != null) {
            this.Error(this, e);
         }
      }
      public void Pause() {
         // Check state
         lock (syncRoot) {
            if (state != RecordingState.Recording) {
               throw new InvalidOperationException("Invalid state.");
            }
            state = RecordingState.Paused;
            this.stateTransition.WaitOne(); // Wait for the record thread to signal
         }
      }
      private void RaiseError(Exception e) {
         Exception exception = new Exception(string.Format(errorMessage, e.Message));
         RecordErrorEventArgs ea = new RecordErrorEventArgs(exception);
         this.OnError(ea);
      }
      public void Record() {
         if (this.fileName == null) {
            throw new InvalidOperationException("FileName is not specified");
         }
         if (this.tracker == null) {
            throw new InvalidOperationException("Tracker is not specified.");
         }
         if (this.compressor == null) {
            throw new InvalidOperationException("Compressor is not specified.");
         }
         // Check state
         lock (syncRoot) {
            if (state != RecordingState.Idle) {
               throw new InvalidOperationException("Invalid state.");
            }
            state = RecordingState.Initializing;
         }
         record = new RecordDelegate(this.RecordPrivate);
         AsyncCallback callback = new AsyncCallback(this.RecordCallBack);
         record.BeginInvoke(callback, null); // Start a new thread for recording
      }
      private void RecordCallBack(IAsyncResult result) {
         if (result.IsCompleted) {
            try {
               this.record.EndInvoke(result);
               this.stateTransition.Set(); // Let the executing thread of Stop() know that recording is stopped
            }
            catch (AviException ae) {
               this.state = RecordingState.Idle;
               this.stateTransition.Set(); // Let the executing thread of Stop() know that recording is stopped
               this.RaiseError(ae);
            }
            catch (ScreenshotException se) {
               this.state = RecordingState.Idle;
               this.stateTransition.Set(); // Let the executing thread of Stop() know that recording is stopped
               this.RaiseError(se);
            }
            catch (TrackingException te) {
               this.state = RecordingState.Idle;
               this.stateTransition.Set(); // Let the executing thread of Stop() know that recording is stopped
               this.RaiseError(te);
            }
         }
      }
      private void RecordPrivate() {      
         Screenshot screenShot = null;
         this.duration = TimeSpan.Zero;
         try {
            Rectangle bounds = this.tracker.Bounds;
            // Create an instance of screenshot class
            screenShot = new Screenshot();
            screenShot.Open(bounds.Width, bounds.Height);
            int bpp = screenShot.BitsPerPixel;
            AviFile aviFile = null;
            try {
               // Open AVI file               
               aviFile = new AviFile();
               aviFile.Open(fileName, bounds.Width, bounds.Height, bpp, fps, this.compressor);
               int frameIndex = 0;
               int frameDuration = (int)(1000 / this.fps);
               int frameBufferLength = screenShot.BufferLength;
               int startingFrameIndex = 0;
               long startTime = DateTime.Now.Ticks;
               // Update state
               lock (syncRoot) {
                  this.state = RecordingState.Recording;
               }
               do {
                  // Check if paused
                  if (this.state == RecordingState.Paused) {
                     this.stateTransition.Set(); // Let the thread executing Pause() know that pause is done
                     while (this.state == RecordingState.Paused) {
                        Thread.Sleep(pauseDelay);  // Sleep recording thread while paused
                     }
                     if (this.state == RecordingState.Idle) {
                        return; // Stop() is called. Signal will be done in the RecordCallBack()
                     }
                     this.stateTransition.Set(); // Resume() is called, let that executing thread known resume is done
                     startingFrameIndex = frameIndex;
                     startTime = DateTime.Now.Ticks;
                  }
                  // Take screenshot
#if (DEBUG)
                  DateTime beforeTake = DateTime.Now;
#endif
                  // Update bounds
	               bounds = this.tracker.Bounds;
                  // Take screenshot
                  screenShot.Take(bounds.Location, this.recordCursor);
#if (DEBUG)
                  Debug.WriteLine("Take delay: " + (DateTime.Now.Ticks - beforeTake.Ticks) / 10000);
#endif

#if (DEBUG)
                  DateTime beforeAddFrame = DateTime.Now;
#endif
                  // Add screenshot to the AVI file
                  aviFile.AddFrame(frameIndex, screenShot.Buffer, frameBufferLength);
#if (DEBUG)
                  Debug.WriteLine("AddFrame delay: " + (DateTime.Now.Ticks - beforeAddFrame.Ticks) / 10000);
#endif
                  frameIndex++;
                  Debug.WriteLine("Frame #" + frameIndex + " - " + DateTime.Now.TimeOfDay);
                  // Measure delay
                  long delay = (DateTime.Now.Ticks - startTime) / 10000 - 
                                frameDuration * ((frameIndex - startingFrameIndex) - 1);
                  if (delay < frameDuration) {
                     // Extra delay to synchornize with fps
                     Thread.Sleep((int)(frameDuration - delay));
                  }
                  else {
                     // Calculate how many frames are missed
                     int missedFrames = (int)Math.Floor((decimal)delay / frameDuration);
                     Debug.WriteLine("Missing " + missedFrames + " @ frame #" + frameIndex);
                     frameIndex += missedFrames;
                     // Extra delay to synchornize with fps
                     Thread.Sleep((int)(frameDuration - delay % frameDuration));
                  }
                  // Update duration
                  this.duration = new TimeSpan(0, 0, 0, 0, frameIndex * frameDuration);
               } while (this.state != RecordingState.Idle);
            }
            finally {
               if (aviFile != null) {
                  aviFile.Dispose();
               }
            }              
         }
         finally {
            if (screenShot != null) {
               screenShot.Dispose();
            }  
         }        
      }
      public void Resume() {
         // Check state
         lock (syncRoot) {
            if (state != RecordingState.Paused) {
               throw new InvalidOperationException("Invalid state.");
            }
            state = RecordingState.Recording;
            this.stateTransition.WaitOne(); // Wait for recording thread to signal
         }
      }
      public void Stop() {
         lock (syncRoot) {
            if (state == RecordingState.Idle) {
               throw new InvalidOperationException("Invalid state.");
            }
            state = RecordingState.Idle;
            this.stateTransition.WaitOne(); // Wait for recording thread to signal
         }
      }
      #endregion
   }
}
