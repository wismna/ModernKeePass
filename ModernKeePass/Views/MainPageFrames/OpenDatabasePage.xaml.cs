using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Models;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenDatabasePage
    {
        private readonly INavigationService _navigation;
        private OpenVm Model => (OpenVm)Resources["ViewModel"];

        public OpenDatabasePage(): this(
            App.Services.GetRequiredService<INavigationService>(),
            App.Services.GetRequiredService<IMessenger>()) { }

        public OpenDatabasePage(INavigationService navigation, IMessenger messenger)
        {
            _navigation = navigation;
            InitializeComponent();

            messenger.Register<DatabaseOpenedMessage>(this, NavigateToPage);
        }

        private void NavigateToPage(DatabaseOpenedMessage message)
        {
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = message.RootGroupId });
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var file = e.Parameter as FileInfo;
            if (file != null)
            {
                await Model.OpenFile(file);
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".kdbx");
            
            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync().AsTask();
            if (file == null) return;


            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            var fileInfo = new FileInfo
            {
                Path = file.Path,
                Name = file.DisplayName,
                Id = token
            };
            await Model.OpenFile(fileInfo);
        }
    }
}
