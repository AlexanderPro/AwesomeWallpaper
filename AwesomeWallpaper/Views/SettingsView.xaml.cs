using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using AwesomeWallpaper.Settings;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (SettingsViewModel)DataContext;
            viewModel.WindowPreviouseHandle = viewModel.WindowHandle;
            viewModel.WindowPreviouseExTool = viewModel.WindowExTool;
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
            viewModel.KeepOpened = false;

            if (TabControlMain.SelectedIndex == 1 && viewModel.WallpaperType != WallpaperType.SystemInformation)
            {
                var result = MessageBox.Show("Change wallpaper type to \"SystemInformation\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = WallpaperType.SystemInformation;
                }
            }

            if (TabControlMain.SelectedIndex == 2 && viewModel.WallpaperType != WallpaperType.Image)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Image\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = WallpaperType.Image;
                }
            }

            if (TabControlMain.SelectedIndex == 3 && viewModel.WallpaperType != WallpaperType.Gallery)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Gallery\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = WallpaperType.Gallery;
                }
            }

            if (TabControlMain.SelectedIndex == 4 && viewModel.WallpaperType != WallpaperType.Video)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Video\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = WallpaperType.Video;
                }
            }

            if (TabControlMain.SelectedIndex == 5 && viewModel.WallpaperType != WallpaperType.Web)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Web\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = WallpaperType.Web;
                }
            }

            if (TabControlMain.SelectedIndex == 6 || (TabControlMain.SelectedIndex == 0 && viewModel.WallpaperType == WallpaperType.Window))
            {
                if (viewModel.Monitor == null && viewModel.Monitors.Count() > 2)
                {
                    MessageBox.Show("You should select only one monitor on the \"General\" tab.", "Attention");
                    viewModel.KeepOpened = true;
                    return;
                }

                if (viewModel.WindowHandle == null || viewModel.WindowHandle == IntPtr.Zero || !IsWindow(viewModel.WindowHandle))
                {
                    MessageBox.Show("The window is not selected or is already closed. Please select a new window again.", "Attention");
                    viewModel.KeepOpened = true;
                    return;
                }
            }

            if (TabControlMain.SelectedIndex == 6 && viewModel.WallpaperType != WallpaperType.Window)
            {
                var result = MessageBox.Show("Change wallpaper type to \"Window\"?", "Attention", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    viewModel.WallpaperType = WallpaperType.Window;
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
                var viewModel = (SettingsViewModel)DataContext;
                viewModel.WindowDelimiterRowHeight = 12;
                viewModel.WindowRowHeight = 25;
                viewModel.WindowImageRowHeight = 0;
                WindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/1x1.png"));
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
                    var parentHandle = WindowUtils.GetParentWindow(handle);
                    var className = WindowUtils.GetClassName(handle);
                    var parentClassName = WindowUtils.GetClassName(parentHandle);
                    var thisWindowHandle = new WindowInteropHelper(this).Handle;
                    var viewModel = (SettingsViewModel)DataContext;
                    var classNames = new List<string>
                    {
                        "Shell_TrayWnd",
                        "TrayNotifyWnd",
                        "SysPager",
                        "ToolbarWindow32",
                        "ReBarWindow32",
                        "MSTaskSwWClass",
                        "MSTaskListWClass",
                        "Progman",
                        "WorkerW",
                        "SHELLDLL_DefView",
                        "SysListView32",
                        "Start",
                        "Shell_SecondaryTrayWnd"
                    };
                    if (thisWindowHandle == parentHandle || classNames.Contains(className) || classNames.Contains(parentClassName))
                    {
                        viewModel.WindowHandle = IntPtr.Zero;
                        viewModel.WindowExTool = false;
                        viewModel.WindowStatus = "Not Selected";
                        viewModel.WindowText = "";
                        viewModel.WindowDelimiterRowHeight = 12;
                        viewModel.WindowRowHeight = 25;
                        viewModel.WindowImageRowHeight = 0;
                        WindowImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/1x1.png"));
                    }
                    else
                    {
                        viewModel.WindowHandle = parentHandle;
                        viewModel.WindowExTool = WindowUtils.IsExToolWindow(parentHandle);
                        viewModel.WindowClassName = WindowUtils.GetClassName(parentHandle);
                        viewModel.WindowStatus = "Selected";
                        viewModel.WindowText = WindowUtils.GetWmGetText(parentHandle);
                        viewModel.WindowProcessName = WindowUtils.GetProcessName(parentHandle);
                        WindowImage.Source = ImageUtils.ConvertImageToBitmapSource(WindowUtils.PrintWindow(parentHandle));
                        viewModel.WindowDelimiterRowHeight = 0;
                        viewModel.WindowRowHeight = 0;
                        viewModel.WindowImageRowHeight = 155;
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
