using Windows.UI.Xaml;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels
{
    public class DatabaseVm : NotifyPropertyChangedBase
    {
        private string _name;
        private Visibility _selectedVisibility = Visibility.Collapsed;
        private bool _isOpen;

        public Visibility SelectedVisibility
        {
            get { return _selectedVisibility; }
            set { SetProperty(ref _selectedVisibility, value); }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set { SetProperty(ref _isOpen, value); }
        }

        public string Name {
            get { return string.IsNullOrEmpty(_name) ? string.Empty : $"Database {_name} selected"; }
            set { SetProperty(ref _name, value); }
        }
    }
}
