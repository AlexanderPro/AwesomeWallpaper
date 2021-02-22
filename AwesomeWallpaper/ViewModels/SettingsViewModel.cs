using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using AwesomeWallpaper.Extensions;
using AwesomeWallpaper.Settings;
using static AwesomeWallpaper.Native.NativeMethods;

namespace AwesomeWallpaper.ViewModels
{
    sealed class SettingsViewModel : DialogViewModelBase
    {
        public ProgramSettings Settings { get; }

        public SettingsViewModel(Window dialog, ProgramSettings settings) : base(dialog)
        {
            Settings = settings;

            WallpaperType = settings.WallpaperType;
            BackgroundMode = settings.BackgroundMode;
            Monitor = settings.Monitor;
            SystemInformationFontFamily = new FontFamily(settings.SystemInformationFontFamily);
            SystemInformationTextColor = settings.SystemInformationTextColor;
            SystemInformationFontSize = settings.SystemInformationFontSize;
            SystemInformationInterval = settings.SystemInformationRefreshInterval == null ? (TimeSpan?)null : TimeSpan.FromSeconds(settings.SystemInformationRefreshInterval.Value);
            SystemInformationHorizontalAlignment = settings.SystemInformationHorizontalAlignment;
            SystemInformationVerticalAlignment = settings.SystemInformationVerticalAlignment;
            SystemInformationTransparency = settings.SystemInformationTransparency;
            ImageFileName = settings.ImageFileName;
            ImageHorizontalAlignment = settings.ImageHorizontalAlignment;
            ImageVerticalAlignment = settings.ImageVerticalAlignment;
            ImageStretch = settings.ImageStretch;
            ImageTransparency = settings.ImageTransparency;
            GalleryIntervalBetweenImages = settings.GalleryIntervalBetweenImages;
            GalleryIntervalForShowImage = settings.GalleryIntervalForShowImage;
            GalleryAutoPlay = settings.GalleryAutoPlay;
            GalleryDirectoryName = settings.GalleryDirectoryName;
            GalleryState = settings.GalleryState;
            GalleryHorizontalAlignment = settings.GalleryHorizontalAlignment;
            GalleryVerticalAlignment = settings.GalleryVerticalAlignment;
            GalleryStretch = settings.GalleryStretch;
            GalleryTransparency = settings.GalleryTransparency;
            VideoFileName = settings.VideoFileName;
            VideoAutoPlay = settings.VideoAutoPlay;
            VideoRepeat = settings.VideoRepeat;
            VideoHorizontalAlignment = settings.VideoHorizontalAlignment;
            VideoVerticalAlignment = settings.VideoVerticalAlignment;
            VideoStretch = settings.VideoStretch;
            VideoVolume = settings.VideoVolume;
            VideoTransparency = settings.VideoTransparency;
            WebUrl = settings.WebUrl;
            WebRefreshInterval = settings.WebRefreshInterval == null ? (TimeSpan?)null : TimeSpan.FromSeconds(settings.WebRefreshInterval.Value);
            WindowHandle = settings.WindowHandle == null ? IntPtr.Zero : new IntPtr(settings.WindowHandle.Value);
            WindowExTool = settings.WindowExTool;
            WindowPreviouseHandle = settings.WindowPreviouseHandle == null ? IntPtr.Zero : new IntPtr(settings.WindowPreviouseHandle.Value);
            WindowPreviouseExTool = settings.WindowPreviouseExTool;
            WindowText = settings.WindowText;
            WindowStatus = settings.WindowStatus;
            WindowClassName = settings.WindowClassName;
            WindowProcessName = settings.WindowProcessName;
            WindowAlignment = settings.WindowAlignment;
            WindowFullScreen = settings.WindowFullScreen;
            WindowUseAfterRestart = settings.WindowUseAfterRestart;
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set { SetProperty(ref _selectedTabIndex, value); }
        }

        private int? _monitor;
        public int? Monitor
        {
            get { return _monitor; }
            set { SetProperty(ref _monitor, value); }
        }

        private WallpaperType _wallpaperType;
        public WallpaperType WallpaperType
        {
            get { return _wallpaperType; }
            set { SetProperty(ref _wallpaperType, value); }
        }

