using System.Threading.Tasks;
using Windows.Storage;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CreateDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Models;

namespace ModernKeePass.ViewModels
{
    public class NewVm : OpenVm
    {
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
        private readonly INavigationService _navigation;
        private string _importFormatHelp;

        public bool IsImportChecked { get; set; }

        public IStorageFile ImportFile { get; set; }

        public string ImportFileExtensionFilter { get; set; } = "*";

        public IImportFormat ImportFormat { get; set; }

        public string ImportFormatHelp
        {
            get { return _importFormatHelp; }
            set
            {
                _importFormatHelp = value;
                RaisePropertyChanged(nameof(ImportFormatHelp));
            }
        }

        public NewVm(): this(
            App.Services.GetRequiredService<IMediator>(), 
            App.Services.GetRequiredService<ISettingsProxy>(), 
            App.Services.GetRequiredService<IMessenger>(), 
            App.Services.GetRequiredService<INavigationService>()) { }

        public NewVm(IMediator mediator, ISettingsProxy settings, IMessenger messenger, INavigationService navigation)
        {
            _mediator = mediator;
            _settings = settings;
            _navigation = navigation;

            messenger.Register<CredentialsSetMessage>(this, async message => await CreateDatabase(message));
        }

        public async Task CreateDatabase(CredentialsSetMessage message)
        {
            await _mediator.Send(new CreateDatabaseCommand
            {
                FilePath = Token,
                KeyFilePath = message.KeyFilePath,
                Password = message.Password,
                Name = "ModernKeePass",
                Version = _settings.GetSetting(Constants.Settings.DefaultFileFormat, "4"),
                CreateSampleData = _settings.GetSetting<bool>(Constants.Settings.Sample)
            });
            var database = await _mediator.Send(new GetDatabaseQuery());
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = database.RootGroupId });
        }
    }
}
