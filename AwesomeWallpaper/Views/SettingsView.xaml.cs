﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using AwesomeWallpaper.ViewModels;
using AwesomeWallpaper.Utils;
using static AwesomeWallpaper.Native.NativeMethods;

namespace AwesomeWallpaper.Views
{
    public partial class SettingsView : Window
    {
        private bool _isButtonTargetMouseDown;

        public SettingsView()
        {
            InitializeComponent();
            _isButtonTargetMouseDown = false;
        }

        private void BrowseVideoFile_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SettingsViewModel)DataContext;
            var openFileDialog = new OpenFileDialog
            {
                Filter = $"Video files ({string.Join(";", viewModel.Settings.VideoFileExtensions)})|{string.Join(";", viewModel.Settings.VideoFileExtensions)}|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                viewModel.VideoFileName = openFileDialog.FileName;
            }
        }

        private void BrowseDirectoryWithImages_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SettingsViewModel)DataContext;
            var openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                viewModel.GalleryDirectoryName = openFolderDialog.SelectedPath;
            }
        }

        private void BrowseImageFile_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SettingsViewModel)DataContext;
            var openFileDialog = new OpenFileDialog
            {
                Filter = $"Image files ({string.Join(";", viewModel.Settings.GalleryFileExtensions)})|{string.Join(";", viewModel.Settings.GalleryFileExtensions)}|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                viewModel.ImageFileName = openFileDialog.FileName;
            }
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SettingsViewModel)DataContext;
            if (TabControlMain.SelectedIndex == 1 && viewModel.WallpaperType != Settings.WallpaperType.SystemInformation)
            {
                var result = MessageBox.Show("Change wallpaper type to \"SystemInformation\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = Settings.WallpaperType.SystemInformation;
                }
            }

            if (TabControlMain.SelectedIndex == 2 && viewModel.WallpaperType != Settings.WallpaperType.Image)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Image\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = Settings.WallpaperType.Image;
                }
            }

            if (TabControlMain.SelectedIndex == 3 && viewModel.WallpaperType != Settings.WallpaperType.Gallery)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Gallery\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = Settings.WallpaperType.Gallery;
                }
            }

            if (TabControlMain.SelectedIndex == 4 && viewModel.WallpaperType != Settings.WallpaperType.Video)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Video\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = Settings.WallpaperType.Video;
                }
            }

            if (TabControlMain.SelectedIndex == 5 && viewModel.WallpaperType != Settings.WallpaperType.Web)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Web\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = Settings.WallpaperType.Web;
                }
            }
        }

        private void TargetButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isButtonTargetMouseDown)
            {
                _isButtonTargetMouseDown = true;
            }
        }

        private void TargetButton_MouseDownUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_isButtonTargetMouseDown)
            {
                _isButtonTargetMouseDown = false;
                SetCursor(System.Windows.Forms.Cursors.Default.Handle);
            }
        }

        private void TargetButton_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_isButtonTargetMouseDown)
            {
                try
                {
                    SetCursor(Properties.Resources.Target32.Handle);
                    var cursorPosition = System.Windows.Forms.Cursor.Position;
                    var handle = WindowFromPoint(new System.Drawing.Point(cursorPosition.X, cursorPosition.Y));
                    var viewModel = (SettingsViewModel)DataContext;
                    handle = WindowUtils.GetParentWindow(handle);
                    var thisWindowHandle = new WindowInteropHelper(this).Handle;
                    if (thisWindowHandle == handle)
                    {
                        viewModel.WindowHandleText = "";
                        viewModel.WindowText = "";
                        WindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/1x1.png"));
                    }
                    else
                    {
                        var windowHandleText = string.Format("0x{0:X}", handle.ToInt64());
                        viewModel.WindowHandleText = windowHandleText;
                        var windowTitle = WindowUtils.GetWmGetText(handle);
                        viewModel.WindowText = windowTitle;
                        var windowImage = WindowUtils.PrintWindow(handle);
                        WindowImage.Source = ImageUtils.ConvertImageToBitmapSource(windowImage);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
