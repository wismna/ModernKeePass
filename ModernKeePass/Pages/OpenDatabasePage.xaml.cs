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
            var mruToken = mru.Add(file, file.DisplayName);

            /*var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Containers.ContainsKey("Recent"))
                localSettings.CreateContainer("Recent", ApplicationDataCreateDisposition.Always);

            localSettings.Containers["Recent"].Values[file.DisplayName] = mruToken;*/
        }

        private void ShowPassword(StorageFile file)
        {
            var databaseVm = DataContext as DatabaseVm;
            if (databaseVm == null) return;
            databaseVm.SelectedVisibility = Visibility.Visible;
            databaseVm.Name = file.Name;
            databaseVm.NotifyPropertyChanged("SelectedVisibility");
            databaseVm.NotifyPropertyChanged("Name");
            PasswordBox.Focus(FocusState.Programmatic);
        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            StatusTextBlock.Text = app.Database.Open(PasswordBox.Password);
            if (string.IsNullOrEmpty(StatusTextBlock.Text)) _mainFrame.Navigate(typeof(GroupDetailPage), app.Database.RootGroup);
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) OpenButton_OnClick(null, null);
        }
    }
}
