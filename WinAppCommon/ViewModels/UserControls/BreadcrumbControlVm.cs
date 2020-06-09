using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Common.Models;
using ModernKeePass.Common;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class BreadcrumbControlVm
    {
        public IEnumerable<BreadcrumbItem> BreadcrumbItems { get; }
        public string ParentGroupName => BreadcrumbItems.Last()?.Name;

        public RelayCommand GoBackCommand { get; }
        public RelayCommand<int> GoToCommand { get; }

        private readonly INavigationService _navigation;
        private readonly IBreadcrumbService _breadcrumb;

        public BreadcrumbControlVm(INavigationService navigation, IBreadcrumbService breadcrumb)
        {
            _navigation = navigation;
            _breadcrumb = breadcrumb;

            BreadcrumbItems = _breadcrumb.GetItems().Reverse();
            GoBackCommand = new RelayCommand(GoBack);
            GoToCommand = new RelayCommand<int>(GoTo);
        }

        private void GoTo(int index)
        {
            var breadcrumb = _breadcrumb.Pop(BreadcrumbItems.Count() - index);
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = breadcrumb.Path });
        }

        private void GoBack()
        {
            _breadcrumb.Pop();
            _navigation.GoBack();
        }
    }
}