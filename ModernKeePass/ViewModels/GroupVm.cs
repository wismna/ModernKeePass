using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
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
        public ObservableCollection<Application.Entry.Models.EntryVm> Entries => new ObservableCollection<Application.Entry.Models.EntryVm>(_group.Entries);

        public ObservableCollection<Application.Group.Models.GroupVm> Groups => new ObservableCollection<Application.Group.Models.GroupVm>(_group.SubGroups);

        public IEnumerable<Application.Entry.Models.EntryVm> SubEntries
        {
            get
            {
                var subEntries = new List<Application.Entry.Models.EntryVm>();
                subEntries.AddRange(Entries);
                foreach (var group in Groups)
                {
                    subEntries.AddRange(group.Entries);
                }

                return subEntries;
            }
        }

        
        public bool IsNotRoot => _database.RootGroup != _group;
        
        public bool ShowRestore => IsNotRoot && _database.RecycleBin != _group;
        
        /// <summary>
        /// Is the Group the database Recycle Bin?
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _database.IsRecycleBinEnabled && _database.RecycleBin == _group;
            }
            set
            {
                if (value && _group != null) _database.RecycleBin = _group;
            }
        }

        public IOrderedEnumerable<IGrouping<char, Application.Entry.Models.EntryVm>> EntriesZoomedOut => from e in Entries
            group e by e.Title.ToUpper().FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Id => _group.Id;

        public string Title
        {
            get { return _group.Title; }
            set { _group.Title = value; }
        }

        public Symbol Icon
        {
            get { return (Symbol) _group.Icon; }
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

        public IEnumerable<Application.Group.Models.GroupVm> BreadCrumb => _group.Breadcrumb;

        public ICommand SaveCommand { get; }
        public ICommand SortEntriesCommand { get; }
        public ICommand SortGroupsCommand { get; }
        public ICommand UndoDeleteCommand { get; }

        private readonly Application.Group.Models.GroupVm _group;
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;
        private bool _isEditMode;
        private Application.Entry.Models.EntryVm _reorderedEntry;
        private bool _isMenuClosed = true;

        public GroupVm() {}
        
        internal GroupVm(Application.Group.Models.GroupVm group) : this(group, App.Mediator)
        { }

        public GroupVm(Application.Group.Models.GroupVm group, IMediator mediator, bool isEditMode = false)
        {
            _group = group;
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            _isEditMode = isEditMode;

            SaveCommand = new RelayCommand(async () => await _mediator.Send(new SaveDatabaseCommand()));
            SortEntriesCommand = new RelayCommand(async () =>
                await SortEntriesAsync().ConfigureAwait(false), () => IsEditMode);
            SortGroupsCommand = new RelayCommand(async () =>
                await SortGroupsAsync().ConfigureAwait(false), () => IsEditMode);
            UndoDeleteCommand = new RelayCommand(async () => await Move(group.ParentGroup), () => _group.ParentGroup != null);
            
            Entries.CollectionChanged += Entries_CollectionChanged;
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
        
        public async Task<Application.Group.Models.GroupVm> AddNewGroup(string name = "")
        {
            return await _mediator.Send(new CreateGroupCommand {Name = name, ParentGroup = _group});
        }
        
        public async Task<Application.Entry.Models.EntryVm> AddNewEntry()
        {
            return await _mediator.Send(new CreateEntryCommand { ParentGroup = _group });
        }

        public async Task MarkForDelete(string recycleBinTitle)
        {
            if (_database.IsRecycleBinEnabled && _database.RecycleBin == null)
                await _mediator.Send(new CreateGroupCommand {ParentGroup = _database.RootGroup, IsRecycleBin = true, Name = recycleBinTitle});
            await Move(_database.IsRecycleBinEnabled && !IsSelected ? _database.RecycleBin : null);
            ((RelayCommand)UndoDeleteCommand).RaiseCanExecuteChanged();
        }
        
        public async Task Move(Application.Group.Models.GroupVm destination)
        {
            await _mediator.Send(new RemoveGroupCommand {ParentGroup = _group.ParentGroup, Group = _group});
            if (destination == null)
            {
                await _mediator.Send(new DeleteGroupCommand { Group = _group });
                return;
            }
            await _mediator.Send(new AddGroupCommand {ParentGroup = destination, Group = _group});
        }

        public async Task CommitDelete()
        {
            await _mediator.Send(new DeleteGroupCommand { Group = _group });
        }

        private async Task SortEntriesAsync()
        {
            await _mediator.Send(new SortEntriesCommand {Group = _group});
            OnPropertyChanged(nameof(Entries));
        }
        
        private async Task SortGroupsAsync()
        {
            await _mediator.Send(new SortGroupsCommand {Group = _group});
            OnPropertyChanged(nameof(Groups));
        }
    }
}
