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
            set
            {
                SetValue(VisualStateProperty, value);
                VisualStateManager.GoToState(this, value, true);
            }
        }
        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.Register(
                "VisualState",
                typeof(string),
                typeof(TopMenuUserControl),
                new PropertyMetadata("Large", (o, args) => { }));
        
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
        
        public bool ShowRestoreButton
        {
            get { return (bool)GetValue(ShowRestoreButtonProperty); }
            set { SetValue(ShowRestoreButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowRestoreButtonProperty =
            DependencyProperty.Register(
                "ShowRestoreButton",
                typeof(bool),
                typeof(TopMenuUserControl),
                new PropertyMetadata(false, (o, args) => { }));


        public TopMenuUserControl()
        {
            InitializeComponent();
        }
    }
}
