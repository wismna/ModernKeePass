using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.ReOpenDatabase
{
    public class ReOpenDatabaseQuery: IRequest<GroupVm>
    {
        public class ReOpenDatabaseQueryHandler : IAsyncRequestHandler<ReOpenDatabaseQuery, GroupVm>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly IDatabaseProxy _databaseProxy;

            public ReOpenDatabaseQueryHandler(IMapper mapper, IMediator mediator, IDatabaseProxy databaseProxy)
            {
                _mapper = mapper;
                _mediator = mediator;
                _databaseProxy = databaseProxy;
            }

            public async Task<GroupVm> Handle(ReOpenDatabaseQuery request)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                var rootGroup = await _databaseProxy.ReOpen();
                return _mapper.Map<GroupVm>(rootGroup);
            }
        }
    }
}