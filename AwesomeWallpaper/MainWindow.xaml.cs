using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Forms;
using static AwesomeWallpaper.NativeConstants;
using static AwesomeWallpaper.NativeMethods;
using static AwesomeWallpaper.Utils.WindowUtils;
using static AwesomeWallpaper.Utils.ImageUtils;

namespace AwesomeWallpaper
{
    public partial class MainWindow : Window
    {
        public bool AllowClose { get; set; } = false;

        public int MonitorLeft { get; set; }

        public int MonitorTop { get; set; }

        public int MonitorWidth { get; set; }

        public int MonitorHeight { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var screens = Screen.AllScreens;
            var handle = new WindowInteropHelper(this).Handle;
            SetStyles(handle);
            SendMessageToProgman();
            var workerWHandle = GetWorkerW(handle);
            var rect = new Rect();
            SetWindowPos(handle, (IntPtr)1, MonitorLeft, MonitorTop, MonitorWidth, MonitorHeight, 0 | 0x0010);
            MapWindowPoints(handle, workerWHandle, ref rect, 2);
            SetParent(handle, workerWHandle);
            SetWindowPos(handle, (IntPtr)1, rect.Left, rect.Top, MonitorWidth, MonitorHeight, 0 | 0x0010);
            var imageBrush = new ImageBrush();
            using (var image = CaptureWindow(workerWHandle, rect.Left, rect.Top, MonitorWidth, MonitorHeight))
            {
                imageBrush.ImageSource = ConvertImageToBitmapSource(image);
                Background = imageBrush;
            }
            RefreshDesktop();
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(WindowProc);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !AllowClose;
            if (e.Cancel)
            {
                RefreshDesktop();
            }
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
                var rc = (Rect*)lParam.ToPointer();
                SetWindowPos(handle, IntPtr.Zero, 0, 0, rc->Right, rc->Left, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOZORDER);
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
