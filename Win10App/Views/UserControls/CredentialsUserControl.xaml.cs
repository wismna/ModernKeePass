using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Autofac;
using ModernKeePass.Common;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Extensions;
using ModernKeePass.ViewModels;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236

namespace ModernKeePass.Views.UserControls
{
    public sealed partial class CredentialsUserControl
    {
        private readonly IDatabaseService _databaseService;
        private readonly IResourceService _resourceService;

        public CredentialsViewModel ViewModel => Grid.DataContext as CredentialsViewModel;
        
        public string DatabaseFilePath
        {
            get => (string)GetValue(DatabaseFilePathProperty);
            set => SetValue(DatabaseFilePathProperty, value);
        }
        public static readonly DependencyProperty DatabaseFilePathProperty =
            DependencyProperty.Register(
                "DatabaseFilePath",
                typeof(string),
                typeof(CredentialsUserControl),
                new PropertyMetadata(null, (o, args) => { }));
        
        public event EventHandler ValidationChecking;
        public event EventHandler ValidationChecked;

        public CredentialsUserControl(): this(App.Container.Resolve<IDatabaseService>(), App.Container.Resolve<IResourceService>())
        { }

        public CredentialsUserControl(IDatabaseService databaseService, IResourceService resourceService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _resourceService = resourceService;
        }

        private async void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            ValidationChecking?.Invoke(this, new EventArgs());

            if (_databaseService.IsOpen)
            {
                await MessageDialogHelper.ShowActionDialog(_resourceService.GetResourceValue("MessageDialogDBOpenTitle"),
                    string.Format(_resourceService.GetResourceValue("MessageDialogDBOpenDesc"), _databaseService.Name),
                    _resourceService.GetResourceValue("MessageDialogDBOpenButtonSave"),
                    _resourceService.GetResourceValue("MessageDialogDBOpenButtonDiscard"),
                    async command =>
                    {
                        await _databaseService.Save();
                        ToastNotificationHelper.ShowGenericToast(
                            _databaseService.Name,
                            _resourceService.GetResourceValue("ToastSavedMessage"));
                        _databaseService.Close();
                        await OpenDatabase();
                    },
                    async command =>
                    {
                        _databaseService.Close();
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
            if (e.Key == VirtualKey.Enter && ViewModel.IsValid)
            {
                OpenButton_OnClick(sender, e);
                // Stop the event from triggering twice
                e.Handled = true;
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

            var token = StorageApplicationPermissions.FutureAccessList.Add(file);
            ViewModel.KeyFilePath = token;
        }

        private async Task OpenDatabase()
        {
            if (await Dispatcher.RunTaskAsync(async () =>
            {
                var fileInfo = new FileInfo
                {
                    Path = DatabaseFilePath
                };
                return await ViewModel.OpenDatabase(fileInfo);
            }))
            {
                ValidationChecked?.Invoke(this, new EventArgs());
            }
        }
    }
}
