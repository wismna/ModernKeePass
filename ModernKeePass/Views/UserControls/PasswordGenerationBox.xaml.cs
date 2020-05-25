using Windows.UI.Xaml;
using ModernKeePass.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class PasswordGenerationBox
    {
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordGenerationBox),
                new PropertyMetadata(string.Empty, (o, args) =>
                {
                    var passwordGenerationBox = o as PasswordGenerationBox;
                    var vm = passwordGenerationBox?.StackPanel.DataContext as PasswordGenerationBoxControlVm;
                    if (vm != null) vm.Password = args.NewValue.ToString();
                }));

        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(PasswordGenerationBox),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public PasswordGenerationBox()
        {
            InitializeComponent();
        }
    }
}
