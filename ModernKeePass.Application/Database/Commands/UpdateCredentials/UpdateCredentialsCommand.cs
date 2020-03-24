using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.UpdateCredentials
{
    public class UpdateCredentialsCommand: IRequest
    {
        public Credentials Credentials { get; set; }

        public class UpdateCredentialsCommandHandler : IAsyncRequestHandler<UpdateCredentialsCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public UpdateCredentialsCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(UpdateCredentialsCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (isDatabaseOpen) await _database.UpdateCredentials(message.Credentials);
                else throw new DatabaseClosedException();
            }
        }
    }
}