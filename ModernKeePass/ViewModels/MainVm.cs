using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using MediatR;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePass.Views;

namespace ModernKeePass.ViewModels
{
    public class MainVm : NotifyPropertyChangedBase, IHasSelectableObject
    {
        private IOrderedEnumerable<IGrouping<string, MainMenuItemVm>> _mainMenuItems;
        private MainMenuItemVm _selectedItem;

        public string Name { get; } = Package.Current.DisplayName;

        public IOrderedEnumerable<IGrouping<string, MainMenuItemVm>> MainMenuItems
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

        internal MainVm(Frame referenceFrame, Frame destinationFrame, StorageFile databaseFile = null) : this(referenceFrame, destinationFrame,
            App.Mediator, new ResourcesService(), RecentService.Instance, databaseFile)
        { }

        public MainVm(Frame referenceFrame, Frame destinationFrame, IMediator mediator, IResourceService resource, IRecentService recent, StorageFile databaseFile = null)
        {
            var database = mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();

            var mainMenuItems = new ObservableCollection<MainMenuItemVm>
            {
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemOpen"),
                    PageType = typeof(OpenDatabasePage),
                    Destination = destinationFrame,
                    Parameter = databaseFile,
                    SymbolIcon = Symbol.Page2,
                    IsSelected = databaseFile != null && !database.IsOpen
                },
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemNew"),
                    PageType = typeof(NewDatabasePage),
                    Destination = destinationFrame,
                    Parameter = referenceFrame,
                    SymbolIcon = Symbol.Add
                },
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemSave"),
                    PageType = typeof(SaveDatabasePage),
                    Destination = destinationFrame,
                    Parameter = referenceFrame,
                    SymbolIcon = Symbol.Save,
                    IsSelected = database.IsOpen,
                    IsEnabled = database.IsOpen
                },
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemRecent"),
                    PageType = typeof(RecentDatabasesPage),
                    Destination = destinationFrame,
                    Parameter = referenceFrame,
                    SymbolIcon = Symbol.Copy,
                    IsSelected = !database.IsOpen && recent.EntryCount > 0,
                    IsEnabled = recent.EntryCount > 0
                },
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemSettings"),
                    PageType = typeof(SettingsPage),
                    Destination = referenceFrame,
                    SymbolIcon = Symbol.Setting
                },
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemAbout"),
                    PageType = typeof(AboutPage),
                    Destination = destinationFrame,
                    SymbolIcon = Symbol.Help
                },
                new MainMenuItemVm
                {
                    Title = resource.GetResourceValue("MainMenuItemDonate"),
                    PageType = typeof(DonatePage),
                    Destination = destinationFrame,
                    SymbolIcon = Symbol.Shop
                }
            };
            // Auto-select the Recent Items menu item if the conditions are met
            SelectedItem = mainMenuItems.FirstOrDefault(m => m.IsSelected);

            // Add currently opened database to the menu
            if (database.IsOpen)
                mainMenuItems.Add(new MainMenuItemVm
                {
                    Title = database.Name,
                    PageType = typeof(GroupDetailPage),
                    Destination = referenceFrame,
                    Parameter = database.RootGroupId,
                    Group = "Databases",
                    SymbolIcon = Symbol.ProtectedDocument
                });
                
            MainMenuItems = from item in mainMenuItems group item by item.Group into grp orderby grp.Key select grp;
        }
    }
}
