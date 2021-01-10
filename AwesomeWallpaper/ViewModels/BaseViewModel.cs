using Prism.Mvvm;
using AwesomeWallpaper.Native;
using AwesomeWallpaper.Settings;

namespace AwesomeWallpaper.ViewModels
{
    class BaseViewModel : BindableBase
    {
        protected MonitorInfo _monitor;
        public ProgramSettings Settings { get; }

        public BaseViewModel(MonitorInfo monitor, ProgramSettings settings)
        {
            _monitor = monitor;
            Settings = settings;
        }

        public bool IsHitTestVisible => Settings.InteractiveMode;

        public double Opacity => Settings.WallpaperType == WallpaperType.SystemInformation ? 1 - Settings.SystemInformationTransparency :
                                 Settings.WallpaperType == WallpaperType.Video ? 1 - Settings.VideoTransparency :
                                 Settings.WallpaperType == WallpaperType.Image ? 1 - Settings.ImageTransparency :
                                 Settings.WallpaperType == WallpaperType.Gallery ? 1 - Settings.GalleryTransparency : 1;

        public virtual void Update()
        {
            RaisePropertyChanged(nameof(IsHitTestVisible));
        }

        public virtual void Refresh()
        {

        }
    }
}
