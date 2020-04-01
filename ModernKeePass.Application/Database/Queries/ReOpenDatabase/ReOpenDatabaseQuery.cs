using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.ReOpenDatabase
{
    public class ReOpenDatabaseQuery: IRequest<GroupVm>
    {
        public class ReOpenDatabaseQueryHandler : IAsyncRequestHandler<ReOpenDatabaseQuery, GroupVm>
        {
            private readonly IMapper _mapper;
            private readonly IDatabaseProxy _database;

            public ReOpenDatabaseQueryHandler(IMapper mapper, IDatabaseProxy database)
            {
                _mapper = mapper;
                _database = database;
            }

            public async Task<GroupVm> Handle(ReOpenDatabaseQuery request)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var rootGroup = await _database.ReOpen();
                return GroupVm.BuildHierarchy(null, rootGroup, _mapper);
            }
        }
    }
}