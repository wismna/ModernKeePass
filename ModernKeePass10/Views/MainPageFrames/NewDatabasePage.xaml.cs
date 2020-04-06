using System;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Autofac;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Events;
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
        private readonly IResourceService _resourceService;

        public NewViewModel ViewModel { get; }

        public NewDatabasePage(): this(App.Container.Resolve<IResourceService>())
        { }

        public NewDatabasePage(IResourceService resourceService)
        {
            InitializeComponent();
            ViewModel = new NewViewModel();
            _resourceService = resourceService;
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

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            var fileInfo = new FileInfo
            {
                Path = token,
                Name = file.DisplayName
            };
            await ViewModel.OpenFile(fileInfo);
        }

        private void ImportFormatComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            switch (comboBox?.SelectedIndex)
            {
                case 0:
                    ViewModel.ImportFormat = ImportFormat.CSV;
                    ViewModel.ImportFileExtensionFilter = ".csv";
                    ViewModel.ImportFormatHelp = _resourceService.GetResourceValue("NewImportFormatHelpCSV");
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
            if (!string.IsNullOrEmpty(ViewModel.ImportFileExtensionFilter))
                picker.FileTypeFilter.Add(ViewModel.ImportFileExtensionFilter);

            // Application now has read/write access to the picked file
            ViewModel.ImportFile = await picker.PickSingleFileAsync();
            if (ViewModel.ImportFile != null) ImportFileLink.Content = ViewModel.ImportFile.Name;
        }

        private void CompositeKeyUserControl_OnValidationChecked(object sender, EventArgs e)
        {
            ViewModel.PopulateInitialData();
            _mainFrame.Navigate(typeof(GroupsPage));
        }
    }
}
