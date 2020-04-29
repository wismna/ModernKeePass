using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class GroupItemVm: NotifyPropertyChangedBase
    {
        private readonly IDatabaseService _databaseService;
        //private Group _reorderedGroup;
        private bool _isEditMode;

        public GroupEntity GroupEntity { get; }
        public GroupItemVm ParentVm { get; }

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
        
        public IEnumerable<EntryItemVm> SubEntries
        {
            get
            {
                var subEntries = new List<EntryItemVm>();
                subEntries.AddRange(Entries);
                foreach (var group in Children)
                {
                    subEntries.AddRange(group.SubEntries);
                }

                return subEntries;
            }
        }

        public Icon Symbol => GroupEntity.Icon;
        public List<EntryItemVm> Entries { get; }
        public ObservableCollection<GroupItemVm> Children { get; set; } = new ObservableCollection<GroupItemVm>();

        public GroupItemVm(GroupEntity groupEntity, GroupItemVm parent): this(App.Container.Resolve<IDatabaseService>(), groupEntity, parent)
        { }

        public GroupItemVm(IDatabaseService databaseService, GroupEntity groupEntity, GroupItemVm parentVm)
        {
            _databaseService = databaseService;
            GroupEntity = groupEntity;
            ParentVm = parentVm;

            Entries = new List<EntryItemVm>();
            foreach (var entry in groupEntity.Entries)
            {
                Entries.Add(new EntryItemVm(entry, this));
            }
            
            foreach (var subGroup in groupEntity.SubGroups)
            {
                Children.Add(new GroupItemVm(subGroup, this));
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
                    _databaseService.AddEntity(ParentVm.GroupEntity, (Entity)e.NewItems[0]);
                    break;
            }
        }
    }
}