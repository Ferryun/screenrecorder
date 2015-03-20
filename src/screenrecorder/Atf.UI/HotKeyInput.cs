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
namespace Atf.UI {
   using System;
   using System.ComponentModel;
   using System.Text;
   using System.Windows.Forms;
   class HotKeyInput : TextBox {
      #region Fields
      private Keys keys;
      #endregion

      #region Constructors
      public HotKeyInput() {
         this.ShortcutsEnabled = false;
      }
      #endregion

      #region Properties
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [EditorBrowsable(EditorBrowsableState.Never)]
      public override bool ShortcutsEnabled {
         get {
            return base.ShortcutsEnabled;
         }
         set {
            base.ShortcutsEnabled = value;
         }
      }
      [DefaultValue(Keys.None)]
      public Keys Value {
         get {
            return this.keys;
         }
         set {
            if (this.keys != value) {
               this.keys = ConvertKeys(value);
               string text = KeysToString(this.keys);
               base.Text = text;
               this.SelectionStart = text.Length;
            }
         }
      }
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [EditorBrowsable(EditorBrowsableState.Never)]
      public override string Text {
         get {
            return base.Text;
         }
         set {
            // base.Text = value;
         }
      }
      #endregion
      #region Methods
      private void CommitKeys() {
         Keys keyCode = this.keys & Keys.KeyCode;
         Keys modifiers = this.keys & Keys.Modifiers;
         bool ctrl = (modifiers & Keys.Control) == Keys.Control;
         bool alt = (modifiers & Keys.Alt) == Keys.Alt;
         bool shift = (modifiers & Keys.Shift) == Keys.Shift;
         if (!Enum.IsDefined(typeof(ValidKeyCodes), (ValidKeyCodes)keyCode)) {
            this.Value = Keys.None;
         }
         else if (shift && !alt && !ctrl) {
            if (!Enum.IsDefined(typeof(ValidKeyCodesWithShift), (ValidKeyCodesWithShift)keyCode)) {
               this.Value = Keys.None;
            }
         }
      }
      private static Keys ConvertKeys(Keys keys) {
         Keys converted = Keys.None;
         Keys modifiers = keys & Keys.Modifiers;
         if ((modifiers & Keys.Control) == Keys.Control) {
            converted |= Keys.Control;
         }
         if ((modifiers & Keys.Alt) == Keys.Alt) {
            converted |= Keys.Alt;
         }
         if ((modifiers & Keys.Shift) == Keys.Shift) {
            converted |= Keys.Shift;
         }
         Keys keyCode = keys & Keys.KeyCode;
         if (!Enum.IsDefined(typeof(ValidKeyCodes), (ValidKeyCodes)keyCode)) {
            keyCode = Keys.None;
         }
         converted |= keyCode;
         return converted;
      }
      private static string KeysToString(Keys keys) {
         StringBuilder sb = new StringBuilder();
         Keys modifiers = keys & Keys.Modifiers;
         if ((modifiers & Keys.Control) == Keys.Control) {
            sb.Append("Ctrl+");
         }
         if ((modifiers & Keys.Alt) == Keys.Alt) {
            sb.Append("Alt+");            
         }
         if ((modifiers & Keys.Shift) == Keys.Shift) {
            sb.Append("Shift+");
         }
         Keys keyCode = keys & Keys.KeyCode;
         if (keyCode != Keys.None) {
            KeysConverter converter = new KeysConverter();
            sb.Append(converter.ConvertToString(keyCode));
         }
         return sb.ToString();
      }
      protected override void OnKeyUp(KeyEventArgs e) {
         if (e.Modifiers == Keys.None) {
            this.CommitKeys();
         }
         base.OnKeyUp(e);
      }
      protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
         if (keyData == Keys.Tab) {
            return false;
         }
         this.Value = keyData;
         return true;
      }
      #endregion
      #region ValidKeyCodes
      enum ValidKeyCodesWithShift {
         //
         // Summary:
         //     The INS key.
         Ins = 45,
         //
         // Summary:
         //     The DEL key.
         Del = 46,
         //
         // Summary:
         //     The F1 key.
         F1 = 112,
         //
         // Summary:
         //     The F2 key.
         F2 = 113,
         //
         // Summary:
         //     The F3 key.
         F3 = 114,
         //
         // Summary:
         //     The F4 key.
         F4 = 115,
         //
         // Summary:
         //     The F5 key.
         F5 = 116,
         //
         // Summary:
         //     The F6 key.
         F6 = 117,
         //
         // Summary:
         //     The F7 key.
         F7 = 118,
         //
         // Summary:
         //     The F8 key.
         F8 = 119,
         //
         // Summary:
         //     The F9 key.
         F9 = 120,
         //
         // Summary:
         //     The F10 key.
         F10 = 121,
         //
         // Summary:
         //     The F11 key.
         F11 = 122,
         //
         // Summary:
         //     The F12 key.
         F12 = 123,
         //
         // Summary:
         //     The F13 key.
         F13 = 124,
         //
         // Summary:
         //     The F14 key.
         F14 = 125,
         //
         // Summary:
         //     The F15 key.
         F15 = 126,
         //
         // Summary:
         //     The F16 key.
         F16 = 127,
         //
         // Summary:
         //     The F17 key.
         F17 = 128,
         //
         // Summary:
         //     The F18 key.
         F18 = 129,
         //
         // Summary:
         //     The F19 key.
         F19 = 130,
         //
         // Summary:
         //     The F20 key.
         F20 = 131,
         //
         // Summary:
         //     The F21 key.
         F21 = 132,
         //
         // Summary:
         //     The F22 key.
         F22 = 133,
         //
         // Summary:
         //     The F23 key.
         F23 = 134,
         //
         // Summary:
         //     The F24 key.
         F24 = 135,
      }
      enum ValidKeyCodes {   
         //
         // Summary:
         //     The TAB key.
         Tab = 9,
         //
         // Summary:
         //     The PAUSE key.
         Pause = 19,
         //
         // Summary:
         //     The SPACEBAR key.
         Space = 32,
         //
         // Summary:
         //     The END key.
         End = 35,
         //
         // Summary:
         //     The LEFT ARROW key.
         Left = 37,
         //
         // Summary:
         //     The UP ARROW key.
         Up = 38,
         //
         // Summary:
         //     The RIGHT ARROW key.
         Right = 39,
         //
         // Summary:
         //     The DOWN ARROW key.
         Down = 40,      
         //
         // Summary:
         //     The INS key.
         Ins = 45,
         //
         // Summary:
         //     The DEL key.
         Del = 46,
         //
         // Summary:
         //     The 0 key.
         D0 = 48,
         //
         // Summary:
         //     The 1 key.
         D1 = 49,
         //
         // Summary:
         //     The 2 key.
         D2 = 50,
         //
         // Summary:
         //     The 3 key.
         D3 = 51,
         //
         // Summary:
         //     The 4 key.
         D4 = 52,
         //
         // Summary:
         //     The 5 key.
         D5 = 53,
         //
         // Summary:
         //     The 6 key.
         D6 = 54,
         //
         // Summary:
         //     The 7 key.
         D7 = 55,
         //
         // Summary:
         //     The 8 key.
         D8 = 56,
         //
         // Summary:
         //     The 9 key.
         D9 = 57,
         //
         // Summary:
         //     The A key.
         A = 65,
         //
         // Summary:
         //     The B key.
         B = 66,
         //
         // Summary:
         //     The C key.
         C = 67,
         //
         // Summary:
         //     The D key.
         D = 68,
         //
         // Summary:
         //     The E key.
         E = 69,
         //
         // Summary:
         //     The F key.
         F = 70,
         //
         // Summary:
         //     The G key.
         G = 71,
         //
         // Summary:
         //     The H key.
         H = 72,
         //
         // Summary:
         //     The I key.
         I = 73,
         //
         // Summary:
         //     The J key.
         J = 74,
         //
         // Summary:
         //     The K key.
         K = 75,
         //
         // Summary:
         //     The L key.
         L = 76,
         //
         // Summary:
         //     The M key.
         M = 77,
         //
         // Summary:
         //     The N key.
         N = 78,
         //
         // Summary:
         //     The O key.
         O = 79,
         //
         // Summary:
         //     The P key.
         P = 80,
         //
         // Summary:
         //     The Q key.
         Q = 81,
         //
         // Summary:
         //     The R key.
         R = 82,
         //
         // Summary:
         //     The S key.
         S = 83,
         //
         // Summary:
         //     The T key.
         T = 84,
         //
         // Summary:
         //     The U key.
         U = 85,
         //
         // Summary:
         //     The V key.
         V = 86,
         //
         // Summary:
         //     The W key.
         W = 87,
         //
         // Summary:
         //     The X key.
         X = 88,
         //
         // Summary:
         //     The Y key.
         Y = 89,
         //
         // Summary:
         //     The Z key.
         Z = 90,
         //
         // Summary:
         //     The 0 key on the numeric keypad.
         NumPad0 = 96,
         //
         // Summary:
         //     The 1 key on the numeric keypad.
         NumPad1 = 97,
         //
         // Summary:
         //     The 2 key on the numeric keypad.
         NumPad2 = 98,
         //
         // Summary:
         //     The 3 key on the numeric keypad.
         NumPad3 = 99,
         //
         // Summary:
         //     The 4 key on the numeric keypad.
         NumPad4 = 100,
         //
         // Summary:
         //     The 5 key on the numeric keypad.
         NumPad5 = 101,
         //
         // Summary:
         //     The 6 key on the numeric keypad.
         NumPad6 = 102,
         //
         // Summary:
         //     The 7 key on the numeric keypad.
         NumPad7 = 103,
         //
         // Summary:
         //     The 8 key on the numeric keypad.
         NumPad8 = 104,
         //
         // Summary:
         //     The 9 key on the numeric keypad.
         NumPad9 = 105,
         //
         // Summary:
         //     The F1 key.
         F1 = 112,
         //
         // Summary:
         //     The F2 key.
         F2 = 113,
         //
         // Summary:
         //     The F3 key.
         F3 = 114,
         //
         // Summary:
         //     The F4 key.
         F4 = 115,
         //
         // Summary:
         //     The F5 key.
         F5 = 116,
         //
         // Summary:
         //     The F6 key.
         F6 = 117,
         //
         // Summary:
         //     The F7 key.
         F7 = 118,
         //
         // Summary:
         //     The F8 key.
         F8 = 119,
         //
         // Summary:
         //     The F9 key.
         F9 = 120,
         //
         // Summary:
         //     The F10 key.
         F10 = 121,
         //
         // Summary:
         //     The F11 key.
         F11 = 122,
         //
         // Summary:
         //     The F12 key.
         F12 = 123,
         //
         // Summary:
         //     The F13 key.
         F13 = 124,
         //
         // Summary:
         //     The F14 key.
         F14 = 125,
         //
         // Summary:
         //     The F15 key.
         F15 = 126,
         //
         // Summary:
         //     The F16 key.
         F16 = 127,
         //
         // Summary:
         //     The F17 key.
         F17 = 128,
         //
         // Summary:
         //     The F18 key.
         F18 = 129,
         //
         // Summary:
         //     The F19 key.
         F19 = 130,
         //
         // Summary:
         //     The F20 key.
         F20 = 131,
         //
         // Summary:
         //     The F21 key.
         F21 = 132,
         //
         // Summary:
         //     The F22 key.
         F22 = 133,
         //
         // Summary:
         //     The F23 key.
         F23 = 134,
         //
         // Summary:
         //     The F24 key.
         F24 = 135,
         //
         // Summary:
         //     The OEM 1 key.
         Oem1 = 186,
         //
         // Summary:
         //     The OEM plus key on any country/region keyboard (Windows 2000 or later).
         Oemplus = 187,
         //
         // Summary:
         //     The OEM comma key on any country/region keyboard (Windows 2000 or later).
         Oemcomma = 188,
         //
         // Summary:
         //     The OEM minus key on any country/region keyboard (Windows 2000 or later).
         OemMinus = 189,
         //
         // Summary:
         //     The OEM period key on any country/region keyboard (Windows 2000 or later).
         OemPeriod = 190,
         //
         // Summary:
         //     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
         OemQuestion = 191,
         //
         // Summary:
         //     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
         Oemtilde = 192,
         //
         // Summary:
         //     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
         OemOpenBrackets = 219,
         //
         // Summary:
         //     The OEM 5 key.
         Oem5 = 220,
         //
         // Summary:
         //     The OEM 6 key.
         Oem6 = 221,
         //
         // Summary:
         //     The OEM 7 key.
         Oem7 = 222,
         //
         // Summary:
         //     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows
         //     2000 or later).
         OemBackslash = 226,
         //
         // Summary:
         //     The CLEAR key.
         OemClear = 254,      
      }
      #endregion
   }
}
