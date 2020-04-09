using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Views
{
    /// <summary>
    /// The import/export page.
    /// </summary>
    public sealed partial class ImportExportPage
    {
        public ImportExportPage()
        {
            InitializeComponent();
        }

        private async void ImportFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
            picker.FileTypeFilter.Add(".csv");

            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync().AsTask();
            if (file == null) return;
            
        }
    }
}
