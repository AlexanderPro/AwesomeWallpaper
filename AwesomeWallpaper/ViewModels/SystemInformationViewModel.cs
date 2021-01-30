using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using AwesomeWallpaper.Settings;
using AwesomeWallpaper.Native;
using static AwesomeWallpaper.Utils.SystemUtils;

namespace AwesomeWallpaper.ViewModels
{
    class SystemInformationViewModel : BaseViewModel
    {
        public SystemInformationViewModel(MonitorInfo monitor, ProgramSettings settings) : base(monitor, settings)
        {
        }

        public string BootTime => GetBootTime();

        public string OSVersion => GetOSVersion();

        public string ComputerName => GetComputerName();

        public string DomainName => GetDomainName();

        public string UserName => GetUserName();

        public string Resolution => GetResolution(_monitor);

        public string Memory => GetTotalMemory();

        public string AvailableMemory => GetAvailableMemory();

        public uint Processes => GetNumberProcesses();

        public uint Threads => GetNumberThreads();

        public uint Handles => GetNumberHandles();

        public string Commit => GetCommits();

        public int ProcessorCount => GetNumberProcessors();

        public string Processor => GetProcessorName();

        public IEnumerable<string> Network => GetNetworks();

        public IEnumerable<DriveInfoViewModel> Drives => DriveInfo.GetDrives().Where(x => x.IsReady).Select(drive => new DriveInfoViewModel(drive));

        public string UpdateTime => DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

        public override void Refresh()
        {
            RaisePropertyChanged(nameof(BootTime));
            RaisePropertyChanged(nameof(OSVersion));
            RaisePropertyChanged(nameof(ComputerName));
            RaisePropertyChanged(nameof(DomainName));
            RaisePropertyChanged(nameof(UserName));
            RaisePropertyChanged(nameof(Resolution));
            RaisePropertyChanged(nameof(Memory));
            RaisePropertyChanged(nameof(AvailableMemory));
            RaisePropertyChanged(nameof(Processes));
            RaisePropertyChanged(nameof(Threads));
            RaisePropertyChanged(nameof(Handles));
            RaisePropertyChanged(nameof(Commit));
            RaisePropertyChanged(nameof(ProcessorCount));
            RaisePropertyChanged(nameof(Processor));
            RaisePropertyChanged(nameof(Network));
            RaisePropertyChanged(nameof(Drives));
            RaisePropertyChanged(nameof(UpdateTime));
            base.Refresh();
        }
    }
}
