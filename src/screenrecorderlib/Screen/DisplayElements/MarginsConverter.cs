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
   using System.ComponentModel;
   using System.Globalization;
   public class MarginsConverter : TypeConverter {
      public override bool CanConvertFrom(ITypeDescriptorContext context,
         Type sourceType) {
         if (sourceType == typeof(string)) {
            return true;
         }
         return base.CanConvertFrom(context, sourceType);
      }
      // Overrides the ConvertFrom method of TypeConverter.
      public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
         if (!(value is string)) {
            return base.ConvertFrom(context, culture, value);
         }
         string[] components = ((string)value).Split(',');
         int componentCount = 4;
         if (components.Length != componentCount) {
            throw new NotSupportedException();
         }
         int[] intComponents = new int[componentCount];
         for (int i = 0; i < componentCount; i++) {
            int intComponent;
            if (!int.TryParse(components[i].Trim(), NumberStyles.Integer, culture, out intComponent)) {
               throw new NotSupportedException();
            }
            intComponents.SetValue(intComponent, i);
         }
         return new Margins(intComponents[0], intComponents[1], intComponents[2], intComponents[3]);

      }
      public override object ConvertTo(ITypeDescriptorContext context,
         CultureInfo culture, object value, Type destinationType) {
         if (value is Margins && destinationType == typeof(string)) {
            Margins margins = (Margins)value;
            return string.Format(culture, "{0}, {1}, {2}, {3}", margins.Left, margins.Top, margins.Right, margins.Bottom);
         }
         return base.ConvertTo(context, culture, value, destinationType);
      }
   }
}
