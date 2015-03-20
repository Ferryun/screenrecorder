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
   using System.Runtime.InteropServices;
   using System.Text;
   class WindowInterop {
      #region Constants
      public const UInt32 WM_GETTEXT = 0x000D;
      public const UInt32 WM_GETTEXTLENGTH = 0x000E;
      public const UInt32 WM_NCPAINT = 0x85;
      public const Int32 CURSOR_SHOWING = 0x00000001;
      #endregion

      #region Structs
      [StructLayout(LayoutKind.Sequential)]
      public struct POINT {
         public int X;
         public int Y;

         public POINT(int x, int y) {
            this.X = x;
            this.Y = y;
         }

         public static implicit operator System.Drawing.Point(POINT p) {
            return new System.Drawing.Point(p.X, p.Y);
         }

         public static implicit operator POINT(System.Drawing.Point p) {
            return new POINT(p.X, p.Y);
         }
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct RECT {
         public int left, top, right, bottom;
      }

      [Flags()]
      public enum RedrawWindowFlags : uint {
         /// <summary>
         /// Invalidates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
         /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_INVALIDATE invalidates the entire window.
         /// </summary>
         Invalidate = 0x1,

         /// <summary>Causes the OS to post a WM_PAINT message to the window regardless of whether a portion of the window is invalid.</summary>
         InternalPaint = 0x2,

         /// <summary>
         /// Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
         /// Specify this value in combination with the RDW_INVALIDATE value; otherwise, RDW_ERASE has no effect.
         /// </summary>
         Erase = 0x4,

         /// <summary>
         /// Validates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
         /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_VALIDATE validates the entire window.
         /// This value does not affect internal WM_PAINT messages.
         /// </summary>
         Validate = 0x8,

         NoInternalPaint = 0x10,

         /// <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
         NoErase = 0x20,

         /// <summary>Excludes child windows, if any, from the repainting operation.</summary>
         NoChildren = 0x40,

         /// <summary>Includes child windows, if any, in the repainting operation.</summary>
         AllChildren = 0x80,

         /// <summary>Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND and WM_PAINT messages before the RedrawWindow returns, if necessary.</summary>
         UpdateNow = 0x100,

         /// <summary>
         /// Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND messages before RedrawWindow returns, if necessary.
         /// The affected windows receive WM_PAINT messages at the ordinary time.
         /// </summary>
         EraseNow = 0x200,

         Frame = 0x400,

         NoFrame = 0x800
      }
      [StructLayout(LayoutKind.Sequential)]
      public struct WINDOWINFO {
         public uint cbSize;
         public RECT rcWindow;
         public RECT rcClient;
         public uint dwStyle;
         public uint dwExStyle;
         public uint dwWindowStatus;
         public uint cxWindowBorders;
         public uint cyWindowBorders;
         public ushort atomWindowType;
         public ushort wCreatorVersion;

         public WINDOWINFO(Boolean? filler)
            : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
         {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
         }

      }

      /// <summary>
      /// Contains information about the placement of a window on the screen.
      /// </summary>
      [Serializable]
      [StructLayout(LayoutKind.Sequential)]
      public struct WINDOWPLACEMENT {
         /// <summary>
         /// The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set this member to sizeof(WINDOWPLACEMENT).
         /// <para>
         /// GetWindowPlacement and SetWindowPlacement fail if this member is not set correctly.
         /// </para>
         /// </summary>
         public int Length;

         /// <summary>
         /// Specifies flags that control the position of the minimized window and the method by which the window is restored.
         /// </summary>
         public int Flags;

         /// <summary>
         /// The current show state of the window.
         /// </summary>
         public ShowWindowCommands ShowCmd;

         /// <summary>
         /// The coordinates of the window's upper-left corner when the window is minimized.
         /// </summary>
         public POINT MinPosition;

         /// <summary>
         /// The coordinates of the window's upper-left corner when the window is maximized.
         /// </summary>
         public POINT MaxPosition;

         /// <summary>
         /// The window's coordinates when the window is in the restored position.
         /// </summary>
         public RECT NormalPosition;

         /// <summary>
         /// Gets the default (empty) value.
         /// </summary>
         public static WINDOWPLACEMENT Default {
            get {
               WINDOWPLACEMENT result = new WINDOWPLACEMENT();
               result.Length = Marshal.SizeOf(result);
               return result;
            }
         }
      }

      public enum ShowWindowCommands : int {
         /// <summary>
         /// Hides the window and activates another window.
         /// </summary>
         Hide = 0,
         /// <summary>
         /// Activates and displays a window. If the window is minimized or 
         /// maximized, the system restores it to its original size and position.
         /// An application should specify this flag when displaying the window 
         /// for the first time.
         /// </summary>
         Normal = 1,
         /// <summary>
         /// Activates the window and displays it as a minimized window.
         /// </summary>
         ShowMinimized = 2,
         /// <summary>
         /// Maximizes the specified window.
         /// </summary>
         Maximize = 3, // is this the right value?
         /// <summary>
         /// Activates the window and displays it as a maximized window.
         /// </summary>       
         ShowMaximized = 3,
         /// <summary>
         /// Displays a window in its most recent size and position. This value 
         /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except 
         /// the window is not activated.
         /// </summary>
         ShowNoActivate = 4,
         /// <summary>
         /// Activates the window and displays it in its current size and position. 
         /// </summary>
         Show = 5,
         /// <summary>
         /// Minimizes the specified window and activates the next top-level 
         /// window in the Z order.
         /// </summary>
         Minimize = 6,
         /// <summary>
         /// Displays the window as a minimized window. This value is similar to
         /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
         /// window is not activated.
         /// </summary>
         ShowMinNoActive = 7,
         /// <summary>
         /// Displays the window in its current size and position. This value is 
         /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
         /// window is not activated.
         /// </summary>
         ShowNA = 8,
         /// <summary>
         /// Activates and displays the window. If the window is minimized or 
         /// maximized, the system restores it to its original size and position. 
         /// An application should specify this flag when restoring a minimized window.
         /// </summary>
         Restore = 9,
         /// <summary>
         /// Sets the show state based on the SW_* value specified in the 
         /// STARTUPINFO structure passed to the CreateProcess function by the 
         /// program that started the application.
         /// </summary>
         ShowDefault = 10,
         /// <summary>
         ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
         /// that owns the window is not responding. This flag should only be 
         /// used when minimizing windows from a different thread.
         /// </summary>
         ForceMinimize = 11
      }

      [StructLayout(LayoutKind.Sequential)]
      public struct CURSORINFO {
         public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
         public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
         public IntPtr hCursor;          // Handle to the cursor. 
         public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
      }
      #endregion

      #region Functions
      [DllImport("user32.dll")]
      public static extern bool GetCursorPos(out POINT lpPoint);

      [DllImport("user32.dll")]
      public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

      [DllImport("user32.dll")]
      public static extern bool IsWindowVisible(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern IntPtr WindowFromPoint(POINT Point);

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, [Out] StringBuilder lParam);

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

      [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
      public static extern int GetWindowTextLength(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern IntPtr GetDC(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowDC(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

      [DllImport("user32.dll")]
      public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);

      [DllImport("user32.dll")]
      public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

      [DllImport("user32.dll")]
      public static extern bool ClientToScreen(IntPtr hWnd, out POINT lpPoint);

      [DllImport("user32.dll", SetLastError = false)]
      public static extern IntPtr GetDesktopWindow();

      [return: MarshalAs(UnmanagedType.Bool)]
      [DllImport("user32.dll", SetLastError = true)]
      public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

      [DllImport("user32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);
      #endregion
   }
}
