﻿using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.CloseDatabase;
using ModernKeePass.Application.Database.Commands.SaveDatabase;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Common;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.ViewModels
{
    public class SaveVm: ViewModelBase
    {
        public bool IsSaveEnabled => _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult().IsDirty;

        public RelayCommand SaveAsCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand CloseCommand { get; }

        private readonly IMediator _mediator;
        private readonly INavigationService _navigation;
        private readonly IFileProxy _file;
        private readonly IResourceProxy _resource;

        public SaveVm(IMediator mediator, INavigationService navigation, IFileProxy file, IResourceProxy resource)
        {
            _mediator = mediator;
            _navigation = navigation;
            _file = file;
            _resource = resource;

            SaveAsCommand = new RelayCommand(async () => await SaveAs());
            SaveCommand = new RelayCommand(async () => await Save(), () => IsSaveEnabled);
            CloseCommand = new RelayCommand(async () => await Close());

            MessengerInstance.Register<DatabaseSavedMessage>(this, _ => SaveCommand.RaiseCanExecuteChanged());
        }
        
        private async Task SaveAs()
        {
            var file = await _file.CreateFile(_resource.GetResourceValue("MessageDialogSaveNameSuggestion"),
                Domain.Common.Constants.Extensions.Kdbx,
                _resource.GetResourceValue("MessageDialogSaveErrorFileTypeDesc"), 
                true);
            if (file == null) return;
            await _mediator.Send(new SaveDatabaseCommand { FilePath = file.Id });
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }

        public async Task Save()
        {
            try
            {
                await _mediator.Send(new SaveDatabaseCommand());
                await Close();
            }
            catch (SaveException e)
            {
                MessengerInstance.Send(new SaveErrorMessage { Message = e.Message });
            }
        }
        
        public async Task Close()
        {
            await _mediator.Send(new CloseDatabaseCommand());
            _navigation.NavigateTo(Constants.Navigation.MainPage);
        }

        public override void Cleanup()
        {
            MessengerInstance.Unregister(this);
            base.Cleanup();
        }
    }
}