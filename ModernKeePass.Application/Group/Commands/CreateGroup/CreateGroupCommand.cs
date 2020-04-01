using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<GroupVm>
    {
        public string ParentGroupId { get; set; }
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

                var group = _database.CreateGroup(message.ParentGroupId, message.Name, message.IsRecycleBin);
                var groupVm = _mapper.Map<GroupVm>(group);
                return groupVm;
            }
        }
    }
}