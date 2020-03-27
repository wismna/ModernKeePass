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

            public UpdateCredentialsCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(UpdateCredentialsCommand message)
            {
                if (_database.IsOpen)
                {
                    await _database.UpdateCredentials(new Credentials
                    {
                        KeyFilePath = message.KeyFilePath,
                        Password = message.Password
                    });
                }
                else throw new DatabaseClosedException();
            }
        }
    }
}