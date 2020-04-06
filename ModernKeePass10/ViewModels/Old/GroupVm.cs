using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
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
            get => _entries;
            private set => SetProperty(ref _entries, value);
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

        public bool IsRecycleOnDelete => _database.IsRecycleBinEnabled && !IsSelected && !ParentGroup.IsSelected;
        
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get => _database != null && _database.IsRecycleBinEnabled && _database.RecycleBin?.Id == Id;
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
            get => _pwGroup == null ? string.Empty : _pwGroup.Name;
            set => _pwGroup.Name = value;
        }

        public int IconId
        {
            get
            {
                if (_pwGroup?.IconId != null) return (int) _pwGroup?.IconId;
                return -1;
            }
            set => _pwGroup.IconId = (PwIcon)value;
        }
        
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                SetProperty(ref _isEditMode, value);
                ((RelayCommand)SortEntriesCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SortGroupsCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsMenuClosed
        {
            get => _isMenuClosed;
            set => SetProperty(ref _isMenuClosed, value);
        }

        public IEnumerable<IPwEntity> BreadCrumb
        {
            get
            {
                var groups = new Stack<GroupVm>();
                var group = this;
                while (group.ParentGroup != null)
                {
                    group = group.ParentGroup;
                    groups.Push(group);
                }
                
                return groups;
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand SortEntriesCommand { get; }
        public ICommand SortGroupsCommand { get; }
        public ICommand UndoDeleteCommand { get; }

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

            SaveCommand = new RelayCommand(() => _database.Save());
            SortEntriesCommand = new RelayCommand(async () =>
                await SortEntriesAsync().ConfigureAwait(false), () => IsEditMode);
            SortGroupsCommand = new RelayCommand(async () =>
                await SortGroupsAsync().ConfigureAwait(false), () => IsEditMode);
            UndoDeleteCommand = new RelayCommand(() => Move(PreviousGroup), () => PreviousGroup != null);

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
            if (_database.IsRecycleBinEnabled && _database.RecycleBin?.IdUuid == null)
                _database.CreateRecycleBin(recycleBinTitle);
            Move(_database.IsRecycleBinEnabled && !IsSelected ? _database.RecycleBin : null);
            ((RelayCommand)UndoDeleteCommand).RaiseCanExecuteChanged();
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
            if (_database.IsRecycleBinEnabled && !PreviousGroup.IsSelected) _database.RecycleBin._pwGroup.AddGroup(_pwGroup, true);
            else _database.AddDeletedItem(IdUuid);
        }
        
        public override string ToString()
        {
            return Name;
        }

        private async Task SortEntriesAsync()
        {
            var comparer = new PwEntryComparer(PwDefs.TitleField, true, false);
            try
            {
                _pwGroup.Entries.Sort(comparer);
                Entries = new ObservableCollection<EntryVm>(Entries.OrderBy(e => e.Name));
            }
            catch (Exception e)
            {
                await MessageDialogHelper.ShowErrorDialog(e);
            }
        }
        
        private async Task SortGroupsAsync()
        {
            try
            {
                _pwGroup.SortSubGroups(false);
                Groups = new ObservableCollection<GroupVm>(Groups.OrderBy(g => g.Name).ThenBy(g => g._pwGroup == null));
                OnPropertyChanged("Groups");
            }
            catch (Exception e)
            {
                await MessageDialogHelper.ShowErrorDialog(e);
            }
        }

    }
}
