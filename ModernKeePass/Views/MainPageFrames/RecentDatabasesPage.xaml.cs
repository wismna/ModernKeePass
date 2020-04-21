// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Common;
using ModernKeePass.Models;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Views
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentDatabasesPage
    {
        private RecentVm Model => (RecentVm)Resources["ViewModel"];

        private readonly INavigationService _navigation;
        private readonly IMessenger _messenger;

        public RecentDatabasesPage(): this(
            App.Services.GetRequiredService<INavigationService>(),
            App.Services.GetRequiredService<IMessenger>())
        { }

        public RecentDatabasesPage(INavigationService navigation, IMessenger messenger)
        {
            _navigation = navigation;
            _messenger = messenger;
            InitializeComponent();

            messenger.Register<DatabaseOpeningMessage>(this, action => Model.UpdateAccessTime(action.Token));
            messenger.Register<DatabaseOpenedMessage>(this, NavigateToPage);
        }
        
        private void NavigateToPage(DatabaseOpenedMessage message)
        {
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = message.RootGroupId });
        }
    }
}
