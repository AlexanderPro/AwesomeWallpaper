using System.Windows;
using AwesomeWallpaper.Settings;

namespace AwesomeWallpaper.ViewModels
{
    class WebViewModel : BaseViewModel
    {
        public WebViewModel(MonitorInfo monitor, ProgramSettings settings) : base(monitor, settings)
        {
        }

        public Visibility ToolBarVisibility => Settings.InteractiveMode ? Visibility.Visible : Visibility.Collapsed;

        public override void Update()
        {
            base.Update();
            RaisePropertyChanged(nameof(ToolBarVisibility));
        }
    }
}
