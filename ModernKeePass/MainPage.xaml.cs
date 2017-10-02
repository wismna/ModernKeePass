using System.Collections.ObjectModel;
using System.Linq;
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
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var mainMenuItems = new ObservableCollection<MainMenuItemVm>
            {
                new MainMenuItemVm {Title = "Open", PageType = typeof(OpenDatabasePage), Destination = MenuFrame, Parameter = Frame, SymbolIcon = Symbol.Page2},
                new MainMenuItemVm {Title = "New" /*, PageType = typeof(NewDatabasePage)*/, Destination = MenuFrame, SymbolIcon = Symbol.Add},
                new MainMenuItemVm {Title = "Save" , PageType = typeof(SaveDatabasePage), Destination = MenuFrame, Parameter = Frame, SymbolIcon = Symbol.Save},
                new MainMenuItemVm {Title = "Recent" , PageType = typeof(RecentDatabasesPage), Destination = MenuFrame, Parameter = Frame, SymbolIcon = Symbol.Copy}
            };
            var app = (App)Application.Current;
            if (app.Database != null && app.Database.IsOpen)
                mainMenuItems.Add(new MainMenuItemVm { Title = app.Database.Name, PageType = typeof(GroupDetailPage), Destination = Frame, Parameter = app.Database.RootGroup, Group = 1, SymbolIcon = Symbol.ProtectedDocument});
            
            var mainVm = DataContext as MainVm;
            mainVm.MainMenuItems = from item in mainMenuItems group item by item.Group into grp orderby grp.Key select grp;
            mainVm.NotifyPropertyChanged("MainMenuItems");
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var mainMenuItem = e.AddedItems[0] as MainMenuItemVm;
            mainMenuItem?.Destination.Navigate(mainMenuItem.PageType, mainMenuItem.Parameter);
        }
    }
}
