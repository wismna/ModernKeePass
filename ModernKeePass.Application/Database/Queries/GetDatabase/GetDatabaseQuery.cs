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
                    IsOpen = _databaseProxy.IsOpen
                };
                if (database.IsOpen)
                {
                    database.Name = _databaseProxy.Name;
                    database.RootGroup = _mapper.Map<GroupVm>(_databaseProxy.RootGroup);
                    database.IsRecycleBinEnabled = _databaseProxy.IsRecycleBinEnabled;
                    database.RecycleBin = _mapper.Map<GroupVm>(_databaseProxy.RecycleBin);
                    database.Compression = _databaseProxy.Compression;
                    database.CipherId = _databaseProxy.CipherId;
                    database.KeyDerivationId = _databaseProxy.CipherId;
                }
                return database;
            }
        }
    }
}