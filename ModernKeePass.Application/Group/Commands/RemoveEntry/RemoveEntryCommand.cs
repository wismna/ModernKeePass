using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
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

            public RemoveEntryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(RemoveEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.RemoveEntry(message.ParentGroup.Id, message.Entry.Id);
                message.ParentGroup.Entries.Remove(message.Entry);
            }
        }
    }
}