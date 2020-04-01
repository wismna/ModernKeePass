using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Queries.OpenDatabase
{
    public class OpenDatabaseQuery: IRequest
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
        public string KeyFilePath { get; set; }

        public class OpenDatabaseQueryHandler : IAsyncRequestHandler<OpenDatabaseQuery>
        {
            private readonly IDatabaseProxy _database;

            public OpenDatabaseQueryHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(OpenDatabaseQuery request)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                await _database.Open(
                    new FileInfo
                    {
                        Path = request.FilePath
                    }, 
                    new Credentials
                    {
                        KeyFilePath = request.KeyFilePath,
                        Password = request.Password
                    });
            }
            
        }
    }
}