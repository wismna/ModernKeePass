using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace ModernKeePass.ViewModels
{
    public class DatabaseVm : INotifyPropertyChanged
    {
        private string _name;
        public Visibility SelectedVisibility { get; set; } = Visibility.Collapsed;

        public bool IsOpen { get; set; }
        public string Name {
            get { return string.IsNullOrEmpty(_name) ? string.Empty : $"Database {_name} selected"; }
            set { _name = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
