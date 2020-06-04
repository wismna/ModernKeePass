using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
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
using ModernKeePass.Application.Group.Commands.MoveGroup;
using ModernKeePass.Application.Group.Commands.RemoveGroup;
using ModernKeePass.Application.Group.Commands.SortEntries;
using ModernKeePass.Application.Group.Commands.SortGroups;
using ModernKeePass.Application.Group.Commands.UpdateGroup;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Common;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Exceptions;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class GroupDetailVm : ViewModelBase
    {
        public ObservableCollection<EntryVm> Entries { get; private set; }

        public ObservableCollection<GroupVm> Groups { get; private set; }
        
        public bool IsNotRoot => Database.RootGroupId != _group.Id;

        public IOrderedEnumerable<IGrouping<char, EntryVm>> EntriesZoomedOut => from e in Entries
            group e by (e.Title.Value ?? string.Empty).ToUpper().FirstOrDefault() into grp
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
                Set(() => IsEditMode, ref _isEditMode, value);
                SortEntriesCommand.RaiseCanExecuteChanged();
                SortGroupsCommand.RaiseCanExecuteChanged();
            }
        }

        public string ParentGroupName => _parent?.Title;

        public bool IsRecycleOnDelete => Database.IsRecycleBinEnabled && !IsInRecycleBin;

        public bool IsInRecycleBin => _parent != null && _parent.Id == Database.RecycleBinId;
        
        public RelayCommand SaveCommand { get; }
        public RelayCommand SortEntriesCommand { get; }
        public RelayCommand SortGroupsCommand { get; }
        public RelayCommand<string> MoveCommand { get; }
        public RelayCommand CreateEntryCommand { get; }
        public RelayCommand<string> CreateGroupCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand GoBackCommand { get; }
        public RelayCommand GoToParentCommand { get; }
        public RelayCommand<GroupVm> GoToGroupCommand { get; }
        public RelayCommand<EntryVm> GoToEntryCommand { get; }

        private DatabaseVm Database => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
        
        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        private readonly INavigationService _navigation;
        private readonly IDialogService _dialog;
        private readonly INotificationService _notification;
        private GroupVm _group;
        private GroupVm _parent;
        private bool _isEditMode;
        private EntryVm _reorderedEntry;
        private GroupVm _reorderedGroup;

        public GroupDetailVm(IMediator mediator, IResourceProxy resource, INavigationService navigation, IDialogService dialog, INotificationService notification)
        {
            _mediator = mediator;
            _resource = resource;
            _navigation = navigation;
            _dialog = dialog;
            _notification = notification;

            SaveCommand = new RelayCommand(async () => await SaveChanges(), () => Database.IsDirty);
            SortEntriesCommand = new RelayCommand(async () => await SortEntriesAsync(), () => IsEditMode);
            SortGroupsCommand = new RelayCommand(async () => await SortGroupsAsync(), () => IsEditMode);
            MoveCommand = new RelayCommand<string>(async destination => await Move(destination), destination => IsNotRoot && !string.IsNullOrEmpty(destination) && destination != Id);
            CreateEntryCommand = new RelayCommand(async () => await AddNewEntry(), () => !IsInRecycleBin && Database.RecycleBinId != Id);
            CreateGroupCommand = new RelayCommand<string>(async newGroupName => await AddNewGroup(newGroupName), _ => !IsInRecycleBin && Database.RecycleBinId != Id);
            DeleteCommand = new RelayCommand(async () => await AskForDelete(),() => IsNotRoot);
            GoBackCommand = new RelayCommand(() => _navigation.GoBack());
            GoToParentCommand = new RelayCommand(() => GoToGroup(_parent.Id), () => _parent != null);
            GoToGroupCommand = new RelayCommand<GroupVm>(group => GoToGroup(group.Id), group => group != null);
            GoToEntryCommand = new RelayCommand<EntryVm>(entry => GoToEntry(entry.Id), entry => entry != null);

            MessengerInstance.Register<DatabaseSavedMessage>(this, _ => SaveCommand.RaiseCanExecuteChanged());
        }

        public async Task Initialize(string groupId)
        {
            _group = await _mediator.Send(new GetGroupQuery { Id = groupId });
            if (!string.IsNullOrEmpty(_group.ParentGroupId))
            {
                _parent = await _mediator.Send(new GetGroupQuery { Id = _group.ParentGroupId });
            }

            Entries = new ObservableCollection<EntryVm>(_group.Entries);
            Entries.CollectionChanged += Entries_CollectionChanged;
            Groups = new ObservableCollection<GroupVm>(_group.Groups);
            Groups.CollectionChanged += Groups_CollectionChanged;
        }

        public void GoToEntry(string entryId, bool isNew = false)
        {
            _navigation.NavigateTo(Constants.Navigation.EntryPage, new NavigationItem
            {
                Id = entryId,
                IsNew = isNew
            });
        }
        public void GoToGroup(string groupId, bool isNew = false)
        {
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem
            {
                Id = groupId,
                IsNew = isNew
            });
        }
        
        public async Task AddNewGroup(string name = "")
        {
            var group = await _mediator.Send(new CreateGroupCommand {Name = name, ParentGroup = _group});
            Groups.Add(group);
        }
        
        public async Task AddNewEntry()
        {
            var entry = await _mediator.Send(new CreateEntryCommand { ParentGroup = _group });
            GoToEntry(entry.Id, true);
        }
        
        public async Task Move(string destinationId)
        {
            await _mediator.Send(new AddGroupCommand {ParentGroupId = destinationId, GroupId = Id });
            await _mediator.Send(new RemoveGroupCommand {ParentGroupId = _parent.Id, GroupId = Id });
            GoToGroup(destinationId);
        }
        
        private async Task SaveChanges()
        {
            try
            {
                await _mediator.Send(new SaveDatabaseCommand());
            }
            catch (SaveException e)
            {
                MessengerInstance.Send(new SaveErrorMessage { Message = e.Message });
            }
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
                        await _mediator.Send(new AddEntryCommand {EntryId = entry.Id, ParentGroupId = Id});
                    }
                    else
                    {
                        await _mediator.Send(new MoveEntryCommand {Entry = _reorderedEntry, ParentGroup = _group, Index = e.NewStartingIndex});
                    }
                    break;
            }
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async void Groups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    var oldIndex = e.OldStartingIndex;
                    _reorderedGroup = _group.Groups[oldIndex];
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (_reorderedGroup == null)
                    {
                        var group = (GroupVm)e.NewItems[0];
                        await _mediator.Send(new AddGroupCommand() { GroupId = group.Id, ParentGroupId = Id });
                    }
                    else
                    {
                        await _mediator.Send(new MoveGroupCommand { Group = _reorderedGroup, ParentGroup = _group, Index = e.NewStartingIndex });
                    }
                    break;
            }
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async Task SortEntriesAsync()
        {
            await _mediator.Send(new SortEntriesCommand {Group = _group});
            Entries = new ObservableCollection<EntryVm>(_group.Entries);
            Entries.CollectionChanged += Entries_CollectionChanged;
            RaisePropertyChanged(nameof(Entries));
            SaveCommand.RaiseCanExecuteChanged();
        }
        
        private async Task SortGroupsAsync()
        {
            await _mediator.Send(new SortGroupsCommand {Group = _group});
            Groups = new ObservableCollection<GroupVm>(_group.Groups);
            RaisePropertyChanged(nameof(Groups));
            SaveCommand.RaiseCanExecuteChanged();
        }

        private async Task AskForDelete()
        {
            if (IsRecycleOnDelete)
            {
                await Delete();
                _notification.Show(_resource.GetResourceValue("EntityDeleting"), string.Format(_resource.GetResourceValue("GroupRecycled"), Title));
            }
            else
            {
                await _dialog.ShowMessage(
                    string.Format(_resource.GetResourceValue("GroupDeletingConfirmation"), Title), 
                    _resource.GetResourceValue("EntityDeleting"),
                    _resource.GetResourceValue("EntityDeleteActionButton"),
                    _resource.GetResourceValue("EntityDeleteCancelButton"),
                    async isOk =>
                    {
                        if (isOk) await Delete();
                    });
            }
        }

        private async Task Delete()
        {
            await _mediator.Send(new DeleteGroupCommand
            {
                GroupId = _group.Id,
                ParentGroupId = _group.ParentGroupId,
                RecycleBinName = _resource.GetResourceValue("RecycleBinTitle")
            });
            _navigation.GoBack();
        }
    }
}
