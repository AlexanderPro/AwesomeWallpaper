using System.Windows;
using AwesomeWallpaper.Settings;

namespace AwesomeWallpaper.ViewModels
{
    class GalleryViewModel : BaseViewModel
    {
        public GalleryViewModel(MonitorInfo monitor, ProgramSettings settings) : base(monitor, settings)
        {
        }

        public Visibility ToolBarVisibility => Settings.InteractiveMode ? Visibility.Visible : Visibility.Collapsed;

        public GridLength ToolBarHeight => Settings.InteractiveMode ? new GridLength(50) : new GridLength(0);

        public override void Update()
        {
            base.Update();
            RaisePropertyChanged(nameof(ToolBarVisibility));
            RaisePropertyChanged(nameof(ToolBarHeight));
        }
    }
}
