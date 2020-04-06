using System;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Autofac;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.ViewModels;
using ModernKeePass.ViewModels.ListItems;

// TODO: check https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/XamlListView/cs/Samples/MasterDetailSelection
namespace ModernKeePass.Views
{
    public partial class EntriesPage
    {
        private readonly IDatabaseService _databaseService;
        private readonly IResourceService _resourceService;
        public EntriesViewModel ViewModel { get; set; }

        public EntriesPage(): this(App.Container.Resolve<IDatabaseService>(), App.Container.Resolve<IResourceService>())
        { }

        public EntriesPage(IDatabaseService databaseService, IResourceService resourceService)
        {
            _databaseService = databaseService;
            _resourceService = resourceService;
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is GroupItemViewModel group) ViewModel = new EntriesViewModel(group);
        }

        private async void DeleteFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem flyout)
            {
                var item = (EntryItemViewModel)flyout.DataContext;

                var deleteFileDialog = new ContentDialog
                {
                    Title = $"{_resourceService.GetResourceValue("EntityDeleteActionButton")} {item.Name} ?",
                    Content = _databaseService.IsRecycleBinEnabled
                        ? _resourceService.GetResourceValue("EntryRecyclingConfirmation")
                        : _resourceService.GetResourceValue("EntryDeletingConfirmation"),
                    PrimaryButtonText = _resourceService.GetResourceValue("EntityDeleteActionButton"),
                    CloseButtonText = _resourceService.GetResourceValue("EntityDeleteCancelButton")
                };

                var result = await deleteFileDialog.ShowAsync();

                // Delete the file if the user clicked the primary button.
                // Otherwise, do nothing.
                // TODO: move this logic to the service
                if (result == ContentDialogResult.Primary)
                {
                    ViewModel.Entries.Remove(item);
                }
            }
        }

        private void NewEntryNameTextBox_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                var text = NewEntryNameTextBox.Text;
                if (string.IsNullOrEmpty(text)) return;
                ViewModel.AddNewEntry(text);
                AddEntryButton.IsChecked = false;
            }
            else if (e.Key == VirtualKey.Escape)
            {
                AddEntryButton.IsChecked = false;
            }
        }

        private void ColorPickerBackground_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ColorPicker colorPicker) ((EntryItemViewModel) colorPicker.DataContext).BackgroundColor = colorPicker.Color;
        }
        private void ColorPickerForeground_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is ColorPicker colorPicker) ((EntryItemViewModel) colorPicker.DataContext).ForegroundColor = colorPicker.Color;
        }

        private void HistoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}