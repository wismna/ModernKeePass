using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ModernKeePass.ViewModels
{
    public class RecentVm : INotifyPropertyChanged
    {
        public ObservableCollection<RecentItemVm> RecentItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
