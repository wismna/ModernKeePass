using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Commands.AddEntry;
using ModernKeePass.Application.Group.Commands.AddGroup;
using ModernKeePass.Application.Group.Commands.CreateEntry;
using ModernKeePass.Application.Group.Commands.CreateGroup;
using ModernKeePass.Application.Group.Commands.DeleteGroup;
using ModernKeePass.Application.Group.Commands.MoveEntry;
using ModernKeePass.Application.Group.Commands.RemoveGroup;
using ModernKeePass.Application.Group.Commands.SortEntries;
using ModernKeePass.Application.Group.Commands.SortGroups;
using ModernKeePass.Application.Group.Commands.UpdateGroup;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Application.Group.Queries.SearchEntries;
using ModernKeePass.Common;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class GroupDetailVm : NotifyPropertyChangedBase, IVmEntity
    {
        public ObservableCollection<EntryVm> Entries { get; }

        public ObservableCollection<GroupVm> Groups { get; }
        
        public bool IsNotRoot => Database.RootGroupId != _group.Id;

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            group e by e.Title.ToUpper().FirstOrDefault() into grp
            orderby grp.Key
            select grp;

        public string Id => _group.Id;

        public string Title
        {
            get { return _group.Title; }
            set
            {
                _mediator.Send(new UpdateGroupCommand {Group = _group, Title = value, Icon = _group.Icon}).Wait();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public Symbol Icon
        {
            get { return (Symbol) Enum.Parse(typeof(Symbol), _group.Icon.ToString()); }
            set
            {
                _mediator.Send(new UpdateGroupCommand { Group = _group, Title = _group.Title, Icon = (Icon)Enum.Parse(typeof(Icon), value.ToString()) }).Wait();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                SetProperty(ref _isEditMode, value);
                SortEntriesCommand.RaiseCanExecuteChanged();
                SortGroupsCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsRecycleOnDelete
        {
            get
            {
                var database = Database;
                return database.IsRecycleBinEnabled && _parent != null && _parent.Id != database.RecycleBinId;
            }
        }
        
        public IEnumerable<GroupVm> BreadCrumb { get; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand SortEntriesCommand { get; }
        public RelayCommand SortGroupsCommand { get; }
        public RelayCommand MoveCommand { get; }

        private DatabaseVm Database => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();

        private readonly IMediator _mediator;
        private readonly GroupVm _group;
        private readonly GroupVm _parent;
        private bool _isEditMode;
        private EntryVm _reorderedEntry;

        public GroupDetailVm() {}
        
        internal GroupDetailVm(string groupId) : this(groupId, App.Services.GetRequiredService<IMediator>())
        { }

        public GroupDetailVm(string groupId, IMediator mediator)
        {
            _mediator = mediator;
            _group = _mediator.Send(new GetGroupQuery { Id = groupId }).GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(_group.ParentGroupId))
            {
                _parent = _mediator.Send(new GetGroupQuery { Id = _group.ParentGroupId }).GetAwaiter().GetResult();
                BreadCrumb = new List<GroupVm> {_parent};
            }

            SaveCommand = new RelayCommand(async () => await SaveChanges(), () => Database.IsDirty);
            SortEntriesCommand = new RelayCommand(async () => await SortEntriesAsync(), () => IsEditMode);
            SortGroupsCommand = new RelayCommand(async () => await SortGroupsAsync(), () => IsEditMode);
            MoveCommand = new RelayCommand(async () => await Move(_parent), () => IsNotRoot);
            
            Entries = new ObservableCollection<EntryVm>(_group.Entries);
            Entries.CollectionChanged += Entries_CollectionChanged;
            Groups = new ObservableCollection<GroupVm>(_group.SubGroups);
        }
        
        public async Task<string> AddNewGroup(string name = "")
        {
            return (await _mediator.Send(new CreateGroupCommand {Name = name, ParentGroup = _group})).Id;
        }
        
        public async Task<string> AddNewEntry()
        {
            return (await _mediator.Send(new CreateEntryCommand { ParentGroup = _group })).Id;
        }

        public async Task MarkForDelete(string recycleBinTitle)
        {
            await _mediator.Send(new DeleteGroupCommand { GroupId = _group.Id, ParentGroupId = _group.ParentGroupId, RecycleBinName = recycleBinTitle });
        }
        
        public async Task Move(GroupVm destination)
        {
            await _mediator.Send(new AddGroupCommand {ParentGroup = destination, Group = _group });
            await _mediator.Send(new RemoveGroupCommand {ParentGroup = _parent, Group = _group });
        }

        public async Task<IEnumerable<EntryVm>> Search(string queryText)
        {
            return await _mediator.Send(new SearchEntriesQuery {GroupId = Id, SearchText = queryText});
        }

        private async Task SaveChanges()
        {
            await _mediator.Send(new SaveDatabaseCommand());
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async void Entries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    var oldIndex = e.OldStartingIndex;
                     _reorderedEntry = _group.Entries[oldIndex];
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (_reorderedEntry == null)
                    {
                        var entry = (EntryVm) e.NewItems[0];
                        await _mediator.Send(new AddEntryCommand {Entry = entry, ParentGroup = _group});
                    }
                    else
                    {
                        await _mediator.Send(new MoveEntryCommand {Entry = _reorderedEntry, ParentGroup = _group, Index = e.NewStartingIndex});
                    }
                    break;
            }
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async Task SortEntriesAsync()
        {
            await _mediator.Send(new SortEntriesCommand {Group = _group});
            OnPropertyChanged(nameof(Entries));
            SaveCommand.RaiseCanExecuteChanged();
        }
        
        private async Task SortGroupsAsync()
        {
            await _mediator.Send(new SortGroupsCommand {Group = _group});
            OnPropertyChanged(nameof(Groups));
            SaveCommand.RaiseCanExecuteChanged();
        }
    }
}
