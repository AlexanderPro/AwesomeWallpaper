using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Management;
using AwesomeWallpaper.Native;
using static AwesomeWallpaper.Native.NativeConstants;
using static AwesomeWallpaper.Native.NativeMethods;


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
        public static void SendMessageToProgman()
        {
            var progmanHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            SendMessageTimeout(progmanHandle, 0x052C, 0, 0, SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out var result);
            //SendMessage(progmanHandle, 0x052C, 0, 0);
            //SendMessage(progmanHandle, 0x052C, 0x0000000D, 0);
            //SendMessage(progmanHandle, 0x052C, 0x0000000D, 1);
        }

        public static IntPtr GetWorkerW(IntPtr hwnd)
        {
            var workerWHandle = IntPtr.Zero;
            EnumWindows(new EnumWindowsProc((topHandle, topParamHandle) =>
            {
                var shellHandle = FindWindowEx(topHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shellHandle != IntPtr.Zero)
                {
                    workerWHandle = FindWindowEx(IntPtr.Zero, topHandle, "WorkerW", null);
                }
                return true;
            }), IntPtr.Zero);
            return workerWHandle;
        }

        public static bool IsExToolWindow(IntPtr hwnd)
        {
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            return (exStyle & WS_EX_TOOLWINDOW) != 0;
        }

        public static void EnableExToolWindow(IntPtr hwnd, bool enable)
        {
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (enable)
            {
                exStyle |= WS_EX_TOOLWINDOW;
            }
            else
            {
                exStyle &= ~WS_EX_TOOLWINDOW;
            }
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
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

        public static void RefreshDesktop()
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, null, SPIF_UPDATEINIFILE);
        }

        public static Image CaptureWindow(IntPtr hwnd, int x, int y, int width, int height)
        {
            // get te hDC of the target window
            var hdcSrc = GetWindowDC(hwnd);
            
            // get the size
            var windowRect = new Rect();
            GetWindowRect(hwnd, out windowRect);

            // create a device context we can copy to
            var hdcDest = CreateCompatibleDC(hdcSrc);

            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            var hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);
            
            // select the bitmap object
            var hOld = SelectObject(hdcDest, hBitmap);
            
            // bitblt over
            BitBlt(hdcDest, 0, 0, width, height, hdcSrc, x, y, SRCCOPY);
            
            // restore selection
            SelectObject(hdcDest, hOld);
            
            // clean up
            DeleteDC(hdcDest);
            ReleaseDC(hwnd, hdcSrc);
            
            // get a .NET image object for it
            var image = Image.FromHbitmap(hBitmap);
            
            // free up the Bitmap object
            DeleteObject(hBitmap);
            return image;
        }

        public static string GetWmGetText(IntPtr hwnd)
        {
            var titleSize = SendMessage(hwnd, WM_GETTEXTLENGTH, 0, 0);
            if (titleSize == 0)
            {
                return string.Empty;
            }

            var title = new StringBuilder(titleSize + 1);
            SendMessage(hwnd, WM_GETTEXT, title.Capacity, title);
            return title.ToString();
        }

        public static string GetClassName(IntPtr hwnd)
        {
            var builder = new StringBuilder(1024);
            NativeMethods.GetClassName(hwnd, builder, builder.Capacity);
            var className = builder.ToString();
            return className;
        }

        public static string GetProcessName(IntPtr hwnd)
        {
            int processId = 0;
            try
            {
                GetWindowThreadProcessId(hwnd, out processId);
                var process = Process.GetProcessById(processId);
                return process.MainModule.FileName;
            }
            catch
            {
                try
                {
                    return GetProcessName(processId);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static Bitmap PrintWindow(IntPtr hwnd)
        {
            Rect rect;
            GetWindowRect(hwnd, out rect);
            var bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var hdc = graphics.GetHdc();
                NativeMethods.PrintWindow(hwnd, hdc, 0);
                graphics.ReleaseHdc(hdc);
            }
            return bitmap;
        }

        public static IntPtr GetParentWindow(IntPtr hwnd)
        {
            var resultHwnd = hwnd;
            var parentHwnd = IntPtr.Zero;
            while ((parentHwnd = GetParent(resultHwnd)) != IntPtr.Zero)
            {
                resultHwnd = parentHwnd;
            }
            return resultHwnd;
        }

        private static string GetProcessName(int processId)
        {
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ProcessId = " + processId))
            using (var objects = searcher.Get())
            {
                var baseObject = objects.Cast<ManagementBaseObject>().FirstOrDefault();
                if (baseObject != null)
                {
                    return baseObject["ExecutablePath"] != null ? baseObject["ExecutablePath"].ToString() : "";
                }
                return "";
            }
        }
    }
}
