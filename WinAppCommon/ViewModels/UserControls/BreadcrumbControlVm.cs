using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using ModernKeePass.Application.Common.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Application.Group.Queries.GetGroup;
using ModernKeePass.Common;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class BreadcrumbControlVm
    {
        public ObservableCollection<BreadcrumbItem> BreadcrumbItems { get; }
        public string ParentGroupName { get; private set; }
        public Icon ParentGroupIcon { get; private set; }

        public RelayCommand GoBackCommand { get; }
        public RelayCommand GoUpCommand { get; private set; }
        public RelayCommand<int> GoToCommand { get; }

        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;

        public BreadcrumbControlVm(IMediator mediator, INavigationService navigation)
        {
            _mediator = mediator;
            _navigation = navigation;
            
            BreadcrumbItems = new ObservableCollection<BreadcrumbItem>();
            GoBackCommand = new RelayCommand(() => _navigation.GoBack());
            GoToCommand = new RelayCommand<int>(GoTo);
        }

        public async Task Initialize(GroupVm group)
        {
            GoUpCommand = new RelayCommand(() => GoTo(BreadcrumbItems.Count - 1), () => group != null);

            if (group == null) return;
            ParentGroupName = group.Title;
            ParentGroupIcon = group.Icon;

            BreadcrumbItems.Insert(0, new BreadcrumbItem { Path = group.Id, Name = group.Title, Icon = group.Icon });
            var parentGroup = group;
            while (!string.IsNullOrEmpty(parentGroup.ParentGroupId))
            {
                parentGroup = await _mediator.Send(new GetGroupQuery {Id = parentGroup.ParentGroupId});
                BreadcrumbItems.Insert(0, new BreadcrumbItem {Path = parentGroup.Id, Name = parentGroup.Title, Icon = parentGroup.Icon});
            }
        }

        private void GoTo(int index)
        {
            if (BreadcrumbItems.Count == 0) return;
            var breadcrumb = BreadcrumbItems[index];
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = breadcrumb.Path });
        }
    }
}