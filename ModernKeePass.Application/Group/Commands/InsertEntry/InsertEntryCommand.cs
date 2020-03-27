using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.InsertEntry
{
    public class InsertEntryCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public EntryVm Entry { get; set; }
        public int Index { get; set; }

        public class InsertEntryCommandHandler : IAsyncRequestHandler<InsertEntryCommand>
        {
            private readonly IDatabaseProxy _database;

            public InsertEntryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(InsertEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.InsertEntry(message.ParentGroup.Id, message.Entry.Id, message.Index);
                message.ParentGroup.Entries.Insert(message.Index, message.Entry);
            }
        }
    }
}