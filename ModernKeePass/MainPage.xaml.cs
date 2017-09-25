using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
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
        
        private async void Button_Click(object sender, RoutedEventArgs e)
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
                //DataContext = new DatabaseVm(file);
                ((App)Application.Current).Database = new Common.DatabaseHelper(file);
                var homeVm = DataContext as HomeVm;
                homeVm.Visibility = Visibility.Visible;
                homeVm.NotifyPropertyChanged("Visibility");
            }
        }
        
        private void openBbutton_Click(object sender, RoutedEventArgs e)
        {
            /*var database = DataContext as DatabaseVm;
            database.Open();
            if (database.IsOpen)
                Frame.Navigate(typeof(GroupDetailPage), database.RootGroup);*/
            var homeVm = DataContext as HomeVm;
            var app = ((App)Application.Current);
            homeVm.ErrorMessage = app.Database.Open(homeVm.Password);
            
            if (!string.IsNullOrEmpty(homeVm.ErrorMessage)) homeVm.NotifyPropertyChanged("ErrorMessage");
            else Frame.Navigate(typeof(GroupDetailPage), app.Database.RootGroup);
        }

        private void saveBbutton_Click(object sender, RoutedEventArgs e)
        {
            var database = DataContext as HomeVm;
            ((App)Application.Current).Database.Save();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Contains(SelectItem) || e.AddedItems.Contains(NewItem))
            {
                SelectGrid.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Collapsed;
            }
            else if (e.AddedItems.Contains(SaveItem))
            {
                SaveButton.Visibility = Visibility.Visible;
                SelectGrid.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var app = (App)Application.Current;
            if (app.Database == null) return;
            var homeVm = DataContext as HomeVm;
            homeVm.IsOpen = app.Database.IsOpen;
            homeVm.NotifyPropertyChanged("IsOpen");
        }
    }
}
