using System;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModernKeePass.Common;
using ModernKeePass.Events;

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
            StatusTextBlock.Text = app.Database.Open(PasswordCheckBox.IsChecked.HasValue && PasswordCheckBox.IsChecked.Value ? PasswordBox.Password : null, CreateNew);
            if (app.Database.Status == DatabaseHelper.DatabaseStatus.Opened)
            {
                ValidationChecked?.Invoke(this, new PasswordEventArgs(app.Database.RootGroup));
            }
            else
            {
                VisualStateManager.GoToState(PasswordBox, "Error", true);
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) OpenButton_OnClick(null, null);
            else
            {
                VisualStateManager.GoToState(PasswordBox, "Normal", true);
                StatusTextBlock.Text = string.Empty;
            }
        }

        private async void KeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker =
                new FileOpenPicker
                {
                    ViewMode = PickerViewMode.List,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary
                };
            picker.FileTypeFilter.Add(".key");

            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            var app = (App)Application.Current;
            app.Database.KeyFile = file;
            StatusTextBlock.Text = $"Key file: {file.Name}";
        }
    }
}
