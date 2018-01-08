using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SaveDatabasePage
    {
        private Frame _mainFrame;
        public SaveVm Model => (SaveVm)DataContext;
        public SaveDatabasePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            await Model.Save();
            _mainFrame.Navigate(typeof(MainPage));
        }

        private async void SaveAsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Database"
            };
            savePicker.FileTypeChoices.Add("KeePass 2.x database", new List<string> { ".kdbx" });

            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;
            Model.Save(file);

            _mainFrame.Navigate(typeof(MainPage));
        }
    }
}
