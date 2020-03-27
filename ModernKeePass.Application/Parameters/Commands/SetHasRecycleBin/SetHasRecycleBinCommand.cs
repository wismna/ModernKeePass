using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Parameters.Commands.SetHasRecycleBin
{
    public class SetHasRecycleBinCommand : IRequest
    {
        public bool HasRecycleBin { get; set; }

        public class SetHasRecycleBinCommandHandler : IRequestHandler<SetHasRecycleBinCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetHasRecycleBinCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetHasRecycleBinCommand message)
            {
                if (_database.IsOpen) _database.IsRecycleBinEnabled = message.HasRecycleBin;
                else throw new DatabaseClosedException();
            }
        }
    }
}