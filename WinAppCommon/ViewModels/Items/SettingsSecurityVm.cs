using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MediatR;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.UpdateCredentials;
using ModernKeePass.Application.Database.Queries.GetDatabase;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsSecurityVm
    {
        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        private readonly INotificationService _notification;

        public SettingsSecurityVm(): this(
            App.Services.GetRequiredService<IMediator>(), 
            App.Services.GetRequiredService<IMessenger>(),
            App.Services.GetRequiredService<IResourceProxy>(), 
            App.Services.GetRequiredService<INotificationService>()) { }

        public SettingsSecurityVm(IMediator mediator, IMessenger messenger, IResourceProxy resource, INotificationService notification)
        {
            _mediator = mediator;
            _resource = resource;
            _notification = notification;

            messenger.Register<CredentialsSetMessage>(this, async message => await UpdateDatabaseCredentials(message));
        }

        public async Task UpdateDatabaseCredentials(CredentialsSetMessage message)
        {
            await _mediator.Send(new UpdateCredentialsCommand
            {
                KeyFilePath = message.KeyFilePath,
                Password = message.Password
            });
            var database = await _mediator.Send(new GetDatabaseQuery());
            _notification.Show(database.Name, _resource.GetResourceValue("CompositeKeyUpdated"));
        }
    }
}