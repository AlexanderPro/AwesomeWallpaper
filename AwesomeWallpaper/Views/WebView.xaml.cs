using System;
using System.Windows.Threading;
using System.Windows.Controls;
using CefSharp;
using AwesomeWallpaper.ViewModels;

namespace AwesomeWallpaper.Views
{
    public partial class WebView : UserControl
    {
        private DispatcherTimer _timer;

        public WebView()
        {
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Browser.GetBrowser()?.Reload(true);
        }

        public void Refresh()
        {
            _timer?.Stop();
            _timer?.Start();
            Browser.GetBrowser().Reload(true);
        }

        private void Browser_IsBrowserInitializedChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (Browser.IsBrowserInitialized && _timer == null)
            {
                var viewModel = (BaseViewModel)DataContext;
                _timer = new DispatcherTimer
                {
                    Interval = viewModel.Settings.WebRefreshInterval.HasValue ? TimeSpan.FromSeconds(viewModel.Settings.WebRefreshInterval.Value) : TimeSpan.FromMilliseconds(int.MaxValue)
                };
                _timer.Tick += Timer_Tick;
                if (viewModel.Settings.WebRefreshInterval.HasValue)
                {
                    _timer.Start();
                }
                Navigate(viewModel.Settings.WebUrl ?? "");
            }
        }

        private void Navigate(string url)
        {
            if (!Browser.IsInitialized)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                Browser.LoadHtml($@"<!DOCTYPE html><html style=""background: white; min-height: 100%;""><head></head><body style=""background: white; min-height: 100%;""></body></html>");
            }
            else
            {
                Browser.Load(url);
            }
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _timer?.Stop();
            _timer = null;
        }
    }
}