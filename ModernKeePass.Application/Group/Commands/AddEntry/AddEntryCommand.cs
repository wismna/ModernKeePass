using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.AddEntry
{
    public class AddEntryCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public EntryVm Entry { get; set; }

        public class AddEntryCommandHandler : IAsyncRequestHandler<AddEntryCommand>
        {
            private readonly IDatabaseProxy _database;

            public AddEntryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(AddEntryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.AddEntry(message.ParentGroup.Id, message.Entry.Id);
                message.ParentGroup.Entries.Add(message.Entry);
            }
        }
    }
}