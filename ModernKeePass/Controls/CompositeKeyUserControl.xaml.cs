using System;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModernKeePass.Events;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Controls
{
    public sealed partial class CompositeKeyUserControl
    {
        public CompositeKeyVm Model => Grid.DataContext as CompositeKeyVm;

        public bool CreateNew
        {
            get { return (bool)GetValue(CreateNewProperty); }
            set { SetValue(CreateNewProperty, value); }
        }
        public static readonly DependencyProperty CreateNewProperty =
            DependencyProperty.Register(
                "CreateNew",
                typeof(bool),
                typeof(CompositeKeyUserControl),
                new PropertyMetadata(false, (o, args) => { }));

        public bool UpdateKey
        {
            get { return (bool)GetValue(UpdateKeyProperty); }
            set { SetValue(UpdateKeyProperty, value); }
        }
        public static readonly DependencyProperty UpdateKeyProperty =
            DependencyProperty.Register(
                "UpdateKey",
                typeof(bool),
                typeof(CompositeKeyUserControl),
                new PropertyMetadata(false, (o, args) => { }));

        public bool ShowComplexityIndicator => CreateNew || UpdateKey;

        public CompositeKeyUserControl()
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

            if (UpdateKey) Model.UpdateKey();
            else if (Model.OpenDatabase(CreateNew))
            {
                ValidationChecked?.Invoke(this, new PasswordEventArgs(Model.RootGroup));
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) OpenButton_OnClick(null, null);
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
            Model.KeyFile = file;
        }

        private async void CreateKeyFileButton_Click(object sender, RoutedEventArgs e)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = "Key"
            };
            savePicker.FileTypeChoices.Add("Key file", new List<string> { ".key" });

            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;
            
            Model.CreateKeyFile(file);
        }
    }
}
