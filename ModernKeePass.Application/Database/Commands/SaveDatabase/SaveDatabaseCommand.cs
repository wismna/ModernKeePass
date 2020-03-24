using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.SaveDatabase
{
    public class SaveDatabaseCommand : IRequest
    {
        public class SaveDatabaseCommandHandler : IRequestHandler<SaveDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public SaveDatabaseCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(SaveDatabaseCommand message, CancellationToken cancellationToken)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery(), cancellationToken);
                if (isDatabaseOpen) await _database.SaveDatabase();
                else throw new DatabaseClosedException();
            }
        }
    }
}