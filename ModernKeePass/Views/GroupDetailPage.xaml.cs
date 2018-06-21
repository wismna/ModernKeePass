using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Common;
using ModernKeePass.Events;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace ModernKeePass.Views
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
        }

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

            var args = e.Parameter as PasswordEventArgs;
            if (args != null)
                DataContext = args.RootGroup;
            else
            {
                var vm = e.Parameter as GroupVm;
                if (vm != null)
                    DataContext = vm;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            NavigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        
        #region Event Handlers

        private void groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            GroupVm group;
            switch (listView?.SelectedIndex)
            {
                case -1:
                    return;
                default:
                    group = listView?.SelectedItem as GroupVm;
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
                default:
                    entry = GridView.SelectedItem as EntryVm;
                    break;
            }
            Frame.Navigate(typeof(EntryDetailPage), entry);
        }
        
        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            // We need to synchronize the two lists (zoomed-in and zoomed-out) because the source is different
            if (!e.IsSourceZoomedInView)
            {
                e.DestinationItem.Item = e.SourceItem.Item;
            }
        }
        private void CreateEntry_ButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EntryDetailPage), Model.AddNewEntry());
        }
        private void CreateGroup_ButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupDetailPage), Model.AddNewGroup());
        }

        private void GridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Cancel = !Model.IsEditMode;
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }

        private void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var imageUri = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appdata://Assets/ModernKeePass-SmallLogo.scale-80.png"));
            var results = Model.SubEntries.Where(e => e.Name.IndexOf(args.QueryText, StringComparison.OrdinalIgnoreCase) >= 0).Take(5);
            foreach (var result in results)
            {
                args.Request.SearchSuggestionCollection.AppendResultSuggestion(result.Name, result.ParentGroup.Name, result.Id, imageUri, string.Empty);
            }
        }

        private void SearchBox_OnResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            var entry = Model.SubEntries.FirstOrDefault(e => e.Id == args.Tag);
            Frame.Navigate(typeof(EntryDetailPage), entry);
        }

        private void GroupDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewSize.Width < 700 ? "Small" : "Large", true);
        }

        #endregion
    }
}
