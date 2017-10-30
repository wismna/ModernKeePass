using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Pages;

namespace ModernKeePass.ViewModels
{
    public class SettingsVM : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private ListMenuItemVm _selectedItem;

        public ObservableCollection<ListMenuItemVm> MenuItems { get; set; }
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

        public SettingsVM()
        {
            MenuItems = new ObservableCollection<ListMenuItemVm>
            {
                new ListMenuItemVm { Title = "Database", SymbolIcon = Symbol.Setting, PageType = typeof(SettingsDatabasePage), IsSelected = true },
                //new ListMenuItemVm { Title = "General", SymbolIcon = Symbol.Edit, PageType = typeof(SettingsGeneralPage) }
            };
            SelectedItem = MenuItems.FirstOrDefault(m => m.IsSelected);
        }
    }
}
