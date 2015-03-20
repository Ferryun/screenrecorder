namespace Atf.ScreenRecorder.Screen {
   using System;
   using System.Runtime.InteropServices;
   static class Gdi32Interop {
      public const int DI_NORMAL = 0x0003;

      public enum DeviceCap {      
         BITSPIXEL = 12,
         /// <summary>
         /// Number of planes
         /// </summary>
         PLANES = 14,
         /// <summary>
         /// Number of brushes the device has
         /// </summary>        
      }

      [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool DeleteObject([In] IntPtr hObject);

      [DllImport("gdi32.dll")]
      public static extern int GetDeviceCaps(IntPtr hdc, DeviceCap nIndex);

      [DllImport("gdi32.dll")]
      public static extern int IntersectClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);
   }
}
