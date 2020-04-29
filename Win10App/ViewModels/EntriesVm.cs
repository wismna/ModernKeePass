using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Commands.AddEntry;
using ModernKeePass.Application.Group.Commands.CreateEntry;
using ModernKeePass.Application.Group.Commands.DeleteEntry;
using ModernKeePass.Application.Group.Commands.MoveEntry;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class EntriesVm : ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly INotificationService _notification;
        private readonly IDialogService _dialog;
        private readonly IResourceProxy _resource;

        private GroupVm _parentGroup;
        private EntryItemVm _reorderedEntry;
        private EntryItemVm _selectedEntry;

        public ObservableCollection<EntryItemVm> Entries { get; set; }

        public EntryItemVm SelectedEntry
        {
            get => _selectedEntry;
            set => Set(() => SelectedEntry, ref _selectedEntry, value);
        }

        public EntriesVm(IMediator mediator, INavigationService navigation, INotificationService notification, IDialogService dialog, IResourceProxy resource)
        {
            _mediator = mediator;
            _navigation = navigation;
            _notification = notification;
            _dialog = dialog;
            _resource = resource;
        }

        public async Task Initialize(string groupId)
        {
            _parentGroup = await _mediator.Send(new GetGroupQuery {Id = groupId});
            Entries = new ObservableCollection<EntryItemVm>(_parentGroup.Entries);
            Entries.CollectionChanged += EntriesOnCollectionChanged;
        }

        private async void EntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    var oldIndex = e.OldStartingIndex;
                    _reorderedEntry = Entries[oldIndex];
                    break;
                case NotifyCollectionChangedAction.Add:
                    if (_reorderedEntry == null)
                    {
                        var entry = (EntryVm)e.NewItems[0];
                        await _mediator.Send(new AddEntryCommand { EntryId = entry.Id, ParentGroupId = _parentGroup.Id });
                    }
                    else
                    {
                        await _mediator.Send(new MoveEntryCommand { Entry = _reorderedEntry, ParentGroup = _parentGroup, Index = e.NewStartingIndex });
                    }
                    break;
            }
        }
        
        private async Task AskForDelete(EntryItemVm entry)
        {
            if (IsRecycleOnDelete)
            {
                await Delete(entry);
                _notification.Show(_resource.GetResourceValue("EntryRecyclingConfirmation"), _resource.GetResourceValue("EntryRecycled"));
            }
            else
            {
                await _dialog.ShowMessage(_resource.GetResourceValue("EntryDeletingConfirmation"),
                    _resource.GetResourceValue("EntityDeleteTitle"),
                    _resource.GetResourceValue("EntityDeleteActionButton"),
                    _resource.GetResourceValue("EntityDeleteCancelButton"),
                    async isOk =>
                    {
                        if (isOk) await Delete(entry);
                    });
            }
        }

        private async Task Delete(EntryItemVm entry)
        {
            await _mediator.Send(new DeleteEntryCommand
            {
                EntryId = entry.Id,
                ParentGroupId = _parentGroup.Id,
                RecycleBinName = _resource.GetResourceValue("RecycleBinTitle")
            });
            Entries.Remove(entry);
        }

        public async Task AddNewEntry(string text)
        {
            var entry = await _mediator.Send(new CreateEntryCommand { ParentGroup = _parentGroup });
            entry.Title = text;
            Entries.Add(entry);
            SelectedEntry = entry;
        }
    }
}