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
   using Atf.ScreenRecorder.Sound;

   using System;
   using System.ComponentModel;
   using System.Diagnostics;
   using System.Drawing;
   using System.Drawing.Imaging;
   using System.Threading;
   using Atf.ScreenRecorder.Sound.Acm;

   public class Recorder {
      #region Events
      public event RecordErrorEventHandler Error;
      #endregion

      #region Delegates
      private delegate void RecordDelegate(DisplayProvider displayProvider, SoundProvider soundProvider);
      #endregion

      #region Fields
      private static readonly int defaultFps = 20;
      private static readonly string errorMessage = "Recording interrupted: {0}";
      private static readonly int maxFps = 120;
      private static readonly int msPerSecond = 1000;
      private static readonly int pauseDelay = 10; // ms
      private static readonly object syncRoot = new object();
      private static readonly int ticksPerMs = 10000;
      private static readonly int ticksPerSec = 10000000;

      private VideoCompressor compressor;
      private TimeSpan duration;
      private string fileName;
      private int fps = defaultFps;
      private RecordDelegate record;
      private RecordingState state;
      private AutoResetEvent stateTransition = new AutoResetEvent(false);
      #endregion

      #region Properties
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
      private int Fps {
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
      public VideoSettings VideoSettings {
         get {
            return new VideoSettings() {
               Compressor = this.compressor != null ? this.compressor.FccHandlerString : string.Empty,
               Fps = this.fps,
               Quality = this.compressor != null ? (int)this.compressor.Quality / 100 : 7500,
            };
         }
         set {
            if (this.state != RecordingState.Idle || value == null) {
               throw new InvalidOperationException();
            }
            // Try create specified compressor
            VideoCompressor videoCompressor = VideoCompressor.CreateOrDefault(value.Compressor);
            if (videoCompressor != null) {
               videoCompressor.Quality = (uint)(value.Quality * 100);
            }
            this.compressor = videoCompressor;
            this.fps = value.Fps;
         }
      }
      #endregion

      #region Methods
      private static int GetOutOfSyncSamples(SoundProvider soundProvider, int sampleIndex, TimeSpan duration, 
                                             int samplesRead) {
         SoundFormat sourceFormat = soundProvider.SourceFormat;
         SoundFormat audioFormat = soundProvider.Format;

         // Get number of source samples read and expected number of source
         double secondsPassed = (double)duration.Ticks / ticksPerSec;
         int expectedSamples = (int)(secondsPassed * sourceFormat.SamplesPerSecond);

         // Get sample ratio
         double samplesRatio = (double)sourceFormat.SamplesPerSecond / audioFormat.SamplesPerSecond;

         // Get number of samples in each packet
         int packetSamples = (int)((soundProvider.PacketLength * sourceFormat.SamplesPerSecond) / 1000.0);

         // Get total number of source samples read
         int totalSamplesRead = samplesRead + (int)(sampleIndex * samplesRatio);

         // Check if more than two packets are missing or out of sync
         if ((totalSamplesRead + 2 * packetSamples < expectedSamples) ||
             (totalSamplesRead > 2 * packetSamples + expectedSamples)) {
            return expectedSamples - totalSamplesRead;
         }
         // Few or zero samples are out of sync
         // We can ignore this now.
         return 0;
      }
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
      public void Record(DisplayProvider displayProvider, SoundProvider soundProvider) {
         if (this.fileName == null) {
            throw new InvalidOperationException("FileName is not specified");
         }
         // Check state
         lock (syncRoot) {
            if (state != RecordingState.Idle) {
               throw new InvalidOperationException("Invalid state.");
            }
            state = RecordingState.Preparing;
         }
         record = new RecordDelegate(this.RecordPrivate);
         AsyncCallback callback = new AsyncCallback(this.RecordCallBack);
         record.BeginInvoke(displayProvider, soundProvider, callback, null); // Start a new thread for recording
      }
      private void RecordCallBack(IAsyncResult result) {
         if (result.IsCompleted) {
            try {
               this.record.EndInvoke(result);
               this.stateTransition.Set(); // Let the executing thread of Stop() know that recording is stopped
            }
            catch (SRException e) {
               this.state = RecordingState.Idle;
               this.stateTransition.Set(); // Let the executing thread of Stop() know that recording is stopped
               this.RaiseError(e);
            }
         }
      }
      private void RecordPrivate(DisplayProvider displayProvider, SoundProvider soundProvider) {
         bool recordDisplay = displayProvider != null;
         bool recordSound = soundProvider != null;
         AviFile aviFile = null;
         AcmEncoder audioEncoder = null;

         this.duration = TimeSpan.Zero;
         try {
            DisplayFormat videoFormat = null;
            SoundFormat audioFormat = null;            

            int soundReadInterval = 0;
            if (recordDisplay) {
               displayProvider.Open();
               videoFormat = displayProvider.Format;
            }
            if (recordSound) {
               soundProvider.Open();
               soundReadInterval = (int)Math.Ceiling(soundProvider.BufferLength / 2.0); // ms
               audioFormat = soundProvider.Format;
               audioEncoder = soundProvider.GetEncoder();
            }
            // Open AVI file           
            aviFile = new AviFile();
            aviFile.Open(fileName, videoFormat, fps, this.compressor, audioFormat, audioEncoder);

            // Initialize helper variables
            int frameIndex = 0;
            int frameDuration = recordDisplay ? (int)(msPerSecond / this.fps) : 0;
            int frameBufferLength = recordDisplay ? displayProvider.BitmapBytes : 0;
            int startingFrameIndex = 0;
            int soundSampleIndex = 0;
            long startTime = DateTime.Now.Ticks;
            long lastSoundRead = DateTime.Now.Ticks;
            TimeSpan prevDuration = TimeSpan.Zero;
            TimeSpan currentDuration = TimeSpan.Zero;

            // Update state
            lock (syncRoot) {
               this.state = RecordingState.Recording;
            }
            if (recordSound) {
               // Start sound recording
               soundProvider.Start();
            }
            // Recording loop; this is a long one huh?!
            do {
               // Check if paused
               if (this.state == RecordingState.Paused) {
                  prevDuration = prevDuration.Add(currentDuration);
                  if (recordSound) {
                     // Read remaining sound data and stop sound recording
                     byte[] soundData = soundProvider.Read(true);
                     soundSampleIndex += aviFile.AddSound(soundSampleIndex, soundData, true);
                     soundProvider.Stop();
                  }
                  // Let the thread executing Pause() know that pause is done
                  this.stateTransition.Set();      
                  while (this.state == RecordingState.Paused) {
                     Thread.Sleep(pauseDelay);
                  }

                  // State is changed, check new state
                  if (this.state == RecordingState.Idle) {
                     return;
                  }

                  // Resume() is called          
                  if (recordSound) {
                     soundProvider.Start();
                     lastSoundRead = DateTime.Now.Ticks;
                  }
                  if (recordDisplay) {
                     startingFrameIndex = frameIndex;
                  }

                  // Reset duration variables
                  startTime = DateTime.Now.Ticks;
                  currentDuration = TimeSpan.Zero;

                  // Let that executing thread known resume is done
                  this.stateTransition.Set();
               }

               // Add a video from
               if (recordDisplay) {
                  // Render display and add rendered bitmap to the avi file
                  displayProvider.Render();
                  IntPtr pFrameData = displayProvider.Lock();
                  try { 
                     aviFile.AddFrame(pFrameData, frameIndex, 1, frameBufferLength);
                  }
                  finally {
                     displayProvider.Unlock();
                  }
                  frameIndex++;
               }           

               // Add sound
               if (recordSound) {
                  // Read recorded sound if it's time to do so
                  if ((DateTime.Now.Ticks - lastSoundRead) / ticksPerMs >= soundReadInterval) {
                     // Read sound data
                     SoundFormat sourceFormat = soundProvider.SourceFormat;
                     byte[] soundData = soundProvider.Read();
                     int samplesRead = (int)(soundData.Length / sourceFormat.BlockAlign);
                     
                     // Get number of out of sync samples
                     TimeSpan durationByNow = prevDuration + new TimeSpan(DateTime.Now.Ticks - startTime);
                     int nOutOfSyncSamples = GetOutOfSyncSamples(soundProvider, soundSampleIndex , durationByNow, 
                                                                 samplesRead);   
                     if (nOutOfSyncSamples > 0) {
                        // Add silence samples if we have less than expected samples
                        soundSampleIndex += aviFile.AddSilence(soundSampleIndex, nOutOfSyncSamples);
                     }
                     else if (nOutOfSyncSamples < 0) {
                        // Drop read samples as much as possible if we have more than expected samples
                        int nSamplesToKeep = Math.Max(0, samplesRead + nOutOfSyncSamples);
                        if (nSamplesToKeep > 0) {
                           int nBytesToKeep = nSamplesToKeep * sourceFormat.BlockAlign;
                           int nBytesToDrop = soundData.Length - nBytesToKeep;
                           byte[] droppedSoundData = new byte[nBytesToKeep];
                           Array.Copy(soundData, nBytesToDrop, droppedSoundData, 0, nBytesToKeep);
                           soundData = droppedSoundData;
                        }
                        samplesRead = nSamplesToKeep;
                     }
                     // Add sound data to the avi file
                     if (samplesRead > 0) {
                        soundSampleIndex += aviFile.AddSound(soundSampleIndex, soundData, false);
                     }
                     lastSoundRead = DateTime.Now.Ticks;
                  }
               }

               // Synchronize display
               if (recordDisplay) {
                  long delay = (DateTime.Now.Ticks - startTime) / ticksPerMs -
                                frameDuration * ((frameIndex - startingFrameIndex) - 1);
                  if (delay < frameDuration) {
                     // Extra delay to synchornize with fps
                     Thread.Sleep((int)(frameDuration - delay));
                  }
                  else {
                     // Calculate how many frames are lost
                     int lostFrames = (int)Math.Floor((decimal)delay / frameDuration);
                     frameIndex += lostFrames;
                     // Extra delay to synchornize with fps
                     Thread.Sleep((int)(frameDuration - delay % frameDuration));
                  }
               }
               else { /* No display recording, just sleep for a while so that sound buffers get filled  */
                  Thread.Sleep(1);
               }

               // Update duration
               currentDuration = new TimeSpan(DateTime.Now.Ticks - startTime);
               this.duration = prevDuration + currentDuration;

            } while (this.state != RecordingState.Idle);

            // Read remaining sound data and stop sound recording
            if (recordSound) {
               byte[] soundData = soundProvider.Read(true);
               soundSampleIndex += aviFile.AddSound(soundSampleIndex, soundData, true);
               soundProvider.Stop();
            }        
         }
         finally {            
            if (recordSound) {
               soundProvider.Close();
               if (audioEncoder != null) {
                  audioEncoder.Dispose();
               }
            }
            if (recordDisplay) {
               displayProvider.Close();
            }
            if (aviFile != null) {
               aviFile.Dispose();
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
         // Check state
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
