using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Models;
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
            InitializeComponent();
            var mainMenuItems = new ObservableCollection<MainMenuItem>
            {
                new MainMenuItem {Title = "File", PageType = typeof(OpenDatabasePage)},
                new MainMenuItem {Title = "New" /*, PageType = typeof(NewDatabasePage)*/},
                new MainMenuItem {Title = "Save" , PageType = typeof(SaveDatabasePage)},
                new MainMenuItem {Title = "Recent files - Coming soon" /*, PageType = typeof(RecentDatabasesPage)*/},
                new MainMenuItem {Title = "Url files - Coming soon" /*, PageType = typeof(OpenUrlPage)*/}
            };
            DataContext = new MainVm {MainMenuItems = mainMenuItems};
            MenuListView.SelectedIndex = -1;
        }
        
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Frame == null) return;
            var listView = sender as ListView;
            if (listView == null) return;
            if (listView.SelectedIndex == -1) return;
            var selectedItem = listView.SelectedItem as MainMenuItem;
            if (selectedItem != null) MenuFrame.Navigate(selectedItem.PageType, Frame);
        }
    }
}
