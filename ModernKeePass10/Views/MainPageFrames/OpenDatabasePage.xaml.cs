using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;
using Windows.Storage.AccessCache;
using ModernKeePass.Domain.Dtos;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenDatabasePage
    {
        public OpenViewModel ViewModel { get; }

        public OpenDatabasePage()
        {
            InitializeComponent();
            ViewModel = new OpenViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is FileInfo fileInfo)
            {
                await ViewModel.OpenFile(fileInfo);
            }
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
            picker.FileTypeFilter.Add(".kdbx");
            
            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            var fileInfo = new FileInfo
            {
                Path = token,
                Name = file.DisplayName
            };
            await ViewModel.OpenFile(fileInfo);
        }
    }
}
