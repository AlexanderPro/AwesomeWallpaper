using System;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using AwesomeWallpaper.Utils;

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
            _manager.LoadSettings();
            _manager.CreateViews();
            _manager.InitTray();

            _timerSystemTray = new DispatcherTimer();
            _timerSystemTray.Interval = TimeSpan.FromSeconds(1);
            _timerSystemTray.Tick += TimerTick;
            _timerSystemTray.Start();
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
