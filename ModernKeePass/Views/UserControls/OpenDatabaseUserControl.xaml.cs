using System;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Events;
using ModernKeePass.Extensions;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class OpenDatabaseUserControl
    {
        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        public OpenDatabaseControlVm Model => Grid.DataContext as OpenDatabaseControlVm;
        
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
        
        public OpenDatabaseUserControl() : this(App.Services.GetRequiredService<IMediator>(), App.Services.GetRequiredService<IResourceProxy>()) { }

        public OpenDatabaseUserControl(IMediator mediator, IResourceProxy resource)
        {
            _mediator = mediator;
            _resource = resource;
            InitializeComponent();
        }

        public event EventHandler DatabaseOpening;
        public event EventHandler<PasswordEventArgs> DatabaseOpened;

        private async void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            DatabaseOpening?.Invoke(this, new EventArgs());
            
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

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key != VirtualKey.Enter || !Model.IsValid) return;
            OpenButton_OnClick(sender, e);
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

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            Model.KeyFilePath = token;
        }
        
        private async Task OpenDatabase()
        {
            var oldLabel = OpenButton.Content;
            OpenButton.Content = _resource.GetResourceValue("CompositeKeyOpening");
            if (await Dispatcher.RunTaskAsync(async () => await Model.OpenDatabase(DatabaseFilePath)))
            {
                DatabaseOpened?.Invoke(this, new PasswordEventArgs(Model.RootGroupId));
            }

            OpenButton.Content = oldLabel;
        }
    }
}
