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
            private readonly IFileProxy _file;

            public OpenDatabaseQueryHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(OpenDatabaseQuery message)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                var file = await _file.OpenBinaryFile(message.FilePath);
                await _database.Open(file, new Credentials
                    {
                        KeyFileContents = !string.IsNullOrEmpty(message.KeyFilePath) ? await _file.OpenBinaryFile(message.KeyFilePath): null,
                        Password = message.Password
                    });
                _database.FileAccessToken = message.FilePath;
            }
            
        }
    }
}