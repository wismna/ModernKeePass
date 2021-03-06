﻿using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<GroupVm>
    {
        public GroupVm ParentGroup { get; set; }
        public string Name { get; set; }
        public bool IsRecycleBin { get; set; }

        public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public CreateGroupCommandHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public GroupVm Handle(CreateGroupCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var group = _database.CreateGroup(message.ParentGroup.Id, message.Name, message.IsRecycleBin);
                var groupVm = _mapper.Map<GroupVm>(group);
                message.ParentGroup.Groups.Add(groupVm);
                return groupVm;
            }
        }
    }
}