using System;
using System.Windows;
using System.Windows.Interop;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using AwesomeWallpaper.Drawing;
using static AwesomeWallpaper.Native.NativeMethods;

namespace AwesomeWallpaper.Extensions
{
    public static class BitmapExtensions
    {
        public static BitmapSource ConvertToBitmapSource(this Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            bitmapSource.Freeze();
            DeleteObject(hBitmap);
            return bitmapSource;
        }

        public static Bitmap Dark(this Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                var brush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));
                graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
            }

            return newBitmap;
        }

        public static Bitmap BlackAndWhite(this Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }

            for (var x = 0; x < newBitmap.Width; x++)
            {
                for (var y = 0; y < newBitmap.Height; y++)
                {
                    var pixel = newBitmap.GetPixel(x, y);
                    var avg = (pixel.R + pixel.G + pixel.B) / 3;
                    newBitmap.SetPixel(x, y, Color.FromArgb(avg, avg, avg));
                }
            }

            return newBitmap;
        }

        public static Bitmap Grayscale(this Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            using (var graphics = Graphics.FromImage(newBitmap))
            using (var attributes = new ImageAttributes())
            {
                var colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                      new float[] {.3f, .3f, .3f, 0, 0},
                      new float[] {.59f, .59f, .59f, 0, 0},
                      new float[] {.11f, .11f, .11f, 0, 0},
                      new float[] {0, 0, 0, 1, 0},
                      new float[] {0, 0, 0, 0, 1}
                   });

                attributes.SetColorMatrix(colorMatrix);
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);

                return newBitmap;
            }
        }

        public static Bitmap Pixelate(this Bitmap bitmap, int pixelateSize)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            // make an exact copy of the bitmap provided
            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }

            // look at every pixel in the rectangle while making sure we're within the bitmap bounds
            for (var xx = 0; xx < newBitmap.Width; xx += pixelateSize)
            {
                for (var yy = 0; yy < newBitmap.Height; yy += pixelateSize)
                {
                    var offsetX = pixelateSize / 2;
                    var offsetY = pixelateSize / 2;

                    // make sure that the offset is within the boundry of the bitmap
                    while (xx + offsetX >= newBitmap.Width) offsetX--;
                    while (yy + offsetY >= newBitmap.Height) offsetY--;

                    // get the pixel color in the center of the soon to be newBitmap area
                    var pixel = newBitmap.GetPixel(xx + offsetX, yy + offsetY);

                    // for each pixel in the pixelate size, set it to the center color
                    for (var x = xx; x < xx + pixelateSize && x < bitmap.Width; x++)
                    {
                        for (var y = yy; y < yy + pixelateSize && y < bitmap.Height; y++)
                        {
                            newBitmap.SetPixel(x, y, pixel);
                        }
                    }
                }
            }

            return newBitmap;
        }

        public static Bitmap Blur(this Bitmap bitmap, int radial)
        {
            var blur = new GaussianBlur(bitmap);
            return blur.Process(radial);
        }

        public static Bitmap Copy(this Bitmap bitmap)
        {
            var newBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            // make an exact copy of the bitmap provided
            using (var graphics = Graphics.FromImage(newBitmap))
            {
                graphics.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }

            return newBitmap;
        }
    }
}