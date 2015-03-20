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
namespace Atf.ScreenRecorder.Sound {
   using System;
   using System.Diagnostics;
   using System.Collections.Generic;
   using System.Runtime.InteropServices;
   using System.Threading;
   public class MMSound : IDisposable, ISoundWrapper {
      #region Enums
      private enum RecordingState {
         Closed,
         Opened,
         Recording,
         Stopping,
      }
      #endregion

      #region Fields
      private static readonly string[] loopbackDeviceNames = {
          "Stereo Mix", "Wave Out", "Sum", "What U Hear", "Loopback" 
      };
      private static readonly int defaultBufferCount = 20;
      private static readonly int defaultBufferLength = 200;//ms
      private static readonly int msPerSecond = 1000;
      private static readonly object syncRoot = new object();

      private int bufferCount = defaultBufferCount;
      private int bufferLength = defaultBufferLength;
      private MMInterop.waveInProc callback;
      private uint deviceId;
      private bool disposed;
      private SoundFormat format;
      private GCHandle[] gchHeaders;
      private IntPtr hWaveIn;
      private int nBytesLost;
      private Queue<byte[]> output;
      private IntPtr pwfx;
      private Thread recorderThread;
      private Semaphore recordingSemaphore;
      private RecordingState state;
      #endregion

      #region Constructors
      public MMSound() {
         this.callback = new MMInterop.waveInProc(this.WaveCallback);
      }
      #endregion

      #region Methods
      private GCHandle CreateHeader(int bufferSize) {
         // Create header
         int size = Marshal.SizeOf(typeof(MMInterop.WAVEHDR));
         MMInterop.WAVEHDR header = new MMInterop.WAVEHDR();
         header.dwBufferLength = (uint)bufferSize;
         header.lpData = Marshal.AllocHGlobal(bufferSize);

         // Prepare header
         // Wave headers need to be fixed in memory
         GCHandle gchHeader = GCHandle.Alloc(header, GCHandleType.Pinned);
         IntPtr pHeader = gchHeader.AddrOfPinnedObject();
         int mmr = MMInterop.waveInPrepareHeader(this.hWaveIn, pHeader, (uint)size);
         if (mmr != 0) {
            throw new SoundException("waveInPrepareHeader", mmr);
         }

         return gchHeader;
      }
      private void DeleteHeader(GCHandle gchHeader) {
         // Unprepare header
         int size = Marshal.SizeOf(typeof(MMInterop.WAVEHDR));
         IntPtr pHeader = gchHeader.AddrOfPinnedObject();
         MMInterop.waveInUnprepareHeader(this.hWaveIn, pHeader, (uint)size);

         // Free buffer (allocated in the CreateHeader method)
         MMInterop.WAVEHDR header = (MMInterop.WAVEHDR)gchHeader.Target;
         if (header.lpData != IntPtr.Zero) {
            Marshal.FreeHGlobal(header.lpData);
            header.lpData = IntPtr.Zero;
         }

         // Unpin header
         if (gchHeader.IsAllocated) {
            gchHeader.Free();
         }
      }
      private void Record() {
         // Create buffer queue
         Queue<GCHandle> gchHeaderQueue = new Queue<GCHandle>();         

         // Pin buffers and add them to queue
         int size = Marshal.SizeOf(typeof(MMInterop.WAVEHDR));
         int mmr;
         foreach (GCHandle gchHeader in this.gchHeaders) {
            IntPtr pHeader = gchHeader.AddrOfPinnedObject();
            mmr = MMInterop.waveInAddBuffer(this.hWaveIn, pHeader, (uint)size);
            if (mmr != 0) {
               throw new SoundException("waveInStart", mmr);
            }
            gchHeaderQueue.Enqueue(gchHeader);
         }
         // Start recording
         mmr = MMInterop.waveInStart(this.hWaveIn);
         if (mmr != 0) {
            throw new SoundException("waveInStart", mmr);
         }

         // Recording loop
         while (this.state != RecordingState.Opened) {
            // Wait a buffer become full
            this.recordingSemaphore.WaitOne();

            // Get the buffer from buffer queue
            GCHandle gchHeader = gchHeaderQueue.Dequeue();
            MMInterop.WAVEHDR header = (MMInterop.WAVEHDR)gchHeader.Target;

            // Copy data from the buffer to the output queue
            int bytesRecorded = (int)header.dwBytesRecorded;
            if (bytesRecorded > 0) {
               byte[] data = new byte[bytesRecorded];
               Marshal.Copy(header.lpData, data, 0, bytesRecorded);
               lock (syncRoot) {
                  if (output.Count == bufferCount) { // Is output queue full?
                     // Drop an item
                     byte[] outputData = output.Dequeue();
                     this.nBytesLost += outputData.Length;
                  }
                  // Add data to output queue
                  output.Enqueue(data);
               }
            }
            if (this.state == RecordingState.Stopping) {
               if (gchHeaderQueue.Count == 0) { // All the buffers are done
                  break; // No more recording
               }
            }
            else if (this.state == RecordingState.Recording) { // Recording is not stopped
               lock (syncRoot) {
                  // Make sure recording is not stopped
                  if (this.state == RecordingState.Recording) {
                     IntPtr pHeader = gchHeader.AddrOfPinnedObject();
                     mmr = MMInterop.waveInAddBuffer(this.hWaveIn, pHeader, (uint)size);
                     if (mmr != 0) {
                        throw new SoundException("waveInAddBuffer", mmr);
                     }
                     gchHeaderQueue.Enqueue(gchHeader);
                  }
               }
            }
         }
         // Stop recording 
         mmr = MMInterop.waveInStop(this.hWaveIn);
      }
      private void WaveCallback(IntPtr hwi, MMInterop.WIMMessages uMsg, IntPtr dwInstance, IntPtr dwParam1,
                                IntPtr dwParam2) {
         if (uMsg == MMInterop.WIMMessages.MM_WIM_DATA) {
            // Let the recorder thread know another buffer is full
            this.recordingSemaphore.Release();
         }
      }
      #endregion

