using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using AwesomeWallpaper.Utils;
using static AwesomeWallpaper.NativeConstants;
using static AwesomeWallpaper.NativeMethods;

namespace AwesomeWallpaper
{
    public partial class MainWindow : Window
    {
        public bool AllowClose { get; set; } = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            WindowUtils.SetStyles(handle);
            WindowUtils.ShowAlwaysOnDesktopBehindIcons(handle);
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(WindowProc);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !AllowClose;
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
