using Windows.UI.Xaml.Controls;
using ModernKeePass.ViewModels;
using ModernKeePass.ViewModels.ListItems;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace ModernKeePass.Views
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class SettingsPage
    {
        private new SettingsVm Model => (SettingsVm)Resources["ViewModel"];

        public SettingsPage()
        {
            InitializeComponent();
            ListView = MenuListView;
            ListViewSource = MenuItemsSource;
        }
        
        private new void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            base.ListView_SelectionChanged(sender, e);
            var selectedItem = Model.SelectedItem as ListMenuItemVm;
            MenuFrame?.Navigate(selectedItem == null ? typeof(SettingsWelcomePage) : selectedItem.PageType);
        }
    }
}
