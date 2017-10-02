using System;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;
using Windows.Storage.AccessCache;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenDatabasePage : Page
    {
        private Frame _mainFrame;

        public OpenDatabasePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
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

            var file = await picker.PickSingleFileAsync();
            // Application now has read/write access to the picked file
            if (file == null) return;
            // Initialize KDBX database
            ((App)Application.Current).Database = new DatabaseHelper(file);
            AddToRecentFiles(file);
            ShowPassword(file);
        }

        private void AddToRecentFiles(StorageFile file)
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            mru.Add(file, file.DisplayName);
        }

        private void ShowPassword(StorageFile file)
        {
            var databaseVm = DataContext as DatabaseVm;
            if (databaseVm == null) return;
            databaseVm.SelectedVisibility = Visibility.Visible;
            databaseVm.Name = file.Name;
            databaseVm.NotifyPropertyChanged("SelectedVisibility");
            databaseVm.NotifyPropertyChanged("Name");
        }

        private void PasswordUserControl_PasswordChecked(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            if (app.Database.IsOpen) _mainFrame.Navigate(typeof(GroupDetailPage), app.Database.RootGroup);
        }
    }
}
