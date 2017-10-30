using Windows.UI.Xaml.Controls;
using ModernKeePass.ViewModels;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace ModernKeePass.Pages
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class SettingsPage
    {
        public new SettingsVM Model => (SettingsVM)DataContext;

        public SettingsPage()
        {
            InitializeComponent();
            ListView = MenuListView;
            ListViewSource = MenuItemsSource;
        }
        
        private void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView_SelectionChanged(sender, e);
            var selectedItem = Model.SelectedItem as ListMenuItemVm;
            if (selectedItem == null) return;
            MenuFrame?.Navigate(selectedItem.PageType);
        }
    }
}