      #region ISoundWrapper Members
      public int BufferLength {
         get {
            return this.bufferLength * this.bufferCount;
         }
      }
      public string DeviceId {
         get {
            return this.deviceId.ToString();
         }
         set {
            if (this.state != RecordingState.Closed) {
               throw new InvalidOperationException();
            }
            uint uintvalue = 0;
            if (uint.TryParse(value, out uintvalue)) {
               this.deviceId = uintvalue;
            }
            else {
               throw new InvalidOperationException();
            }
         }
      }
      public SoundFormat Format {
         get {
            return this.format;
         }
         set {
            if (this.state != RecordingState.Closed) {
               throw new InvalidOperationException();
            }
            this.format = value;
            // TODO: CHECK FORMAT
         }
      }
      public int PacketLength {
         get {
            return this.bufferLength;
         }
      }
      public void Close() {
         // Stop recording first
         if (this.state == RecordingState.Recording) {
            this.Stop();
         }
         // Delete headers (and associated buffers)
         if (this.gchHeaders != null) {
            foreach (GCHandle gchHeader in this.gchHeaders) {
               this.DeleteHeader(gchHeader);
            }
            this.gchHeaders = null;
         }
         // Close wave handle
         if (this.hWaveIn != IntPtr.Zero) {
            MMInterop.waveInClose(this.hWaveIn);
            this.hWaveIn = IntPtr.Zero;
         }
         if (this.pwfx != IntPtr.Zero) {
            Marshal.FreeHGlobal(this.pwfx);
            this.pwfx = IntPtr.Zero;
         }
         this.output = null;
         this.state = RecordingState.Closed;
      }
      public SoundDevice[] GetDevices() {
         uint numDevices = MMInterop.waveInGetNumDevs();
         List<SoundDevice> devices = new List<SoundDevice>((int)numDevices);
         for (uint i = 0; i < numDevices; i++) {
            // Get device capabilities
            MMInterop.WAVEINCAPS caps = new MMInterop.WAVEINCAPS();
            int mmr = MMInterop.waveInGetDevCaps(i, ref caps, (uint)Marshal.SizeOf(caps));
            if (mmr == MMInterop.MMSYSERR_NODRIVER) {
               // No device driver is present.
               continue;
            }
            if (mmr != 0) {
               throw new SoundException("waveInGetDevCaps", mmr);
            }
            // Compare device name to loopback device names to see if
            // it is a loopback device
            bool isLoopback = false;
            foreach (string loopbackName in loopbackDeviceNames) {
               if (loopbackName.Equals(caps.szPname)) {
                  isLoopback = true;
                  break;
               }
            }
            SoundDevice device = new SoundDevice(i.ToString(), caps.szPname, isLoopback);
            devices.Add(device);
         }
         return devices.ToArray();
      }
      public SoundFormat[] GetDeviceFormats(string deviceId) {
         if (string.IsNullOrEmpty(deviceId)) {
            throw new ArgumentNullException("deviceId");
         }
         uint uintId;
         if (!uint.TryParse(deviceId, out uintId)) {
            throw new ArgumentException("deviceId");
         }
         uint nDevice = MMInterop.waveInGetNumDevs();
         if (uintId <= nDevice - 1) {
            MMInterop.WAVEINCAPS caps = new MMInterop.WAVEINCAPS();
            int mmr = MMInterop.waveInGetDevCaps(uintId, ref caps, (uint)Marshal.SizeOf(caps));
            if (mmr != 0) {
               throw new SoundException("waveInGetDevCaps", mmr);
            }
            return Util.WaveFormatToSoundFormats((MMInterop.WaveFormat)caps.dwFormats);
         }
         throw new SoundException("Device not found.");
      }
      public void Open() {
         if (this.state != RecordingState.Closed) {
            throw new InvalidOperationException();
         }
         if (format == null) {
            throw new InvalidOperationException("Format is not specified.");
         }
         // Open wave
         this.pwfx = this.format.ToPtr();
         int mmr = MMInterop.waveInOpen(ref this.hWaveIn, this.deviceId, this.pwfx, this.callback, 0,
                                         MMInterop.CALLBACK_FUNCTION);
         if (mmr != 0) {
            throw new SoundException("waveInOpen", mmr);
         }
         try {
            // Initialise buffers
            int bufferSize = (int)(((double)this.bufferLength / msPerSecond) * this.format.AverageBytesPerSecond);
            this.gchHeaders = new GCHandle[bufferCount];
            if (bufferSize % this.format.BlockAlign != 0) {
               bufferSize += this.format.BlockAlign - (bufferSize % this.format.BlockAlign);
            }
            for (int i = 0; i < bufferCount; i++) {
               GCHandle gchHeader = this.CreateHeader(bufferSize);
               this.gchHeaders.SetValue(gchHeader, i);
            }
            this.nBytesLost = 0;
            this.output = new Queue<byte[]>();
            this.state = RecordingState.Opened;
         }
         finally {
            if (this.state != RecordingState.Opened) {
               this.Close();
            }
         }
      }
      public int Read(byte[] buffer, int offset, int length, bool isEnd) {
         if (buffer == null) {
            throw new ArgumentNullException("buffer");
         }
         int bytesRead = 0;
         if (isEnd && this.state == RecordingState.Recording) {
            lock (syncRoot) {
               this.state = RecordingState.Stopping;
               // Get remaining data from the buffer, this causes a call to the callback function
               int mmr = MMInterop.waveInReset(this.hWaveIn);
               if (mmr != 0) {
                  throw new SoundException("waveInReset", mmr);
               }
            }
            // Wait for the recorder thread to stop recording if it is still alive
            if (this.recorderThread.IsAlive) {
               this.recorderThread.Join();
            }
         }
         // Copy data from output queue to the buffer parameter
         lock (syncRoot) {
            if (nBytesLost > 0) {
               int nEmptyDataBytes = Math.Min(nBytesLost, length);
               for (int i = 0; i < nEmptyDataBytes; i++) {
                  buffer[offset + i] = 0;
               }
               nBytesLost -= nEmptyDataBytes;
               bytesRead += nEmptyDataBytes;
            }
            while (bytesRead < length && output.Count > 0) {
               byte[] data = output.Peek();
               int dataLength = data.Length;
               if (dataLength <= (length - bytesRead)) {
                  Array.Copy(data, 0, buffer, bytesRead, dataLength);
                  bytesRead += dataLength;
                  output.Dequeue();
               }
               else {
                  if (isEnd) {
                     // Copy data from the queue to the buffer as much as possible
                     // since it is last read
                     Array.Copy(data, 0, buffer, bytesRead, length - bytesRead);
                  }
                  break;
               }
            }
            if (isEnd) {
               output.Clear();
            }
         }
         return bytesRead;
      }
      public void Start() {
         if (this.state != RecordingState.Opened) {
            throw new InvalidOperationException();
         }
         // Initialize controller semaphore
         this.recordingSemaphore = new Semaphore(0, this.bufferCount + 1);
         // Create recorder thread
         this.recorderThread = new Thread(new ThreadStart(this.Record));
         // Start
         this.state = RecordingState.Recording;
         this.recorderThread.Start();
      }
      public void Stop() {
         if (this.state == RecordingState.Opened || this.state == RecordingState.Closed) {
            throw new InvalidOperationException();
         }
         if (this.state == RecordingState.Recording) {
            this.state = RecordingState.Stopping;
            // Get remaining data from the buffer, this causes a call to the callback function
            int mmr = MMInterop.waveInReset(this.hWaveIn);
            if (mmr != 0) {
               throw new SoundException("waveInReset", mmr);
            }
            // Wait for the recorder thread to stop recording if it is still alive
            if (this.recorderThread.IsAlive) {
               this.recorderThread.Join();
            }
         }
         // Close stuff
         this.recordingSemaphore.Close();
         // Here recording is stopped
         this.state = RecordingState.Opened;
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
      protected virtual void Dispose(bool disposing) {
         if (!this.disposed) {
            if (disposing) {
               this.Close();
            }
            disposed = true;
         }
      }
      #endregion
   }
}