using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ModernKeePassLib;

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

        public GroupVm(PwGroup pwGroup)
        {
            _pwGroup = pwGroup;
            Name = pwGroup.Name;
            Entries = new ObservableCollection<EntryVm>(pwGroup.Entries.Select(e => new EntryVm(e)));
            //Entries.Insert(0, new EntryVm { Title = " + New entry" });
            Groups = new ObservableCollection<GroupVm>(pwGroup.Groups.Select(g => new GroupVm(g)));
            //Groups.Insert(0, new GroupVm { Name = " + New group" });
        }

        public void CreateNewGroup(string title)
        {
            var pwGroup = new PwGroup(true, true, title, PwIcon.Folder);
            _pwGroup.AddGroup(pwGroup, true);
            Groups.Add(new GroupVm(pwGroup));
            NotifyPropertyChanged("Groups");
        }
        
        public void CreateNewEntry(string title)
        {
            var pwEntry = new PwEntry(true, true);
            _pwGroup.AddEntry(pwEntry, true);
            Entries.Add(new EntryVm(pwEntry));
            NotifyPropertyChanged("Entries");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
