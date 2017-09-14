using System.Collections.ObjectModel;
using System.Linq;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm
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

        public GroupVm() { }

        public GroupVm(PwGroup group)
        {
            Name = group.Name;
            Entries = new ObservableCollection<EntryVm>(group.Entries.Select(e => new EntryVm(e)));
            Groups = new ObservableCollection<GroupVm>(group.Groups.Select(g => new GroupVm(g)));
        }
    }
}
