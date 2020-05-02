using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
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

        private async void KeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add("*");

            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;

            var token = StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            Model.KeyFilePath = token;
            Model.KeyFileText = file.Name;
            Model.HasKeyFile = true;
        }
    }
}
