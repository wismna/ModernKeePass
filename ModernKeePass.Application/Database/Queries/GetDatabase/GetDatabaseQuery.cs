using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;

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
                    database.RootGroupId = _databaseProxy.RootGroupId;
                    database.IsRecycleBinEnabled = _databaseProxy.IsRecycleBinEnabled;
                    database.RecycleBinId = _databaseProxy.RecycleBinId;
                    database.Compression = _databaseProxy.Compression;
                    database.CipherId = _databaseProxy.CipherId;
                    database.KeyDerivationId = _databaseProxy.KeyDerivationId;
                }
                return database;
            }
        }
    }
}