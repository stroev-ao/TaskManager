using System;
using System.Runtime.InteropServices;

namespace TaskManager
{
    public static class CHelper
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        public static void StopControlUpdating(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, false, 0);
        }

        public static void ResumeControlUpdating(IntPtr handle)
        {
            SendMessage(handle, WM_SETREDRAW, true, 0);
        }
    }
}
