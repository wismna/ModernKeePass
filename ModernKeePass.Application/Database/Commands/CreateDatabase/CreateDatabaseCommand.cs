using MediatR;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CreateDatabase
{
    public class CreateDatabaseCommand : IRequest
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
        public string KeyFilePath { get; set; }

        public class CreateDatabaseCommandHandler : IAsyncRequestHandler<CreateDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IFileProxy _file;

            public CreateDatabaseCommandHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(CreateDatabaseCommand message)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                var file = await _file.OpenBinaryFile(message.FilePath);
                await _database.Create(file,
                    new Credentials
                    {
                        KeyFileContents = !string.IsNullOrEmpty(message.KeyFilePath) ? await _file.OpenBinaryFile(message.KeyFilePath) : null,
                        Password = message.Password
                    });
                _database.FileAccessToken = message.FilePath;
            }

        }
    }
}