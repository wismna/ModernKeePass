using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePassLib;
using System;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : NotifyPropertyChangedBase, IPwEntity
    {
        public GroupVm ParentGroup { get; }
        public ObservableCollection<EntryVm> Entries { get; set; } = new ObservableCollection<EntryVm>();
        
        public ObservableCollection<GroupVm> Groups { get; set; } = new ObservableCollection<GroupVm>();

        public int EntryCount => Entries.Count() - 1;
        public int GroupCount => Groups.Count - 1;
        public bool IsNotRoot => ParentGroup != null;
        public FontWeight FontWeight => _pwGroup == null ? FontWeights.Bold : FontWeights.Normal;
        public string Id => _pwGroup.Uuid.ToHexString();

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut
        {
            get
            {
                return from e in Entries
                       where e.Entry != null
                       group e by e.Name.FirstOrDefault() into grp
                       orderby grp.Key
                       select grp;
            }
        }

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
            Entries = new ObservableCollection<EntryVm>(pwGroup.Entries.Select(e => new EntryVm(e, this)).OrderBy(e => e.Name));
            Entries.Insert(0, new EntryVm ());
            Groups = new ObservableCollection<GroupVm>(pwGroup.Groups.Select(g => new GroupVm(g, this)).OrderBy(g => g.Name));
            Groups.Insert(0, new GroupVm ());
        }

        public GroupVm CreateNewGroup()
        {
            var pwGroup = new PwGroup(true, true, string.Empty, PwIcon.Folder);
            _pwGroup.AddGroup(pwGroup, true);
            var newGroup = new GroupVm(pwGroup, this) {IsEditMode = true};
            Groups.Add(newGroup);
            return newGroup;
        }
        
        public EntryVm CreateNewEntry()
        {
            var pwEntry = new PwEntry(true, true);
            _pwGroup.AddEntry(pwEntry, true);
            var newEntry = new EntryVm(pwEntry, this) {IsEditMode = true};
            Entries.Add(newEntry);
            return newEntry;
        }
        
        public void MarkForDelete()
        {
            var app = (App)Application.Current;
            app.PendingDeleteEntities.Add(Id, this);
            ParentGroup.Groups.Remove(this);
        }

        public void CommitDelete()
        {
            _pwGroup.ParentGroup.Groups.Remove(_pwGroup);
        }
        public void UndoDelete()
        {
            ParentGroup.Groups.Add(this);
        }
    }
}
