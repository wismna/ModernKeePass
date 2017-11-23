using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Pages;

namespace ModernKeePass.ViewModels
{
    public class MainVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> _mainMenuItems;
        private MainMenuItemVm _selectedItem;

        public string Name { get; } = Package.Current.DisplayName;

        public IOrderedEnumerable<IGrouping<int, MainMenuItemVm>> MainMenuItems
        {
            get { return _mainMenuItems; }
            set { SetProperty(ref _mainMenuItems, value); }
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

                SetProperty(ref _selectedItem, (MainMenuItemVm)value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }

        public MainVm() {}

        internal MainVm(Frame referenceFrame, Frame destinationFrame) : this(referenceFrame, destinationFrame, (Application.Current as App)?.Database) { }

        public MainVm(Frame referenceFrame, Frame destinationFrame, IDatabase database)
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var isDatabaseOpen = database != null && database.Status == (int) DatabaseHelper.DatabaseStatus.Opened;

            var mainMenuItems = new ObservableCollection<MainMenuItemVm>
            {
                new MainMenuItemVm
                {
                    Title = "Open", PageType = typeof(OpenDatabasePage), Destination = destinationFrame, Parameter = referenceFrame, SymbolIcon = Symbol.Page2,
                    IsSelected = database != null && database.Status == (int) DatabaseHelper.DatabaseStatus.Opening
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
                    IsSelected = (database == null || database.Status == (int) DatabaseHelper.DatabaseStatus.Closed) && mru.Entries.Count > 0, IsEnabled = mru.Entries.Count > 0
                },
                new MainMenuItemVm
                {
                    Title = "About" , PageType = typeof(AboutPage), Destination = destinationFrame, SymbolIcon = Symbol.Help
                }
            };
            // Auto-select the Recent Items menu item if the conditions are met
            SelectedItem = mainMenuItems.FirstOrDefault(m => m.IsSelected);
            if (database != null && database.Status == (int) DatabaseHelper.DatabaseStatus.Opened)
                mainMenuItems.Add(new MainMenuItemVm
                {
                    Title = database.Name,
                    PageType = typeof(GroupDetailPage),
                    Destination = referenceFrame,
                    Parameter = database.RootGroup,
                    Group = 1,
                    SymbolIcon = Symbol.ProtectedDocument
                });
                
            MainMenuItems = from item in mainMenuItems group item by item.Group into grp orderby grp.Key select grp;
        }
    }
}
