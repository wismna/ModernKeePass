using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.Models;
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
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var recentVm = DataContext as RecentVm;
            recentVm.RecentItems = new ObservableCollection<RecentItem>(
                from entry in mru.Entries
                select new RecentItem() { Name = entry.Metadata, Token = entry.Token });
            recentVm.NotifyPropertyChanged("RecentItems");
        }

        private async void RecentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var recentItem = e.AddedItems[0] as RecentItem;
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var file = await mru.GetFileAsync(recentItem.Token) as StorageFile;

            var app = (App)Application.Current;
            app.Database = new DatabaseHelper(file);
            recentItem.PasswordVisibility = Visibility.Visible;
            recentItem.NotifyPropertyChanged("PasswordVisibility");
        }

        private void PasswordUserControl_PasswordChecked(object sender, Events.DatabaseEventArgs e)
        {
            var app = (App)Application.Current;
            if (e.IsOpen) _mainFrame.Navigate(typeof(GroupDetailPage), app.Database.RootGroup);
        }
    }
}
