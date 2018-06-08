using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Mappings;
using ModernKeePass.Services;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : NotifyPropertyChangedBase, IPwEntity, ISelectableModel
    {
        public GroupVm ParentGroup { get; private set; }
        public GroupVm PreviousGroup { get; private set; }

        public ObservableCollection<EntryVm> Entries
        {
            get { return _entries; }
            private set { SetProperty(ref _entries, value); }
        }

        public IEnumerable<EntryVm> SubEntries
        {
            get
            {
                var subEntries = new List<EntryVm>();
                subEntries.AddRange(Entries);
                foreach (var group in Groups)
                {
                    subEntries.AddRange(group.SubEntries);
                }

                return subEntries;
            }
        }

        public ObservableCollection<GroupVm> Groups { get; set; } = new ObservableCollection<GroupVm>();
        
        public PwUuid IdUuid => _pwGroup?.Uuid;
        public string Id => IdUuid?.ToHexString();
        public bool IsNotRoot => ParentGroup != null;
        
        public bool ShowRestore => IsNotRoot && ParentGroup.IsSelected;

        public bool IsRecycleOnDelete => _database.RecycleBinEnabled && !IsSelected && !ParentGroup.IsSelected;
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get { return _database != null && _database.RecycleBinEnabled && _database.RecycleBin?.Id == Id; }
            set
            {
                if (value && _pwGroup != null) _database.RecycleBin = this;
            }
        }

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            group e by e.Name.ToUpper().FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Name
        {
            get { return _pwGroup == null ? string.Empty : _pwGroup.Name; }
            set { _pwGroup.Name = value; }
        }

        public Symbol IconSymbol
        {
            get
            {
                var result = PwIconToSegoeMapping.GetSymbolFromIcon(_pwGroup.IconId);
                return result == Symbol.More ? Symbol.Folder : result;
            }
            set { _pwGroup.IconId = PwIconToSegoeMapping.GetIconFromSymbol(value); }
        }
        
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set { SetProperty(ref _isEditMode, value); }
        }

        public bool IsMenuClosed
        {
            get { return _isMenuClosed; }
            set { SetProperty(ref _isMenuClosed, value); }
        }
        
        public string Path
        {
            get
            {
                if (ParentGroup == null) return string.Empty;
                var path = new StringBuilder(ParentGroup.Path);
                path.Append($" > {ParentGroup.Name}");
                return path.ToString();
            }
        }

        private readonly PwGroup _pwGroup;
        private readonly IDatabaseService _database;
        private bool _isEditMode;
        private PwEntry _reorderedEntry;
        private ObservableCollection<EntryVm> _entries = new ObservableCollection<EntryVm>();
        private bool _isMenuClosed = true;

        public GroupVm() {}
        
        internal GroupVm(PwGroup pwGroup, GroupVm parent, PwUuid recycleBinId = null) : this(pwGroup, parent,
            DatabaseService.Instance, recycleBinId)
        { }

        public GroupVm(PwGroup pwGroup, GroupVm parent, IDatabaseService database, PwUuid recycleBinId = null)
        {
            _pwGroup = pwGroup;
            _database = database;
            ParentGroup = parent;

            if (recycleBinId != null && _pwGroup.Uuid.Equals(recycleBinId)) _database.RecycleBin = this;
            Entries = new ObservableCollection<EntryVm>(pwGroup.Entries.Select(e => new EntryVm(e, this)));
            Entries.CollectionChanged += Entries_CollectionChanged;
            Groups = new ObservableCollection<GroupVm>(pwGroup.Groups.Select(g => new GroupVm(g, this, recycleBinId)));
        }
        
        private void Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    var oldIndex = (uint) e.OldStartingIndex;
                     _reorderedEntry = _pwGroup.Entries.GetAt(oldIndex);
                    _pwGroup.Entries.RemoveAt(oldIndex);
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (_reorderedEntry == null) _pwGroup.AddEntry(((EntryVm) e.NewItems[0]).GetPwEntry(), true);
                    else _pwGroup.Entries.Insert((uint)e.NewStartingIndex, _reorderedEntry);
                    break;
            }
        }
        
        public GroupVm AddNewGroup(string name = "")
        {
            var pwGroup = new PwGroup(true, true, name, PwIcon.Folder);
            _pwGroup.AddGroup(pwGroup, true);
            var newGroup = new GroupVm(pwGroup, this) {Name = name, IsEditMode = string.IsNullOrEmpty(name)};
            Groups.Add(newGroup);
            return newGroup;
        }
        
        public EntryVm AddNewEntry()
        {
            var pwEntry = new PwEntry(true, true);
            var newEntry = new EntryVm(pwEntry, this) {IsEditMode = true};
            newEntry.GeneratePassword();
            Entries.Add(newEntry);
            return newEntry;
        }

        public void MarkForDelete(string recycleBinTitle)
        {
            if (_database.RecycleBinEnabled && _database.RecycleBin?.IdUuid == null)
                _database.CreateRecycleBin(recycleBinTitle);
            Move(_database.RecycleBinEnabled && !IsSelected ? _database.RecycleBin : null);
        }

        public void UndoDelete()
        {
            Move(PreviousGroup);
        }
        
        public void Move(GroupVm destination)
        {
            PreviousGroup = ParentGroup;
            PreviousGroup.Groups.Remove(this);
            PreviousGroup._pwGroup.Groups.Remove(_pwGroup);
            if (destination == null)
            {
                _database.AddDeletedItem(IdUuid);
                return;
            }
            ParentGroup = destination;
            ParentGroup.Groups.Add(this);
            ParentGroup._pwGroup.AddGroup(_pwGroup, true);
        }

        public void CommitDelete()
        {
            _pwGroup.ParentGroup.Groups.Remove(_pwGroup);
            if (_database.RecycleBinEnabled && !PreviousGroup.IsSelected) _database.RecycleBin._pwGroup.AddGroup(_pwGroup, true);
            else _database.AddDeletedItem(IdUuid);
        }

        public void Save()
        {
            _database.Save();
        }
        
        public void SortEntries()
        {
            var comparer = new PwEntryComparer(PwDefs.TitleField, true, false);
            try
            {
                _pwGroup.Entries.Sort(comparer);
                Entries = new ObservableCollection<EntryVm>(Entries.OrderBy(e => e.Name));
            }
            catch (Exception e)
            {
                MessageDialogHelper.ShowErrorDialog(e);
            }
        }
        
        public void SortGroups()
        {
            try
            {
                _pwGroup.SortSubGroups(false);
                Groups = new ObservableCollection<GroupVm>(Groups.OrderBy(g => g.Name).ThenBy(g => g._pwGroup == null));
                OnPropertyChanged("Groups");
            }
            catch (Exception e)
            {
                MessageDialogHelper.ShowErrorDialog(e);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
