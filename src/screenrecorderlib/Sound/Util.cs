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
   static class Util {
      public static SoundFormat WaveFormatToSoundFormat(MMInterop.WaveFormat format) {
         switch (format) {
            case MMInterop.WaveFormat.WAVE_FORMAT_1M08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 1, 11025);
            case MMInterop.WaveFormat.WAVE_FORMAT_1M16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 1, 11025);
            case MMInterop.WaveFormat.WAVE_FORMAT_1S08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 2, 11025);
            case MMInterop.WaveFormat.WAVE_FORMAT_1S16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 2, 11025);
            case MMInterop.WaveFormat.WAVE_FORMAT_2M08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 1, 22050);
            case MMInterop.WaveFormat.WAVE_FORMAT_2M16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 1, 22050);
            case MMInterop.WaveFormat.WAVE_FORMAT_2S08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 2, 22050);
            case MMInterop.WaveFormat.WAVE_FORMAT_2S16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 2, 22050);
            case MMInterop.WaveFormat.WAVE_FORMAT_44M08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 1, 44100);
            case MMInterop.WaveFormat.WAVE_FORMAT_44M16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 1, 44100);
            case MMInterop.WaveFormat.WAVE_FORMAT_44S08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 2, 44100);
            case MMInterop.WaveFormat.WAVE_FORMAT_44S16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 2, 44100);
            case MMInterop.WaveFormat.WAVE_FORMAT_48M08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 1, 48000);
            case MMInterop.WaveFormat.WAVE_FORMAT_48M16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 1, 48000);
            case MMInterop.WaveFormat.WAVE_FORMAT_48S08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 2, 48000);
            case MMInterop.WaveFormat.WAVE_FORMAT_48S16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 2, 48000);
            case MMInterop.WaveFormat.WAVE_FORMAT_96M08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 1, 96000);
            case MMInterop.WaveFormat.WAVE_FORMAT_96M16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 1, 96000);
            case MMInterop.WaveFormat.WAVE_FORMAT_96S08:
               return new SoundFormat(SoundFormatTag.PCM, 8, 2, 96000);
            case MMInterop.WaveFormat.WAVE_FORMAT_96S16:
               return new SoundFormat(SoundFormatTag.PCM, 16, 2, 96000);            
            default:
               throw new ArgumentOutOfRangeException();
         }
      }
      public static SoundFormat[] WaveFormatToSoundFormats(MMInterop.WaveFormat formats) {
         List<SoundFormat> soundFormats = new List<SoundFormat>();
         Array allValues = Enum.GetValues(typeof(MMInterop.WaveFormat));
         foreach (var value in allValues) {
            MMInterop.WaveFormat vformat = (MMInterop.WaveFormat)value;
            if ((int)value == 0) { // WAVE_INVALID_FORMAT
               continue;
            }
            if ((formats & vformat) == vformat) {
               SoundFormat format = WaveFormatToSoundFormat(vformat);
               soundFormats.Add(format);
            }
         }
         return soundFormats.ToArray();
      }
   }
}
