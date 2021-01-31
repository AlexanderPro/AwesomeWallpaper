using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace AwesomeWallpaper.ViewModels
{
    public abstract class DialogViewModelBase : BindableBase
    {
        private bool? _result;
        internal Window Dialog;
        private Func<bool> _canExecuteOKCommand = () => true;
        private DelegateCommand _okCommand;
        private bool _keepOpened;

        protected DialogViewModelBase(Window dialog)
        {
            Dialog = dialog;
            _keepOpened = false;
            _okCommand = new DelegateCommand(() => OnOK(), _canExecuteOKCommand);
        }

        protected DialogViewModelBase(bool? result) : this((Window)null)
        {
            _result = result;
        }

        protected virtual void OnOK()
        {
            if (!_keepOpened)
            {
                Close(true);
            }
        }

        protected virtual void OnCancel()
        {
            Close(false);
        }

        protected virtual void OnClose(bool? result)
        {
        }

        protected void Close(bool? result = true)
        {
            OnClose(result);
            if (Dialog != null)
            {
                Dialog.DialogResult = result;
                Dialog.Close();
            }
        }

        public bool? ShowDialog()
        {
            return Dialog != null ? Dialog.ShowDialog() : _result;
        }

        public void Show()
        {
            Dialog?.Show();
        }

        public Func<bool> CanExecuteOKCommand
        {
            get
            {
                return _canExecuteOKCommand;
            }
            set
            {
                if (SetProperty(ref _canExecuteOKCommand, value))
                {
                    OKCommand = new DelegateCommand(() => OnOK(), value);
                    OKCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Func<bool> CanExecuteCancelCommand { get; set; } = () => true;

        public DelegateCommand CancelCommand => new DelegateCommand(() => OnCancel(), CanExecuteCancelCommand);

        public DelegateCommand OKCommand
        {
            get
            {
                return _okCommand;
            }
            set
            {
                SetProperty(ref _okCommand, value);
            }
        }

        public bool KeepOpened
        {
            get
            {
                return _keepOpened;
            }
            set
            {
                SetProperty(ref _keepOpened, value);
            }
        }
    }
}
