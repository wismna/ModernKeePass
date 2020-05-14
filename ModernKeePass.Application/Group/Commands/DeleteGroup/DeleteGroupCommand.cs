using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Common;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.DeleteGroup
{
    public class DeleteGroupCommand : IRequest
    {
        public string ParentGroupId { get; set; }
        public string GroupId { get; set; }
        public string RecycleBinName { get; set; }

        public class DeleteGroupCommandHandler : IAsyncRequestHandler<DeleteGroupCommand>
        {
            private readonly IDatabaseProxy _database;

            public DeleteGroupCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(DeleteGroupCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var isRecycleBin = message.GroupId.Equals(_database.RecycleBinId);
                if (_database.IsRecycleBinEnabled && (string.IsNullOrEmpty(_database.RecycleBinId) || _database.RecycleBinId.Equals(Constants.EmptyId)))
                {
                    _database.CreateGroup(_database.RootGroupId, message.RecycleBinName, true);
                }

                if (!_database.IsRecycleBinEnabled || message.ParentGroupId.Equals(_database.RecycleBinId) || isRecycleBin)
                {
                    _database.DeleteEntity(message.GroupId);
                }
                else
                {
                    await _database.AddGroup(_database.RecycleBinId, message.GroupId);
                }

                await _database.RemoveGroup(message.ParentGroupId, message.GroupId);
                if (isRecycleBin) _database.RecycleBinId = Constants.EmptyId;
            }
        }
    }
}