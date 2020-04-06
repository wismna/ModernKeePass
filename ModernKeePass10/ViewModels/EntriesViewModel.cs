using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Autofac;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class EntriesViewModel : NotifyPropertyChangedBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly GroupItemViewModel _parentGroupViewModel;
        //private Entry _reorderedEntry;
        private EntryItemViewModel _selectedEntry;

        public ObservableCollection<EntryItemViewModel> Entries { get; set; }

        public EntryItemViewModel SelectedEntry
        {
            get => _selectedEntry;
            set
            {
                _selectedEntry = value;
                OnPropertyChanged(nameof(SelectedEntry));
            }
        }

        public EntriesViewModel(GroupItemViewModel parentGroup): this(App.Container.Resolve<IDatabaseService>(), parentGroup)
        { }

        public EntriesViewModel(IDatabaseService databaseService, GroupItemViewModel parentParentGroupViewModel)
        {
            _databaseService = databaseService;
            _parentGroupViewModel = parentParentGroupViewModel;
            Entries = new ObservableCollection<EntryItemViewModel>();
            foreach (var entry in parentParentGroupViewModel.Entries)
            {
                Entries.Add(entry);
            }
            Entries.CollectionChanged += EntriesOnCollectionChanged;
        }

        private void EntriesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO: rewrite this (with service)
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    /*var oldIndex = (uint)e.OldStartingIndex;
                    _reorderedEntry = _group.Group.Entries.GetAt(oldIndex);
                    _group.Group.Entries.RemoveAt(oldIndex);*/
                    _databaseService.DeleteEntity((Entity)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Add:
                    /*if (_reorderedEntry == null) _group.Group.AddEntry(((EntryItemViewModel)e.NewItems[0]).Entry, true);
                    else _group.Group.Entries.Insert((uint)e.NewStartingIndex, _reorderedEntry);*/
                    _databaseService.AddEntity(_parentGroupViewModel.GroupEntity, (Entity)e.NewItems[0]);
                    break;
            }
        }

        public void AddNewEntry(string text)
        {
            var entry = new EntryItemViewModel(new EntryEntity(), _parentGroupViewModel) {Name = text};
            Entries.Add(entry);
            SelectedEntry = entry;
        }
    }
}