using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.MoveGroup
{
    public class MoveGroupCommand : IRequest
    {
        public GroupVm ParentGroup { get; set; }
        public GroupVm Group { get; set; }
        public int Index { get; set; }

        public class MoveGroupCommandHandler : IAsyncRequestHandler<MoveGroupCommand>
        {
            private readonly IDatabaseProxy _database;

            public MoveGroupCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(MoveGroupCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.MoveGroup(message.ParentGroup.Id, message.Group.Id, message.Index);
                message.ParentGroup.SubGroups.Insert(message.Index, message.Group);
            }
        }
    }
}