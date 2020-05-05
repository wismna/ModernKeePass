using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.AddEntry
{
    public class AddEntryCommand : IRequest
    {
        public string ParentGroupId { get; set; }
        public string EntryId { get; set; }

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

                await _database.AddEntry(message.ParentGroupId, message.EntryId);
            }
        }
    }
}