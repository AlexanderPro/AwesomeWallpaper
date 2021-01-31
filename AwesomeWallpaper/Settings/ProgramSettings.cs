using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Xml.Serialization;
using Prism.Mvvm;

namespace AwesomeWallpaper.Settings
{
    [Serializable]
    public class ProgramSettings : BindableBase
    {
        [NonSerialized]
        private readonly ColorConverter _colorConverter = new ColorConverter();

        private int? _monitor = null;
        public int? Monitor
        {
            get { return _monitor; }
            set { SetProperty(ref _monitor, value); }
        }

        private WallpaperType _wallpaperType = WallpaperType.SystemInformation;
        public WallpaperType WallpaperType
        {
            get { return _wallpaperType; }
            set { SetProperty(ref _wallpaperType, value); }
        }

        private bool _interactiveMode = false;
        [XmlIgnore]
        public bool InteractiveMode
        {
            get { return _interactiveMode; }
            set { SetProperty(ref _interactiveMode, value); }
        }
       
        private string _videoFileName = "";
        public string VideoFileName
        {
            get { return _videoFileName; }
            set { SetProperty(ref _videoFileName, value); }
        }

        private bool _videoAutoPlay = true;
        public bool VideoAutoPlay
        {
            get { return _videoAutoPlay; }
            set { SetProperty(ref _videoAutoPlay, value); }
        }

        private bool _videoRepeat = true;
        public bool VideoRepeat
        {
            get { return _videoRepeat; }
            set { SetProperty(ref _videoRepeat, value); }
        }

        private MediaState _videoState = MediaState.Stop;
        public MediaState VideoState
        {
            get { return _videoState; }
            set { SetProperty(ref _videoState, value); }
        }

        private double _videoVolume = 0.5;
        public double VideoVolume
        {
            get { return _videoVolume; }
            set { SetProperty(ref _videoVolume, value); }
        }

        private HorizontalAlignment _videoHorizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment VideoHorizontalAlignment
        {
            get { return _videoHorizontalAlignment; }
            set { SetProperty(ref _videoHorizontalAlignment, value); }
        }

        private VerticalAlignment _videoVerticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment VideoVerticalAlignment
        {
            get { return _videoVerticalAlignment; }
            set { SetProperty(ref _videoVerticalAlignment, value); }
        }

        private Stretch _videoStretch = Stretch.None;
        public Stretch VideoStretch
        {
            get { return _videoStretch; }
            set { SetProperty(ref _videoStretch, value); }
        }

        private double _videoTransparency = 0;
        public double VideoTransparency
        {
            get { return _videoTransparency; }
            set { SetProperty(ref _videoTransparency, value); }
        }

        private int _galleryIntervalBetweenImages = 3;
        public int GalleryIntervalBetweenImages
        {
            get { return _galleryIntervalBetweenImages; }
            set { SetProperty(ref _galleryIntervalBetweenImages, value); }
        }

        private int _galleryIntervalForShowImage = 7;
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

        private bool _galleryAutoPlay = true;
        public bool GalleryAutoPlay
        {
            get { return _galleryAutoPlay; }
            set { SetProperty(ref _galleryAutoPlay, value); }
        }

        private GalleryState _galleryState = GalleryState.Stop;
        public GalleryState GalleryState
        {
            get { return _galleryState; }
            set { SetProperty(ref _galleryState, value); }
        }

        private HorizontalAlignment _galleryHorizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment GalleryHorizontalAlignment
        {
            get { return _galleryHorizontalAlignment; }
            set { SetProperty(ref _galleryHorizontalAlignment, value); }
        }

        private VerticalAlignment _galleryVerticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment GalleryVerticalAlignment
        {
            get { return _galleryVerticalAlignment; }
            set { SetProperty(ref _galleryVerticalAlignment, value); }
        }

        private Stretch _galleryStretch = Stretch.None;
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

        private HorizontalAlignment _imageHorizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment ImageHorizontalAlignment
        {
            get { return _imageHorizontalAlignment; }
            set { SetProperty(ref _imageHorizontalAlignment, value); }
        }

        private VerticalAlignment _imageVerticalAlignment = VerticalAlignment.Center;
        public VerticalAlignment ImageVerticalAlignment
        {
            get { return _imageVerticalAlignment; }
            set { SetProperty(ref _imageVerticalAlignment, value); }
        }

