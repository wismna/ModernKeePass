using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Models;
using ModernKeePass.ViewModels.ListItems;
using ModernKeePass.Views;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.ViewModels
{
    public class MainVm : ViewModelBase, IHasSelectableObject
    {
        private readonly IMediator _mediator;
        private readonly IRecentProxy _recent;
        private readonly IResourceProxy _resource;
        private readonly IDialogService _dialog;
        private readonly INotificationService _notification;
        private readonly INavigationService _navigation;
        private IOrderedEnumerable<IGrouping<string, MainMenuItemVm>> _mainMenuItems;
        private ISelectableModel _selectedItem;

        public string Name { get; } = Package.Current.DisplayName;

        public IOrderedEnumerable<IGrouping<string, MainMenuItemVm>> MainMenuItems
        {
            get { return _mainMenuItems; }
            set { Set(() => MainMenuItems, ref _mainMenuItems, value); }
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

        public MainVm(
            IMediator mediator, 
            IRecentProxy recent, 
            IResourceProxy resource, 
            IDialogService dialog,
            INotificationService notification,
            INavigationService navigation)
        {
            _mediator = mediator;
            _recent = recent;
            _resource = resource;
            _dialog = dialog;
            _notification = notification;
            _navigation = navigation;
            
            MessengerInstance.Register<DatabaseOpenedMessage>(this, NavigateToPage);
            MessengerInstance.Register<DatabaseAlreadyOpenedMessage>(this, async message => await DisplaySaveConfirmation(message));
        }

        public async Task Initialize(Frame referenceFrame, Frame destinationFrame, FileInfo databaseFile = null)
        {
            var database = await _mediator.Send(new GetDatabaseQuery());
            var mainMenuItems = new ObservableCollection<MainMenuItemVm>
            {
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemOpen"),
                    PageType = typeof(OpenDatabasePage),
                    Destination = destinationFrame,
                    Parameter = databaseFile,
                    SymbolIcon = Symbol.Page2,
                    IsSelected = databaseFile != null
                },
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemNew"),
                    PageType = typeof(NewDatabasePage),
                    Destination = destinationFrame,
                    SymbolIcon = Symbol.Add
                },
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemSave"),
                    PageType = typeof(SaveDatabasePage),
                    Destination = destinationFrame,
                    Parameter = referenceFrame,
                    SymbolIcon = Symbol.Save,
                    IsSelected = database.IsOpen,
                    IsEnabled = database.IsOpen
                },
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemRecent"),
                    PageType = typeof(RecentDatabasesPage),
                    Destination = destinationFrame,
                    Parameter = referenceFrame,
                    SymbolIcon = Symbol.Copy,
                    IsSelected = !database.IsOpen && _recent.EntryCount > 0,
                    IsEnabled = _recent.EntryCount > 0
                },
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemSettings"),
                    PageType = typeof(SettingsPage),
                    Destination = referenceFrame,
                    SymbolIcon = Symbol.Setting
                },
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemAbout"),
                    PageType = typeof(AboutPage),
                    Destination = destinationFrame,
                    SymbolIcon = Symbol.Help
                },
                new MainMenuItemVm
                {
                    Title = _resource.GetResourceValue("MainMenuItemDonate"),
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
                    Parameter = new NavigationItem { Id = database.RootGroupId },
                    Group = "Databases",
                    SymbolIcon = Symbol.ProtectedDocument
                });

            MainMenuItems = from item in mainMenuItems group item by item.Group into grp orderby grp.Key select grp;
        }

        private void NavigateToPage(DatabaseOpenedMessage message)
        {
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = message.RootGroupId });
        }

        private async Task DisplaySaveConfirmation(DatabaseAlreadyOpenedMessage message)
        {
            var database = await _mediator.Send(new GetDatabaseQuery());
            await _dialog.ShowMessage(string.Format(_resource.GetResourceValue("MessageDialogDBOpenDesc"), database.Name),
                _resource.GetResourceValue("MessageDialogDBOpenTitle"),
                _resource.GetResourceValue("MessageDialogDBOpenButtonSave"),
                _resource.GetResourceValue("MessageDialogDBOpenButtonDiscard"),
                async isOk =>
                {
                    if (isOk)
                    {
                        try
                        {
                            await _mediator.Send(new SaveDatabaseCommand());
                            _notification.Show(database.Name, _resource.GetResourceValue("ToastSavedMessage"));
                        }
                        catch (SaveException exception)
                        {
                            _notification.Show(exception.Source, exception.Message);
                            return;
                        }
                    }

                    await _mediator.Send(new CloseDatabaseCommand());
                    MessengerInstance.Send(new DatabaseClosedMessage { Parameter = message.Parameter });
                });
        }
    }
}
