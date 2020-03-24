using MediatR;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CloseDatabase
{
    public class CloseDatabaseCommand: IRequest
    {
        public class CloseDatabaseCommandHandler : IAsyncRequestHandler<CloseDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public CloseDatabaseCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }
            public async Task Handle(CloseDatabaseCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (isDatabaseOpen) _database.CloseDatabase();
                else throw new DatabaseClosedException();
            }
        }
    }
}