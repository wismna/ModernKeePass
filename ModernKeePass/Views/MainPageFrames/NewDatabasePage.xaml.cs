using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Events;
using ModernKeePass.ImportFormats;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDatabasePage
    {
        private Frame _mainFrame;

        public NewVm Model => (NewVm)DataContext;

        public NewDatabasePage()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
        }

        private async void CreateDatabaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Database"
            };
            savePicker.FileTypeChoices.Add("KeePass 2.x database", new List<string> { ".kdbx" });

            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;
            Model.OpenFile(file);
        }

        private void ImportFormatComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var resources = new ResourcesService();
            var comboBox = sender as ComboBox;
            switch (comboBox?.SelectedIndex)
            {
                case 0:
                    Model.ImportFormat = new CsvImportFormat();
                    Model.ImportFileExtensionFilter = ".csv";
                    Model.ImportFormatHelp = resources.GetResourceValue("NewImportFormatHelpCSV");
                    break;
                default:
                    Model.ImportFormat = new NullImportFormat();
                    break;
            }
        }

        private async void ImportFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
            if (!string.IsNullOrEmpty(Model.ImportFileExtensionFilter))
                picker.FileTypeFilter.Add(Model.ImportFileExtensionFilter);

            // Application now has read/write access to the picked file
            Model.ImportFile = await picker.PickSingleFileAsync();
            if (Model.ImportFile != null) ImportFileLink.Content = Model.ImportFile.Name;
        }

        private void CompositeKeyUserControl_OnValidationChecked(object sender, PasswordEventArgs e)
        {
            Model.PopulateInitialData(DatabaseService.Instance, new SettingsService(), new ImportService());
            _mainFrame.Navigate(typeof(GroupDetailPage), DatabaseService.Instance.RootGroup);
        }
    }
}
