using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Forms;
using AwesomeWallpaper.Settings;
using AwesomeWallpaper.Native;
using static AwesomeWallpaper.Native.NativeConstants;
using static AwesomeWallpaper.Native.NativeMethods;
using static AwesomeWallpaper.Utils.WindowUtils;
using static AwesomeWallpaper.Utils.ImageUtils;

namespace AwesomeWallpaper
{
    public partial class MainWindow : Window
    {
        public bool AllowClose { get; set; } = false;

        public Native.Rect MonitorRect { get; private set; }

        public ProgramSettings Settings { get; private set; }

        public MainWindow(ProgramSettings settings, Native.Rect monitorRect)
        {
            InitializeComponent();
            Settings = settings;
            MonitorRect = monitorRect;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var screens = Screen.AllScreens;
            var handle = Settings.WallpaperType == WallpaperType.Window ? new IntPtr(Settings.WindowHandle.Value) : new WindowInteropHelper(this).Handle;            
            SetStyles(handle);
            SendMessageToProgman();
            var workerWHandle = GetWorkerW(handle);
            var rect = new Native.Rect();
            if (Settings.WallpaperType == WallpaperType.Window)
            {
                Hide();
                if (Settings.WindowFullScreen)
                {
                    SetWindowPos(handle, (IntPtr)1, MonitorRect.Left, MonitorRect.Top, MonitorRect.Width, MonitorRect.Height, 0 | 0x0010);
                }
                SetParent(handle, workerWHandle);
                RefreshDesktop();
            }
            else
            {
                if (Settings.WindowHandle != null)
                {
                    PostMessage(new IntPtr(Settings.WindowHandle.Value), WM_CLOSE, 0, 0);
                    Thread.Sleep(1000);
                }
                Settings.ClearWindow();
                SetWindowPos(handle, (IntPtr)1, MonitorRect.Left, MonitorRect.Top, MonitorRect.Width, MonitorRect.Height, 0 | 0x0010);
                MapWindowPoints(handle, workerWHandle, ref rect, 2);
                SetParent(handle, workerWHandle);
                SetWindowPos(handle, (IntPtr)1, rect.Left, rect.Top, MonitorRect.Width, MonitorRect.Height, 0 | 0x0010);
                var imageBrush = new ImageBrush();
                using (var image = CaptureWindow(workerWHandle, rect.Left, rect.Top, MonitorRect.Width, MonitorRect.Height))
                {
                    imageBrush.ImageSource = ConvertImageToBitmapSource(image);
                    Background = imageBrush;
                }
                RefreshDesktop();
                var hwndSource = HwndSource.FromHwnd(handle);
                hwndSource.AddHook(WindowProc);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !AllowClose;
            /*if (Settings.WallpaperType == WallpaperType.Window && Settings.WindowHandle != null && Settings.WindowHandle != IntPtr.Zero)
            {
                PostMessage(Settings.WindowHandle, WM_CLOSE, 0, 0);
                Thread.Sleep(1000);
            }*/
            RefreshDesktop();
        }

        private unsafe IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_WINDOWPOSCHANGING)
            {
                var windowPos = Marshal.PtrToStructure<WindowPos>(lParam);
                windowPos.hwndInsertAfter = new IntPtr(HWND_BOTTOM);
                windowPos.flags &= ~(uint)SWP_NOZORDER;
                handled = true;
            }

            if (msg == WM_DPICHANGED)
            {
                var handle = new WindowInteropHelper(this).Handle;
                var rc = (Native.Rect*)lParam.ToPointer();
                SetWindowPos(handle, IntPtr.Zero, 0, 0, rc->Right, rc->Left, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOZORDER);
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
