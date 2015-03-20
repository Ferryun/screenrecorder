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
   using System.Collections.Generic;
   using System.Runtime.InteropServices;
   public class WASAPISound : ISoundWrapper, IDisposable {
      #region Enums
      private enum RecordingState {
         Closed,
         Opened,
         Recording,
         Stopping,
      }
      #endregion

      #region Fields
      private static readonly int maxFormatSize = ushort.MaxValue;

      private uint bufferLength;
      private List<WSSoundInterop.DEVICE> callbackDevices;
      private List<SoundFormat> callbackDeviceFormats;
      private WSSoundInterop.WSEnumDevicesCallback deviceCallback;
      private WSSoundInterop.WSEnumDeviceFormatsCallback deviceFormatCallback;
      private bool disposed;
      private string deviceId;
      private SoundFormat format;
      private uint packetLength;
      private IntPtr pws;
      private RecordingState state;
      #endregion

      #region Constructors
      public WASAPISound() {
         this.InitWS();
         this.deviceCallback = new WSSoundInterop.WSEnumDevicesCallback(this.EnumDevicesCallback);
         this.deviceFormatCallback = new WSSoundInterop.WSEnumDeviceFormatsCallback(this.EnumDeviceFormatsCallback);
      }
      #endregion

      #region Methods
      private bool EnumDevicesCallback(IntPtr pws, ref WSSoundInterop.DEVICE device) {
         callbackDevices.Add(device);
         return true;
      }
      private bool EnumDeviceFormatsCallback(IntPtr lpws, string lpDevice, IntPtr pwfx) {
         SoundFormat soundFormat = new SoundFormat(pwfx);
         callbackDeviceFormats.Add(soundFormat);
         return true;
      }
      private void InitWS() {
         int hr = WSSoundInterop.WSInit(ref this.pws);
         if (hr != 0) {
            throw new SoundException("WSOpen", hr);
         }
      }
      private void UninitWS() {
         if (this.pws != IntPtr.Zero) {
            WSSoundInterop.WSUninit(this.pws);
            this.pws = IntPtr.Zero;
         }
      }
      #endregion

      #region ISoundWrapper Members
      public int BufferLength {
         get {
            if (this.state == RecordingState.Closed) {
               throw new InvalidOperationException();
            }
            return (int)this.bufferLength;            
         }
      }
      public string DeviceId {
         get {
            return this.deviceId;
         }
         set {
            int hr = WSSoundInterop.WSSetDeviceId(this.pws, value);
            if (hr != 0) {
               throw new SoundException("WSSetDeviceId", hr);
            }
            this.deviceId = value;
         }
      }
      public SoundFormat Format {
         get {
            return this.format;
         }
         set {
            IntPtr pwfx = IntPtr.Zero;
            if (value != null) {
               pwfx = value.ToPtr();
            }
            try {
               int hr = WSSoundInterop.WSSetFormat(this.pws, pwfx);
               if (hr != 0) {
                  throw new SoundException("WSSetFormat", hr);
               }
            }
            finally {
               if (pwfx != IntPtr.Zero) {
                  Marshal.FreeHGlobal(pwfx);
               }
            }
            this.format = value;
         }
      }
      public int PacketLength {
         get {
            if (this.state == RecordingState.Closed) {
               throw new InvalidOperationException();
            }
            return (int)this.packetLength;
         }
      }
      public void Close() {
         if (this.state == RecordingState.Recording) {
            this.Stop();
         }
         if (this.state == RecordingState.Opened) {
            WSSoundInterop.WSClose(this.pws);
            this.state = RecordingState.Closed;
         }
      }
      public SoundDevice[] GetDevices() {
         // Create a DEVICE object to get device info
         WSSoundInterop.DEVICE device = new WSSoundInterop.DEVICE();
         device.nMaxDesc = WSSoundInterop.WS_MAXNAMELEN;
         device.nMaxId = WSSoundInterop.WS_MAXNAMELEN;
         device.nMaxName = WSSoundInterop.WS_MAXNAMELEN;
         // Create device list to add devices into it during enumeration
         callbackDevices = new List<WSSoundInterop.DEVICE>();
         // Enumerate devices
         int hr = WSSoundInterop.WSEnumDevices(this.pws, ref device, this.deviceCallback);
         if (hr != 0) {
            throw new SoundException("WSGetNumDevices", hr);
         }
         // Convert returned DEVICEs to SoundDevice
         int nDevices = callbackDevices.Count;
         SoundDevice[] soundDevices = new SoundDevice[nDevices];
         for (int i = 0; i < nDevices; i++) {
            device = callbackDevices[i];         
            // Is device input or ouput (loopback)
            WSSoundInterop.DeviceType type = (WSSoundInterop.DeviceType)device.dwType;
            bool isLoopback = type == WSSoundInterop.DeviceType.Output;
            // Create Sound Device
            SoundDevice soundDevice = new SoundDevice(device.szId, device.szName, isLoopback, device.szDescription);
            soundDevices.SetValue(soundDevice, i);
         }
         callbackDevices.Clear();
         return soundDevices;
      }
      public SoundFormat[] GetDeviceFormats(string deviceId) {
         if (string.IsNullOrEmpty(deviceId)) {
            throw new ArgumentNullException("deviceId");
         }
         IntPtr pwfx = Marshal.AllocHGlobal(maxFormatSize);
         try {
            callbackDeviceFormats = new List<SoundFormat>();
            int hr = WSSoundInterop.WSEnumDeviceFormats(this.pws, deviceId, pwfx, maxFormatSize, deviceFormatCallback);
            if (hr != 0) {
               throw new SoundException("WSEnumDeviceFormats", hr);
            }
         }
         finally {
            Marshal.FreeHGlobal(pwfx);
         }
         SoundFormat[] formats = callbackDeviceFormats.ToArray();
         callbackDeviceFormats = null;
         return formats;
      }
      public void Open() {
         if (this.state != RecordingState.Closed) {
            throw new InvalidOperationException();
         }
         int hr = WSSoundInterop.WSOpen(this.pws);
         if (hr != 0) {
            throw new SoundException("WSOpen", hr);
         }
         hr = WSSoundInterop.WSGetBufferLength(this.pws, out this.bufferLength);
         if (hr != 0) {
            throw new SoundException("WSGetBufferLength", hr);
         }
         hr = WSSoundInterop.WSGetPacketLength(this.pws, ref this.packetLength);
         if (hr != 0) {
            throw new SoundException("WSGetBufferLength", hr);
         }
         this.state = RecordingState.Opened;
      }
      public int Read(byte[] buffer, int offset, int length, bool isEnd) {
         if (this.state != RecordingState.Recording) {
            throw new InvalidOperationException();
         }
         uint bytesRead;
         int hr = WSSoundInterop.WSRead(this.pws, buffer, (uint)offset, (uint)length, isEnd, out bytesRead);
         if (hr != 0) {
            throw new SoundException("WSRead", hr);
         }
         return (int)bytesRead;
      }
      public void Start() {
         if (this.state != RecordingState.Opened) {
            throw new InvalidOperationException();
         }
         int hr = WSSoundInterop.WSStart(this.pws);
         if (hr != 0) {
            throw new SoundException("WSStart", hr);
         }
         this.state = RecordingState.Recording;
      }
      public void Stop() {
         if (this.state == RecordingState.Opened || this.state == RecordingState.Closed) {
            throw new InvalidOperationException();
         }
         int hr = WSSoundInterop.WSStop(this.pws);
         if (hr != 0) {
            throw new SoundException("WSStop", hr);
         }
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
               this.UninitWS();
            }
            disposed = true;
         }
      }
      #endregion
   }
}
