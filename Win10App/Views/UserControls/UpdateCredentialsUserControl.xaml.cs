using System;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.ViewModels;
using ModernKeePass.Extensions;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class UpdateCredentialsUserControl
    {
        public UpdateCredentialsViewModel ViewModel => Grid.DataContext as UpdateCredentialsViewModel;
        public string DatabaseFilePath
        {
            get => (string)GetValue(DatabaseFilePathProperty);
            set => SetValue(DatabaseFilePathProperty, value);
        }
        public static readonly DependencyProperty DatabaseFilePathProperty =
            DependencyProperty.Register(
                "DatabaseFilePath",
                typeof(string),
                typeof(CredentialsUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public event EventHandler CredentialsUpdated;

        public UpdateCredentialsUserControl()
        {
            InitializeComponent();
        }
        
        private async void KeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
            picker.FileTypeFilter.Add(".key");

            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            ViewModel.KeyFilePath = token;
        }

        private async void CreateKeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "Key"
            };
            savePicker.FileTypeChoices.Add("Key file", new List<string> { ".key" });

            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            ViewModel.KeyFilePath = token;
        }

        private async void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            if (await Dispatcher.RunTaskAsync(async () =>
            {
                var fileInfo = new FileInfo
                {
                    Path = DatabaseFilePath
                };
                return await ViewModel.CreateDatabase(fileInfo);
            }))
            {
                CredentialsUpdated?.Invoke(this, new EventArgs());
            }
        }
    }
}
