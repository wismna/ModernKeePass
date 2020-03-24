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

            public GetDatabaseQueryHandler(IDatabaseProxy databaseProxy)
            {
                _databaseProxy = databaseProxy;
            }
            
            public DatabaseVm Handle(GetDatabaseQuery request)
            {
                var database = new DatabaseVm
                {
                    IsOpen = _databaseProxy.IsOpen,
                    Name = _databaseProxy.Name
                };
                return database;
            }
        }
    }
}