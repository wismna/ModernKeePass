using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public new MainVm Model => (MainVm)DataContext;
        
        public MainPage()
        {
            InitializeComponent();
            ListView = MenuListView;
            ListViewSource = MenuItemsSource;
        }

        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView_SelectionChanged(sender, e);
            var selectedItem = Model.SelectedItem as MainMenuItemVm;
            selectedItem?.Destination.Navigate(selectedItem.PageType, selectedItem.Parameter);
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           base.OnNavigatedTo(e);
            DataContext = new MainVm(Frame, MenuFrame);
            if (Model.SelectedItem == null) MenuFrame.Navigate(typeof(WelcomePage));
        }
    }
}
