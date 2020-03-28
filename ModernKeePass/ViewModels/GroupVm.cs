using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MediatR;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Group.Commands.AddEntry;
using ModernKeePass.Application.Group.Commands.AddGroup;
using ModernKeePass.Application.Group.Commands.CreateEntry;
using ModernKeePass.Application.Group.Commands.CreateGroup;
using ModernKeePass.Application.Group.Commands.DeleteGroup;
using ModernKeePass.Application.Group.Commands.InsertEntry;
using ModernKeePass.Application.Group.Commands.RemoveEntry;
using ModernKeePass.Application.Group.Commands.RemoveGroup;
using ModernKeePass.Application.Group.Commands.SortEntries;
using ModernKeePass.Application.Group.Commands.SortGroups;
using ModernKeePass.Common;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Interfaces;

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

        public bool IsRecycleOnDelete => GetDatabase().IsRecycleBinEnabled && !IsSelected && !ParentGroup.IsSelected;
        
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get
            {
                var database = GetDatabase();
                return database.IsRecycleBinEnabled && database.RecycleBinId == Id;
            }
            set
            {
                if (value && _group != null) _database.RecycleBin = this;
            }
        }

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            group e by e.Title.ToUpper().FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Title
        {
            get { return _group == null ? string.Empty : _group.Title; }
            set { _group.Title = value; }
        }

        public int Icon
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
            UndoDeleteCommand = new RelayCommand(async () => await Move(PreviousGroup), () => PreviousGroup != null);

            if (recycleBinId != null && _group.Id.Equals(recycleBinId)) _database.RecycleBin = this;
            Entries = new ObservableCollection<EntryVm>(group.Entries.Select(e => new EntryVm(e, this)));
            Entries.CollectionChanged += Entries_CollectionChanged;
            Groups = new ObservableCollection<GroupVm>(group.SubGroups.Select(g => new GroupVm(g, this, recycleBinId)));
        }
        
        private async void Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    var oldIndex = e.OldStartingIndex;
                     _reorderedEntry = _group.Entries[oldIndex];
                    await _mediator.Send(new RemoveEntryCommand {Entry = _reorderedEntry, ParentGroup = _group});
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (_reorderedEntry == null)
                    {
                        var entry = ((EntryVm) e.NewItems[0]).GetEntry();
                        await _mediator.Send(new AddEntryCommand {Entry = entry, ParentGroup = _group});
                    }
                    else
                    {
                        await _mediator.Send(new InsertEntryCommand {Entry = _reorderedEntry, ParentGroup = _group, Index = e.NewStartingIndex});
                    }
                    break;
            }
        }
        
        public async Task<GroupVm> AddNewGroup(string name = "")
        {
            var newGroup = await _mediator.Send(new CreateGroupCommand {Name = name, ParentGroup = _group});
            var newGroupVm = new GroupVm(newGroup, this) {Title = name, IsEditMode = string.IsNullOrEmpty(name)};
            Groups.Add(newGroupVm);
            return newGroupVm;
        }
        
        public async Task<EntryVm> AddNewEntry()
        {
            var newEntry = await _mediator.Send(new CreateEntryCommand { ParentGroup = _group });
            var newEntryVm = new EntryVm(newEntry, this) {IsEditMode = true};
            await newEntryVm.GeneratePassword();
            Entries.Add(newEntryVm);
            return newEntryVm;
        }

        public async Task MarkForDelete(string recycleBinTitle)
        {
            var database = GetDatabase();
            if (database.IsRecycleBinEnabled && database.RecycleBinId == null)
                await _mediator.Send(new CreateGroupCommand {ParentGroup = database.RootGroup, IsRecycleBin = true, Name = recycleBinTitle});
            await Move(database.IsRecycleBinEnabled && !IsSelected ? _database.RecycleBin : null);
            ((RelayCommand)UndoDeleteCommand).RaiseCanExecuteChanged();
        }

        public async Task UndoDelete()
        {
            await Move(PreviousGroup);
        }
        
        public async Task Move(GroupVm destination)
        {
            PreviousGroup = ParentGroup;
            PreviousGroup.Groups.Remove(this);
            await _mediator.Send(new RemoveGroupCommand {ParentGroup = PreviousGroup._group, Group = _group});
            if (destination == null)
            {
                await _mediator.Send(new DeleteGroupCommand { Group = _group });
                return;
            }
            ParentGroup = destination;
            ParentGroup.Groups.Add(this);
            await _mediator.Send(new AddGroupCommand {ParentGroup = ParentGroup._group, Group = _group});
        }

        public async Task CommitDelete()
        {
            await _mediator.Send(new DeleteGroupCommand { Group = _group });
        }
        
        public override string ToString()
        {
            return Title;
        }

        private async Task SortEntriesAsync()
        {
            await _mediator.Send(new SortEntriesCommand {Group = _group});
            Entries = new ObservableCollection<EntryVm>(Entries.OrderBy(e => e.Title));
        }
        
        private async Task SortGroupsAsync()
        {
            await _mediator.Send(new SortGroupsCommand {Group = _group});
            Groups = new ObservableCollection<GroupVm>(Groups.OrderBy(g => g.Title).ThenBy(g => g._group == null));
            // TODO: should not be needed
            OnPropertyChanged(nameof(Groups));
        }

        private DatabaseVm GetDatabase()
        {
            return _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
        }
    }
}
