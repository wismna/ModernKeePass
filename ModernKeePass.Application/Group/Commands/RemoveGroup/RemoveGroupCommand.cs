using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.RemoveGroup
{
    public class RemoveGroupCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public GroupVm Group { get; set; }
        public bool IsDelete { get; set; }

        public class RemoveGroupCommandHandler : IAsyncRequestHandler<RemoveGroupCommand>
        {
            private readonly IDatabaseProxy _database;

            public RemoveGroupCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(RemoveGroupCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.RemoveGroup(message.ParentGroup.Id, message.Group.Id, message.IsDelete);
                message.ParentGroup.SubGroups.Remove(message.Group);
            }
        }
    }
}