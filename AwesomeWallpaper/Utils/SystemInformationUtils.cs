using System;
using System.Runtime.InteropServices;
using System.Management;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.IO;
using static AwesomeWallpaper.NativeMethods;

namespace AwesomeWallpaper.Utils
{
    static class SystemInformationUtils
    {
        public static string GetBootTime()
        {
            return (DateTime.Now - TimeSpan.FromMilliseconds(Environment.TickCount)).ToString("dd.MM.yyyy HH:mm");
        }

        public static string GetOSVersion()
        {
            return Environment.OSVersion.ToString();
        }

        public static string GetComputerName()
        {
            return Environment.MachineName;
        }

        public static string GetDomainName()
        {
            return Environment.UserDomainName;
        }

        public static string GetUserName()
        {
            return Environment.UserName;
        }

        public static string GetTotalMemory()
        {
            GetPerformanceInfo(out var info, Marshal.SizeOf<PerformanceInformation>());
            return $"{info.PhysicalTotal >> 8} MB";
        }

        public static string GetAvailableMemory()
        {
            GetPerformanceInfo(out var info, Marshal.SizeOf<PerformanceInformation>());
            return $"{info.PhysicalAvailable >> 8} MB";
        }

        public static string GetCommits()
        {
            GetPerformanceInfo(out var info, Marshal.SizeOf<PerformanceInformation>());
            return $"{info.CommitTotal >> 8} MB / {info.CommitLimit >> 8} MB";
        }

        public static int GetNumberProcessors()
        {
            return Environment.ProcessorCount;
        }

        public static uint GetNumberProcesses()
        {
            GetPerformanceInfo(out var info, Marshal.SizeOf<PerformanceInformation>());
            return info.ProcessCount;
        }

        public static uint GetNumberThreads()
        {
            GetPerformanceInfo(out var info, Marshal.SizeOf<PerformanceInformation>());
            return info.ThreadCount;
        }

        public static uint GetNumberHandles()
        {
            GetPerformanceInfo(out var info, Marshal.SizeOf<PerformanceInformation>());
            return info.HandleCount;
        }

        public static string GetProcessorName()
        {
            var mgt = new ManagementClass("Win32_Processor");
            var processors = mgt.GetInstances();
            if (processors.Count == 0)
            {
                return "Unknown";
            }
            return processors.Cast<ManagementObject>().First().Properties["Name"].Value.ToString();
        }

        public static string GetResolution(MonitorInfo monitor)
        {
            return $"{monitor.rcMonitor.Width} X {monitor.rcMonitor.Height}";
        }

        public static IEnumerable<string> GetNetworks()
        {
            var networks = new List<string>(4);
            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                var address = nic.GetPhysicalAddress().ToString();
                if (!string.IsNullOrEmpty(address) && address.Length == 12)
                {
                    networks.Add($"{nic.Description} {nic.Speed / 1000000} Mb/s {ToMacAddress(address)}");
                }
            }
            return networks.Distinct();
        }

        private static string ToMacAddress(string address)
        {
            var mac = new StringBuilder(32);
            for (int i = 0; i < address.Length; i += 2)
            {
                mac.Append(address.Substring(i, 2));
                if (i < address.Length - 2)
                {
                    mac.Append("-");
                }
            }
            return mac.ToString();
        }

        public static string GetDriveName(DriveInfo driveInfo)
        {
            return driveInfo.Name;
        }

        public static string GetDriveTotalSize(DriveInfo driveInfo)
        {
            return GetSize(driveInfo.TotalSize);
        }

        public static string GetDriveFreeSpace(DriveInfo driveInfo)
        {
            return GetSize(driveInfo.TotalFreeSpace);
        }

        private static string GetSize(long size)
        {
            return (size > 1 << 30) ? $"{size >> 30} GB" : $"{size >> 20} MB";
        }
    }
}
