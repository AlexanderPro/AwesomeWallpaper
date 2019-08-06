using Microsoft.Win32;
using AwesomeWallpaper.Utils;

namespace AwesomeWallpaper
{
    static class StartUpManager
    {
        public static void AddToStartup()
        {
            using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.SetValue(AssemblyUtils.AssemblyProductName, AssemblyUtils.AssemblyLocation);
            }
        }

        public static void RemoveFromStartup()
        {
            using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                key.DeleteValue(AssemblyUtils.AssemblyProductName, false);
            }
        }

        public static bool IsInStartup()
        {
            using (var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                var value = key.GetValue(AssemblyUtils.AssemblyProductName, "");
                return value != null && !string.IsNullOrWhiteSpace(value.ToString());
            }
        }
    }
}
