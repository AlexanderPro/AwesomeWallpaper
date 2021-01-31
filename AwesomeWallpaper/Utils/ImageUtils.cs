using System;
using System.Windows;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Media.Imaging;
using static AwesomeWallpaper.Native.NativeMethods;

namespace AwesomeWallpaper.Utils
{
    static class ImageUtils
    {
        public static BitmapSource ConvertImageToBitmapSource(Image image)
        {
            using (var bitmap = new Bitmap(image))
            {
                var hBitmap = bitmap.GetHbitmap();
                var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                bitmapSource.Freeze();
                DeleteObject(hBitmap);
                return bitmapSource;
            }
        }
    }
}