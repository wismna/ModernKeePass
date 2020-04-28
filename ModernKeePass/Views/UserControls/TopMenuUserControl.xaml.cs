using System;
using System.Linq;
using System.Windows.Input;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Command;
using ModernKeePass.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class TopMenuUserControl
    {
        public TopMenuVm Model => (TopMenuVm)StackPanel.DataContext;

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register(
                nameof(SaveCommand),
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }
        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register(
                nameof(EditCommand),
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(
                nameof(DeleteCommand),
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public RelayCommand<string> MoveCommand
        {
            get { return (RelayCommand<string>)GetValue(MoveCommandProperty); }
            set { SetValue(MoveCommandProperty, value); }
        }
        public static readonly DependencyProperty MoveCommandProperty =
            DependencyProperty.Register(
                nameof(MoveCommand),
                typeof(RelayCommand<string>),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));
        
        public ICommand RestoreCommand
        {
            get { return (ICommand)GetValue(RestoreCommandProperty); }
            set { SetValue(RestoreCommandProperty, value); }
        }
        public static readonly DependencyProperty RestoreCommandProperty =
            DependencyProperty.Register(
                nameof(RestoreCommand),
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public ICommand SortEntriesCommand
        {
            get { return (ICommand)GetValue(SortEntriesCommandProperty); }
            set { SetValue(SortEntriesCommandProperty, value); }
        }
        public static readonly DependencyProperty SortEntriesCommandProperty =
            DependencyProperty.Register(
                nameof(SortEntriesCommand),
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));
        
        public ICommand SortGroupsCommand
        {
            get { return (ICommand)GetValue(SortGroupsCommandProperty); }
            set { SetValue(SortGroupsCommandProperty, value); }
        }
        public static readonly DependencyProperty SortGroupsCommandProperty =
            DependencyProperty.Register(
                nameof(SortGroupsCommand),
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));
        
        public Visibility SortButtonVisibility
        {
            get { return (Visibility)GetValue(SortButtonVisibilityProperty); }
            set { SetValue(SortButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty SortButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(SortButtonVisibility),
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public Visibility MoveButtonVisibility
        {
            get { return (Visibility)GetValue(MoveButtonVisibilityProperty); }
            set { SetValue(MoveButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MoveButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(MoveButtonVisibility),
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Visible, (o, args) => { }));

        public Visibility RestoreButtonVisibility
        {
            get { return (Visibility)GetValue(RestoreButtonVisibilityProperty); }
            set { SetValue(RestoreButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty RestoreButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(RestoreButtonVisibility),
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public bool IsDeleteButtonEnabled
        {
            get { return (bool)GetValue(IsDeleteButtonEnabledProperty); }
            set { SetValue(IsDeleteButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsDeleteButtonEnabledProperty =
            DependencyProperty.Register(
                nameof(IsDeleteButtonEnabled),
                typeof(bool),
                typeof(TopMenuUserControl),
                new PropertyMetadata(true, (o, args) => { }));
        
        public bool IsEditButtonChecked
        {
            get { return (bool)GetValue(IsEditButtonCheckedProperty); }
            set { SetValue(IsEditButtonCheckedProperty, value); }
        }
        public static readonly DependencyProperty IsEditButtonCheckedProperty =
            DependencyProperty.Register(
                nameof(IsEditButtonChecked),
                typeof(bool),
                typeof(TopMenuUserControl),
                new PropertyMetadata(false, (o, args) => { }));
        
        public event EventHandler<RoutedEventArgs> EditButtonClick;
        public event EventHandler<RoutedEventArgs> DeleteButtonClick;
        
        public TopMenuUserControl()
        {
            InitializeComponent();
            EditFlyout.Click += EditFlyout_Click;
        }

        private void EditFlyout_Click(object sender, RoutedEventArgs e)
        {
            IsEditButtonChecked = EditFlyout.IsChecked;
            EditButton_Click(sender, e);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditButtonClick?.Invoke(sender, e);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteButtonClick?.Invoke(sender, e);
        }

        private void OverflowFlyout_OnOpening(object sender, object e)
        {
            DeleteFlyout.IsEnabled = IsDeleteButtonEnabled;
            DeleteFlyout.IsEnabled = IsDeleteButtonEnabled;

            EditFlyout.IsChecked = IsEditButtonChecked;

            MoveFlyout.Visibility = MoveButtonVisibility;
            RestoreFlyout.Visibility = RestoreButtonVisibility;

            SortEntriesFlyout.Visibility = SortButtonVisibility;
            SortGroupsFlyout.Visibility = SortButtonVisibility;
            SortEntriesFlyout.Command = SortEntriesCommand;
            SortGroupsFlyout.Command = SortGroupsCommand;
        }

        private void SortFlyout_OnOpening(object sender, object e)
        {
            SortEntriesButtonFlyout.Command = SortEntriesCommand;
            SortGroupsButtonFlyout.Command = SortGroupsCommand;
        }
        
        private void SearchBox_OnSuggestionsRequested(SearchBox sender, SearchBoxSuggestionsRequestedEventArgs args)
        {
            var imageUri = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appdata://Assets/ModernKeePass-SmallLogo.scale-80.png"));
            foreach (var group in Model.Groups.Where(g => g.Title.IndexOf(args.QueryText, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                args.Request.SearchSuggestionCollection.AppendResultSuggestion(group.Title, group.ParentGroupName, group.Id, imageUri, string.Empty);
            }
        }

        private void SearchBox_OnResultSuggestionChosen(SearchBox sender, SearchBoxResultSuggestionChosenEventArgs args)
        {
            Model.SelectedDestinationGroup = args.Tag;
            MoveCommand.RaiseCanExecuteChanged();
        }
    }
}
