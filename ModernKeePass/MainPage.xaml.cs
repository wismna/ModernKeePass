using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

using ModernKeePass.Pages;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".kdbx");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                DataContext = new DatabaseVm(file);
            }
            else
            {
            }
        }
        
        private void openBbutton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var database = DataContext as DatabaseVm;
            database.Open();
            if (database.IsOpen)
                Frame.Navigate(typeof(GroupDetailPage), database.RootGroup);
        }
    }
}
