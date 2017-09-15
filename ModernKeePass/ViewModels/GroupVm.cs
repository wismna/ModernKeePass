using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : INotifyPropertyChanged
    {
        public ObservableCollection<EntryVm> Entries { get; set; }
        public ObservableCollection<GroupVm> Groups { get; set; }
        public string Name { get; set; }

        public string EntryCount {
            get
            {
                return $"{Entries?.Count} entries.";
            }
        }

        public GroupVm()
        {
            Name = "GroupName";
            Entries = new ObservableCollection<EntryVm>();
            Groups = new ObservableCollection<GroupVm>();

        }

        public GroupVm(PwGroup group)
        {
            Name = group.Name;
            Entries = new ObservableCollection<EntryVm>(group.Entries.Select(e => new EntryVm(e)));
            Groups = new ObservableCollection<GroupVm>(group.Groups.Select(g => new GroupVm(g)));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
