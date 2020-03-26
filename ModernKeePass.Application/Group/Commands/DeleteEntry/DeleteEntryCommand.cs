using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
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

            public DeleteEntryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(DeleteEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.DeleteEntry(message.Entry.Id);
                message.Entry = null;
            }
        }
    }
}