        private BackgroundMode _backgroundMode;
        public BackgroundMode BackgroundMode
        {
            get { return _backgroundMode; }
            set { SetProperty(ref _backgroundMode, value); }
        }

        private string _videoFileName = "";
        public string VideoFileName
        {
            get { return _videoFileName; }
            set { SetProperty(ref _videoFileName, value); }
        }

        private bool _videoAutoPlay;
        public bool VideoAutoPlay
        {
            get { return _videoAutoPlay; }
            set { SetProperty(ref _videoAutoPlay, value); }
        }

        private bool _videoRepeat;
        public bool VideoRepeat
        {
            get { return _videoRepeat; }
            set { SetProperty(ref _videoRepeat, value); }
        }

        private HorizontalAlignment _videoHorizontalAlignment;
        public HorizontalAlignment VideoHorizontalAlignment
        {
            get { return _videoHorizontalAlignment; }
            set { SetProperty(ref _videoHorizontalAlignment, value); }
        }

        private VerticalAlignment _videoVerticalAlignment;
        public VerticalAlignment VideoVerticalAlignment
        {
            get { return _videoVerticalAlignment; }
            set { SetProperty(ref _videoVerticalAlignment, value); }
        }

        private Stretch _videoStretch;
        public Stretch VideoStretch
        {
            get { return _videoStretch; }
            set { SetProperty(ref _videoStretch, value); }
        }

        private double _videoVolume;
        public double VideoVolume
        {
            get { return _videoVolume; }
            set { SetProperty(ref _videoVolume, value); }
        }

        private double _videoTransparency = 0;
        public double VideoTransparency
        {
            get { return _videoTransparency; }
            set { SetProperty(ref _videoTransparency, value); }
        }

        private int _galleryIntervalBetweenImages;
        public int GalleryIntervalBetweenImages
        {
            get { return _galleryIntervalBetweenImages; }
            set { SetProperty(ref _galleryIntervalBetweenImages, value); }
        }

        private int _galleryIntervalForShowImage;
        public int GalleryIntervalForShowImage
        {
            get { return _galleryIntervalForShowImage; }
            set { SetProperty(ref _galleryIntervalForShowImage, value); }
        }

        private string _galleryDirectoryName = "";
        public string GalleryDirectoryName
        {
            get { return _galleryDirectoryName; }
            set { SetProperty(ref _galleryDirectoryName, value); }
        }

        private bool _galleryAutoPlay;
        public bool GalleryAutoPlay
        {
            get { return _galleryAutoPlay; }
            set { SetProperty(ref _galleryAutoPlay, value); }
        }

        private GalleryState _galleryState;
        public GalleryState GalleryState
        {
            get { return _galleryState; }
            set { SetProperty(ref _galleryState, value); }
        }

        private HorizontalAlignment _galleryHorizontalAlignment;
        public HorizontalAlignment GalleryHorizontalAlignment
        {
            get { return _galleryHorizontalAlignment; }
            set { SetProperty(ref _galleryHorizontalAlignment, value); }
        }

        private VerticalAlignment _galleryVerticalAlignment;
        public VerticalAlignment GalleryVerticalAlignment
        {
            get { return _galleryVerticalAlignment; }
            set { SetProperty(ref _galleryVerticalAlignment, value); }
        }

        private Stretch _galleryStretch;
        public Stretch GalleryStretch
        {
            get { return _galleryStretch; }
            set { SetProperty(ref _galleryStretch, value); }
        }

        private double _galleryTransparency = 0;
        public double GalleryTransparency
        {
            get { return _galleryTransparency; }
            set { SetProperty(ref _galleryTransparency, value); }
        }

        private string _imageFileName = "";
        public string ImageFileName
        {
            get { return _imageFileName; }
            set { SetProperty(ref _imageFileName, value); }
        }

        private HorizontalAlignment _imageHorizontalAlignment;
        public HorizontalAlignment ImageHorizontalAlignment
        {
            get { return _imageHorizontalAlignment; }
            set { SetProperty(ref _imageHorizontalAlignment, value); }
        }

