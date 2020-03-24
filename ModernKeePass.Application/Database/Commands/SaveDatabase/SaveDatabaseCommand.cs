using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.SaveDatabase
{
    public class SaveDatabaseCommand : IRequest
    {
        public FileInfo FileInfo { get; set; }

        public class SaveDatabaseCommandHandler : IAsyncRequestHandler<SaveDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public SaveDatabaseCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(SaveDatabaseCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (isDatabaseOpen)
                {
                    if (message.FileInfo != null) await _database.SaveDatabase(message.FileInfo);
                    else await _database.SaveDatabase();
                }
                else throw new DatabaseClosedException();
            }
        }
    }
}