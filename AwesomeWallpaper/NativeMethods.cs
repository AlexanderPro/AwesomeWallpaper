using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace AwesomeWallpaper
{
    [SuppressUnmanagedCodeSecurity]
    static class NativeMethods
    {
        public delegate bool EnumMonitorProc(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect rcMonitor, IntPtr data);

        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int dx, int cy, uint flags);

        [DllImport("user32")]
        public static extern bool EnumDisplayMonitors(IntPtr hDC, IntPtr clipRect, EnumMonitorProc proc, IntPtr data);

        [DllImport("user32")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfo info);

        [DllImport("user32")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32")]
        public static extern int SetWindowLong(IntPtr hWnd, int index, int value);

        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("psapi", SetLastError = true)]
        public static extern bool GetPerformanceInfo(ref PerformanceInformation pi, int size);

        [DllImport("kernel32")]
        public static extern bool GetVersionEx(ref OSVersionInfoEx versionInfo);

        [DllImport("user32")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32")]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder name, int count);

        [DllImport("user32")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
    }
}
