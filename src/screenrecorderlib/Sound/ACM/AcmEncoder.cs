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
namespace Atf.ScreenRecorder.Sound.Acm {
   using System;
   using System.Collections.Generic;
   using System.Runtime.InteropServices;
   public class AcmEncoder : IDisposable {
      #region Feilds
      private static readonly int defaultBufferLength = 4000; // ms.
      private static int[] driverIds;
      private static List<int> callbackDriverIdList;
      private static List<SoundFormat> callbackFormats;
      private static int maxFormatSize;

      private int bufferLength = defaultBufferLength;
      private bool disposed;
      private AcmInterop.ACMSTREAMHEADER header;
      private GCHandle headerGCHandle;
      private SoundFormat inputFormat;
      private bool isStart;
      private bool opened;
      private SoundFormat outputFormat;
      private IntPtr pStream;
      private IntPtr pwfxDest;
      private IntPtr pwfxSource;
      #endregion

      #region Properties
      public int BufferLength {
         get {
            return this.bufferLength;
         }
         set {
            if (this.opened) {
               throw new InvalidOperationException();
            }
            this.bufferLength = value;
         }
      }
      public SoundFormat InputFormat {
         get {
            return this.inputFormat;
         }
         set {
            if (this.opened) {
               throw new InvalidOperationException();
            }
            this.inputFormat = value;
         }
      }
      public SoundFormat OutputFormat {
         get {
            return this.outputFormat;
         }
         set {
            if (this.opened) {
               throw new InvalidOperationException();
            }
            this.outputFormat = value;
         }
      }
      #endregion

