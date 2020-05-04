using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class OpenDatabaseUserControl
    {
        private OpenDatabaseControlVm Model => (OpenDatabaseControlVm)Grid.DataContext;

        public string DatabaseFilePath
        {
            get { return (string)GetValue(DatabaseFilePathProperty); }
            set { SetValue(DatabaseFilePathProperty, value); }
        }
        public static readonly DependencyProperty DatabaseFilePathProperty =
            DependencyProperty.Register(
                nameof(DatabaseFilePath),
                typeof(string),
                typeof(OpenDatabaseUserControl),
                new PropertyMetadata(null, (o, args) => { }));
        
        public OpenDatabaseUserControl()
        {
            InitializeComponent();
        }

        private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || !Model.IsValid) return;
            await Model.TryOpenDatabase(DatabaseFilePath);
            // Stop the event from triggering twice
            e.Handled = true;
        }
    }
}
