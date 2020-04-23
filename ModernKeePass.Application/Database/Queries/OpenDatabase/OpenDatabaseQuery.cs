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
                if (_database.IsDirty) throw new DatabaseOpenException();

                var file = await _file.OpenBinaryFile(message.FilePath);
                var hasKeyFile = !string.IsNullOrEmpty(message.KeyFilePath);
                await _database.Open(file, new Credentials
                    {
                        KeyFileContents = hasKeyFile ? await _file.OpenBinaryFile(message.KeyFilePath): null,
                        Password = message.Password
                    });
                if (hasKeyFile) _file.ReleaseFile(message.KeyFilePath);
                _database.Size = file.Length;
                _database.FileAccessToken = message.FilePath;
            }
        }
    }
}