using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Models;
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
        public GroupDetailVm Model => (GroupDetailVm)DataContext;

        public GroupDetailPage()
        {
            InitializeComponent();
        }

        #region NavigationHelper registration
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigationItem = e.Parameter as NavigationItem;
            if (navigationItem != null)
            {
                await Model.Initialize(navigationItem.Id);
                Model.IsEditMode = navigationItem.IsNew;
            }
        }
        
        #endregion
        
        #region Event Handlers

        private void groups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            switch (listView?.SelectedIndex)
            {
                case -1:
                    return;
                default:
                    var group = listView?.SelectedItem as Application.Group.Models.GroupVm;
                    Model.GoToGroup(group?.Id);
                    break;
            }
        }
        
        private void entries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (GridView.SelectedIndex)
            {
                case -1:
                    return;
                default:
                    var entry = GridView.SelectedItem as EntryVm;
                    Model.GoToEntry(entry?.Id);
                    break;
            }
        }
        
        private void SemanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            // We need to synchronize the two lists (zoomed-in and zoomed-out) because the source is different
            if (!e.IsSourceZoomedInView)
            {
                e.DestinationItem.Item = e.SourceItem.Item;
            }
        }

        private void GridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Cancel = !Model.IsEditMode;
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }
        
        private void GroupDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width <= 640)
            {
                VisualStateManager.GoToState(this, "Small", true);
                VisualStateManager.GoToState(TopMenu, "Collapsed", true);
                VisualStateManager.GoToState(HamburgerMenu, "Hidden", true);
            }
            else if (e.NewSize.Width > 640 && e.NewSize.Width <= 1008)
            {
                VisualStateManager.GoToState(this, "Medium", true);
                VisualStateManager.GoToState(TopMenu, "Overflowed", true);
                VisualStateManager.GoToState(HamburgerMenu, "Collapsed", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Large", true);
                VisualStateManager.GoToState(TopMenu, "Overflowed", true);
                VisualStateManager.GoToState(HamburgerMenu, "Expanded", true);
            }
        }
        
        #endregion
    }
}
