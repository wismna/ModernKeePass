using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Group.Queries.GetAllGroups
{
    public class GetAllGroupsQuery : IRequest<IEnumerable<GroupVm>>
    {
        public string GroupId { get; set; }

        public class GetAllGroupsQueryHandler : IRequestHandler<GetAllGroupsQuery, IEnumerable<GroupVm>>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMapper _mapper;

            public GetAllGroupsQueryHandler(IDatabaseProxy database, IMapper mapper)
            {
                _database = database;
                _mapper = mapper;
            }

            public IEnumerable<GroupVm> Handle(GetAllGroupsQuery message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                var groups = new List<GroupVm> {_mapper.Map<GroupVm>(_database.GetGroup(message.GroupId))};
                groups.AddRange(_database.GetAllGroups(message.GroupId).Select(g => _mapper.Map<GroupVm>(g)));
                return groups;
            }
        }
    }
}