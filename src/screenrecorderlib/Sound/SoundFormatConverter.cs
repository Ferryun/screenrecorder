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
   using System.ComponentModel;
   using System.Globalization;
   public class SoundFormatConverter : TypeConverter {
      private static readonly string bit = "bit";
      private static readonly string block = "bl";
      private static readonly string bps = "bps";
      private static readonly int bytesPow = 1000;
      private static readonly int hertzPow = 1000;
      private static readonly string hz = "hz";
      private static readonly string kilo = "k";
      private static readonly string mega = "m";
      private static readonly string mono = "Mono";
      private static readonly string stereo = "Stereo";
      public static string[] supportedTags;
      static SoundFormatConverter() {
         // Convert soundformat tags to string values
         Array tagsArray = Enum.GetValues(typeof(SoundFormatTag));
         supportedTags = new string[tagsArray.Length - 1];
         for (int i = 1; i < tagsArray.Length; i++) {
            SoundFormatTag tag = (SoundFormatTag)tagsArray.GetValue(i);
            supportedTags.SetValue(tag.ToString(), i - 1);
         }
      }
      public override bool CanConvertFrom(ITypeDescriptorContext context,
         Type sourceType) {
         if (sourceType == typeof(string)) {
            return true;
         }
         return base.CanConvertFrom(context, sourceType);
      }
      // Overrides the ConvertFrom method of TypeConverter.
      public override object ConvertFrom(ITypeDescriptorContext context,
         CultureInfo culture, object value) {
         if (!(value is string)) {
            return base.ConvertFrom(context, culture, value);
         }
         SoundFormatTag tag = SoundFormatTag.UNKNOWN;
         int bytesPerSecond = 0;
         int bitsPerSample = 0;
         int blockAlign = 0;
         int channels = 0;
         int samplesPerSecond = 0;
         string[] properties = ((string)value).Split(new char[] { ',' });

         // Read properties...
         foreach (var property in properties) {
            string tproperty = property.Trim();
            if (tproperty.EndsWith(bit, StringComparison.InvariantCultureIgnoreCase)) {
                string bitsString = tproperty.Substring(0, tproperty.Length - bit.Length).Trim();
                if (!int.TryParse(bitsString, NumberStyles.Any, culture, out bitsPerSample)) {
                   throw new NotSupportedException();
                }
            }
            else if (tproperty.EndsWith(block, StringComparison.InvariantCultureIgnoreCase)) {
               string blockAlignString = tproperty.Substring(0, tproperty.Length - block.Length).Trim();
               if (!int.TryParse(blockAlignString, NumberStyles.Any, culture, out blockAlign)) {
                  throw new NotSupportedException();
               }             
            }
            else if (tproperty.EndsWith(bps, StringComparison.InvariantCultureIgnoreCase)) {
               string bpsString = tproperty.Substring(0, tproperty.Length - bps.Length).Trim();
               int bitsPerSecond;
               if (!TryParseNumber(bpsString, bytesPow, NumberStyles.Any, culture, out bitsPerSecond)) {
                  throw new NotSupportedException();
               }
               bytesPerSecond = bitsPerSecond / 8;
            }
            else if (tproperty.Equals(mono, StringComparison.InvariantCultureIgnoreCase)) {
               channels = 1;
            }
            else if (tproperty.Equals(stereo, StringComparison.InvariantCultureIgnoreCase)) {
               channels = 2;
            }
            else if (tproperty.EndsWith(hz, StringComparison.InvariantCultureIgnoreCase)) {
               string samplesPerSecondString = tproperty.Substring(0, tproperty.Length - hz.Length).Trim();
               if (!TryParseNumber(samplesPerSecondString, hertzPow, NumberStyles.Any, culture,
                                         out samplesPerSecond)) {
                  throw new NotSupportedException();
               }
            }
            else if (tproperty.EndsWith(bps, StringComparison.InvariantCultureIgnoreCase)) {
               // 
            }
            else {
               bool isTag = false;
               foreach (var supportedTag in supportedTags) {
                  if (string.Equals(supportedTag, tproperty, StringComparison.InvariantCultureIgnoreCase)) {
                     tag = (SoundFormatTag)Enum.Parse(typeof(SoundFormatTag), supportedTag);
                     isTag = true;
                     break;
                  }
               }
               if (!isTag) {
                  throw new NotSupportedException();
               }
            }               
         }
         if (bytesPerSecond != 0 && blockAlign != 0) {
            return new SoundFormat(tag, bitsPerSample, blockAlign, channels, samplesPerSecond, bytesPerSecond);
         }
         return new SoundFormat(tag, bitsPerSample, channels, samplesPerSecond);
      }
      public override object ConvertTo(ITypeDescriptorContext context,
         CultureInfo culture, object value, Type destinationType) {
         if (destinationType == typeof(string)) {
            SoundFormat soundFormat = (SoundFormat)value;
            return string.Format(
               culture,
               "{0}, {1:.000} {2}{3}, {4:.000} {5}{6}, {7} {8}, {9}, {10} {11}", 
               soundFormat.Tag,
               (float)(soundFormat.AverageBytesPerSecond * 8) / bytesPow,
               kilo,
               bps,
               (float)soundFormat.SamplesPerSecond / hertzPow,
               kilo,
               hz, 
               soundFormat.BitsPerSample,
               bit,
               soundFormat.Channels == 1 ? mono : stereo,
               soundFormat.BlockAlign,
               block
            );
                                 
         }
         return base.ConvertTo(context, culture, value, destinationType);
      }
      private static bool TryParseNumber(string s, int baseVal, NumberStyles numberStyle, 
                                         IFormatProvider formatPromatProvider, out int result) {
         string num = s;
         int unitPower = 0;
         if (num.EndsWith(kilo, StringComparison.InvariantCultureIgnoreCase)) {
            unitPower = 1;
            num = s.Substring(0, s.Length - kilo.Length);
         }
         else if (num.EndsWith(mega, StringComparison.InvariantCultureIgnoreCase)) {
            unitPower = 2;
            num = s.Substring(0, s.Length - mega.Length);
         }
         else {
            num = s;
         }
         decimal dresult;
         if (!decimal.TryParse(num, numberStyle, formatPromatProvider, out dresult)) {
            result = 0;
            return false;
         }
         result = (int)(dresult * (decimal)Math.Pow(baseVal, unitPower));
         return true;
      }
   }
}
