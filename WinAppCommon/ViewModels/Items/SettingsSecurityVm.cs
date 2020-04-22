using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.UpdateCredentials;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsSecurityVm
    {
        private readonly IMediator _mediator;
        private readonly ISettingsProxy _settings;
        private readonly INavigationService _navigation;

        public SettingsSecurityVm(): this(
            App.Services.GetRequiredService<IMediator>(), 
            App.Services.GetRequiredService<ISettingsProxy>(), 
            App.Services.GetRequiredService<IMessenger>(), 
            App.Services.GetRequiredService<INavigationService>()) { }

        public SettingsSecurityVm(IMediator mediator, ISettingsProxy settings, IMessenger messenger, INavigationService navigation)
        {
            _mediator = mediator;
            _settings = settings;
            _navigation = navigation;

            messenger.Register<CredentialsSetMessage>(this, async message => await UpdateDatabaseCredentials(message));
        }

        public async Task UpdateDatabaseCredentials(CredentialsSetMessage message)
        {
            await _mediator.Send(new UpdateCredentialsCommand
            {
                KeyFilePath = message.KeyFilePath,
                Password = message.Password
            });
            //TODO: Add Toast
        }
    }
}