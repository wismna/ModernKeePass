using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Domain.Common;

namespace ModernKeePass.Application.Database.Queries.GetDatabase
{
    public class GetDatabaseQuery: IRequest<DatabaseVm>
    {
        public class GetDatabaseQueryHandler : IRequestHandler<GetDatabaseQuery, DatabaseVm>
        {
            private readonly IDatabaseProxy _databaseProxy;

            public GetDatabaseQueryHandler(IDatabaseProxy databaseProxy)
            {
                _databaseProxy = databaseProxy;
            }
            
            public DatabaseVm Handle(GetDatabaseQuery message)
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
                    database.RecycleBinId = _databaseProxy.RecycleBinId == Constants.EmptyId ? null : _databaseProxy.RecycleBinId;
                    database.Compression = _databaseProxy.Compression;
                    database.CipherId = _databaseProxy.CipherId;
                    database.KeyDerivationId = _databaseProxy.KeyDerivationId;
                    database.Size = _databaseProxy.Size;
                    database.IsDirty = _databaseProxy.IsDirty;
                    database.MaxHistoryCount = _databaseProxy.MaxHistoryCount;
                }
                return database;
            }
        }
    }
}