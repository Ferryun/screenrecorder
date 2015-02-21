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
namespace Atf.ScreenRecorder.Util {
   using System;
   using System.Diagnostics;
   using System.IO;
   using System.Security;
   static class IOUtil {      
      public static string CreateNewFile(string directory, string extension) {
         short number = 1;
         string fileName;
         for (; number < short.MaxValue; number++) {
            fileName = string.Format("{0:yyyy}-{0:MM}-{0:dd}_{1:0000}.{2}", DateTime.Now, number, extension);
            string fullPath = Path.Combine(directory, fileName);
            if (!File.Exists(fullPath)) {
               try {
                  using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write)) {
                     return fullPath;
                  }
               }               
               catch (NotSupportedException e) {
                  string message = string.Format("Failed to create ouput file in '{0}': {1}", fullPath, e.Message);
                  throw new InvalidOperationException(message, e);
               }
               catch (DirectoryNotFoundException e) {
                  string message = string.Format(
                                    "Failed to create ouput file '{0}': Specified directory was not found.", fullPath);
                  throw new InvalidOperationException(message, e);
               }
               catch (SecurityException e) {
                  string message = string.Format("Failed to create ouput file '{0}': Access denied.", fullPath);
                  throw new InvalidOperationException(message, e);
               }
               catch (UnauthorizedAccessException e) {
                  string message = string.Format("Failed to create ouput file '{0}': Access denied.", fullPath);
                  throw new InvalidOperationException(message, e);
               }
               catch (IOException e) {
                  string message = string.Format("Failed to create ouput file '{0}': I/O Error: {1}", fullPath, 
                                                 e.Message);
                  throw new InvalidOperationException(message, e);
               }
               catch (ArgumentException e) {
                  string message = string.Format("Failed to create ouput file '{0}': {1}", fullPath, e.Message);
                  throw new InvalidOperationException(message, e);
               }
            }            
         }
         throw new InvalidOperationException("Failed to create ouput file.");
      }
      public static void DeleteFile(string fullPath) {
         File.Delete(fullPath);
      }
      public static bool FileExists(string fullPath) {
         return File.Exists(fullPath);
      }
      public static void LaunchFile(string fullPath) {
         Process proc = new Process();
         proc.EnableRaisingEvents = false;
         proc.StartInfo.FileName = "explorer.exe";
         proc.StartInfo.Arguments = fullPath;
         proc.Start();
      }
      public static void LaunchUrl(string url) {
         Process proc = new Process();
         proc.EnableRaisingEvents = false;
         proc.StartInfo.FileName = "iexplore.exe";
         proc.StartInfo.Arguments = url;
         proc.Start();
      }
      public static bool OpenFolder(string fullPath) {
         if (!Directory.Exists(fullPath)) {
            return false;
         }
         string argument = string.Format("\"{0}\"", fullPath);
         Process.Start("explorer.exe", argument);
         return true;
      }
      public static bool SelectFile(string fullPath) {
         if (!File.Exists(fullPath)) {
            return false;
         }
         string argument = string.Format("/select, \"{0}\"", fullPath);
         Process.Start("explorer.exe", argument);
         return true;
      }
   }
}
