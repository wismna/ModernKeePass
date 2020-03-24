using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using ModernKeePass.Common;
using ModernKeePass.Events;
using ModernKeePass.Extensions;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
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

        public string ButtonLabel
        {
            get { return (string)GetValue(ButtonLabelProperty); }
            set { SetValue(ButtonLabelProperty, value); }
        }
        public static readonly DependencyProperty ButtonLabelProperty =
            DependencyProperty.Register(
                "ButtonLabel",
                typeof(string),
                typeof(CompositeKeyUserControl),
                new PropertyMetadata("OK", (o, args) => { }));

        public StorageFile DatabaseFile
        {
            get { return (StorageFile)GetValue(DatabaseFileProperty); }
            set { SetValue(DatabaseFileProperty, value); }
        }
        public static readonly DependencyProperty DatabaseFileProperty =
            DependencyProperty.Register(
                "DatabaseFile",
                typeof(StorageFile),
                typeof(CompositeKeyUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public bool ShowComplexityIndicator => CreateNew || UpdateKey;

        public CompositeKeyUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler ValidationChecking;
        public event EventHandler<PasswordEventArgs> ValidationChecked;

        private async void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            ValidationChecking?.Invoke(this, new EventArgs());

            if (UpdateKey)
            {
                await Model.UpdateKey();
                ValidationChecked?.Invoke(this, new PasswordEventArgs(Model.RootGroup));
            }
            else
            {
                var database = DatabaseService.Instance;
                var resource = new ResourcesService();
                if (database.IsOpen)
                {
                    await MessageDialogHelper.ShowActionDialog(resource.GetResourceValue("MessageDialogDBOpenTitle"),
                        string.Format(resource.GetResourceValue("MessageDialogDBOpenDesc"), database.Name),
                        resource.GetResourceValue("MessageDialogDBOpenButtonSave"),
                        resource.GetResourceValue("MessageDialogDBOpenButtonDiscard"),
                        async command =>
                        {
                            database.Save();
                            ToastNotificationHelper.ShowGenericToast(
                                database.Name,
                                resource.GetResourceValue("ToastSavedMessage"));
                            database.Close(false);
                            await OpenDatabase(resource);
                        },
                        async command =>
                        {
                            database.Close(false);
                            await OpenDatabase(resource);
                        });
                }
                else
                {
                    await OpenDatabase(resource);
                }
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter && Model.IsValid)
            {
                OpenButton_OnClick(sender, e);
                // Stop the event from triggering twice
                e.Handled = true;
            }
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
            
            await Model.CreateKeyFile(file);
        }

        private async Task OpenDatabase(IResourceService resource)
        {
            var oldLabel = ButtonLabel;
            ButtonLabel = resource.GetResourceValue("CompositeKeyOpening");
            if (await Dispatcher.RunTaskAsync(async () => await Model.OpenDatabase(DatabaseFile, CreateNew)))
            {
                ValidationChecked?.Invoke(this, new PasswordEventArgs(Model.RootGroup));
            }

            ButtonLabel = oldLabel;
        }
    }
}
