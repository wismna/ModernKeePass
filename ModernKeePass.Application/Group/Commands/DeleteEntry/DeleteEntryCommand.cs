using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.DeleteEntry
{
    public class DeleteEntryCommand : IRequest
    {
        public string ParentGroupId { get; set; }
        public string EntryId { get; set; }
        public string RecycleBinName { get; set; }

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

                await _database.DeleteEntry(message.ParentGroupId, message.EntryId, message.RecycleBinName);
            }
        }
    }
}