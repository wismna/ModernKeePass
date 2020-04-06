using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Core;
using ModernKeePass.ViewModels;
using ModernKeePass.Views.SettingsPageFrames;

namespace ModernKeePass.Views
{
    public partial class SettingsPage10
    {
        private SettingsViewModel ViewModel { get; }

        private readonly IList<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("welcome", typeof(SettingsWelcomePage)),
            ("new", typeof(SettingsNewDatabasePage)),
            ("save", typeof(SettingsSavePage)),
            ("general", typeof(SettingsDatabasePage)),
            ("security", typeof(SettingsSecurityPage))
        };

        public SettingsPage10()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();
            SetTitleBar();
        }

        private void SetTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(AppTitleBar);
        }

        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            // Getting the Tag from Content (args.InvokedItem is the content of NavigationViewItem)
            var navItem = NavigationView.MenuItems
                .OfType<NavigationViewItem>()
                .First(i => args.InvokedItem.Equals(i.Content));

            NavigationView_Navigate(navItem);
        }

        private void NavigationView_Navigate(NavigationViewItem navItem)
        {
            var item = _pages.First(p => p.Tag.Equals(navItem.Tag));
            NavigationView.Header = navItem.Content;
            ContentFrame.Navigate(item.Page);
        }

        private void NavigationView_OnLoaded(object sender, RoutedEventArgs e)
        {
            NavigationView_Navigate(Welcome);
            NavigationView.IsBackEnabled = Frame.CanGoBack;
        }

        private void NavigationView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            Frame.GoBack();
        }
    }
}