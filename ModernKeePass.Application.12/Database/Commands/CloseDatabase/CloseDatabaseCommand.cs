using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CloseDatabase
{
    public class CloseDatabaseCommand: IRequest
    {
        public class CloseDatabaseCommandHandler : IRequestHandler<CloseDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public CloseDatabaseCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(CloseDatabaseCommand message, CancellationToken cancellationToken)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery(), cancellationToken);
                if (isDatabaseOpen) _database.CloseDatabase();
                else throw new DatabaseClosedException();
            }
        }
    }
}