using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.DeleteEntry
{
    public class DeleteEntryCommand : IRequest
    {
        public EntryVm Entry { get; set; }

        public class DeleteEntryCommandHandler : IAsyncRequestHandler<DeleteEntryCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public DeleteEntryCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(DeleteEntryCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                await _database.DeleteEntry(message.Entry.Id);
                message.Entry = null;
            }
        }
    }
}