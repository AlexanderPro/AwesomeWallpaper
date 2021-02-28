using System.ComponentModel.DataAnnotations;

namespace AwesomeWallpaper.Settings
{
    public enum BackgroundMode
    {
        [Display(Name = "None")]
        None = 0,

        [Display(Name = "Blur")]
        Blur = 1,

        [Display(Name = "Pixelate")]
        Pixelate = 2,

        [Display(Name = "Dark")]
        Dark = 3,

        [Display(Name = "Black And White")]
        BlackAndWhite = 4,

        [Display(Name = "Grayscale")]
        Grayscale = 5
    }
}
