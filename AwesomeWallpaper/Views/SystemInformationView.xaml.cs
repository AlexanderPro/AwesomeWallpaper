using System;
using System.Windows.Threading;
using System.Windows.Controls;
using AwesomeWallpaper.ViewModels;

namespace AwesomeWallpaper.Views
{
    public partial class SystemInformationView : UserControl
    {
        private DispatcherTimer _timer;

        public SystemInformationView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var viewModel = DataContext as BaseViewModel;
            if (_timer == null && viewModel != null)
            {
                _timer = new DispatcherTimer();
                _timer.Interval = viewModel.Settings.SystemInformationRefreshInterval.HasValue ? TimeSpan.FromSeconds(viewModel.Settings.SystemInformationRefreshInterval.Value) : TimeSpan.FromMilliseconds(int.MaxValue);
                _timer.Tick += timer_Tick;
                if (viewModel.Settings.SystemInformationRefreshInterval.HasValue)
                {
                    _timer.Start();
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            (DataContext as BaseViewModel)?.Refresh();
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _timer?.Stop();
            _timer = null;
        }

        public void Refresh()
        {
            _timer?.Stop();
            _timer?.Start();
            (DataContext as BaseViewModel)?.Refresh();
        }
    }
}