        private VerticalAlignment _imageVerticalAlignment;
        public VerticalAlignment ImageVerticalAlignment
        {
            get { return _imageVerticalAlignment; }
            set { SetProperty(ref _imageVerticalAlignment, value); }
        }

        private Stretch _imageStretch;
        public Stretch ImageStretch
        {
            get { return _imageStretch; }
            set { SetProperty(ref _imageStretch, value); }
        }

        private double _imageTransparency = 0;
        public double ImageTransparency
        {
            get { return _imageTransparency; }
            set { SetProperty(ref _imageTransparency, value); }
        }

        private FontFamily _systemInformationFontFamily;
        public FontFamily SystemInformationFontFamily
        {
            get { return _systemInformationFontFamily; }
            set { SetProperty(ref _systemInformationFontFamily, value); }
        }

        private Color _systemInformationTextColor;
        public Color SystemInformationTextColor
        {
            get { return _systemInformationTextColor; }
            set { SetProperty(ref _systemInformationTextColor, value); }
        }

        private int _systemInformationFontSize;
        public int SystemInformationFontSize
        {
            get { return _systemInformationFontSize; }
            set { SetProperty(ref _systemInformationFontSize, value); }
        }

        private TimeSpan? _systemInformationInterval;
        public TimeSpan? SystemInformationInterval
        {
            get { return _systemInformationInterval; }
            set { SetProperty(ref _systemInformationInterval, value); }
        }

        private HorizontalAlignment _systemInformationHorizontalAlignment;
        public HorizontalAlignment SystemInformationHorizontalAlignment
        {
            get { return _systemInformationHorizontalAlignment; }
            set { SetProperty(ref _systemInformationHorizontalAlignment, value); }
        }

        private VerticalAlignment _systemInformationVerticalAlignment;
        public VerticalAlignment SystemInformationVerticalAlignment
        {
            get { return _systemInformationVerticalAlignment; }
            set { SetProperty(ref _systemInformationVerticalAlignment, value); }
        }

        private double _systemInformationTransparency = 0;
        public double SystemInformationTransparency
        {
            get { return _systemInformationTransparency; }
            set { SetProperty(ref _systemInformationTransparency, value); }
        }

        private TimeSpan? _webRefreshInterval;
        public TimeSpan? WebRefreshInterval
        {
            get { return _webRefreshInterval; }
            set { SetProperty(ref _webRefreshInterval, value); }
        }

        private string _webUrl = "";
        public string WebUrl
        {
            get { return _webUrl; }
            set { SetProperty(ref _webUrl, value); }
        }

        private IntPtr _windowHandle = IntPtr.Zero;
        public IntPtr WindowHandle
        {
            get { return _windowHandle; }
            set { SetProperty(ref _windowHandle, value); }
        }

        private bool _windowExTool = false;
        public bool WindowExTool
        {
            get { return _windowExTool; }
            set { SetProperty(ref _windowExTool, value); }
        }

        private IntPtr _windowPreviouseHandle = IntPtr.Zero;
        public IntPtr WindowPreviouseHandle
        {
            get { return _windowPreviouseHandle; }
            set { SetProperty(ref _windowPreviouseHandle, value); }
        }

        private bool _windowPreviouseExTool = false;
        public bool WindowPreviouseExTool
        {
            get { return _windowPreviouseExTool; }
            set { SetProperty(ref _windowPreviouseExTool, value); }
        }

        private string _windowStatus = "Not Selected";
        public string WindowStatus
        {
            get { return _windowStatus; }
            set { SetProperty(ref _windowStatus, value); }
        }

        private string _windowText = "";
        public string WindowText
        {
            get { return _windowText; }
            set { SetProperty(ref _windowText, value); }
        }

        private string _windowClassName = "";
        public string WindowClassName
        {
            get { return _windowClassName; }
            set { SetProperty(ref _windowClassName, value); }
        }

        private string _windowProcessName = "";
        public string WindowProcessName
        {
            get { return _windowProcessName; }
            set { SetProperty(ref _windowProcessName, value); }
        }

