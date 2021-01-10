using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Drawing;

namespace AwesomeWallpaper.Native
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

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("user32")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("psapi")]
        public static extern bool GetPerformanceInfo(ref PerformanceInformation pi, int size);

        [DllImport("kernel32")]
        public static extern bool GetVersionEx(ref OSVersionInfoEx versionInfo);

        [DllImport("user32")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32")]
        public static extern IntPtr FindWindowEx(IntPtr hWndParent, IntPtr hWndChildAfter, string lpWindowClass, string lpWindowName);

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

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessageTimeout(IntPtr hWnd, int wMsg, int wParam, int lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out int lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out Rect rect);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32")]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Rect rect, [MarshalAs(UnmanagedType.U4)] int cPoints);

        [DllImport("user32")]
        public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Point pt, [MarshalAs(UnmanagedType.U4)] int cPoints);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
}