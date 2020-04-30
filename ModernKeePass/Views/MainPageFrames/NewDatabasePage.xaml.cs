using System;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDatabasePage
    {
        private NewVm Model => (NewVm)DataContext;
        
        public NewDatabasePage()
        {
            InitializeComponent();
        }

        private async void CreateDatabaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Database"
            };
            savePicker.FileTypeChoices.Add("KeePass 2.x database", new List<string> {".kdbx"});

            var file = await savePicker.PickSaveFileAsync().AsTask();
            if (file == null) return;

            Model.Token = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            Model.Name = file.Name;
            Model.Path = file.Path;
        }
    }
}
