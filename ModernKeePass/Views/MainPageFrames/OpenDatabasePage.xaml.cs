using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenDatabasePage
    {
        private OpenVm Model => (OpenVm)DataContext;
        
        public OpenDatabasePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var file = e.Parameter as FileInfo;
            if (file != null)
            {
                Model.Path = file.Path;
                Model.Name = file.Name;
                Model.Token = file.Id;
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
            
            // TODO: use service
            Model.Token = StorageApplicationPermissions.MostRecentlyUsedList.Add(file, file.Path);
            Model.Path = file.Path;
            Model.Name = file.Name;
        }
    }
}
