using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.ReOpenDatabase
{
    public class ReOpenDatabaseQuery: IRequest
    {
        public class ReOpenDatabaseQueryHandler : IAsyncRequestHandler<ReOpenDatabaseQuery>
        {
            private readonly IDatabaseProxy _database;

            public ReOpenDatabaseQueryHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(ReOpenDatabaseQuery request)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.ReOpen();
            }
        }
    }
}