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
   public class SoundDevice {
      private string description;
      private string id;
      private bool isLoopback;
      private string name;
      public SoundDevice(string id) : this(id, string.Empty, false) { }
      public SoundDevice(string id, string name) : this(id, name, false) { }
      public SoundDevice(string id, string name, bool isLoopback) : this(id, name, isLoopback, name) { }
      public SoundDevice(string id, string name, bool isLoopback, string description) {
         if (string.IsNullOrEmpty(id)) {
            throw new ArgumentNullException("id");
         }
         this.description = description;
         this.id = id;
         this.isLoopback = isLoopback;
         this.name = name;
      }
      public string Description {
         get {
            return this.description;
         }
      }
      public string Id {
         get {
            return this.id;
         }
      }
      public bool IsLoopback {
         get {
            return this.isLoopback;
         }
      }
      public string Name {
         get {
            return this.name;
         }      
      }
      public override bool Equals(object obj) {
         SoundDevice soundDevice = obj as SoundDevice;
         if (soundDevice == null) {
            return false;
         }
         return this.id.Equals(soundDevice.id) && this.isLoopback.Equals(soundDevice.isLoopback);
      }
      public override int GetHashCode() {
         return this.id != null ? this.id.GetHashCode() : 0;
      }
      public override string ToString() {
         return this.name;
      }
   }
}
