using System.Windows;
using AwesomeWallpaper.Utils;

namespace AwesomeWallpaper.ViewModels
{
    class AboutViewModel : DialogViewModelBase
    {
        public AboutViewModel(Window dialog) : base(dialog)
        {
        }

        public string Title
        {
            get
            {
                return $"About {AssemblyUtils.AssemblyProductName}";
            }
        }

        public string ProductName
        {
            get
            {
                return $"{AssemblyUtils.AssemblyProductName} v{AssemblyUtils.AssemblyVersion}";
            }
        }

        public string Copyright
        {
            get
            {
                return $"{AssemblyUtils.AssemblyCopyright} {AssemblyUtils.AssemblyCompany}";
            }
        }
    }
}
