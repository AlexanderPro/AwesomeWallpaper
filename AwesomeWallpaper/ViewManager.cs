using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Interop;
using System.Linq;
using System.IO;
using System.Text;
using Hardcodet.Wpf.TaskbarNotification;
using AwesomeWallpaper.Native;
using AwesomeWallpaper.Utils;
using AwesomeWallpaper.ViewModels;
using AwesomeWallpaper.Views;
using AwesomeWallpaper.Settings;
using static AwesomeWallpaper.Native.NativeMethods;

namespace AwesomeWallpaper
{
    class ViewManager
    {
        private TaskbarIcon _tray;
        private TaskbarIconViewModel _trayViewModel;
        private ObservableCollection<BaseViewModel> _viewModels;
        private List<UserControl> _views;
        private List<MainWindow> _windows;

        public ProgramSettings Settings { get; private set; } = new ProgramSettings();

        public ViewManager()
        {
            _viewModels = new ObservableCollection<BaseViewModel>();
            _views = new List<UserControl>();
            _windows = new List<MainWindow>();
        }

        public void ApplySettings(SettingsViewModel settings)
        {
            Settings.WallpaperType = settings.WallpaperType;
            Settings.BackgroundMode = settings.BackgroundMode;
            Settings.Monitor = settings.Monitor;
            Settings.SystemInformationFontFamily = settings.SystemInformationFontFamily.Source;
            Settings.SystemInformationFontSize = settings.SystemInformationFontSize;
            Settings.SystemInformationTextColor = settings.SystemInformationTextColor;
            Settings.SystemInformationRefreshInterval = settings.SystemInformationInterval == null ? null : (int?)settings.SystemInformationInterval.Value.TotalSeconds;
            Settings.SystemInformationHorizontalAlignment = settings.SystemInformationHorizontalAlignment;
            Settings.SystemInformationVerticalAlignment = settings.SystemInformationVerticalAlignment;
            Settings.SystemInformationTransparency = settings.SystemInformationTransparency;
            Settings.ImageFileName = settings.ImageFileName;
            Settings.ImageFileName = settings.ImageFileName;
            Settings.ImageHorizontalAlignment = settings.ImageHorizontalAlignment;
            Settings.ImageVerticalAlignment = settings.ImageVerticalAlignment;
            Settings.ImageStretch = settings.ImageStretch;
            Settings.ImageTransparency = settings.ImageTransparency;
            Settings.GalleryIntervalBetweenImages = settings.GalleryIntervalBetweenImages;
            Settings.GalleryIntervalForShowImage = settings.GalleryIntervalForShowImage;
            Settings.GalleryAutoPlay = settings.GalleryAutoPlay;
            Settings.GalleryDirectoryName = settings.GalleryDirectoryName;
            Settings.GalleryState = settings.GalleryState;
            Settings.GalleryHorizontalAlignment = settings.GalleryHorizontalAlignment;
            Settings.GalleryVerticalAlignment = settings.GalleryVerticalAlignment;
            Settings.GalleryStretch = settings.GalleryStretch;
            Settings.GalleryTransparency = settings.GalleryTransparency;
            Settings.VideoFileName = settings.VideoFileName;
            Settings.VideoAutoPlay = settings.VideoAutoPlay;
            Settings.VideoRepeat = settings.VideoRepeat;
            Settings.VideoHorizontalAlignment = settings.VideoHorizontalAlignment;
            Settings.VideoVerticalAlignment = settings.VideoVerticalAlignment;
            Settings.VideoStretch = settings.VideoStretch;
            Settings.VideoVolume = settings.VideoVolume;
            Settings.VideoTransparency = settings.VideoTransparency;
            Settings.WebUrl = settings.WebUrl;
            Settings.WebRefreshInterval = settings.WebRefreshInterval == null ? null : (int?)settings.WebRefreshInterval.Value.TotalSeconds;
            Settings.WindowHandle = settings.WindowHandle == IntPtr.Zero ? null : (long?)settings.WindowHandle.ToInt64();
            Settings.WindowExTool = settings.WindowExTool;
            Settings.WindowPreviouseHandle = settings.WindowPreviouseHandle == IntPtr.Zero ? null: (long?)settings.WindowPreviouseHandle.ToInt64();
            Settings.WindowPreviouseExTool = settings.WindowPreviouseExTool;
            Settings.WindowText = settings.WindowText;
            Settings.WindowStatus = settings.WindowStatus;
            Settings.WindowClassName = settings.WindowClassName;
            Settings.WindowProcessName = settings.WindowProcessName;
            Settings.WindowAlignment = settings.WindowAlignment;
            Settings.WindowFullScreen = settings.WindowFullScreen;
            Settings.WindowUseAfterRestart = settings.WindowUseAfterRestart;

            Update();
            CreateViews();
            SaveSettings();
        }

