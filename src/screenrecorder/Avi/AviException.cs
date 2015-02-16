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
   using System.Runtime.Serialization;
   [Serializable]
   public class AviException : Exception {
      public AviException() { }
      public AviException(int errorCode) {
         base.HResult = errorCode;
      }
      public AviException(string message) : base(message) { }
      public AviException(string message, Exception inner) : base(message, inner) { }
      public AviException(int errorCode, string message)
         : base(message) {
         base.HResult = errorCode;
      }
      public AviException(int errorCode, string message, Exception inner)
         : base(message, inner) {
         base.HResult = errorCode;
      }
      protected AviException(SerializationInfo info, StreamingContext context)
         : base(info, context) {        
      }
   }
}
