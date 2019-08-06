using System;
using System.Windows;
using Prism.Mvvm;
using AwesomeWallpaper.Settings;

namespace AwesomeWallpaper.ViewModels
{
    class BaseViewModel : BindableBase
    {
        protected MonitorInfo _monitor;
        public ProgramSettings Settings { get; }

        public BaseViewModel(MonitorInfo monitor, ProgramSettings settings)
        {
            _monitor = monitor;
            Settings = settings;
        }

        public bool IsHitTestVisible => Settings.InteractiveMode;

        public virtual void Update()
        {
            RaisePropertyChanged(nameof(IsHitTestVisible));
        }

        public virtual void Refresh()
        {

        }
    }
}
