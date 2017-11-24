using System;
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.Events;
using ModernKeePass.ViewModels;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace ModernKeePass.Pages
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage
    {
        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper { get; }
        public GroupVm Model => (GroupVm)DataContext;

        public GroupDetailPage()
        {
            InitializeComponent();
            NavigationHelper = new NavigationHelper(this);
            NavigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {}

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedTo(e);

            if (e.Parameter is PasswordEventArgs)
                DataContext = ((PasswordEventArgs) e.Parameter).RootGroup;
            else if (e.Parameter is GroupVm)
                DataContext = (GroupVm) e.Parameter;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        
        #region Event Handlers

        private void groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupVm group;
            switch (LeftListView.SelectedIndex)
            {
                case -1:
                    return;
                case 0:
                    group = Model.AddNewGroup();
                    break;
                default:
                    group = LeftListView.SelectedItem as GroupVm;
                    break;
            }
            Frame.Navigate(typeof(GroupDetailPage), group);
        }
        
        private void entries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EntryVm entry;
            switch (GridView.SelectedIndex)
            {
                case -1:
                    return;
                case 0:
                    entry = Model.AddNewEntry();
                    break;
                default:
                    entry = GridView.SelectedItem as EntryVm;
                    break;
            }
            Frame.Navigate(typeof(EntryDetailPage), entry);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var isRecycleBinEnabled = ((App)Application.Current).Database.RecycleBinEnabled && !Model.IsSelected && !Model.ParentGroup.IsSelected;
            var message = isRecycleBinEnabled
                ? "Are you sure you want to send the whole group and all its entries to the recycle bin?"
                : "Are you sure you want to delete the whole group and all its entries?";
            var text = isRecycleBinEnabled ? "Item moved to the Recycle bin" : "Item permanently removed";
            MessageDialogHelper.ShowActionDialog("Warning", message, "Delete", "Cancel", a =>
            {
                ToastNotificationHelper.ShowMovedToast(Model, "Deleting", text);
                Model.MarkForDelete();
                if (Frame.CanGoBack) Frame.GoBack();
            });
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            ToastNotificationHelper.ShowMovedToast(Model, "Restored", "Item returned to its original group");
            if (Frame.CanGoBack) Frame.GoBack();
        }

        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            // We need to synchronize the two lists (zoomed-in and zoomed-out) because the source is different
            if (e.IsSourceZoomedInView == false)
            {
                e.DestinationItem.Item = e.SourceItem.Item;
            }
        }

        private void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var imageUri = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx://Assets/Logo.scale-80.png"));
            var results = Model.Entries.Skip(1).Where(e => e.Name.IndexOf(args.QueryText, StringComparison.OrdinalIgnoreCase) >= 0).Take(5);
            foreach (var result in results)
            {
                args.Request.SearchSuggestionCollection.AppendResultSuggestion(result.Name, result.ParentGroup.Name, result.Id, imageUri, string.Empty);
            }
        }

        private void SearchBox_OnResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            var entry = Model.Entries.Skip(1).FirstOrDefault(e => e.Id == args.Tag);
            Frame.Navigate(typeof(EntryDetailPage), entry);
        }

        #endregion
    }
}
