using Autofac;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;

namespace ModernKeePass.ViewModels
{
    public class GroupsViewModel : NotifyPropertyChangedBase
    {
        private string _title;
        private string _newGroupName;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        // TODO: check why binding not working
        public string NewGroupName
        {
            get => _newGroupName;
            set
            {
                _newGroupName = value;
                OnPropertyChanged(nameof(NewGroupName));
            }
        }

        public GroupItemViewModel RootItemViewModel { get; set; }

        public GroupsViewModel(): this(App.Container.Resolve<IDatabaseService>().RootGroupEntity)
        { }

        public GroupsViewModel(GroupEntity groupEntity)
        {
            Title = groupEntity.Name;
            RootItemViewModel = new GroupItemViewModel(groupEntity, null);
        }

        public void AddNewGroup(string groupName = "")
        {
            var group = new GroupEntity
            {
                Name = groupName,
                Icon = Icon.Folder,
            };
            RootItemViewModel.Children.Add(new GroupItemViewModel(group, RootItemViewModel));
        }
    }
}