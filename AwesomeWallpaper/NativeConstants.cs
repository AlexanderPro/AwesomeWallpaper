namespace AwesomeWallpaper
{
    static class NativeConstants
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_NOACTIVATE = 0x8000000;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int HWND_BOTTOM = 1;
        public const int SWP_NOMOVE = 2;
        public const int SWP_NOSIZE = 1;
        public const int SWP_NOACTIVATE = 0x10;
        public const int SWP_NOZORDER = 4;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public const int WM_DPICHANGED = 0x02E0;
        public const uint SPI_SETDESKWALLPAPER = 20;
        public const uint SPIF_UPDATEINIFILE = 0x1;
        public const uint SPI_SETCLIENTAREAANIMATION = 0x1043;
        public const int SRCCOPY = 0x00CC0020;
    }
}
