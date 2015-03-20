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
namespace Atf.ScreenRecorder.Screen {
   using System;
   using System.Drawing;
   public class RenderingContext {
      #region Field
      private Bitmap bitmap;
      private Rectangle bounds;
      private Graphics graphics;
      #endregion

      #region Constructors
      public RenderingContext(Bitmap bitmap, Graphics graphics, Rectangle bounds) {
         this.bitmap = bitmap;
         this.bounds = bounds;
         this.graphics = graphics;
      }
      #endregion

      #region Properties
      public Bitmap Bitmap {
         get {
            return this.bitmap;
         }
      }
      public Rectangle Bounds {
         get {
            return this.bounds;
         }
      }
      public Graphics Graphics {
         get {
            return this.graphics;
         }
      }
      public Size Size {
         get {
            return this.bitmap.Size;
         }
      }
      #endregion
   }
}
