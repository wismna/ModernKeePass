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

                if (_database.IsRecycleBinEnabled && (string.IsNullOrEmpty(_database.RecycleBinId) || _database.RecycleBinId.Equals(_database.ZeroId)))
                {
                    _database.CreateGroup(_database.RootGroupId, message.RecycleBinName, true);
                }

                if (!_database.IsRecycleBinEnabled || message.ParentGroupId.Equals(_database.RecycleBinId))
                {
                    _database.DeleteEntity(message.EntryId);
                }
                else
                {
                    await _database.AddEntry(_database.RecycleBinId, message.EntryId);
                }

                await _database.RemoveEntry(message.ParentGroupId, message.EntryId);
            }
        }
    }
}