        private WindowAlignment _windowAlignment = WindowAlignment.None;
        public WindowAlignment WindowAlignment
        {
            get { return _windowAlignment; }
            set { SetProperty(ref _windowAlignment, value); }
        }

        private bool _windowFullScreen = false;
        public bool WindowFullScreen
        {
            get { return _windowFullScreen; }
            set { SetProperty(ref _windowFullScreen, value); }
        }

        private bool _windowUseAfterRestart = false;
        public bool WindowUseAfterRestart
        {
            get { return _windowUseAfterRestart; }
            set { SetProperty(ref _windowUseAfterRestart, value); }
        }

        private int _windowRowHeight = 25;
        public int WindowRowHeight
        {
            get { return _windowRowHeight; }
            set { SetProperty(ref _windowRowHeight, value); }
        }

        private int _windowDelimiterRowHeight = 12;
        public int WindowDelimiterRowHeight
        {
            get { return _windowDelimiterRowHeight; }
            set { SetProperty(ref _windowDelimiterRowHeight, value); }
        }

        private int _windowImageRowHeight = 0;
        public int WindowImageRowHeight
        {
            get { return _windowImageRowHeight; }
            set { SetProperty(ref _windowImageRowHeight, value); }
        }

        public IEnumerable<FontFamily> SystemFonts => Fonts.SystemFontFamilies.OrderBy(font => font.Source);

        public IEnumerable<int> FontSizes => new[] { 8, 10, 12, 14, 16, 18, 20, 24, 28, 32, 36, 40 };

        public IEnumerable<KeyValuePair<TimeSpan?, string>> RefreshIntervalItems => new int?[] { null, 10, 20, 30, 60, 120, 300, 600, 1800, 3600, 7200, 14400, 24 * 3600 }
        .Select(i => i == null ? new KeyValuePair<TimeSpan?, string>(null, "Disabled") : new KeyValuePair<TimeSpan?, string>(TimeSpan.FromSeconds(i.Value), TimeSpan.FromSeconds(i.Value).ToString()));

        public IEnumerable<KeyValuePair<bool, string>> TrueFalseItems => new List<KeyValuePair<bool, string>>() { new KeyValuePair<bool, string>(true, "True"), new KeyValuePair<bool, string>(false, "False") };

        public IEnumerable<KeyValuePair<int?, string>> Monitors
        {
            get
            {
                var monitors = new List<KeyValuePair<int?, string>>() { new KeyValuePair<int?, string>(null, "All") };
                var monitor = 0;
                EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref Native.Rect rect, IntPtr data) =>
                {
                    monitors.Add(new KeyValuePair<int?, string>(monitor, (monitor + 1).ToString()));
                    monitor++;
                    return true;
                }, IntPtr.Zero);
                return monitors;
            }
        }

        public IEnumerable<KeyValuePair<WindowAlignment, string>> WindowAlignments => EnumExtensions.AsEnumerable<WindowAlignment>().Select(x => new KeyValuePair<WindowAlignment, string>(x, x.ToString()));

        public IEnumerable<KeyValuePair<HorizontalAlignment, string>> HorizontalAlignments => EnumExtensions.AsEnumerable<HorizontalAlignment>().Select(x => new KeyValuePair<HorizontalAlignment, string>(x, x.ToString()));

        public IEnumerable<KeyValuePair<VerticalAlignment, string>> VerticalAlignments => EnumExtensions.AsEnumerable<VerticalAlignment>().Select(x => new KeyValuePair<VerticalAlignment, string>(x, x.ToString()));

        public IEnumerable<KeyValuePair<Stretch, string>> Stretches => EnumExtensions.AsEnumerable<Stretch>().Select(x => new KeyValuePair<Stretch, string>(x, x.ToString()));

        public IEnumerable<KeyValuePair<WallpaperType, string>> WallpaperTypes => EnumExtensions.AsEnumerable<WallpaperType>().Select(x => new KeyValuePair<WallpaperType, string>(x, x.ToString()));

        public IEnumerable<KeyValuePair<BackgroundMode, string>> BackgroundModes => EnumExtensions.AsEnumerable<BackgroundMode>().Select(x => new KeyValuePair<BackgroundMode, string>(x, x.GetDisplayName()));
    }
}
