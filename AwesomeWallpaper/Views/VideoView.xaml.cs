using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Reflection;
using Microsoft.Win32;
using AwesomeWallpaper.ViewModels;

namespace AwesomeWallpaper.Views
{
    public partial class VideoView : UserControl
    {
        private bool _userIsDraggingSlider = false;
        private DispatcherTimer _timer;

        public VideoView()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        public void Play()
        {
            if (!string.IsNullOrEmpty(mediaPlayer.Source?.OriginalString))
            {
                mediaPlayer.Play();
                ((BaseViewModel)DataContext).Settings.VideoState = GetMediaState(mediaPlayer);
            }
        }

        public void Pause()
        {
            mediaPlayer.Pause();
            ((BaseViewModel)DataContext).Settings.VideoState = GetMediaState(mediaPlayer);
        }

        public void Stop()
        {
            mediaPlayer.Stop();
            ((BaseViewModel)DataContext).Settings.VideoState = GetMediaState(mediaPlayer);
        }

        public MediaState MediaState => GetMediaState(mediaPlayer);

        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((mediaPlayer.Source != null) && (mediaPlayer.NaturalDuration.HasTimeSpan) && (!_userIsDraggingSlider))
            {
                sliProgress.Minimum = 0;
                sliProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                sliProgress.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var viewModel = (VideoViewModel)DataContext;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = $"Media files ({string.Join(";", viewModel.Settings.VideoFileExtensions)})|{string.Join(";", viewModel.Settings.VideoFileExtensions)}|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaPlayer.Source = new Uri(openFileDialog.FileName);
                if (viewModel.Settings.VideoAutoPlay)
                {
                    Play();
                }
            }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mediaPlayer != null) && !string.IsNullOrEmpty(mediaPlayer.Source?.OriginalString) && MediaState != MediaState.Play;
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Play();
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mediaPlayer != null) && !string.IsNullOrEmpty(mediaPlayer.Source?.OriginalString) && MediaState == MediaState.Play;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mediaPlayer != null) && !string.IsNullOrEmpty(mediaPlayer.Source?.OriginalString) && (MediaState == MediaState.Play || MediaState == MediaState.Pause);
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Stop();
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            _userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            _userIsDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromSeconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            mediaPlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            var viewModel = (VideoViewModel)DataContext;
            if (viewModel.Settings.VideoRepeat)
            {
                mediaPlayer.Position = new TimeSpan(0, 0, 1);
                Play();
            }
            else
            {
                Stop();
            }
        }

        private MediaState GetMediaState(MediaElement media)
        {
            var fieldInfo = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldValue = fieldInfo.GetValue(media);
            var stateFieldInfo = fieldValue.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            var state = (MediaState)stateFieldInfo.GetValue(fieldValue);
            return state;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            _timer = null;
            mediaPlayer?.Stop();
            mediaPlayer?.Close();
            mediaPlayer = null;
        }
    }
}
