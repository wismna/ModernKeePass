using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Autofac;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class GroupItemViewModel: NotifyPropertyChangedBase
    {
        private readonly IDatabaseService _databaseService;
        //private Group _reorderedGroup;
        private bool _isEditMode;

        public GroupEntity GroupEntity { get; }
        public GroupItemViewModel ParentViewModel { get; }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => GroupEntity.Name;
            set
            {
                GroupEntity.Name = value;
                OnPropertyChanged();
            }
        }
        
        public IEnumerable<EntryItemViewModel> SubEntries
        {
            get
            {
                var subEntries = new List<EntryItemViewModel>();
                subEntries.AddRange(Entries);
                foreach (var group in Children)
                {
                    subEntries.AddRange(group.SubEntries);
                }

                return subEntries;
            }
        }

        public Icon Symbol => GroupEntity.Icon;
        public List<EntryItemViewModel> Entries { get; }
        public ObservableCollection<GroupItemViewModel> Children { get; set; } = new ObservableCollection<GroupItemViewModel>();

        public GroupItemViewModel(GroupEntity groupEntity, GroupItemViewModel parent): this(App.Container.Resolve<IDatabaseService>(), groupEntity, parent)
        { }

        public GroupItemViewModel(IDatabaseService databaseService, GroupEntity groupEntity, GroupItemViewModel parentViewModel)
        {
            _databaseService = databaseService;
            GroupEntity = groupEntity;
            ParentViewModel = parentViewModel;

            Entries = new List<EntryItemViewModel>();
            foreach (var entry in groupEntity.Entries)
            {
                Entries.Add(new EntryItemViewModel(entry, this));
            }
            
            foreach (var subGroup in groupEntity.SubGroups)
            {
                Children.Add(new GroupItemViewModel(subGroup, this));
            }
            Children.CollectionChanged += Children_CollectionChanged;
        }

        // TODO: not triggered when reordering
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    /*var oldIndex = (uint)e.OldStartingIndex;
                    _reorderedGroup = Group.SubGroups.GetAt(oldIndex);
                    Group.SubGroups.RemoveAt(oldIndex);*/
                    _databaseService.DeleteEntity((Entity)e.OldItems[0]);
                    break;
                case NotifyCollectionChangedAction.Add:
                    /*if (_reorderedGroup == null) Group.AddGroup(((GroupItem)e.NewItems[0]).Group, true);
                    else Group.Groups.Insert((uint)e.NewStartingIndex, _reorderedGroup);*/
                    _databaseService.AddEntity(ParentViewModel.GroupEntity, (Entity)e.NewItems[0]);
                    break;
            }
        }
    }
}