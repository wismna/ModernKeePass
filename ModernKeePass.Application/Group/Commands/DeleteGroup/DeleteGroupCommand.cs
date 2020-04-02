using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
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

                await _database.DeleteEntry(message.ParentGroupId, message.GroupId, message.RecycleBinName);
            }
        }
    }
}