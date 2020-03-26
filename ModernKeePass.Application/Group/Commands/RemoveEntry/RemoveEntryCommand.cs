using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.RemoveEntry
{
    public class RemoveEntryCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public EntryVm Entry { get; set; }

        public class RemoveEntryCommandHandler : IAsyncRequestHandler<RemoveEntryCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public RemoveEntryCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(RemoveEntryCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                await _database.RemoveEntry(message.ParentGroup.Id, message.Entry.Id);
                message.ParentGroup.Entries.Remove(message.Entry);
            }
        }
    }
}