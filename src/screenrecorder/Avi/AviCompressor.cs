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
namespace Atf.ScreenRecorder.Avi {
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Runtime.InteropServices;

   class AviCompressor {
      #region Fields
      private static readonly ushort BitCount = 16;
      private static readonly int BiHeight = 20;
      private static readonly string[] DefaultCompressors = new string[] { "xvid", "msvc" };
      private static readonly uint DefaultQuality = 75;
      private uint fccHandler;
      private string fccHandlerString;
      private string name;
      private uint quality;
      private bool supportsQuality;
      #endregion

      #region Constructors
      private AviCompressor(uint fccHandler) {
         // Init bitmap info header
         Avi32Interop.BITMAPINFOHEADER bih = new Avi32Interop.BITMAPINFOHEADER();
         bih.Init();
         bih.biBitCount = BitCount;
         bih.biCompression = 0;
         bih.biWidth = bih.biHeight = BiHeight;
         bih.biPlanes = 1;
         // Open compressor
         IntPtr hic = Avi32Interop.ICOpen(Avi32Interop.ICTYPE_VIDEO, fccHandler, Avi32Interop.ICMODE_QUERY);
         if (hic == IntPtr.Zero) {
            int errorCode = Marshal.GetLastWin32Error();
            throw new AviException(errorCode, string.Format("ICOpen failed, error code = 0x{0:X8}.", errorCode));
         }
         try {
            // Check if compressor supports specified format
            int hr = Avi32Interop.ICCompressQuery(hic, ref bih, IntPtr.Zero);
            if (hr != Avi32Interop.ICERR_OK) {
               throw new AviException(hr, string.Format("ICCompressQuery failed, error code = 0x{0:X8}.", hr));
            }
            // Get compressor info
            Avi32Interop.ICINFO ici = new Avi32Interop.ICINFO();
            ici.Init();
            hr = Avi32Interop.ICGetInfo(hic, out ici, ici.biSize);
            if (hr != 0) {
               this.name = ici.szDescription;
               // Check quality support
               bool supportsQuality = (ici.dwFlags & Avi32Interop.VIDCF_QUALITY) == Avi32Interop.VIDCF_QUALITY;
               if (supportsQuality) {
                  if (Avi32Interop.ICERR_OK == Avi32Interop.ICGetDefaultQuality(hic, ref this.quality)) {
                     this.supportsQuality = true;
                  }
               }
               else {
                  this.quality = DefaultQuality;
               }
               this.fccHandler = fccHandler;
               this.fccHandlerString = FccHandlerToString(fccHandler);
            }
            else {
               throw new AviException(hr, string.Format("ICGetInfo failed, error code = 0x{0:X8}.", hr));               
            }
         }
         finally {
            Avi32Interop.ICClose(hic);
         }
      }
      public static AviCompressor Create(string fccHandler) {
         if (string.IsNullOrEmpty(fccHandler)) {
            throw new ArgumentNullException("fccHandler");
         }
         if (fccHandler.Length != 4) {
            throw new ArgumentException("fccHandler");
         }
         uint ffcHandler = Avi32Interop.mmioFOURCC(fccHandler[0], fccHandler[1], fccHandler[2], fccHandler[3]);
         AviCompressor compressor = new AviCompressor(ffcHandler);
         return compressor;
      }
      public static AviCompressor CreateOrDefault(string fccHandler) {
         try {
            return Create(fccHandler);
         }
         catch {
            return GetDefault();
         }
      }
      private static string FccHandlerToString(uint fccHandler) {
         char ch0 = (char)(fccHandler & 0xFF);
         char ch1 = (char)((fccHandler & 0xFF00) >> 8);
         char ch2 = (char)((fccHandler & 0xFF0000) >> 16);
         char ch3 = (char)((fccHandler & 0xFF000000) >> 24);
         return new string(new char[] { ch0, ch1, ch2, ch3 });
      }
      #endregion

      #region Properties
      internal uint FccHandler {
         get {
            return this.fccHandler;
         }
      }
      public string FccHandlerString {
         get {
            return this.fccHandlerString;
         }
      }
      public string Name {
         get {
            return this.name;
         }
      }
      public uint Quality {
         get {
            return this.quality;
         }
         set {
            this.quality = value;
         }
      }
      public bool SupportsQuality {
         get {
            return this.supportsQuality;
         }
      }
      #endregion

      #region Methods
      public bool About(IntPtr owner) {
         IntPtr hic = Avi32Interop.ICOpen(Avi32Interop.ICTYPE_VIDEO, this.fccHandler, Avi32Interop.ICMODE_QUERY);
         if (hic != IntPtr.Zero) {
            bool result = Avi32Interop.ICERR_OK == Avi32Interop.ICAbout(hic, owner);
            Avi32Interop.ICClose(hic);
            return result;
         }
         return false;
      }
      public bool Configure(IntPtr owner) {
         IntPtr hic = Avi32Interop.ICOpen(Avi32Interop.ICTYPE_VIDEO, this.fccHandler, Avi32Interop.ICMODE_QUERY);
         if (hic != IntPtr.Zero) {
            bool result = Avi32Interop.ICERR_OK == Avi32Interop.ICConfigure(hic, owner);
            Avi32Interop.ICClose(hic);
            return result;
         }
         return false;
      }
      public static uint FccHandlerFromString(string sFccHandler) {
         if (string.IsNullOrEmpty(sFccHandler)) {
            throw new ArgumentNullException("sFccHandler");
         }
         if (sFccHandler.Length != 4) {
            throw new ArgumentException("sFccHandler");
         }
         return Avi32Interop.mmioFOURCC(sFccHandler[0], sFccHandler[1], sFccHandler[2], sFccHandler[3]);
      }
      public static AviCompressor[] GetAll() {
         List<AviCompressor> compressors = new List<AviCompressor>();
         Avi32Interop.ICINFO ici = new Avi32Interop.ICINFO();
         ici.Init();
         int hr = int.MaxValue;
         for (uint i = 0; hr != 0; i++) {
            // Get info for compressor #i
            ici.biSize = (uint)Marshal.SizeOf(ici);
            hr = Avi32Interop.ICInfo(Avi32Interop.ICTYPE_VIDEO, i, ref ici);
            try {
               // Create compressor using fccHandler
               AviCompressor compressor = new AviCompressor(ici.fccHandler);
               compressors.Add(compressor);
            }
            catch (AviException) {
            }
         }
         return compressors.ToArray();
      }
      public static AviCompressor GetDefault() {
         // Try default compressors
         foreach (string defaultCompressor in DefaultCompressors) {
            try {
               uint fccHandler = FccHandlerFromString(defaultCompressor);
               return new AviCompressor(fccHandler);
            }
            catch (AviException) {
            }
         }
         // If no defualt compressor was found, simply return first one
         AviCompressor[] compressors = GetAll();
         if (compressors.Length > 0) {
            return compressors[0];
         }
         // If no compressor was found, return null
         return null;
      }
      public override string ToString() {
         return this.name;
      }
      #endregion

   }
}
