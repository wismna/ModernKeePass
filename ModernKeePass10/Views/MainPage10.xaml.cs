using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using MUXC = Microsoft.UI.Xaml.Controls;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Views
{
    public partial class MainPage10
    {
        private MainViewModel ViewModel { get; }

        private readonly IList<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("welcome", typeof(WelcomePage)),
            ("open", typeof(OpenDatabasePage)),
            ("new", typeof(NewDatabasePage)),
            ("save", typeof(SaveDatabasePage)),
            ("recent", typeof(RecentDatabasesPage)),
            ("about", typeof(AboutPage)),
            ("donate", typeof(DonatePage)),
            ("database", typeof(GroupsPage))
        };

        public MainPage10()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            SetTitleBar();
        }

        private void SetTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(AppTitleBar);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.File = e.Parameter as StorageFile;
        }

        private void NavigationView_OnItemInvoked(MUXC.NavigationView navigationView, MUXC.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
                Frame.Navigate(typeof(SettingsPage10));
            else
            {
                // Getting the Tag from Content (args.InvokedItem is the content of NavigationViewItem)
                var navItem = NavigationView.MenuItems
                    .OfType<MUXC.NavigationViewItem>()
                    .First(i => args.InvokedItem.Equals(i.Content));

                NavigationView_Navigate(navItem);
            }
        }

        private void NavigationView_Navigate(MUXC.NavigationViewItem navItem, object parameter = null)
        {
            var (_, page) = _pages.First(p => p.Tag.Equals(navItem.Tag));
            NavigationView.Header = navItem.Content;
            ContentFrame.Navigate(page, parameter);
        }

        private void NavigationView_OnLoaded(object sender, RoutedEventArgs e)
        {
            object parameter = null;
            if (ViewModel.IsDatabaseOpened)
            {
                NavigationView.SelectedItem = Save;
                parameter = Frame;
            }
            else if (ViewModel.File != null)
            {
                NavigationView.SelectedItem = Open;
                parameter = ViewModel.File;
            }
            else if (ViewModel.HasRecentItems)
            {
                NavigationView.SelectedItem = Recent;
            }
            else
            {
                NavigationView.SelectedItem = Welcome;
            }
            NavigationView_Navigate((MUXC.NavigationViewItem)NavigationView.SelectedItem, parameter);
        }
    }
}