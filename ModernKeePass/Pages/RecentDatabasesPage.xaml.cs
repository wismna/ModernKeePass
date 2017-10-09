using System;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace ModernKeePass.Pages
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentDatabasesPage : Page
    {
        private Frame _mainFrame;

        public RecentDatabasesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _mainFrame = e.Parameter as Frame;
        }

        private async void RecentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var recentVm = DataContext as RecentVm;
            if (recentVm.SelectedItem == null) return;
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var file = await mru.GetFileAsync(recentVm.SelectedItem.Token);

            // TODO: this closes the current opened database
            var app = (App)Application.Current;
            app.Database = new DatabaseHelper(file);
        }

        private void PasswordUserControl_PasswordChecked(object sender, EventArgs e)
        {
            var app = (App)Application.Current;
            if (app.Database.IsOpen) _mainFrame.Navigate(typeof(GroupDetailPage), app.Database.RootGroup);
        }
    }
}
