using System;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;

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
            DataContext = new DatabaseVm();
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
            if (file == null) return;
            // Application now has read/write access to the picked file
            ((App)Application.Current).Database = new DatabaseHelper(file);
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