      #region Methods
      public void Close() {
         int mmr;
         if (this.opened) {
            AcmInterop.acmStreamUnprepareHeader(this.pStream, ref this.header, 0);
         }
         if (headerGCHandle.IsAllocated) {
            headerGCHandle.Free();
         }
         if (this.header.pbDst != IntPtr.Zero) {
            Marshal.FreeHGlobal(this.header.pbDst);
            this.header.pbDst = IntPtr.Zero;
         }
         if (this.header.pbSrc != IntPtr.Zero) {
            Marshal.FreeHGlobal(this.header.pbSrc);
            this.header.pbSrc = IntPtr.Zero;
         }       
         if (this.pStream != IntPtr.Zero) {
            mmr = AcmInterop.acmStreamClose(this.pStream, 0);
            this.pStream = IntPtr.Zero;
         }
         this.opened = false;
      }
      public byte[] Convert(byte[] source, int offset, int length) {
         return this.Convert(source, offset, length, false);
      }
      public byte[] Convert(byte[] source, int offset, int length, bool isEnd) {
         if (!this.opened) {
            throw new InvalidOperationException();
         }
         // Copy data to source pointer
         this.header.cbSrcLength = length;
         this.header.cbSrcLengthUsed = 0;
         this.header.cbDstLengthUsed = 0;
         Marshal.Copy(source, offset, this.header.pbSrc, length);

         // Convert source
         AcmConvertFlags flags = AcmConvertFlags.None;
         if (!isStart && !isEnd) {
            flags = AcmConvertFlags.BlockAlign;
         }
         if (this.isStart) {
            flags |= AcmConvertFlags.Start;
            this.isStart = false;
         }
         if (isEnd) {
            flags |= AcmConvertFlags.End;
         }    
         int mmr = AcmInterop.acmStreamConvert(this.pStream, ref this.header, (int)flags);
         if (mmr != 0) {
            throw new SoundException("acmStreamConvert", mmr);
         }
         // Copy data from source pointer to byte array
         int destLength = this.header.cbDstLengthUsed;
         byte[] dest = new byte[destLength];
         Marshal.Copy(this.header.pbDst, dest, 0, destLength);

         return dest;
      }
      private static bool DriverCallback(int hadid, UIntPtr dwInstance, int fdwSupport) {
         callbackDriverIdList.Add(hadid);
         return true;
      }
      private static bool FormatEnumCallback(IntPtr had, ref AcmInterop.ACMFORMATDETAILS pafd, UIntPtr dwInstance,
                                             int fdwSupport) {
         if (pafd.cbwfx >= Marshal.SizeOf(typeof(MMInterop.WAVEFORMATEX))) {
            SoundFormat soundFormat = new SoundFormat(pafd.pwfx);
            callbackFormats.Add(soundFormat);
         }
         return true;
      }
      public static int[] GetDriverIds() {
         if (driverIds != null) {
            return driverIds;
         }
         callbackDriverIdList = new List<int>();
         int mmr = AcmInterop.acmDriverEnum(DriverCallback, 0, 0);
         if (mmr != 0) {
            throw new SoundException("acmDriverEnum", mmr);
         }
         driverIds = callbackDriverIdList.ToArray();
         return driverIds;
      }
      public static AcmConvertionMap GetConvertionMap(SoundFormat[] inputFormats, SoundFormatTag tagFilter) {
         // First, we enumerate convertion formats
         AcmConvertionMap initConvertionMap = new AcmConvertionMap();
         int maxFormatSize = GetMaxFormatSize();
         
         // Enumerate acm drivers
         foreach (int driverId in GetDriverIds()) {
            // Open driver
            IntPtr phDriver;
            int mmr = AcmInterop.acmDriverOpen(out phDriver, driverId, 0);
            if (mmr != 0) {
               continue;
            }
            // For each input format, we do enumeration
            foreach (SoundFormat inputFormat in inputFormats) {

               // Fill format details struct
               AcmInterop.ACMFORMATDETAILS fmtDetails = new AcmInterop.ACMFORMATDETAILS();
               IntPtr pwfxFormat = inputFormat.ToPtr(maxFormatSize);
               fmtDetails.cbStruct = Marshal.SizeOf(fmtDetails);
               fmtDetails.pwfx = pwfxFormat;
               fmtDetails.cbwfx = maxFormatSize;    

               // Enumerate convertion formats
               callbackFormats = new List<SoundFormat>();
               IntPtr pwfxInput = inputFormat.ToPtr();
               mmr = AcmInterop.acmFormatEnum(phDriver, ref fmtDetails, FormatEnumCallback, IntPtr.Zero,
                                              AcmInterop.ACM_FORMATENUMF_CONVERT);
               Marshal.FreeHGlobal(pwfxInput);

               // Add formats to the map (if succeed)
               if (mmr == 0) {
                  initConvertionMap.Add(inputFormat, callbackFormats);
               }
               callbackFormats = null;
            }

            // Close driver
            mmr = AcmInterop.acmDriverClose(phDriver, 0);
         }

         // Now we query ACM to make sure each convertion is supported
         AcmConvertionMap finalConvertionMap = new AcmConvertionMap();
         SoundFormat[] inputs = initConvertionMap.GetInputs();
         foreach (SoundFormat inputFormat in inputs) {
            IntPtr pwfxSrc = inputFormat.ToPtr();
            foreach (SoundFormat outputFormat in initConvertionMap.GetOutputs(inputFormat)) {
               // Filter tags
               if (tagFilter != SoundFormatTag.UNKNOWN && outputFormat.Tag != tagFilter) {
                  continue;
               }
               IntPtr phs;
               IntPtr pwfxDst = outputFormat.ToPtr();
               // Open acm stream using the query flag
               int mmr = AcmInterop.acmStreamOpen(out phs, IntPtr.Zero, pwfxSrc, pwfxDst, IntPtr.Zero, IntPtr.Zero,
                                                  UIntPtr.Zero, AcmInterop.ACM_STREAMOPENF_QUERY);
               Marshal.FreeHGlobal(pwfxDst);

               // Add format to the final map if succeed
               if (mmr == 0) {
                  finalConvertionMap.Add(inputFormat, outputFormat);
               }
            }
            Marshal.FreeHGlobal(pwfxSrc);
         }
         return finalConvertionMap;
      }
      public static int GetMaxFormatSize() {
         if (maxFormatSize > 0) {
            return maxFormatSize;
         }
         // Get maximum size of WAVEFORMATEX
         int mmr = AcmInterop.acmMetrics(IntPtr.Zero, AcmInterop.ACM_METRIC_MAX_SIZE_FORMAT, out maxFormatSize);
         if (mmr != 0) {
            throw new SoundException("acmMetrics", mmr);
         }
         return maxFormatSize;
      }
      public void Open() {
         if (this.opened) {
            throw new InvalidOperationException();
         }
         if (this.inputFormat == null) {
            throw new InvalidOperationException("Input format is not specified.");
         }
         if (this.outputFormat == null) {
            throw new InvalidOperationException("Output format is not specified.");
         }
         this.pwfxSource = this.inputFormat.ToPtr();
         this.pwfxDest = this.outputFormat.ToPtr();
         this.outputFormat = new SoundFormat(this.pwfxDest);

         int mmr = AcmInterop.acmStreamOpen(out this.pStream, IntPtr.Zero, this.pwfxSource, this.pwfxDest,
                                            IntPtr.Zero, IntPtr.Zero, UIntPtr.Zero, 0);
         if (mmr != 0) {
            throw new SoundException("acmStreamOpen", mmr);
         }
         int cbSrcLength = (int)((this.bufferLength / 1000.0) * this.inputFormat.AverageBytesPerSecond);
         this.header = new AcmInterop.ACMSTREAMHEADER();
         this.header.cbStruct = Marshal.SizeOf(this.header);
         this.header.cbSrcLength = cbSrcLength;
         int suggestedDstLength;
         mmr = AcmInterop.acmStreamSize(this.pStream, cbSrcLength, out suggestedDstLength,
                                        AcmInterop.ACM_STREAMSIZEF_SOURCE);
         try {
            this.header.cbDstLength = suggestedDstLength;
            this.header.pbDst = Marshal.AllocHGlobal(suggestedDstLength);
            this.header.pbSrc = Marshal.AllocHGlobal(cbSrcLength);
            this.headerGCHandle = GCHandle.Alloc(this.header, GCHandleType.Pinned);
            mmr = AcmInterop.acmStreamPrepareHeader(this.pStream, ref this.header, 0);
            if (mmr != 0) {
               throw new SoundException("acmStreamPrepareHeader", mmr);
            }
            this.isStart = true;
            this.opened = true;
         }
         finally {
            if (!this.opened) {
               this.Close();
            }
         }
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
