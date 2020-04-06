using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.UpdateCredentials
{
    public class UpdateCredentialsCommand: IRequest
    {
        public string Password { get; set; }
        public string KeyFilePath { get; set; }

        public class UpdateCredentialsCommandHandler : IAsyncRequestHandler<UpdateCredentialsCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IFileProxy _file;

            public UpdateCredentialsCommandHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(UpdateCredentialsCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                _database.UpdateCredentials(new Credentials
                {
                    KeyFileContents = await _file.OpenBinaryFile(message.KeyFilePath),
                    Password = message.Password
                });
            }
        }
    }
}