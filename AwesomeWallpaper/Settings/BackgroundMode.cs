using System.ComponentModel.DataAnnotations;

namespace AwesomeWallpaper.Settings
{
    public enum BackgroundMode
    {
        [Display(Name = "None")]
        None = 0,

        [Display(Name = "Pixelate")]
        Pixelate = 1,

        [Display(Name = "Dark")]
        Dark = 2,

        [Display(Name = "Black And White")]
        BlackAndWhite = 3,

        [Display(Name = "Grayscale")]
        Grayscale = 4
    }
}
