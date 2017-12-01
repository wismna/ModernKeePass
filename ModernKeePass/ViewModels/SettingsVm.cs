using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Pages;
using ModernKeePass.Pages.SettingsPageFrames;

namespace ModernKeePass.ViewModels
{
    public class SettingsVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private ListMenuItemVm _selectedItem;
        
        private IOrderedEnumerable<IGrouping<string, ListMenuItemVm>> _menuItems;

        public IOrderedEnumerable<IGrouping<string, ListMenuItemVm>> MenuItems
        {
            get { return _menuItems; }
            set { SetProperty(ref _menuItems, value); }
        }

        public ISelectableModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                SetProperty(ref _selectedItem, (ListMenuItemVm)value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }

        public SettingsVm() : this((Application.Current as App)?.Database) { }

        public SettingsVm(IDatabase database)
        {
            var menuItems = new ObservableCollection<ListMenuItemVm>
            {
                new ListMenuItemVm
                {
                    Title = "New",
                    Group = "Application",
                    SymbolIcon = Symbol.Add,
                    PageType = typeof(SettingsNewDatabasePage),
                    IsSelected = true
                },
                new ListMenuItemVm
                {
                    Title = "General",
                    Group = "Database",
                    SymbolIcon = Symbol.Setting,
                    PageType = typeof(SettingsDatabasePage),
                    IsEnabled = database?.Status == 2
                },
                new ListMenuItemVm
                {
                    Title = "Security",
                    Group = "Database",
                    SymbolIcon = Symbol.Permissions,
                    PageType = typeof(SettingsSecurityPage),
                    IsEnabled = database?.Status == 2
                }
            };
            SelectedItem = menuItems.FirstOrDefault(m => m.IsSelected);

            MenuItems = from item in menuItems group item by item.Group into grp orderby grp.Key select grp;
        }
    }
}
