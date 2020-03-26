using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<GroupVm>
    {
        public GroupVm ParentGroup { get; set; }
        public string Name { get; set; }
        public bool IsRecycleBin { get; set; }

        public class CreateGroupCommandHandler : IAsyncRequestHandler<CreateGroupCommand, GroupVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;
            private readonly IMapper _mapper;

            public CreateGroupCommandHandler(IDatabaseProxy database, IMediator mediator, IMapper mapper)
            {
                _database = database;
                _mediator = mediator;
                _mapper = mapper;
            }

            public async Task<GroupVm> Handle(CreateGroupCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                var group = _database.CreateGroup(message.ParentGroup.Id, message.Name, message.IsRecycleBin);
                var groupVm = _mapper.Map<GroupVm>(group);
                message.ParentGroup.SubGroups.Add(groupVm);
                return groupVm;
            }
        }
    }
}