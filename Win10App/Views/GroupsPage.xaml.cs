using System;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Autofac;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels.ListItems;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Views
{
    public partial class GroupsPage
    {
        private readonly IDatabaseService _databaseService;
        private readonly IResourceService _resourceService;

        public GroupsVm Vm { get; set; }

        public GroupsPage(): this(App.Container.Resolve<IDatabaseService>(), App.Container.Resolve<IResourceService>())
        { }

        public GroupsPage(IDatabaseService databaseService, IResourceService resourceService)
        {
            InitializeComponent();
            SetTitleBar();
            _databaseService = databaseService;
            _resourceService = resourceService;
        }

        private void SetTitleBar()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            //coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            coreTitleBar.ExtendViewIntoTitleBar = false;
            //UpdateTitleBarLayout(coreTitleBar);

            // Set XAML element as a draggable region.
            //Window.Current.SetTitleBar(AppTitleBar);
        }

        /*private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args) => 
            AppTitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar coreTitleBar, object args) => UpdateTitleBarLayout(coreTitleBar);

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // Get the size of the caption controls area and back button 
            // (returned in logical pixels), and move your content around as necessary.
            LeftPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayLeftInset);
            RightPaddingColumn.Width = new GridLength(coreTitleBar.SystemOverlayRightInset);
            //BackBorder.Margin = new Thickness(0, 0, coreTitleBar.SystemOverlayLeftInset, 0);
            AutoSuggestBox.Margin = new Thickness(0, 5, coreTitleBar.SystemOverlayRightInset, 5);

            // Update title bar control size as needed to account for system size changes.
            //AppTitleBar.Height = coreTitleBar.Height;
            AppTitleBar.Height = 44;
        }*/

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is GroupEntity group)) group = _databaseService.RootGroupEntity;
            Vm = new GroupsVm(group);
        }

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e)
        {
            //SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
            switch (SplitView.DisplayMode)
            {
                case SplitViewDisplayMode.Inline:
                    SplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                    break;
                case SplitViewDisplayMode.CompactInline:
                    SplitView.DisplayMode = SplitViewDisplayMode.Inline;
                    break;
            }
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void NavigationTree_OnItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem is GroupItemVm group)
            {
                Vm.Title = group.Text;
                SplitViewFrame.Navigate(typeof(EntriesPage), group);
            }
        }
        
        private void RenameFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem flyout) ((GroupItemVm)flyout.DataContext).IsEditMode = true;
        }

        private async void DeleteFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem flyout)
            {
                var item = (GroupItemVm) flyout.DataContext;

                var deleteFileDialog = new ContentDialog
                {
                    Title = $"{_resourceService.GetResourceValue("EntityDeleteActionButton")} {item.Text} ?",
                    Content = _databaseService.IsRecycleBinEnabled
                        ? _resourceService.GetResourceValue("GroupRecyclingConfirmation")
                        : _resourceService.GetResourceValue("GroupDeletingConfirmation"),
                    PrimaryButtonText = _resourceService.GetResourceValue("EntityDeleteActionButton"),
                    CloseButtonText = _resourceService.GetResourceValue("EntityDeleteCancelButton")
                };

                var result = await deleteFileDialog.ShowAsync();

                // Delete the file if the user clicked the primary button.
                // Otherwise, do nothing.
                // TODO: move this logic to the service
                if (result == ContentDialogResult.Primary)
                {
                    item.ParentVm.Children.Remove(item);
                    // TODO: refresh treeview
                }
            }
        }

        private void GroupNameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) GroupNameTextBox_OnLostFocus(sender, null);
        }

        private void GroupNameTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox) ((GroupItemVm)textBox.DataContext).IsEditMode = false;
        }

        private void NewGroupNameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                var text = ((TextBox)sender).Text;
                if (string.IsNullOrEmpty(text)) return;
                Vm.AddNewGroup(text);
                AddButton.IsEnabled = true;
                NewGroupNameTextBox.Visibility = Visibility.Collapsed;
                NewGroupNameTextBox.Text = string.Empty;
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
                sender.ItemsSource = Vm.RootItemVm.SubEntries.Where(e => e.Name.IndexOf(sender.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // Use args.QueryText to determine what to do.
            }
        }

        private void NewGroupNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewGroupNameTextBox.Text))
            {
                NewGroupNameTextBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}