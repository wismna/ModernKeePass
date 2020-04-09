using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.MoveEntry
{
    public class MoveEntryCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public EntryVm Entry { get; set; }
        public int Index { get; set; }

        public class MoveEntryCommandHandler : IAsyncRequestHandler<MoveEntryCommand>
        {
            private readonly IDatabaseProxy _database;

            public MoveEntryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(MoveEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.MoveEntry(message.ParentGroup.Id, message.Entry.Id, message.Index);
                message.ParentGroup.Entries.Insert(message.Index, message.Entry);
            }
        }
    }
}