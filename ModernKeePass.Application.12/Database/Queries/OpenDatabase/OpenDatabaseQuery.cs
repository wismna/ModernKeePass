using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.OpenDatabase
{
    public class OpenDatabaseQuery: IRequest<DatabaseVm>
    {
        public FileInfo FileInfo { get; set; }
        public Credentials Credentials { get; set; }

        public class OpenDatabaseQueryHandler : IRequestHandler<OpenDatabaseQuery, DatabaseVm>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly IDatabaseProxy _databaseProxy;

            public OpenDatabaseQueryHandler(IMapper mapper, IMediator mediator, IDatabaseProxy databaseProxy)
            {
                _mapper = mapper;
                _mediator = mediator;
                _databaseProxy = databaseProxy;
            }

            public async Task<DatabaseVm> Handle(OpenDatabaseQuery request, CancellationToken cancellationToken)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery(), cancellationToken);
                if (isDatabaseOpen) throw new DatabaseOpenException();

                var database = await _databaseProxy.Open(request.FileInfo, request.Credentials);
                var databaseVm = new DatabaseVm
                {
                    IsOpen = true,
                    Name = database.Name,
                    RootGroup = _mapper.Map<GroupVm>(database.RootGroupEntity)
                };
                return databaseVm;
            }
        }
    }
}