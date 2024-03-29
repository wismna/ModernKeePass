﻿using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
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
        private readonly IFileProxy _file;
        private readonly IResourceProxy _resource;

        public RelayCommand CreateDatabaseFileCommand { get; }
        
        public NewVm(IMediator mediator, ISettingsProxy settings, INavigationService navigation, IFileProxy file, IResourceProxy resource): base(file)
        {
            _mediator = mediator;
            _settings = settings;
            _navigation = navigation;
            _file = file;
            _resource = resource;

            CreateDatabaseFileCommand = new RelayCommand(async () => await CreateDatabaseFile());

            MessengerInstance.Register<CredentialsSetMessage>(this, async m => await TryCreateDatabase(m));
        }

        private async Task CreateDatabaseFile()
        {
            var file = await _file.CreateFile(_resource.GetResourceValue("MessageDialogSaveNameSuggestion"),
                Domain.Common.Constants.Extensions.Kdbx,
                _resource.GetResourceValue("MessageDialogSaveErrorFileTypeDesc"), 
                true);
            if (file == null) return;
            Token = file.Id;
            Path = file.Path;
            Name = file.Name;
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
                CreateSampleData = _settings.GetSetting(Constants.Settings.Sample, true)
            });

            var database = await _mediator.Send(new GetDatabaseQuery());
            _navigation.NavigateTo(Constants.Navigation.GroupPage, new NavigationItem { Id = database.RootGroupId });
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}
