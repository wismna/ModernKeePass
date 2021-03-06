﻿using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.AddGroup
{
    public class AddGroupCommand : IRequest
    {
        public string ParentGroupId { get; set; }
        public string GroupId { get; set; }

        public class AddGroupCommandHandler : IAsyncRequestHandler<AddGroupCommand>
        {
            private readonly IDatabaseProxy _database;

            public AddGroupCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(AddGroupCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.AddGroup(message.ParentGroupId, message.GroupId);
            }
        }
    }
}