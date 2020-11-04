using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Prism.Commands;
using Prism.Mvvm;
using AwesomeWallpaper.Views;
using AwesomeWallpaper.Settings;

namespace AwesomeWallpaper.ViewModels
{
    sealed class TaskbarIconViewModel : BindableBase
    {
        private readonly ViewManager _manager;
        private readonly IDialogService _dialogService;
        private readonly ProgramSettings _settings;

        public TaskbarIconViewModel(ViewManager manager, ProgramSettings settings, IDialogService dialogService)
        {
            _manager = manager;
            _dialogService = dialogService;
            _settings = settings;

            RefreshCommand = new DelegateCommand(() => _manager.Refresh());
            ExitCommand = new DelegateCommand(() => Application.Current.Shutdown());

            SettingsCommand = new DelegateCommand(() =>
            {
                var dialog = _dialogService.CreateDialog<SettingsViewModel, SettingsView>(_manager.Settings);
                dialog.SelectedTabIndex = _manager.Settings.WallpaperType == WallpaperType.SystemInformation ? 1 : _manager.Settings.WallpaperType == WallpaperType.Image ? 2 : 
                _manager.Settings.WallpaperType == WallpaperType.Gallery ? 3 : _manager.Settings.WallpaperType == WallpaperType.Video ? 4 :
                _manager.Settings.WallpaperType == WallpaperType.Web ? 5 : 0;
                if (dialog.ShowDialog() == true)
                {
                    manager.ApplySettings(dialog);
                }
            });

            PlayCommand = new DelegateCommand(() => {
                if (_settings.WallpaperType == WallpaperType.Video)
                {
                    _manager.VideoPlay();
                }
                if (_settings.WallpaperType == WallpaperType.Gallery)
                {
                    _manager.GalleryPlay();
                }
            });

            PauseCommand = new DelegateCommand(() => {
                if (_settings.WallpaperType == WallpaperType.Video)
                {
                    _manager.VideoPause();
                }
                if (_settings.WallpaperType == WallpaperType.Gallery)
                {
                    _manager.GalleryPause();
                }
            });

            StopCommand = new DelegateCommand(() => {
                if (_settings.WallpaperType == WallpaperType.Video)
                {
                    _manager.VideoStop();
                }
                if (_settings.WallpaperType == WallpaperType.Gallery)
                {
                    _manager.GalleryStop();
                }
            });

            AboutCommand = new DelegateCommand(() =>
            {
                var dialog = _dialogService.CreateDialog<AboutViewModel, AboutView>();
                dialog.ShowDialog();
            });

            ShowHideCommand = new DelegateCommand(() =>
            {
                manager.ShowWindow(!manager.IsWindowVisible);
                RaisePropertyChanged(nameof(MenuItemShowHideText));
            });

            AutoStartCommand = new DelegateCommand(() =>
            {
                if (StartUpManager.IsInStartup())
                {
                    StartUpManager.RemoveFromStartup();
                }
                else
                {
                    StartUpManager.AddToStartup();
                }
                RaisePropertyChanged(nameof(AutoStart));
            });

            EnableInteractiveModeCommand = new DelegateCommand(() =>
            {
                _settings.InteractiveMode = !_settings.InteractiveMode;
                manager.EnableInteractive(_settings.InteractiveMode);
                manager.Update();
                RaisePropertyChanged(nameof(MenuItemInteractiveModeText));
            });
        }

        public Visibility MenuItemPlayVisibility => _settings.WallpaperType == WallpaperType.Video || _settings.WallpaperType == WallpaperType.Gallery ? Visibility.Visible : Visibility.Collapsed;
        public Visibility MenuItemPauseVisibility => _settings.WallpaperType == WallpaperType.Video || _settings.WallpaperType == WallpaperType.Gallery ? Visibility.Visible : Visibility.Collapsed;
        public Visibility MenuItemStopVisibility => _settings.WallpaperType == WallpaperType.Video || _settings.WallpaperType == WallpaperType.Gallery ? Visibility.Visible : Visibility.Collapsed;
        public Visibility MenuItemSeparatorVisibility => _settings.WallpaperType == WallpaperType.Video || _settings.WallpaperType == WallpaperType.Gallery ? Visibility.Visible : Visibility.Collapsed;
        public Visibility MenuItemRefreshVisibility => _settings.WallpaperType == WallpaperType.SystemInformation || _settings.WallpaperType == WallpaperType.Web ? Visibility.Visible : Visibility.Collapsed;
        //public Visibility MenuItemInteractiveModeVisibility => _settings.WallpaperType == WallpaperType.Video || _settings.WallpaperType == WallpaperType.Gallery ? Visibility.Visible : Visibility.Collapsed;
        public Visibility MenuItemInteractiveModeVisibility => Visibility.Collapsed;
        public bool MenuItemPlayEnabled => (_settings.WallpaperType == WallpaperType.Video && _settings.VideoState != MediaState.Play) || (_settings.WallpaperType == WallpaperType.Gallery && _settings.GalleryState != GalleryState.Play);
        public bool MenuItemPauseEnabled => (_settings.WallpaperType == WallpaperType.Video && _settings.VideoState == MediaState.Play) || (_settings.WallpaperType == WallpaperType.Gallery && _settings.GalleryState == GalleryState.Play);
        public bool MenuItemStopEnabled => (_settings.WallpaperType == WallpaperType.Video && (_settings.VideoState == MediaState.Play || _settings.VideoState == MediaState.Pause)) || (_settings.WallpaperType == WallpaperType.Gallery && (_settings.GalleryState == GalleryState.Play || _settings.GalleryState == GalleryState.Pause));
        public string MenuItemInteractiveModeText => _settings.InteractiveMode ? "Disable Interactive Mode" : "Enable Interactive Mode";
        public string MenuItemShowHideText => _manager.IsWindowVisible ? "Hide" : "Show";
        public bool AutoStart => StartUpManager.IsInStartup();

        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand EnableInteractiveModeCommand { get; }
        public ICommand ShowHideCommand { get; }
        public ICommand AutoStartCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(MenuItemPlayVisibility));
            RaisePropertyChanged(nameof(MenuItemPauseVisibility));
            RaisePropertyChanged(nameof(MenuItemStopVisibility));
            RaisePropertyChanged(nameof(MenuItemSeparatorVisibility));
            RaisePropertyChanged(nameof(MenuItemRefreshVisibility));
            RaisePropertyChanged(nameof(MenuItemInteractiveModeVisibility));
            RaisePropertyChanged(nameof(MenuItemPlayEnabled));
            RaisePropertyChanged(nameof(MenuItemPauseEnabled));
            RaisePropertyChanged(nameof(MenuItemStopEnabled));
        }
    }
}
