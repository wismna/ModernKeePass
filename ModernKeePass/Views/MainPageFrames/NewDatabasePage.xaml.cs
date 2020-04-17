﻿using System;
using System.Collections.Generic;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Infrastructure.File;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewDatabasePage
    {
        private readonly IResourceProxy _resource;
        public NewVm Model => (NewVm)DataContext;

        public NewDatabasePage(): this(App.Services.GetRequiredService<IResourceProxy>()) { }
        public NewDatabasePage(IResourceProxy resource)
        {
            _resource = resource;
            InitializeComponent();
        }

        private async void CreateDatabaseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "New Database"
            };
            savePicker.FileTypeChoices.Add("KeePass 2.x database", new List<string> { ".kdbx" });

            var file = await savePicker.PickSaveFileAsync().AsTask();
            if (file == null) return;

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            var fileInfo = new FileInfo
            {
                Id = token,
                Path = file.Path,
                Name = file.DisplayName
            };
            await Model.OpenFile(fileInfo);
        }

        private void ImportFormatComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            switch (comboBox?.SelectedIndex)
            {
                case 0:
                    Model.ImportFormat = new CsvImportFormat();
                    Model.ImportFileExtensionFilter = ".csv";
                    Model.ImportFormatHelp = _resource.GetResourceValue("NewImportFormatHelpCSV");
                    break;
            }
        }

        private async void ImportFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            if (!string.IsNullOrEmpty(Model.ImportFileExtensionFilter))
                picker.FileTypeFilter.Add(Model.ImportFileExtensionFilter);

            // Application now has read/write access to the picked file
            Model.ImportFile = await picker.PickSingleFileAsync().AsTask();
            if (Model.ImportFile != null) ImportFileLink.Content = Model.ImportFile.Name;
        }
    }
}
