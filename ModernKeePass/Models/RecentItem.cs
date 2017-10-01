using System.ComponentModel;
using Windows.UI.Xaml;

namespace ModernKeePass.Models
{
    public class RecentItem: INotifyPropertyChanged
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public Visibility PasswordVisibility { get; set; } = Visibility.Collapsed;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
