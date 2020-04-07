using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Events;
using ModernKeePass.Extensions;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class CompositeKeyUserControl
    {
        private readonly IMediator _mediator;
        private readonly ResourceHelper _resource;
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

        public string DatabaseFilePath
        {
            get { return (string)GetValue(DatabaseFilePathProperty); }
            set { SetValue(DatabaseFilePathProperty, value); }
        }
        public static readonly DependencyProperty DatabaseFilePathProperty =
            DependencyProperty.Register(
                "DatabaseFilePath",
                typeof(string),
                typeof(CompositeKeyUserControl),
                new PropertyMetadata(null, (o, args) => { }));

        public bool ShowComplexityIndicator => CreateNew || UpdateKey;

        public CompositeKeyUserControl(): this(App.Services.GetService<IMediator>())
        { }

        public CompositeKeyUserControl(IMediator mediator)
        {
            _mediator = mediator;
            _resource = new ResourceHelper();
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
                ValidationChecked?.Invoke(this, new PasswordEventArgs(Model.RootGroupId));
            }
            else
            {
                var database = await _mediator.Send(new GetDatabaseQuery());
                if (database.IsOpen)
                {
                    await MessageDialogHelper.ShowActionDialog(_resource.GetResourceValue("MessageDialogDBOpenTitle"),
                        string.Format(_resource.GetResourceValue("MessageDialogDBOpenDesc"), database.Name),
                        _resource.GetResourceValue("MessageDialogDBOpenButtonSave"),
                        _resource.GetResourceValue("MessageDialogDBOpenButtonDiscard"),
                        async command =>
                        {
                            await _mediator.Send(new SaveDatabaseCommand());
                            ToastNotificationHelper.ShowGenericToast(
                                database.Name,
                                _resource.GetResourceValue("ToastSavedMessage"));
                            await _mediator.Send(new CloseDatabaseCommand());
                            await OpenDatabase();
                        },
                        async command =>
                        {
                            await _mediator.Send(new CloseDatabaseCommand());
                            await OpenDatabase();
                        });
                }
                else
                {
                    await OpenDatabase();
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

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            Model.KeyFilePath = token;
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
            
            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            await Model.CreateKeyFile(new FileInfo
            {
                Id = token,
                Name = file.DisplayName,
                Path = file.Path
            });
        }

        private async Task OpenDatabase()
        {
            var oldLabel = ButtonLabel;
            ButtonLabel = _resource.GetResourceValue("CompositeKeyOpening");
            if (await Dispatcher.RunTaskAsync(async () => await Model.OpenDatabase(DatabaseFilePath, CreateNew)))
            {
                ValidationChecked?.Invoke(this, new PasswordEventArgs(Model.RootGroupId));
            }

            ButtonLabel = oldLabel;
        }
    }
}
