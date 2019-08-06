using System;
using static AwesomeWallpaper.NativeConstants;
using static AwesomeWallpaper.NativeMethods;


namespace AwesomeWallpaper.Utils
{
    static class WindowUtils
    {
        /// <summary>
        /// Special hack from https://www.codeproject.com/Articles/856020/Draw-behind-Desktop-Icons-in-Windows
        /// Send 0x052C to Progman. This message directs Progman to spawn a 
        /// WorkerW behind the desktop icons. If it is already there, nothing 
        /// happens.
        /// </summary>
        public static void ShowAlwaysOnDesktopBehindIcons(IntPtr hwnd)
        {
            var progmanHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            SendMessage(progmanHandle, 0x052C, 0x0000000D, 0);
            SendMessage(progmanHandle, 0x052C, 0x0000000D, 1);

            var workerWHandle = IntPtr.Zero;
            EnumWindows(new EnumWindowsProc((topHandle, topParamHandle) =>
            {
                IntPtr shellHandle = FindWindowEx(topHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shellHandle != IntPtr.Zero)
                {
                    workerWHandle = FindWindowEx(IntPtr.Zero, topHandle, "WorkerW", null);
                }
                return true;
            }), IntPtr.Zero);
            workerWHandle = workerWHandle == IntPtr.Zero ? progmanHandle : workerWHandle;
            SetParent(hwnd, workerWHandle);
        }

        public static void SetStyles(IntPtr hwnd)
        {
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
            SetWindowPos(hwnd, new IntPtr(HWND_BOTTOM), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
        }

        public static void EnableNoActive(IntPtr hwnd, bool enable)
        {
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (enable)
            {
                exStyle |= WS_EX_NOACTIVATE;
            }
            else
            {
                exStyle &= ~WS_EX_NOACTIVATE;
            }
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
        }
    }
}