        public void CreateViews()
        {
            var windows = 0;
            _views.Clear();
            _viewModels.Clear();
            foreach (var window in _windows)
            {
                window.AllowClose = true;
                window.Hide();
            }

            WindowUtils.RefreshDesktop();

            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref Native.Rect rect, IntPtr data) =>
            {
                if (Settings.Monitor == null || Settings.Monitor == windows)
                {
                    var info = new MonitorInfo();
                    info.Init();
                    GetMonitorInfo(hMonitor, ref info);

                    var viewModel = Settings.WallpaperType == WallpaperType.SystemInformation ? new SystemInformationViewModel(info, Settings) :
                                    Settings.WallpaperType == WallpaperType.Video ? new VideoViewModel(info, Settings) :
                                    Settings.WallpaperType == WallpaperType.Image ? new ImageViewModel(info, Settings) :
                                    Settings.WallpaperType == WallpaperType.Web ? new WebViewModel(info, Settings) :
                                    (BaseViewModel)new GalleryViewModel(info, Settings);
                    var view = Settings.WallpaperType == WallpaperType.SystemInformation ? new SystemInformationView() :
                                    Settings.WallpaperType == WallpaperType.Video ? new VideoView() :
                                    Settings.WallpaperType == WallpaperType.Image ? new ImageView() :
                                    Settings.WallpaperType == WallpaperType.Web ? new WebView() :
                                    (UserControl)new GalleryView();
                    var mainWindow = new MainWindow (Settings, info.rcMonitor)
                    {
                        DataContext = viewModel,
                    };
                    mainWindow.GridContainer.Children.Add(view);
                    _viewModels.Add(viewModel);
                    _views.Add(view);

                    mainWindow.Show();
                    _windows.Add(mainWindow);

                    if (Settings.WallpaperType == WallpaperType.Video && Settings.VideoAutoPlay)
                    {
                        VideoPlay();
                    }
                }

                windows++;
                return true;
            }, IntPtr.Zero);

            foreach (var window in _windows)
            {
                if (window.AllowClose)
                {
                    window.Close();
                }
            }
            _windows.RemoveAll(x => x.AllowClose == true);
        }

        public void EnableInteractive(bool enable)
        {
            foreach (var window in _windows)
            {
                var handle = new WindowInteropHelper(window).Handle;
                WindowUtils.EnableNoActive(handle, enable);
                //if (enable)
                //{
                //    WindowUtils.ShowOnDesktopInFrontOfIcons(handle);
                //}
                //else
                //{
                //    WindowUtils.ShowAlwaysOnDesktopBehindIcons(handle);
                //}
            }
        }

        public void ShowWindow(bool show)
        {
            foreach (var window in _windows)
            {
                window.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public bool IsWindowVisible
        {
            get
            {
                return _windows.Any() && _windows[0].Visibility == Visibility.Visible;
            }
        }

        public void EnableTray(bool enable)
        {
            _tray.Visibility = enable ? Visibility.Visible : Visibility.Collapsed;
        }

        public void InitTray()
        {
            _tray = Application.Current.FindResource("TrayIcon") as TaskbarIcon;
            _trayViewModel = new TaskbarIconViewModel(this, Settings, DialogService.Instance);
            _tray.DataContext = _trayViewModel;
            TaskbarIcon.SetParentTaskbarIcon(Application.Current.MainWindow, _tray);
        }

        public void Update()
        {
            foreach (var viewModel in _viewModels)
            {
                viewModel.Update();
            }
        }

        public void Refresh()
        {
            foreach (var view in _views)
            {
                (view as SystemInformationView)?.Refresh();
                (view as WebView)?.Refresh();
            }
        }

        public void RefreshTray()
        {
            _trayViewModel?.Refresh();
        }

        public void VideoPlay()
        {
            foreach (var view in _views)
            {
                var mediaView = view as VideoView;
                if (mediaView != null)
                {
                    mediaView.Play();
                    RefreshTray();
                }
            }
        }

        public void VideoPause()
        {
            foreach (var view in _views)
            {
                var mediaView = view as VideoView;
                if (mediaView != null)
                {
                    mediaView.Pause();
                    RefreshTray();
                }
            }
        }

        public void VideoStop()
        {
            foreach (var view in _views)
            {
                var mediaView = view as VideoView;
                if (mediaView != null)
                {
                    mediaView.Stop();
                    RefreshTray();
                }
            }
        }

        public void GalleryPlay()
        {
            foreach (var view in _views)
            {
                var albumView = view as GalleryView;
                if (albumView != null)
                {
                    albumView.Play();
                    RefreshTray();
                }
            }
        }

        public void GalleryPause()
        {
            foreach (var view in _views)
            {
                var albumView = view as GalleryView;
                if (albumView != null)
                {
                    albumView.Pause();
                    RefreshTray();
                }
            }
        }

        public void GalleryStop()
        {
            foreach (var view in _views)
            {
                var albumView = view as GalleryView;
                if (albumView != null)
                {
                    albumView.Stop();
                    RefreshTray();
                }
            }
        }

        public bool LoadSettings()
        {
            var settingsFileName = Path.GetFileNameWithoutExtension(AssemblyUtils.AssemblyLocation) + ".xml";
            settingsFileName = Path.Combine(AssemblyUtils.AssemblyDirectoryName, settingsFileName);
            if (File.Exists(settingsFileName))
            {
                try
                {
                    var xml = File.ReadAllText(settingsFileName, Encoding.UTF8);
                    Settings = SerializeUtils.Deserialize<ProgramSettings>(xml);
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed to load settings from the file {settingsFileName}{Environment.NewLine}{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            else
            {
                Settings.VideoFileExtensions = new List<string> { "*.mp4", "*.mp3", "*.mpg", "*.mpeg", "*.avi" };
                Settings.GalleryFileExtensions = new List<string> { "*.bmp", "*.jpg", "*.jpeg", "*.png", "*.gif", "*.tiff" };
                return true;
            }
        }

        private void SaveSettings()
        {
            var settingsFileName = Path.GetFileNameWithoutExtension(AssemblyUtils.AssemblyLocation) + ".xml";
            settingsFileName = Path.Combine(AssemblyUtils.AssemblyDirectoryName, settingsFileName);
            try
            {
                File.WriteAllText(settingsFileName, SerializeUtils.Serialize(Settings), Encoding.UTF8);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Failed to save settings to the file {settingsFileName}{Environment.NewLine}{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
