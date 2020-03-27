using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Cryptography.Commands.SetCipher
{
    public class SetCipherCommand : IRequest
    {
        public string CipherId { get; set; }

        public class SetCipherCommandHandler : IRequestHandler<SetCipherCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetCipherCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetCipherCommand message)
            {
                if (_database.IsOpen) _database.CipherId = message.CipherId;
                else throw new DatabaseClosedException();
            }
        }
    }
}