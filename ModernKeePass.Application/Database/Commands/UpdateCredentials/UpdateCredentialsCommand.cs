using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.UpdateCredentials
{
    public class UpdateCredentialsCommand: IRequest
    {
        public Credentials Credentials { get; set; }

        public class UpdateCredentialsCommandHandler : IAsyncRequestHandler<UpdateCredentialsCommand>
        {
            private readonly IDatabaseProxy _database;

            public UpdateCredentialsCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(UpdateCredentialsCommand message)
            {
                if (_database.IsOpen) await _database.UpdateCredentials(message.Credentials);
                else throw new DatabaseClosedException();
            }
        }
    }
}