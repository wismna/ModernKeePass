using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;
using ModernKeePass.Views;
using ModernKeePass.Views.SettingsPageFrames;

namespace ModernKeePass.ViewModels
{
    public class SettingsVm : ObservableObject, IHasSelectableObject
    {
        private ISelectableModel _selectedItem;
        
        private IOrderedEnumerable<IGrouping<string, ListMenuItemVm>> _menuItems;

        public IOrderedEnumerable<IGrouping<string, ListMenuItemVm>> MenuItems
        {
            get { return _menuItems; }
            set { Set(() => MenuItems, ref _menuItems, value); }
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

                Set(() => SelectedItem, ref _selectedItem, value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }
        
        public SettingsVm(IMediator mediator, IResourceProxy resource)
        {
            var database = mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            var menuItems = new ObservableCollection<ListMenuItemVm>
            {
                new ListMenuItemVm
                {
                    Title = resource.GetResourceValue("SettingsMenuItemNew"),
                    Group = resource.GetResourceValue("SettingsMenuGroupApplication"),
                    SymbolIcon = Symbol.Add,
                    PageType = typeof(SettingsNewDatabasePage),
                    IsSelected = true
                },
                new ListMenuItemVm
                {
                    Title = resource.GetResourceValue("SettingsMenuItemGeneral"),
                    Group = resource.GetResourceValue("SettingsMenuGroupApplication"),
                    SymbolIcon = Symbol.Setting,
                    PageType = typeof(SettingsGeneralPage)
                },
                new ListMenuItemVm
                {
                    Title = resource.GetResourceValue("SettingsMenuItemSecurity"),
                    Group = resource.GetResourceValue("SettingsMenuGroupDatabase"),
                    SymbolIcon = Symbol.Permissions,
                    PageType = typeof(SettingsSecurityPage),
                    IsEnabled = database.IsOpen
                },
                new ListMenuItemVm
                {
                    Title = resource.GetResourceValue("SettingsMenuItemHistory"),
                    Group = resource.GetResourceValue("SettingsMenuGroupDatabase"),
                    SymbolIcon = Symbol.Undo,
                    PageType = typeof(SettingsHistoryPage),
                    IsEnabled = database.IsOpen
                },
                new ListMenuItemVm
                {
                    Title = resource.GetResourceValue("SettingsMenuItemRecycleBin"),
                    Group = resource.GetResourceValue("SettingsMenuGroupDatabase"),
                    SymbolIcon = Symbol.Delete,
                    PageType = typeof(SettingsRecycleBinPage),
                    IsEnabled = database.IsOpen
                },
                new ListMenuItemVm
                {
                    Title = resource.GetResourceValue("SettingsMenuItemCredentials"),
                    Group = resource.GetResourceValue("SettingsMenuGroupDatabase"),
                    SymbolIcon = Symbol.Account,
                    PageType = typeof(SettingsCredentialsPage),
                    IsEnabled = database.IsOpen
                }
            };
            SelectedItem = menuItems.FirstOrDefault(m => m.IsSelected);

            MenuItems = from item in menuItems group item by item.Group into grp orderby grp.Key select grp;
        }
    }
}
