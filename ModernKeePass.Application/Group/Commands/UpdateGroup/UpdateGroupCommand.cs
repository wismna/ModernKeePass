using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.UpdateGroup
{
    public class UpdateGroupCommand : IRequest
    {
        public GroupVm Group { get; set; }
        public string Title { get; set; }
        public Icon Icon { get; set; }

        public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
        {
            private readonly IDatabaseProxy _database;

            public UpdateGroupCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(UpdateGroupCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var group = new GroupEntity
                {
                    Id = message.Group.Id,
                    Name = message.Title,
                    Icon = message.Icon
                };
                _database.UpdateGroup(group);
                message.Group.Title = message.Title;
                message.Group.Icon = message.Icon;
            }
        }
    }
}