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
   // More info: http://stackoverflow.com/questions/2450373/set-global-hotkeys-using-c-sharp
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Windows.Forms;
   public sealed class HotKeyManager : IDisposable {
      #region Enums
      /// <summary>
      /// The enumeration of possible modifiers.
      /// </summary>
      [Flags]
      private enum ModifierKeys : uint {
         None = 0,
         Alt = 1,
         Control = 2,
         Shift = 4,
         Win = 8
      }
      #endregion

      #region Events
      public event KeyEventHandler HotKey;
      #endregion

      #region Fields
      private static readonly object syncRoot = new object();
      private int currentId;
      private Dictionary<Keys, int> registredKeys;
      private bool disposed;
      private Window window;
      #endregion

      #region Constructors
      public HotKeyManager() {
         this.registredKeys = new Dictionary<Keys, int>();
         this.window = new HotKeyWindow(this);
      }
      #endregion

      #region Methods
      private static Keys CombineModifierKeys(ModifierKeys modifiers, Keys keys) {
         Keys combinedKeys = keys;
         if ((modifiers & ModifierKeys.Alt) == ModifierKeys.Alt) {
            combinedKeys |= Keys.Alt;
         }
         if ((modifiers & ModifierKeys.Control) == ModifierKeys.Control) {
            combinedKeys |= Keys.Control;
         }
         if ((modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) {
            combinedKeys |= Keys.Shift;
         }
         if ((modifiers & ModifierKeys.Win) == ModifierKeys.Win) {
            combinedKeys |= (Keys.LWin | Keys.RWin);
         }
         return combinedKeys;
      }
      private static void ExtractKeys(Keys combinedKeys, out ModifierKeys modifiers, out Keys keys) {
         keys = combinedKeys;
         modifiers = ModifierKeys.None;
         if ((combinedKeys & Keys.Alt) == Keys.Alt) {
            keys = keys & ~Keys.Alt;
            modifiers |= ModifierKeys.Alt;
         }
         if ((combinedKeys & Keys.Control) == Keys.Control) {
            keys = keys & ~Keys.Control;
            modifiers |= ModifierKeys.Control;
         }
         if ((combinedKeys & Keys.Shift) == Keys.Shift) {
            keys = keys & ~Keys.Shift;
            modifiers |= ModifierKeys.Shift;
         }
         if ((combinedKeys & Keys.RWin) == Keys.RWin ||
             (combinedKeys & Keys.LWin) == Keys.LWin) {
            keys = keys & ~Keys.RWin;
            keys = keys & ~Keys.LWin;
            modifiers |= ModifierKeys.Win;
         }
      }
      private void OnHotKey(KeyEventArgs e) {
         if (this.HotKey != null) {
            this.HotKey(this, e);
         }
      }
      /// <summary>
      /// Registers a hot key in the system.
      /// </summary>
      /// <param name="keys">The keys that are associated with the hot key.</param>
      public void RegisterHotKey(Keys hotKey) {
         lock (syncRoot) {
            if (this.registredKeys.ContainsKey(hotKey)) {
               throw new ArgumentException("Specified hot key already registered.");
            }
            this.currentId++;
            this.registredKeys.Add(hotKey, this.currentId);
            // register the hot key.
            ModifierKeys modifiers;
            Keys extractedKeys;
            ExtractKeys(hotKey, out modifiers, out extractedKeys);
            if (!User32Interop.RegisterHotKey(window.Handle, this.currentId, (uint)modifiers, (uint)extractedKeys)) {
               throw new InvalidOperationException("Couldn’t register the hot key.", new Win32Exception());
            }
         }
      }
      /// <summary>
      /// Unregisters a hot key from the system.
      /// </summary>
      /// <param name="keys">The keys that are associated with the hot key.</param>
      public void UnregisterHotKey(Keys hotKey) {
         lock (syncRoot) {
            if (!this.registredKeys.ContainsKey(hotKey)) {
               throw new ArgumentException("Specified hot key is not registered.");
            }
            int id = this.registredKeys[hotKey];
            User32Interop.UnregisterHotKey(window.Handle, id);
            this.registredKeys.Remove(hotKey);
         }
      }
      #endregion

      #region IDisposable Members
      public void Dispose() {
         Dispose(true);
         // This object will be cleaned up by the Dispose method. 
         // Therefore, you should call GC.SupressFinalize to 
         // take this object off the finalization queue 
         // and prevent finalization code for this object 
         // from executing a second time.
         GC.SuppressFinalize(this);
      }
      private void Dispose(bool disposing) {
         // Check to see if Dispose has already been called. 
         if (!this.disposed) {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources. 
            if (disposing) {
               // unregister all the registered hot keys.
               foreach (var item in this.registredKeys) {
                  User32Interop.UnregisterHotKey(window.Handle, item.Value);
               }
               this.registredKeys.Clear();
               // dispose the inner native window.
               window.Dispose();
            }
            // Note disposing has been done.
            disposed = true;
         }
      }
      #endregion

      class HotKeyWindow : Window {
         #region Fields
         private HotKeyManager hotKeyManager;
         #endregion

         #region Constructors
         public HotKeyWindow(HotKeyManager hotKeyManager) {
            if (hotKeyManager == null) {
               throw new ArgumentNullException("hotKeyManager");
            }
            this.hotKeyManager = hotKeyManager;
         }
         #endregion

         #region Methods
         /// <summary>
         /// Overridden to get the notifications.
         /// </summary>
         /// <param name="m"></param>
         protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            // check if we got a hot key pressed.
            if (m.Msg == User32Interop.WM_HOTKEY) {
               // get the keys.
               Keys keys = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
               ModifierKeys modifiers = (ModifierKeys)((int)m.LParam & 0xFFFF);
               Keys combinedKeys = HotKeyManager.CombineModifierKeys(modifiers, keys);
               KeyEventArgs ea = new KeyEventArgs(combinedKeys);
               this.hotKeyManager.OnHotKey(ea);
            }
         }
         #endregion
      }
   }
}
