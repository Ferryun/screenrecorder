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
   using Atf.ScreenRecorder.Sound.Acm;
   using System;
   using System.Collections.Generic;
   public class SoundProvider : IDisposable {
      #region Fields
      private static AcmConvertionMap convertionMap = new AcmConvertionMap();
      private static readonly ISoundWrapper sharedWrapper = SoundWrapperFactory.Create();
      private static readonly SoundFormat preferredFormat = new SoundFormat(SoundFormatTag.PCM, 8, 1, 22050);

      private string deviceId;
      private bool disposed;
      // private AcmEncoder encoder;
      private SoundFormat format;
      private bool opened;
      private ISoundWrapper wrapper;
      #endregion

      #region Constructors
      public SoundProvider() {
         this.wrapper = SoundWrapperFactory.Create();
         SoundDevice defaultDevice = GetDeviceOrDefault(null);
         //if (defaultDevice == null) {
         //   this.deviceId = defaultDevice.Id;
         //}
      }
      #endregion

      #region Properties
      public int BufferLength {
         get {
            if (!this.opened) {
               throw new InvalidOperationException();
            }
            return this.wrapper.BufferLength;
         }
      }
      public string DeviceId {
         get {
            return this.deviceId;
         }
         set {
            if (this.opened) {
               throw new InvalidOperationException();
            }
            this.deviceId = value;
         }
      }
      public SoundFormat Format {
         get {
            return this.format;
         }
         set {
            if (this.opened) {
               throw new InvalidOperationException();
            }
            this.format = value;
         }
      }
      public int PacketLength {
         get {
            if (!this.opened) {
               throw new InvalidOperationException();
            }
            return this.wrapper.PacketLength;
         }
      }
      public SoundFormat SourceFormat {
         get {
            if (!this.opened) {
               throw new InvalidOperationException();
            }
            return this.wrapper.Format;
         }
      }
      #endregion

      #region Methods
      public void Close() {
         this.wrapper.Close();
         this.opened = false;
      }     
      private static SoundDevice GetDevice(string deviceId) {
         if (string.IsNullOrEmpty(deviceId)) {
            throw new ArgumentNullException();
         }
         SoundDevice[] soundDevices = sharedWrapper.GetDevices();
         foreach (SoundDevice soundDevice in soundDevices) {
            if (deviceId.Equals(soundDevice.Id)) {
               return soundDevice;
            }
         }
         throw new SoundException("Device not found");
      }
      public static SoundDevice GetDeviceOrDefault(string deviceId) {         
         SoundDevice[] soundDevices = sharedWrapper.GetDevices();
         if (!string.IsNullOrEmpty(deviceId)) {
            foreach (SoundDevice soundDevice in soundDevices) {
               if (deviceId.Equals(soundDevice.Id)) {
                  return soundDevice;
               }
            }
         }
         if (soundDevices.Length > 0) {
            return soundDevices[0];
         }
         return null;
      }
      public static SoundDevice[] GetDevices() {
         return sharedWrapper.GetDevices();
      }
      private SoundFormat GetInputFormat() {
         bool encoderNeeded;
         return GetInputFormat(out encoderNeeded);
      }
      private SoundFormat GetInputFormat(out bool encoderNeeded) {
         int nMaxAvgBytesPerSec = 0;
 
         // Get device formats
         SoundFormat[] deviceFormats = this.wrapper.GetDeviceFormats(deviceId);
         List<SoundFormat> deviceFormatList = new List<SoundFormat>(deviceFormats);
         SoundFormat inputFormat = null;
         if (this.format == null) {
            // If format is not specified, find the format with maximum average bytes per second
            foreach (SoundFormat deviceFormat in deviceFormatList) {
               if (inputFormat == null || nMaxAvgBytesPerSec < deviceFormat.AverageBytesPerSecond) {
                  inputFormat = deviceFormat;
                  nMaxAvgBytesPerSec = deviceFormat.AverageBytesPerSecond;
               }
            }
            if (inputFormat == null) {
               // This happens only if device has not formats
               throw new InvalidOperationException("Cannot find an appropriate input format.");
            }
            encoderNeeded = false;
            return inputFormat;
         }

         // Check if device supports the format   
         if (deviceFormatList.Contains(this.format)) {
            encoderNeeded = false;
            return this.format;
         }

         // Get available input formats for convertion
         SoundFormat[] availableInputs = convertionMap.GetInputs(this.format);
         if (availableInputs.Length == 0) {
            // Get convertion map again
            // We currenty use PCM format for output.
            convertionMap.Add(AcmEncoder.GetConvertionMap(deviceFormatList.ToArray(), preferredFormat.Tag));
            // Get available input formats for convertion
            availableInputs = convertionMap.GetInputs(this.format);
            if (availableInputs.Length == 0) {
               throw new InvalidOperationException("Cannot find an appropriate input format.");
            }
         }

         // Find the input format that device supports and has
         // maximum average bytes per second     
         foreach (SoundFormat input in availableInputs) {
            if (deviceFormatList.Contains(input)) {
               if (nMaxAvgBytesPerSec < input.AverageBytesPerSecond &&
                  (inputFormat == null ||
                  input.AverageBytesPerSecond == (input.BitsPerSample / 8) * input.Channels * input.SamplesPerSecond)) {
                  inputFormat = input;
                  nMaxAvgBytesPerSec = (int)input.AverageBytesPerSecond;
               }
            }
         }
         if (inputFormat == null) {
            throw new InvalidOperationException("Cannot find an appropriate input format.");
         }
         encoderNeeded = true;
         return inputFormat;
      }
      public AcmEncoder GetEncoder() {
         bool encoderNeeded;
         SoundFormat inputFormat = GetInputFormat(out encoderNeeded);
         if (!encoderNeeded) {
            return null;
         }
         AcmEncoder encoder = new AcmEncoder();
         encoder.InputFormat = inputFormat;
         encoder.OutputFormat = this.format;
         return encoder;
      }
      public static SoundFormat[] GetFormats(string deviceId, bool queryEncoderFormats) {
         // Get device by id
         if (string.IsNullOrEmpty(deviceId)) {
            throw new ArgumentNullException("deviceId");
         }
         if (!queryEncoderFormats) {
            return sharedWrapper.GetDeviceFormats(deviceId);
         }
         // Get device formats
         SoundFormat[] deviceFormats = sharedWrapper.GetDeviceFormats(deviceId);
         // Get list of input formats that has not been quieried yet
         List<SoundFormat> newInputFormats = new List<SoundFormat>();
         foreach (SoundFormat deviceFormat in deviceFormats) {
            // Any convertion exists?
            if (!convertionMap.Contains(deviceFormat) && deviceFormat.Tag == preferredFormat.Tag) {
               newInputFormats.Add(deviceFormat);
            }
         }
         // Is there any new format to query
         if (newInputFormats.Count > 0) {
            var encoderConvertionMap = AcmEncoder.GetConvertionMap(newInputFormats.ToArray(), preferredFormat.Tag);
            // Add new map to the current map
            convertionMap.Add(encoderConvertionMap);
            // Add no convertion -input format as output format- to the map
            foreach (SoundFormat newInputFormat in newInputFormats) {
               convertionMap.Add(newInputFormat, newInputFormat);
            }
         }
         // Get all of the output formats matching device formats as input
         return convertionMap.GetOutputs(deviceFormats);
      }
      public byte[] Read() {
         return Read(false);
      }
      public byte[] Read(bool isEnd) {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         // Allocate buffer
         int bufferLength = this.wrapper.BufferLength;
         int bufferSize = (bufferLength / 1000) * this.SourceFormat.AverageBytesPerSecond;
         byte[] buffer = new byte[bufferSize];

         // Read data
         int bytesRead = this.wrapper.Read(buffer, 0, buffer.Length, isEnd);         
 
         // Resize buffer
         Array.Resize(ref buffer, bytesRead);

         return buffer;
      }
       public void Open() {
         if (this.opened) {
            throw new InvalidOperationException();
         }
         if (string.IsNullOrEmpty(this.deviceId)) {
            throw new InvalidOperationException("Sound device is not specified.");
         }
         if (this.format == null) {
            throw new InvalidOperationException("Format is not specified.");
         }
         this.wrapper.DeviceId = this.deviceId;
         this.wrapper.Format = this.GetInputFormat();
         this.wrapper.Open();
         this.opened = true;
      }
      public void Start() {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         wrapper.Start();
      }
      public void Stop() {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         wrapper.Stop();
      }
      public static SoundFormat SuggestFormat(string deviceId, SoundFormatTag? tag, int? samplesPerSecond,
                                              int? channels) {
         if (string.IsNullOrEmpty(deviceId)) {
            throw new ArgumentNullException("deviceId");
         }
         if (tag == null) {
            tag = preferredFormat.Tag;
         }
         if (samplesPerSecond == null) {
            samplesPerSecond = preferredFormat.SamplesPerSecond;
         }
         if (channels == null) {
            channels = preferredFormat.Channels;
         }
         SoundFormat[] availableFormats = GetFormats(deviceId, true);
         // Find the closest format to the specified paramters
         // Priorities: 1) Tag 2) Sample Rate 3) Channels
         SoundFormat suggestedFormat = null;
         float suggestedMatchValue = int.MinValue;
         foreach (SoundFormat format in availableFormats) {
            float matchValue = 0;
            if (tag != null && format.Tag == tag.Value) {
               matchValue += 4.0f;
            }            
            if (samplesPerSecond != null) {
               float diffRatio = 2.0f - (Math.Abs(format.SamplesPerSecond - samplesPerSecond.Value) 
                                      / (samplesPerSecond.Value + format.SamplesPerSecond + 1.0f));
               matchValue += diffRatio;
            }
            if (channels != null && channels.Value == format.Channels) {
               matchValue += 1.0f;
            }
            if (matchValue > suggestedMatchValue) {
               suggestedFormat = format;
               suggestedMatchValue = matchValue;
            }
         }
         return suggestedFormat;
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
