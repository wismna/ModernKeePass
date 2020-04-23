﻿using System.Threading.Tasks;
using Windows.Storage;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
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
        
        public NewVm(IMediator mediator, IRecentProxy recent, ISettingsProxy settings, INavigationService navigation) : base(recent)
        {
            _mediator = mediator;
            _settings = settings;
            _navigation = navigation;

            MessengerInstance.Register<CredentialsSetMessage>(this, async m => await TryCreateDatabase(m));
        }

        private async Task TryCreateDatabase(CredentialsSetMessage message)
        {
            var database = await _mediator.Send(new GetDatabaseQuery());
            if (database.IsDirty)
            {
                MessengerInstance.Register<DatabaseClosedMessage>(this, async m => await CreateDatabase(m.Parameter as CredentialsSetMessage));
                MessengerInstance.Send(new DatabaseAlreadyOpenedMessage { Parameter = message });
            }
            else await CreateDatabase(message);
        }
        
        private async Task CreateDatabase(CredentialsSetMessage message)
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