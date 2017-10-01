using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ModernKeePass.Events;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Controls
{
    public sealed partial class PasswordUserControl : UserControl
    {
        public PasswordUserControl()
        {
            InitializeComponent();
            PasswordBox.Focus(FocusState.Programmatic);
        }

        public delegate void PasswordCheckedEventHandler(object sender, DatabaseEventArgs e);
        public event PasswordCheckedEventHandler PasswordChecked;

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            var app = (App)Application.Current;
            StatusTextBlock.Text = app.Database.Open(PasswordBox.Password);
            PasswordChecked(this, new DatabaseEventArgs { IsOpen = app.Database.IsOpen });
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) OpenButton_OnClick(null, null);
        }
    }
}
