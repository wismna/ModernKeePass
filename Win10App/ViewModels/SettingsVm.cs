using MediatR;
using ModernKeePass.Application.Database.Queries.GetDatabase;

namespace ModernKeePass.ViewModels
{
    public class SettingsVm
    {
        public bool IsDatabaseOpened { get; }

        public SettingsVm(IMediator mediator)
        {
            var database = mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            IsDatabaseOpened = database.IsOpen;
        }
    }
}