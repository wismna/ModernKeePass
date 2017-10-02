using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ModernKeePass.ViewModels
{
    public class MainVm : INotifyPropertyChanged
    {
        public IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> MainMenuItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
