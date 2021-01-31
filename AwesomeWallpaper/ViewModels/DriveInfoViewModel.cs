using System.IO;
using Prism.Mvvm;
using static AwesomeWallpaper.Utils.SystemUtils;

namespace AwesomeWallpaper.ViewModels
{
    class DriveInfoViewModel : BindableBase
    {
        public DriveInfo _driveInfo;

        public DriveInfoViewModel(DriveInfo driveInfo)
        {
            _driveInfo = driveInfo;
        }

        public string Name => GetDriveName(_driveInfo);

        public string TotalSize => GetDriveTotalSize(_driveInfo);

        public string FreeSpace => GetDriveFreeSpace(_driveInfo);
    }
}
