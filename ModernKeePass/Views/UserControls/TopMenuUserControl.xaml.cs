using System.Windows.Input;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class TopMenuUserControl
    {
        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.Register(
                "SaveCommand",
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
                "EditCommand",
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
                "DeleteCommand",
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public ICommand RestoreCommand
        {
            get { return (ICommand)GetValue(RestoreCommandProperty); }
            set { SetValue(RestoreCommandProperty, value); }
        }
        public static readonly DependencyProperty RestoreCommandProperty =
            DependencyProperty.Register(
                "RestoreCommand",
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
                "SortEntriesCommand",
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
                "SortGroupsCommand",
                typeof(ICommand),
                typeof(TopMenuUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public Visibility RestoreButtonVisibility
        {
            get { return (Visibility)GetValue(RestoreButtonVisibilityProperty); }
            set { SetValue(RestoreButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty RestoreButtonVisibilityProperty =
            DependencyProperty.Register(
                "RestoreButtonVisibility",
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public Visibility DeleteButtonVisibility
        {
            get { return (Visibility)GetValue(DeleteButtonVisibilityProperty); }
            set { SetValue(DeleteButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty DeleteButtonVisibilityProperty =
            DependencyProperty.Register(
                "DeleteButtonVisibility",
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public Visibility MoreButtonVisibility
        {
            get { return (Visibility)GetValue(MoreButtonVisibilityProperty); }
            set { SetValue(MoreButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MoreButtonVisibilityProperty =
            DependencyProperty.Register(
                "MoreButtonVisibility",
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public Visibility OverflowButtonsVisibility
        {
            get { return (Visibility)GetValue(OverflowButtonsVisibilityProperty); }
            set { SetValue(OverflowButtonsVisibilityProperty, value); }
        }
        public static readonly DependencyProperty OverflowButtonsVisibilityProperty =
            DependencyProperty.Register(
                "OverflowButtonsVisibility",
                typeof(Visibility),
                typeof(TopMenuUserControl),
                new PropertyMetadata(Visibility.Collapsed, (o, args) => { }));

        public Visibility SortButtonVisibility
        {
            get { return (Visibility)GetValue(SortButtonVisibilityProperty); }
            set { SetValue(SortButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty SortButtonVisibilityProperty =
            DependencyProperty.Register(
                "SortButtonVisibility",
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
                "IsDeleteButtonEnabled",
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
                "IsEditButtonChecked",
                typeof(bool),
                typeof(TopMenuUserControl),
                new PropertyMetadata(false, (o, args) => { }));

        public bool IsRestoreButtonEnabled
        {
            get { return (bool)GetValue(IsRestoreButtonEnabledProperty); }
            set { SetValue(IsRestoreButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsRestoreButtonEnabledProperty =
            DependencyProperty.Register(
                "IsRestoreButtonEnabled",
                typeof(bool),
                typeof(TopMenuUserControl),
                new PropertyMetadata(false, (o, args) => { }));
        
        public event EditButtonClickEventHandler EditButtonClick;
        public delegate void EditButtonClickEventHandler(object sender, RoutedEventArgs e);
        public event DeleteButtonClickEventHandler DeleteButtonClick;
        public delegate void DeleteButtonClickEventHandler(object sender, RoutedEventArgs e);
        public event RestoreButtonClickEventHandler RestoreButtonClick;
        public delegate void RestoreButtonClickEventHandler(object sender, RoutedEventArgs e);

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

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            RestoreButtonClick?.Invoke(sender, e);
        }

        private void FlyoutBase_OnOpening(object sender, object e)
        {
            DeleteFlyout.IsEnabled = IsDeleteButtonEnabled;
            DeleteFlyout.Visibility = DeleteButtonVisibility;

            EditFlyout.IsChecked = IsEditButtonChecked;

            RestoreFlyout.IsEnabled = IsRestoreButtonEnabled;
            RestoreFlyout.Visibility = RestoreButtonVisibility;

            SortEntriesFlyout.Visibility = SortButtonVisibility;
            SortGroupsFlyout.Visibility = SortButtonVisibility;
        }
    }
}
