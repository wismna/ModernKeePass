using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Events;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDatabasePage
    {
        private Frame _mainFrame;

        public NewDatabasePage()
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
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Database"
            };
            savePicker.FileTypeChoices.Add("KeePass 2.x database", new List<string> { ".kdbx" });

            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;
            var viewModel = DataContext as OpenVm;
            viewModel?.OpenFile(file);
        }

        private void PasswordUserControl_PasswordChecked(object sender, PasswordEventArgs e)
        {
            _mainFrame.Navigate(typeof(GroupDetailPage), e.RootGroup);
        }
    }
}
