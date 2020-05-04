using Windows.UI.Xaml;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class SetCredentialsUserControl
    {
        public string ButtonLabel
        {
            get { return (string)GetValue(ButtonLabelProperty); }
            set { SetValue(ButtonLabelProperty, value); }
        }
        public static readonly DependencyProperty ButtonLabelProperty =
            DependencyProperty.Register(
                nameof(ButtonLabel),
                typeof(string),
                typeof(SetCredentialsUserControl),
                new PropertyMetadata("OK", (o, args) => { }));
        
        public SetCredentialsUserControl()
        {
            InitializeComponent();
        }
    }
}
