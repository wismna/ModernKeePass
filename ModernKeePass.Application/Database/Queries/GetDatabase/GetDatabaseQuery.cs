using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Group.Models;

namespace ModernKeePass.Application.Database.Queries.GetDatabase
{
    public class GetDatabaseQuery: IRequest<DatabaseVm>
    {
        public class GetDatabaseQueryHandler : IRequestHandler<GetDatabaseQuery, DatabaseVm>
        {
            private readonly IDatabaseProxy _databaseProxy;
            private readonly IMapper _mapper;

            public GetDatabaseQueryHandler(IDatabaseProxy databaseProxy, IMapper mapper)
            {
                _databaseProxy = databaseProxy;
                _mapper = mapper;
            }
            
            public DatabaseVm Handle(GetDatabaseQuery request)
            {
                var database = new DatabaseVm
                {
                    IsOpen = _databaseProxy.IsOpen,
                    Name = _databaseProxy.Name,
                    RootGroup = _mapper.Map<GroupVm>(_databaseProxy.RootGroup),
                    IsRecycleBinEnabled = _databaseProxy.IsRecycleBinEnabled,
                    RecycleBin = _mapper.Map<GroupVm>(_databaseProxy.RecycleBin),
                    Compression = _databaseProxy.Compression,
                    CipherId = _databaseProxy.CipherId,
                    KeyDerivationId = _databaseProxy.CipherId
                };
                return database;
            }
        }
    }
}