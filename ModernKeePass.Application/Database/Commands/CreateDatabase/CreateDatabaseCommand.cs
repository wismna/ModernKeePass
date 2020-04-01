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

            public CreateDatabaseCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(CreateDatabaseCommand message)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                await _database.Create(
                    new FileInfo
                    {
                        Path = message.FilePath
                    },
                    new Credentials
                    {
                        KeyFilePath = message.KeyFilePath,
                        Password = message.Password
                    });
            }

        }
    }
}