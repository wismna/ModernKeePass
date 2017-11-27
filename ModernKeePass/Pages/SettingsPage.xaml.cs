using Windows.UI.Xaml.Controls;
using ModernKeePass.Pages.SettingsPageFrames;
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
        public new SettingsVm Model => (SettingsVm)DataContext;

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
            MenuFrame?.Navigate(selectedItem == null ? typeof(SettingsWelcomePage) : selectedItem.PageType);
        }
    }
}
