using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using AwesomeWallpaper.Extensions;
using AwesomeWallpaper.Settings;
using AwesomeWallpaper.ViewModels;

namespace AwesomeWallpaper.Views
{
    public partial class GalleryView : UserControl
    {
        private DispatcherTimer _timerForShow;
        private DispatcherTimer _timerForHide;
        private IList<string> _files = new List<string>();
        private int _currentFileIndex = -1;

        public GalleryState State { get; set; } = GalleryState.Stop;

        private string CurrentFile
        {
            get
            {
                _currentFileIndex = _currentFileIndex >= _files.Count - 1 ? 0 : _currentFileIndex + 1;
                return _files[_currentFileIndex];
            }
        }

        public GalleryView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_timerForShow == null && _timerForHide == null)
            {
                var viewModel = (BaseViewModel)DataContext;
                if (Directory.Exists(viewModel.Settings.GalleryDirectoryName))
                {
                    var directoryInfo = new DirectoryInfo(viewModel.Settings.GalleryDirectoryName);
                    _files = directoryInfo.GetFilesByExtensions(viewModel.Settings.GalleryFileExtensions.Select(x => x.Replace("*", "")).ToArray()).Select(x => x.FullName).ToList();
                }

                _timerForShow = new DispatcherTimer();
                _timerForShow.Interval = TimeSpan.FromSeconds(viewModel.Settings.GalleryIntervalBetweenImages);
                _timerForShow.Tick += TimerForShowTick;
                _timerForHide = new DispatcherTimer();
                _timerForHide.Interval = TimeSpan.FromSeconds(viewModel.Settings.GalleryIntervalForShowImage);
                _timerForHide.Tick += TimerForHideTick;
                if (_files.Any() && viewModel.Settings.GalleryAutoPlay)
                {
                    _timerForShow.Start();
                    State = GalleryState.Play;
                    viewModel.Settings.GalleryState = State;
                }
            }
        }

        private void TimerForShowTick(object sender, EventArgs e)
        {
            _timerForShow.Stop();
            Image.ChangeSource(new BitmapImage(new Uri(CurrentFile)), TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
            _timerForHide.Start();
        }

        private void TimerForHideTick(object sender, EventArgs e)
        {
            _timerForHide.Stop();
            Image.ChangeSource(new BitmapImage(new Uri("pack://application:,,,/Images/1x1.png")), TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(500));
            _timerForShow.Start();
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = (BaseViewModel)DataContext;
            var browseDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (browseDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _currentFileIndex = -1;
                if (Directory.Exists(browseDialog.SelectedPath))
                {
                    var directoryInfo = new DirectoryInfo(browseDialog.SelectedPath);
                    _files = directoryInfo.GetFilesByExtensions(viewModel.Settings.GalleryFileExtensions.Select(x => x.Replace("*", "")).ToArray()).Select(x => x.FullName).ToList();
                }
            }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = State != GalleryState.Play && _files.Any();
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Play();
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = State == GalleryState.Play;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = State != GalleryState.Stop;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Stop();
        }

        public void Play()
        {
            _timerForShow.Start();
            State = GalleryState.Play;
            ((BaseViewModel)DataContext).Settings.GalleryState = State;
        }

        public void Pause()
        {
            _timerForShow.Stop();
            _timerForHide.Stop();
            State = GalleryState.Pause;
            ((BaseViewModel)DataContext).Settings.GalleryState = State;
        }

        public void Stop()
        {
            _timerForShow.Stop();
            _timerForHide.Stop();

            State = GalleryState.Stop;
            var viewModel = (BaseViewModel)DataContext;
            viewModel.Settings.GalleryState = State;
            _currentFileIndex = -1;
            if (Directory.Exists(viewModel.Settings.GalleryDirectoryName))
            {
                var directoryInfo = new DirectoryInfo(viewModel.Settings.GalleryDirectoryName);
                _files = directoryInfo.GetFilesByExtensions(viewModel.Settings.GalleryFileExtensions.Select(x => x.Replace("*", "")).ToArray()).Select(x => x.FullName).ToList();
            }
        }
    }
}