        private Stretch _imageStretch = Stretch.None;
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

        private string _systemInformationFontFamily = "Arial";
        public string SystemInformationFontFamily
        {
            get { return _systemInformationFontFamily; }
            set { SetProperty(ref _systemInformationFontFamily, value); }
        }

        private int _systemInformationFontSize = 16;
        public int SystemInformationFontSize
        {
            get { return _systemInformationFontSize; }
            set { SetProperty(ref _systemInformationFontSize, value); }
        }

        private string _systemInformationTextColorString;
        public string SystemInformationTextColorString
        {
            get { return _systemInformationTextColorString; }
            set
            {
                _systemInformationTextColor = (Color)_colorConverter.ConvertFrom(value);
                SetProperty(ref _systemInformationTextColorString, value);
                SetProperty(ref _systemInformationTextColor, (Color)_colorConverter.ConvertFrom(value));
            }
        }

        private Color _systemInformationTextColor = Colors.White;
        [XmlIgnore]
        public Color SystemInformationTextColor
        {
            get { return _systemInformationTextColor; }
            set
            {
                _systemInformationTextColorString = _colorConverter.ConvertToString(value);
                SetProperty(ref _systemInformationTextColor, value);
                SetProperty(ref _systemInformationTextColorString, _colorConverter.ConvertToString(value));
            }
        }

        [XmlIgnore]
        public SolidColorBrush SystemInformationSolidColorBrush
        {
            get
            {
                return new SolidColorBrush(SystemInformationTextColor);
            }
        }

        private int? _systemInformationRefreshInterval = 60;
        public int? SystemInformationRefreshInterval
        {
            get { return _systemInformationRefreshInterval; }
            set { SetProperty(ref _systemInformationRefreshInterval, value); }
        }

        private HorizontalAlignment _systemInformationHorizontalAlignment = HorizontalAlignment.Right;
        public HorizontalAlignment SystemInformationHorizontalAlignment
        {
            get { return _systemInformationHorizontalAlignment; }
            set { SetProperty(ref _systemInformationHorizontalAlignment, value); }
        }

        private VerticalAlignment _systemInformationVerticalAlignment = VerticalAlignment.Top;
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

        private int? _webRefreshInterval = null;
        public int? WebRefreshInterval
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

        private long? _windowHandle = null;
        [XmlIgnore]
        public long? WindowHandle
        {
            get { return _windowHandle; }
            set { SetProperty(ref _windowHandle, value); }
        }

        private bool _windowExTool = false;
        [XmlIgnore]
        public bool WindowExTool
        {
            get { return _windowExTool; }
            set { SetProperty(ref _windowExTool, value); }
        }

        private long? _windowPreviouseHandle = null;
        [XmlIgnore]
        public long? WindowPreviouseHandle
        {
            get { return _windowPreviouseHandle; }
            set { SetProperty(ref _windowPreviouseHandle, value); }
        }

        private bool _windowPreviouseExTool = false;
        [XmlIgnore]
        public bool WindowPreviouseExTool
        {
            get { return _windowPreviouseExTool; }
            set { SetProperty(ref _windowPreviouseExTool, value); }
        }

        private string _windowStatus = "Not Selected";
        [XmlIgnore]
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

        private List<string> _videoFileExtensions = new List<string> {};
        [XmlArray("VideoFileExtensions")]
        [XmlArrayItem("VideoFileExtension")]
        public List<string> VideoFileExtensions
        {
            get { return _videoFileExtensions; }
            set { SetProperty(ref _videoFileExtensions, value); }
        }

        private List<string> _galleryFileExtensions = new List<string> {};
        [XmlArray("GalleryFileExtensions")]
        [XmlArrayItem("GalleryFileExtension")]
        public List<string> GalleryFileExtensions
        {
            get { return _galleryFileExtensions; }
            set { SetProperty(ref _galleryFileExtensions, value); }
        }

        public void ClearWindow()
        {
            WindowHandle = null;
            WindowPreviouseHandle = null;
            WindowExTool = false;
            WindowPreviouseExTool = false;
            WindowStatus = "";
            WindowText = "";
            WindowClassName = "";
            WindowProcessName = "";
            WindowAlignment = WindowAlignment.None;
            WindowFullScreen = false;
            WindowUseAfterRestart = false;
        }
    }
}