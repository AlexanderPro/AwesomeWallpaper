using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;

namespace AwesomeWallpaper.ViewModels
{
    [Export("default", typeof(IDialogService))]
    sealed class DialogService : IDialogService
    {
        private DialogService() { }

        public TViewModel CreateDialog<TViewModel, TDialog>(params object[] args)
            where TDialog : Window, new()
            where TViewModel : DialogViewModelBase
        {
            var dlg = new TDialog();
            var vm = (TViewModel)Activator.CreateInstance(typeof(TViewModel), new object[] { dlg }.Concat(args).ToArray());
            dlg.DataContext = vm;
            if (dlg.Content == null)
                dlg.Content = vm;

            return vm;
        }

        public TViewModel CreateDialog<TViewModel, TDialog>(TViewModel vm)
            where TViewModel : DialogViewModelBase
            where TDialog : Window, new()
        {
            var dlg = new TDialog();
            vm.Dialog = dlg;
            dlg.DataContext = vm;
            if (dlg.Content == null)
                dlg.Content = vm;

            return vm;
        }

        public static readonly IDialogService Instance = new DialogService();
    }
}
