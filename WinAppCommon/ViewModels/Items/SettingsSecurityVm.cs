using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MediatR;
using Messages;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Commands.UpdateCredentials;
using ModernKeePass.Application.Database.Queries.GetDatabase;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsSecurityVm: ViewModelBase
    {
        private readonly IMediator _mediator;
        private readonly IResourceProxy _resource;
        private readonly INotificationService _notification;
        
        public SettingsSecurityVm(IMediator mediator, IResourceProxy resource, INotificationService notification)
        {
            _mediator = mediator;
            _resource = resource;
            _notification = notification;

            MessengerInstance.Register<CredentialsSetMessage>(this, async message => await UpdateDatabaseCredentials(message));
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