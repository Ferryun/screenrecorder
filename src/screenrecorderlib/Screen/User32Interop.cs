namespace Atf.ScreenRecorder.Screen {
   using System;
   using System.Drawing;
   using System.Runtime.InteropServices;
   static class User32Interop {
      public const int IDC_ARROW = 32512;

      [StructLayout(LayoutKind.Sequential)]
      public struct ICONINFO {
         public bool fIcon;
         public Int32 xHotspot;
         public Int32 yHotspot;
         public IntPtr hbmMask;
         public IntPtr hbmColor;
      }
      public enum SystemMetric {
         SM_CXSCREEN = 0,  // 0x00
         SM_CYSCREEN = 1,  // 0x01    
         SM_CXCURSOR = 13, // 0x0D
         SM_CYCURSOR = 14, // 0x0E
      }

      [DllImport("user32.dll")]
      public static extern bool DestroyCursor(IntPtr hCursor);

      [DllImport("user32.dll", SetLastError = true)]
      public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);

      [DllImport("user32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool GetCursorPos(out Point lpPoint);

      [DllImport("user32.dll", SetLastError = true)]
      public static extern IntPtr GetDC(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

      [DllImport("user32.dll")]
      public static extern int GetSystemMetrics(SystemMetric smIndex);

      [DllImport("user32.dll")]
      public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

      [DllImport("user32.dll")]
      public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

   }
}
