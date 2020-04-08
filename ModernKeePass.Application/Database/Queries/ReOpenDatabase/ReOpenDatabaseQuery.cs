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
            private readonly IFileProxy _file;

            public ReOpenDatabaseQueryHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(ReOpenDatabaseQuery message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var file = await _file.OpenBinaryFile(_database.FileAccessToken);
                await _database.ReOpen(file);
            }
        }
    }
}