using System;
using System.Collections.ObjectModel;
using System.Linq;
using ModernKeePass.Common;
using ModernKeePass.Models;
using ModernKeePass.ViewModels;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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
            //RecentListView.SelectedIndex = -1;
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var recentVm = DataContext as RecentVm;
            recentVm.RecentItems = new ObservableCollection<RecentItem>(
                from entry in mru.Entries
                select new RecentItem() { Name = entry.Metadata, Token = entry.Token });
            recentVm.NotifyPropertyChanged("RecentItems");
        }

        private async void RecentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (RecentListView == null || e.RemovedItems.Count > 0) return;
            var recentItem = e.AddedItems[0] as RecentItem;
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            var file = await mru.GetFileAsync(recentItem.Token) as StorageFile;

            var app = (App)Application.Current;
            app.Database = new DatabaseHelper(file);
            app.Database.Open("test");
            _mainFrame.Navigate(typeof(GroupDetailPage), app.Database.RootGroup);
        }
    }
}
