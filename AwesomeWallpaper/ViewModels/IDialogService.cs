using System.Windows;

namespace AwesomeWallpaper.ViewModels
{
    public interface IDialogService
    {
        TViewModel CreateDialog<TViewModel, TDialog>(params object[] args) where TDialog : Window, new() where TViewModel : DialogViewModelBase;
        TViewModel CreateDialog<TViewModel, TDialog>(TViewModel vm) where TDialog : Window, new() where TViewModel : DialogViewModelBase;
    }
}
