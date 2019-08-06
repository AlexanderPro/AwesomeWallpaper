using System;
using System.Runtime.InteropServices;

namespace AwesomeWallpaper
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PerformanceInformation
    {
        public int cb;
        public uint CommitTotal;
        public uint CommitLimit;
        public uint CommitPeak;
        public uint PhysicalTotal;
        public uint PhysicalAvailable;
        public uint SystemCache;
        public uint KernelTotal;
        public uint KernelPaged;
        public uint KernelNonpaged;
        public uint PageSize;
        public uint HandleCount;
        public uint ProcessCount;
        public uint ThreadCount;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct OSVersionInfoEx
    {
        public int dwOSVersionInfoSize;
        public uint dwMajorVersion;
        public uint dwMinorVersion;
        public uint dwBuildNumber;
        public uint dwPlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string szCSDVersion;
        public ushort wServicePackMajor;
        public ushort wServicePackMinor;
        public ushort wSuiteMask;
        public byte wProductType;
        public byte wReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPos
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left, Top, Right, Bottom;

        public int Width => Right - Left;
        public int Height => Bottom - Top;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorInfo
    {
        public uint cbSize;
        public Rect rcMonitor;
        public Rect rcWork;
        public uint dwFlags;

        public void Init()
        {
            cbSize = (uint)Marshal.SizeOf(this);
        }
    }
}
