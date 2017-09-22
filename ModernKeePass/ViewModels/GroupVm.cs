using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ModernKeePassLibPCL;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : INotifyPropertyChanged
    {
        private PwGroup _pwGroup;

        public ObservableCollection<EntryVm> Entries { get; set; }
        public ObservableCollection<GroupVm> Groups { get; set; }
        public string Name { get; set; }

        public string EntryCount {
            get
            {
                return $"{Entries?.Count} entries.";
            }
        }
        public string GroupCount
        {
            get
            {
                return $"{Groups?.Count} groups.";
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
            _pwGroup = group;
            Name = group.Name;
            Entries = new ObservableCollection<EntryVm>(group.Entries.Select(e => new EntryVm(e)));
            Groups = new ObservableCollection<GroupVm>(group.Groups.Select(g => new GroupVm(g)));
        }

        public void AddGroup(string title)
        {
            var pwGroup = new PwGroup
            {
                Name = title
            };
            Groups.Add(new GroupVm(pwGroup));
            NotifyPropertyChanged("Groups");
            this._pwGroup.Groups.Add(pwGroup);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
