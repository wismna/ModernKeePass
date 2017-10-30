using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : NotifyPropertyChangedBase, IPwEntity, ISelectableModel
    {
        public GroupVm ParentGroup { get; }
        public ObservableCollection<EntryVm> Entries { get; set; } = new ObservableCollection<EntryVm>();
        
        public ObservableCollection<GroupVm> Groups { get; set; } = new ObservableCollection<GroupVm>();

        public int EntryCount => Entries.Count() - 1;
        public FontWeight FontWeight => _pwGroup == null ? FontWeights.Bold : FontWeights.Normal;
        public int GroupCount => Groups.Count - 1;
        public PwUuid IdUuid => _pwGroup?.Uuid;
        public string Id => IdUuid?.ToHexString();
        public bool IsNotRoot => ParentGroup != null;
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get { return _app.Database.RecycleBinEnabled && _app.Database.RecycleBin.Id == Id; }
            set
            {
                // TODO: if _pwGroup is null, create a new group
                if (value && _pwGroup != null) _app.Database.RecycleBin = this;
            }
        }

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            where !e.IsFirstItem
            group e by e.Name.FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Name
        {
            get { return _pwGroup == null ? "< New group >" : _pwGroup.Name; }
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
        
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        private readonly PwGroup _pwGroup;
        private readonly App _app = (App)Application.Current;
        private bool _isEditMode;

        public GroupVm() {}

        public GroupVm(PwGroup pwGroup, GroupVm parent, PwUuid recycleBinId = null)
        {
            _pwGroup = pwGroup;
            ParentGroup = parent;

            if (recycleBinId != null && _pwGroup.Uuid.Equals(recycleBinId)) _app.Database.RecycleBin = this;
            Entries = new ObservableCollection<EntryVm>(pwGroup.Entries.Select(e => new EntryVm(e, this)).OrderBy(e => e.Name));
            Entries.Insert(0, new EntryVm ());
            Groups = new ObservableCollection<GroupVm>(pwGroup.Groups.Select(g => new GroupVm(g, this, recycleBinId)).OrderBy(g => g.Name));
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
            _app.PendingDeleteEntities.Add(Id, this);
            ParentGroup.Groups.Remove(this);
            if (_app.Database.RecycleBinEnabled && !IsSelected) _app.Database.RecycleBin.Groups.Add(this);
        }

        public void CommitDelete()
        {
            _pwGroup.ParentGroup.Groups.Remove(_pwGroup);
            if (_app.Database.RecycleBinEnabled && !IsSelected) _app.Database.RecycleBin._pwGroup.AddGroup(_pwGroup, true);
        }

        public void UndoDelete()
        {
            ParentGroup.Groups.Add(this);
            if (_app.Database.RecycleBinEnabled && !IsSelected) _app.Database.RecycleBin.Groups.Remove(this);
        }

        public void Save()
        {
            _app.Database.Save();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
