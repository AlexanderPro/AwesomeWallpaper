using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Drawing;
using AwesomeWallpaper.Extensions;
using AwesomeWallpaper.Settings;
using AwesomeWallpaper.Native;
using static AwesomeWallpaper.Native.NativeConstants;
using static AwesomeWallpaper.Native.NativeMethods;
using static AwesomeWallpaper.Utils.WindowUtils;

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
            if (Settings.WindowPreviouseHandle != null)
            {
                var handle = new IntPtr(Settings.WindowPreviouseHandle.Value);
                SetParent(handle, IntPtr.Zero);
                if (!Settings.WindowPreviouseExTool)
                {
                    EnableExToolWindow(handle, false);
                }

                SetWindowPos(handle, new IntPtr(HWND_TOP), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
                Settings.WindowPreviouseHandle = null;
                Settings.WindowPreviouseExTool = false;
                RefreshDesktop();
                Thread.Sleep(1000);
                SetParent(handle, IntPtr.Zero);
            }

            if (Settings.WindowHandle != null)
            {
                var handle = new IntPtr(Settings.WindowHandle.Value);
                SetParent(handle, IntPtr.Zero);
                if (!Settings.WindowExTool)
                {
                    EnableExToolWindow(handle, false);
                }
                SetWindowPos(handle, new IntPtr(HWND_TOP), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
                RefreshDesktop();
                Thread.Sleep(1000);
                SetParent(handle, IntPtr.Zero);
            }

            var rect = new Native.Rect();
            if (Settings.WallpaperType == WallpaperType.Window)
            {
                Hide();
                var handle = Settings.WindowHandle == null ? IntPtr.Zero : new IntPtr(Settings.WindowHandle.Value);
                if (handle == IntPtr.Zero)
                {
                    RefreshDesktop();
                    return;
                }
                SendMessageToProgman();
                EnableExToolWindow(handle, true);
                var workerWHandle = GetWorkerW(handle);
                if (Settings.WindowFullScreen)
                {
                    SetWindowPos(handle, (IntPtr)1, MonitorRect.Left, MonitorRect.Top, MonitorRect.Width, MonitorRect.Height, 0 | 0x0010);
                }
                else
                {
                    // Move a window to new monitor if nessesary
                    var currentMonitorHandle = MonitorFromWindow(handle, MONITOR_DEFAULTTONEAREST);
                    var currentMonitorInfo = new MonitorInfo();
                    currentMonitorInfo.Init();

                    GetMonitorInfo(currentMonitorHandle, ref currentMonitorInfo);
                    var currentMonitorRect = currentMonitorInfo.rcMonitor;

                    GetWindowRect(handle, out Native.Rect windowRect);
                    var left = MonitorRect.Left + windowRect.Left - currentMonitorRect.Left;
                    var top = MonitorRect.Top + windowRect.Top - currentMonitorRect.Top;
                    MoveWindow(handle, left, top, windowRect.Width, windowRect.Height, true);

                    // Alignment a window
                    switch (Settings.WindowAlignment)
                    {
                        case WindowAlignment.TopLeft:
                            {
                                left = MonitorRect.Left;
                                top = MonitorRect.Top;
                            }
                            break;

                        case WindowAlignment.TopCenter:
                            {
                                left = MonitorRect.Left + ((MonitorRect.Width - windowRect.Width) / 2);
                                top = MonitorRect.Top;
                            }
                            break;

                        case WindowAlignment.TopRight:
                            {
                                left = MonitorRect.Left + (MonitorRect.Width - windowRect.Width);
                                top = MonitorRect.Top;
                            }
                            break;

                        case WindowAlignment.MiddleLeft:
                            {
                                left = MonitorRect.Left;
                                top = MonitorRect.Top + ((MonitorRect.Height - windowRect.Height) / 2);
                            }
                            break;

                        case WindowAlignment.MiddleCenter:
                            {
                                left = MonitorRect.Left + ((MonitorRect.Width - windowRect.Width) / 2);
                                top = MonitorRect.Top + ((MonitorRect.Height - windowRect.Height) / 2);
                            }
                            break;

                        case WindowAlignment.MiddleRight:
                            {
                                left = MonitorRect.Left + (MonitorRect.Width - windowRect.Width);
                                top = MonitorRect.Top + ((MonitorRect.Height - windowRect.Height) / 2);
                            }
                            break;

                        case WindowAlignment.BottomLeft:
                            {
                                left = MonitorRect.Left;
                                top = MonitorRect.Top + (MonitorRect.Height - windowRect.Height);
                            }
                            break;

                        case WindowAlignment.BottomCenter:
                            {
                                left = MonitorRect.Left + ((MonitorRect.Width - windowRect.Width) / 2);
                                top = MonitorRect.Top + (MonitorRect.Height - windowRect.Height);
                            }
                            break;

                        case WindowAlignment.BottomRight:
                            {
                                left = MonitorRect.Left + (MonitorRect.Width - windowRect.Width);
                                top = MonitorRect.Top + (MonitorRect.Height - windowRect.Height);
                            }
                            break;
                    }
                    MoveWindow(handle, left, top, windowRect.Width, windowRect.Height, true);
                }
                SetParent(handle, workerWHandle);
                RefreshDesktop();
            }
            else
            {
                Settings.ClearWindow();
                var handle = new WindowInteropHelper(this).Handle;
                SendMessageToProgman();
                EnableExToolWindow(handle, true);
                var workerWHandle = GetWorkerW(handle);
                SetWindowPos(handle, (IntPtr)1, MonitorRect.Left, MonitorRect.Top, MonitorRect.Width, MonitorRect.Height, 0 | 0x0010);
                MapWindowPoints(handle, workerWHandle, ref rect, 2);
                SetParent(handle, workerWHandle);
                SetWindowPos(handle, (IntPtr)1, rect.Left, rect.Top, MonitorRect.Width, MonitorRect.Height, 0 | 0x0010);
                var imageBrush = new ImageBrush();
                using (var originalBitmap = CaptureWindow(workerWHandle, rect.Left, rect.Top, MonitorRect.Width, MonitorRect.Height))
                using (var bitmap = ConvertBitmap(originalBitmap, Settings.BackgroundMode))
                {
                    imageBrush.ImageSource = bitmap.ConvertToBitmapSource();
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
            if (e.Cancel && Settings.WindowHandle != null)
            {
                var handle = new IntPtr(Settings.WindowHandle.Value);
                SetParent(handle, IntPtr.Zero);
                if (!Settings.WindowExTool)
                {
                    EnableExToolWindow(handle, false);
                }
                RefreshDesktop();
                SetParent(handle, IntPtr.Zero);
            }
            else
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
                var rc = (Native.Rect*)lParam.ToPointer();
                SetWindowPos(handle, IntPtr.Zero, 0, 0, rc->Right, rc->Left, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOZORDER);
                handled = true;
            }

            return IntPtr.Zero;
        }

        private Bitmap ConvertBitmap(Bitmap original, BackgroundMode mode)
        {
            switch (mode)
            {
                case BackgroundMode.None: return original.Copy();
                case BackgroundMode.Blur: return original.Blur(3);
                case BackgroundMode.Pixelate: return original.Pixelate(3);
                case BackgroundMode.Dark: return original.Dark();
                case BackgroundMode.BlackAndWhite: return original.BlackAndWhite();
                case BackgroundMode.Grayscale: return original.Grayscale();
                default: return original.Copy();
            }
        }
    }
}
