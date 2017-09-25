using System.ComponentModel;
using Windows.UI.Xaml;

namespace ModernKeePass.ViewModels
{
    public class HomeVm : INotifyPropertyChanged
    {
        public string Password { get; set; }
        public Visibility Visibility { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsOpen { get; set; }

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
