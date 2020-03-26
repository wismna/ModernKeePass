using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MediatR;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Interfaces;
using ModernKeePassLib;

namespace ModernKeePass.ViewModels
{
    public class GroupVm : NotifyPropertyChangedBase, IVmEntity, ISelectableModel
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
        
        public string Id => _group.Id;
        public bool IsNotRoot => ParentGroup != null;
        
        public bool ShowRestore => IsNotRoot && ParentGroup.IsSelected;

        public bool IsRecycleOnDelete => IsRecycleBinEnabled().GetAwaiter().GetResult() && !IsSelected && !ParentGroup.IsSelected;
        
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get { return IsRecycleBinEnabled().GetAwaiter().GetResult() && _database.RecycleBin?.Id == Id; }
            set
            {
                if (value && _group != null) _database.RecycleBin = this;
            }
        }

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            group e by e.Name.ToUpper().FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Name
        {
            get { return _group == null ? string.Empty : _group.Title; }
            set { _group.Title = value; }
        }

        public int IconId
        {
            get
            {
                if (_group?.Icon != null) return (int) _group?.Icon;
                return -1;
            }
            set { _group.Icon = (Icon)value; }
        }
        
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                SetProperty(ref _isEditMode, value);
                ((RelayCommand)SortEntriesCommand).RaiseCanExecuteChanged();
                ((RelayCommand)SortGroupsCommand).RaiseCanExecuteChanged();
            }
        }

        public bool IsMenuClosed
        {
            get { return _isMenuClosed; }
            set { SetProperty(ref _isMenuClosed, value); }
        }

        public IEnumerable<IVmEntity> BreadCrumb
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

        private readonly Application.Group.Models.GroupVm _group;
        private readonly IMediator _mediator;
        private bool _isEditMode;
        private Application.Entry.Models.EntryVm _reorderedEntry;
        private ObservableCollection<EntryVm> _entries = new ObservableCollection<EntryVm>();
        private bool _isMenuClosed = true;

        public GroupVm() {}
        
        internal GroupVm(Application.Group.Models.GroupVm group, GroupVm parent, string recycleBinId = null) : this(group, parent, App.Mediator, recycleBinId)
        { }

        public GroupVm(Application.Group.Models.GroupVm group, GroupVm parent, IMediator mediator, string recycleBinId = null)
        {
            _group = group;
            _mediator = mediator;
            ParentGroup = parent;

            SaveCommand = new RelayCommand(async () => await _mediator.Send(new SaveDatabaseCommand()));
            SortEntriesCommand = new RelayCommand(async () =>
                await SortEntriesAsync().ConfigureAwait(false), () => IsEditMode);
            SortGroupsCommand = new RelayCommand(async () =>
                await SortGroupsAsync().ConfigureAwait(false), () => IsEditMode);
            UndoDeleteCommand = new RelayCommand(() => Move(PreviousGroup), () => PreviousGroup != null);

            if (recycleBinId != null && _group.Id.Equals(recycleBinId)) _database.RecycleBin = this;
            Entries = new ObservableCollection<EntryVm>(group.Entries.Select(e => new EntryVm(e, this)));
            Entries.CollectionChanged += Entries_CollectionChanged;
            Groups = new ObservableCollection<GroupVm>(group.SubGroups.Select(g => new GroupVm(g, this, recycleBinId)));
        }
        
        private void Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    var oldIndex = (uint) e.OldStartingIndex;
                     _reorderedEntry = _group.Entries.GetAt(oldIndex);
                    _group.Entries.RemoveAt(oldIndex);
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (_reorderedEntry == null) _group.AddEntry(((EntryVm) e.NewItems[0]).GetPwEntry(), true);
                    else _group.Entries.Insert((uint)e.NewStartingIndex, _reorderedEntry);
                    break;
            }
        }
        
        public GroupVm AddNewGroup(string name = "")
        {
            var pwGroup = new PwGroup(true, true, name, PwIcon.Folder);
            _group.AddGroup(pwGroup, true);
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

        public async Task MarkForDelete(string recycleBinTitle)
        {
            var isRecycleBinEnabled = await IsRecycleBinEnabled();
            if (isRecycleBinEnabled && _database.RecycleBin?.IdUuid == null)
                _database.CreateRecycleBin(recycleBinTitle);
            Move(isRecycleBinEnabled && !IsSelected ? _database.RecycleBin : null);
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
            PreviousGroup._group.SubGroups.Remove(_group);
            if (destination == null)
            {
                _database.AddDeletedItem(IdUuid);
                return;
            }
            ParentGroup = destination;
            ParentGroup.Groups.Add(this);
            ParentGroup._group.AddGroup(_group, true);
        }

        public async Task CommitDelete()
        {
            _group.ParentGroup.Groups.Remove(_group);
            if (await IsRecycleBinEnabled() && !PreviousGroup.IsSelected) _database.RecycleBin._group.AddGroup(_group, true);
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
                _group.Entries.Sort(comparer);
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
                _group.SortSubGroups(false);
                Groups = new ObservableCollection<GroupVm>(Groups.OrderBy(g => g.Name).ThenBy(g => g._group == null));
                OnPropertyChanged("Groups");
            }
            catch (Exception e)
            {
                await MessageDialogHelper.ShowErrorDialog(e);
            }
        }

        private async Task<bool> IsRecycleBinEnabled()
        {
            var database = await _mediator.Send(new GetDatabaseQuery());
            return database.IsRecycleBinEnabled;
        }
    }
}
