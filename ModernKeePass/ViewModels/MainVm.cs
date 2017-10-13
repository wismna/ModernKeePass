using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Pages;

namespace ModernKeePass.ViewModels
{
    public class MainVm : NotifyPropertyChangedBase
    {
        private IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> _mainMenuItems;
        private MainMenuItemVm _selectedItem;

        public IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> MainMenuItems
        {
            get { return _mainMenuItems; }
            set { SetProperty(ref _mainMenuItems, value); }
        }

        public MainMenuItemVm SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                SetProperty(ref _selectedItem, value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }

        public MainVm() {}

        public MainVm(Frame referenceFrame, Frame destinationFrame)
        {
            var app = (App)Application.Current;
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var isDatabaseOpen = app.Database != null && app.Database.Status == DatabaseHelper.DatabaseStatus.Opened;

            var mainMenuItems = new ObservableCollection<MainMenuItemVm>
            {
                new MainMenuItemVm
                {
                    Title = "Open", PageType = typeof(OpenDatabasePage), Destination = destinationFrame, Parameter = referenceFrame, SymbolIcon = Symbol.Page2,
                    IsSelected = app.Database.Status == DatabaseHelper.DatabaseStatus.Opening
                },
                new MainMenuItemVm
                {
                    Title = "New" , PageType = typeof(NewDatabasePage), Destination = destinationFrame, Parameter = referenceFrame, SymbolIcon = Symbol.Add
                },
                new MainMenuItemVm
                {
                    Title = "Save" , PageType = typeof(SaveDatabasePage), Destination = destinationFrame, Parameter = referenceFrame, SymbolIcon = Symbol.Save,
                    IsSelected = isDatabaseOpen, IsEnabled = isDatabaseOpen
                },
                new MainMenuItemVm {
                    Title = "Recent" , PageType = typeof(RecentDatabasesPage), Destination = destinationFrame, Parameter = referenceFrame, SymbolIcon = Symbol.Copy,
                    IsSelected = (app.Database == null || app.Database.Status == DatabaseHelper.DatabaseStatus.Closed) && mru.Entries.Count > 0, IsEnabled = mru.Entries.Count > 0
                }
            };
            // Auto-select the Recent Items menu item if the conditions are met
            SelectedItem = mainMenuItems.FirstOrDefault(m => m.IsSelected);
            if (app.Database != null && app.Database.Status == DatabaseHelper.DatabaseStatus.Opened)
                mainMenuItems.Add(new MainMenuItemVm
                {
                    Title = app.Database.Name,
                    PageType = typeof(GroupDetailPage),
                    Destination = referenceFrame,
                    Parameter = app.Database.RootGroup,
                    Group = 1,
                    SymbolIcon = Symbol.ProtectedDocument
                });
                
            MainMenuItems = from item in mainMenuItems group item by item.Group into grp orderby grp.Key select grp;
        }
    }
}
