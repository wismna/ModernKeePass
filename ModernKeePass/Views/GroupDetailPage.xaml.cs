using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Views;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Common;
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
        private readonly IResourceProxy _resource;
        private readonly INavigationService _navigation;
        
        public GroupDetailVm Model => (GroupDetailVm)DataContext;

        public GroupDetailPage(): this (App.Services.GetRequiredService<IResourceProxy>(), App.Services.GetRequiredService<INavigationService>()) { }
        public GroupDetailPage(IResourceProxy resource, INavigationService navigation)
        {
            InitializeComponent();
            _resource = resource;
            _navigation = navigation;
        }

        #region NavigationHelper registration
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var navigationItem = e.Parameter as NavigationItem;
            if (navigationItem != null)
                DataContext = new GroupDetailVm(navigationItem.Id) { IsEditMode = navigationItem.IsNew };
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
                    _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = group?.Id });
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
                    _navigation.NavigateTo(Constants.Navigation.EntryPage, new NavigationItem { Id = entry?.Id });
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

        private async void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var imageUri = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appdata://Assets/ModernKeePass-SmallLogo.scale-80.png"));
            var results = (await Model.Search(args.QueryText)).Take(5);
            foreach (var result in results)
            {
                args.Request.SearchSuggestionCollection.AppendResultSuggestion(result.Title, result.ParentGroupName, result.Id, imageUri, string.Empty);
            }
        }

        private void SearchBox_OnResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            _navigation.NavigateTo(Constants.Navigation.EntryPage, new NavigationItem { Id = args.Tag });
        }

        private void GroupDetailPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, e.NewSize.Width < 800 ? "Small" : "Large", true);
            VisualStateManager.GoToState(TopMenu, e.NewSize.Width < 800 ? "Collapsed" : "Overflowed", true);
        }
        
        #endregion
    }
}
