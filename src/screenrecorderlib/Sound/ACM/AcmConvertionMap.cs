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
   public class AcmConvertionMap : AcmConvertionMapBase<SoundFormat> {
   }
   public class AcmConvertionMapBase<T> {
      private Dictionary<T, List<T>> dictionary;
      public AcmConvertionMapBase() {
         dictionary = new Dictionary<T, List<T>>();
      }
      public void Add(T input, T output) {
         List<T> currentOutput;
         if (this.dictionary.TryGetValue(input, out currentOutput)) {
            if (!currentOutput.Contains(output)) {
               currentOutput.Add(output);
            }
         }
         else {
            List<T> outputs = new List<T>();
            outputs.Add(output);
            this.dictionary.Add(input, outputs);
         }
      }
      public void Add(T input, IEnumerable<T> outputs) {
         List<T> currentOutputs;
         if (this.dictionary.TryGetValue(input, out currentOutputs)) {
            foreach (T output in outputs) {
               if (!currentOutputs.Contains(output)) {
                  currentOutputs.Add(output);
               }
            }
         }
         else {
            List<T> outputsList = new List<T>(outputs);
            this.dictionary.Add(input, outputsList);
         }
      }
      public void Add(AcmConvertionMapBase<T> map) {
         foreach (var pair in map.dictionary) {
            this.Add(pair.Key, pair.Value);
         }
      }
      public bool Contains(T input) {
         return this.dictionary.ContainsKey(input);
      }
      public T[] GetInputs() {
         List<T> inputs = new List<T>();
         foreach (var key in this.dictionary.Keys) {
            inputs.Add(key);
         }
         return inputs.ToArray();
      }
      public T[] GetInputs(T output) {
         List<T> inputs = new List<T>();
         foreach (var pair in this.dictionary) {
            if (pair.Value.Contains(output)) {
               T input = pair.Key;
               if (!inputs.Contains(input)) {
                  inputs.Add(input);
               }
            }
         }
         return inputs.ToArray();
      }
      public T[] GetOutputs(T input) {
         List<T> outputs;
         if (this.dictionary.TryGetValue(input, out outputs)) {
            return outputs.ToArray();
         }
         return new T[] {};
      }
      public T[] GetOutputs(IEnumerable<T> inputs) {
         List<T> outputs = new List<T>();
         foreach (var input in inputs) {
            List<T> outputsOfInput;
            if (this.dictionary.TryGetValue(input, out outputsOfInput)) {
               foreach (var outputOfInput in outputsOfInput) {
                  if (!outputs.Contains(outputOfInput)) {
                     outputs.Add(outputOfInput);
                  }
               }
            }
         }
         return outputs.ToArray();
      }
      public void Remove(T input, T output) {
         List<T> currentOutputs;
         if (this.dictionary.TryGetValue(input, out currentOutputs)) {
            currentOutputs.Remove(output);
         }
      }
      public void Remove(T input, IEnumerable<T> outputs) {
         List<T> currentOutputs;
         if (this.dictionary.TryGetValue(input, out currentOutputs)) {
            foreach (T output in outputs) {
               currentOutputs.Remove(output);
            }
         }
      }
   }
}
