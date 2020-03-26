using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.SortEntries
{
    public class SortEntriesCommand : IRequest
    {
        public GroupVm Group { get; set; }

        public class SortEntriesCommandHandler : IAsyncRequestHandler<SortEntriesCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public SortEntriesCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(SortEntriesCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                _database.SortEntries(message.Group.Id);
            }
        }
    }
}