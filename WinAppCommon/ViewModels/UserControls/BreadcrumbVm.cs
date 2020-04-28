using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using ModernKeePass.Common;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class BreadcrumbVm
    {
        public RelayCommand<string> NavigateCommand { get; }

        public BreadcrumbVm(INavigationService navigation)
        {
            NavigateCommand = new RelayCommand<string>(groupId =>
                navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem {Id = groupId}), 
                groupId => !string.IsNullOrEmpty(groupId));
        }
    }
}