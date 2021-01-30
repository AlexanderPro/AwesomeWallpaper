using System;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using CefSharp;
using CefSharp.Wpf;
using AwesomeWallpaper.Utils;
using static AwesomeWallpaper.Native.NativeMethods;

namespace AwesomeWallpaper
{
    public partial class App : Application
    {
        private Mutex _oneInstanceMutex;
        private ViewManager _manager;
        private DispatcherTimer _timerSystemTray;

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;

            var settings = new CefSettings();
            settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(settings, true, browserProcessHandler: null);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _oneInstanceMutex = new Mutex(false, "AwesomeWallpaperOneInstanceMutex", out var createNew);
            if (!createNew)
            {
                Shutdown();
                return;
            }

            _manager = new ViewManager();
            var isSettingsLoaded = _manager.LoadSettings();
            if (isSettingsLoaded)
            {
                if (_manager.Settings.WallpaperType == Settings.WallpaperType.Window && 
                    _manager.Settings.WindowUseAfterRestart && 
                    !string.IsNullOrEmpty(_manager.Settings.WindowProcessName))
                {
                    EnumWindows(new EnumWindowsProc((hWnd, lParam) =>
                    {
                        var processName = WindowUtils.GetProcessName(hWnd);
                        var windowText = WindowUtils.GetWmGetText(hWnd);
                        var windowClassName = WindowUtils.GetClassName(hWnd);
                        if (string.Compare(windowText, _manager.Settings.WindowText, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                            string.Compare(windowClassName, _manager.Settings.WindowClassName, StringComparison.CurrentCultureIgnoreCase) == 0 &&
                            string.Compare(processName, _manager.Settings.WindowProcessName, StringComparison.CurrentCultureIgnoreCase) == 0)
                        {
                            _manager.Settings.WindowHandle = hWnd == IntPtr.Zero ? null : (long?)hWnd.ToInt64();
                            _manager.Settings.WindowExTool = WindowUtils.IsExToolWindow(hWnd);
                            _manager.Settings.WindowStatus = "Selected";
                            return false;
                        }
                        return true;
                    }), IntPtr.Zero);
                }
                _manager.CreateViews();
                _manager.InitTray();

                _timerSystemTray = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(1),
                };
                _timerSystemTray.Tick += TimerTick;
                _timerSystemTray.Start();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            _manager.RefreshTray();
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            exception = exception ?? new Exception("OnCurrentDomainUnhandledException");
            var message = exception.Message;
            if (exception is Win32Exception)
            {
                message = $"Win32 Error Code = {((Win32Exception)exception).ErrorCode},{Environment.NewLine}{message}";
            }
            MessageBox.Show(message, AssemblyUtils.AssemblyProductName, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
