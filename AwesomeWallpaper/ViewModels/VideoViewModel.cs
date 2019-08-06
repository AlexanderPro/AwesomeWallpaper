using System.Windows;
using AwesomeWallpaper.Settings;

namespace AwesomeWallpaper.ViewModels
{
    class VideoViewModel : BaseViewModel
    {
        public Visibility TopBarVisibility => Settings.InteractiveMode ? Visibility.Visible : Visibility.Collapsed;

        public Visibility BottomBarVisibility => Settings.InteractiveMode ? Visibility.Visible : Visibility.Collapsed;

        public VideoViewModel(MonitorInfo monitor, ProgramSettings settings) : base(monitor, settings)
        {
        }

        public override void Update()
        {
            base.Update();
            RaisePropertyChanged(nameof(TopBarVisibility));
            RaisePropertyChanged(nameof(BottomBarVisibility));
        }
    }
}
