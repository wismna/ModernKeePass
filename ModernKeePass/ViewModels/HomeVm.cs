using System.ComponentModel;
using Windows.Storage;

using ModernKeePassLib;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Interfaces;
using Windows.UI.Xaml;
using System;

namespace ModernKeePass.ViewModels
{
    public class HomeVm : INotifyPropertyChanged
    {
        public string Password { get; set; }
        public Visibility Visibility { get; set; }
        public string ErrorMessage { get; set; }

        public HomeVm()
        {
            Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
