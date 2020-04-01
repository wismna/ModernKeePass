using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Queries.GetGroup
{
    public class GetGroupQuery : IRequest<GroupVm>
    {
        public string Id { get; set; }

        public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, GroupVm>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public GetGroupQueryHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public GroupVm Handle(GetGroupQuery message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                return _mapper.Map<GroupVm>(_database.GetGroup(message.Id));
            }
        }
    }
}