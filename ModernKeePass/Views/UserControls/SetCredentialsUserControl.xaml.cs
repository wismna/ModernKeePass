using System;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using ModernKeePass.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class SetCredentialsUserControl
    {
        private SetCredentialsVm Model => (SetCredentialsVm)Grid.DataContext;

        public string ButtonLabel
        {
            get { return (string)GetValue(ButtonLabelProperty); }
            set { SetValue(ButtonLabelProperty, value); }
        }
        public static readonly DependencyProperty ButtonLabelProperty =
            DependencyProperty.Register(
                nameof(ButtonLabel),
                typeof(string),
                typeof(SetCredentialsUserControl),
                new PropertyMetadata("OK", (o, args) => { }));
        
        public SetCredentialsUserControl()
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
            picker.FileTypeFilter.Add("*");

            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            var token = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            Model.KeyFilePath = token;
            Model.KeyFileText = file.DisplayName;
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

            var token = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            Model.KeyFilePath = token;
            Model.KeyFileText = file.DisplayName;
            await Model.GenerateKeyFile();
        }
    }
}
