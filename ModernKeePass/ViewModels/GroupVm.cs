using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Mappings;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : NotifyPropertyChangedBase
    {
        public GroupVm ParentGroup { get; }
        public ObservableCollection<EntryVm> Entries { get; set; } = new ObservableCollection<EntryVm>();
        public ObservableCollection<GroupVm> Groups { get; set; } = new ObservableCollection<GroupVm>();

        public int EntryCount => Entries.Count - 1;
        public int GroupCount => Groups.Count - 1;
        public bool IsNotRoot => ParentGroup != null;
        public FontWeight FontWeight => _pwGroup == null ? FontWeights.Bold : FontWeights.Normal;
        
        public string Name
        {
            get { return _pwGroup == null ? "New group" : _pwGroup.Name; }
            set { _pwGroup.Name = value; }
        }
        public Symbol IconSymbol
        {
            get
            {
                if (_pwGroup == null) return Symbol.Add;
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(_pwGroup.IconId);
                return result == Symbol.More ? Symbol.Folder : result;
            }
        }

        public bool IsLeftPaneOpen
        {
            get { return _isLeftPaneOpen; }
            set { SetProperty(ref _isLeftPaneOpen, value); }
        }

        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        private readonly PwGroup _pwGroup;
        private bool _isLeftPaneOpen;
        private bool _isEditMode;

        public GroupVm() {}

        public GroupVm(PwGroup pwGroup, GroupVm parent)
        {
            _pwGroup = pwGroup;
            ParentGroup = parent;
            Entries = new ObservableCollection<EntryVm>(pwGroup.Entries.Select(e => new EntryVm(e, this)));
            Entries.Insert(0, new EntryVm ());
            //Entries.Add(new EntryVm { Title = " New entry" });
            Groups = new ObservableCollection<GroupVm>(pwGroup.Groups.Select(g => new GroupVm(g, this)));
            //Groups.Insert(0, new GroupVm { Name = " + New group" });
            Groups.Insert(0, new GroupVm ());
        }

        public void CreateNewGroup()
        {
            var pwGroup = new PwGroup(true, true, "New group", PwIcon.Folder);
            _pwGroup.AddGroup(pwGroup, true);
            Groups.Add(new GroupVm(pwGroup, this));
        }
        
        public void CreateNewEntry()
        {
            var pwEntry = new PwEntry(true, true);
            _pwGroup.AddEntry(pwEntry, true);
            Entries.Add(new EntryVm(pwEntry, this));
        }

        public void RemoveGroup()
        {
            _pwGroup.ParentGroup.Groups.Remove(_pwGroup);
            ParentGroup.Groups.Remove(this);
        }
        
        public void RemoveEntry(EntryVm entry)
        {
            _pwGroup.Entries.Remove(entry.Entry);
            Entries.Remove(entry);
        }
    }
}
