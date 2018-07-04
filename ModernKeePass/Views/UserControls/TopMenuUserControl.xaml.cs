using System.Windows.Input;
using Windows.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class TopMenuUserControl
    {
        public string VisualState
        {
            get { return (string)GetValue(VisualStateProperty); }
            set { SetValue(VisualStateProperty, value); }
        }
        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.Register(
                "VisualState",
                typeof(string),
                typeof(TopMenuUserControl),
                new PropertyMetadata("None", (o, args) => { }));
        
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
                new PropertyMetadata(false, (o, args) => { }));

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
                new PropertyMetadata(false, (o, args) => { }));

        public bool EnableDeleteButton
        {
            get { return (bool)GetValue(EnableDeleteButtonProperty); }
            set { SetValue(EnableDeleteButtonProperty, value); }
        }
        public static readonly DependencyProperty EnableDeleteButtonProperty =
            DependencyProperty.Register(
                "EnableDeleteButton",
                typeof(bool),
                typeof(TopMenuUserControl),
                new PropertyMetadata(true, (o, args) => { }));

        public TopMenuUserControl()
        {
            InitializeComponent();
        }
    }
}
