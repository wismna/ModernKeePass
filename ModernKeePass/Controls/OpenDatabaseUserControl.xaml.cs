using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModernKeePass.Common;
using ModernKeePass.Events;
using ModernKeePassLib.Cryptography;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Controls
{
    public sealed partial class OpenDatabaseUserControl
    {
        public bool CreateNew
        {
            get { return (bool)GetValue(CreateNewProperty); }
            set { SetValue(CreateNewProperty, value); }
        }
        public static readonly DependencyProperty CreateNewProperty =
            DependencyProperty.Register(
                "CreateNew",
                typeof(bool),
                typeof(OpenDatabaseUserControl),
                new PropertyMetadata(false, (o, args) => { }));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(OpenDatabaseUserControl),
                new PropertyMetadata(string.Empty, (o, args) => { }));

        public OpenDatabaseUserControl()
        {
            InitializeComponent();
        }

        public event PasswordCheckingEventHandler ValidationChecking;
        public delegate void PasswordCheckingEventHandler(object sender, EventArgs e);
        public event PasswordCheckedEventHandler ValidationChecked;
        public delegate void PasswordCheckedEventHandler(object sender, PasswordEventArgs e);

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            ValidationChecking?.Invoke(this, new EventArgs());
            var app = (App)Application.Current;
            StatusTextBlock.Text = app.Database.Open(PasswordBox.Password, CreateNew);
            if (app.Database.Status == DatabaseHelper.DatabaseStatus.Opened)
                ValidationChecked?.Invoke(this, new PasswordEventArgs(app.Database.RootGroup));
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) OpenButton_OnClick(null, null);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(
                () => Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                        () => PasswordBox.Focus(FocusState.Programmatic)));
        }
    }
}
