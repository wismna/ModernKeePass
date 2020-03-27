using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Cryptography.Commands.SetKeyDerivation
{
    public class SetKeyDerivationCommand : IRequest
    {
        public string KeyDerivationId { get; set; }

        public class SetKeyDerivationCommandHandler : IRequestHandler<SetKeyDerivationCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetKeyDerivationCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetKeyDerivationCommand message)
            {
                if (_database.IsOpen) _database.KeyDerivationId = message.KeyDerivationId;
                else throw new DatabaseClosedException();
            }
        }
    }
}