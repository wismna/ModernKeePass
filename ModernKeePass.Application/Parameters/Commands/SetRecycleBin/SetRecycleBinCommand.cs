using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Common;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Parameters.Commands.SetRecycleBin
{
    public class SetRecycleBinCommand : IRequest
    {
        public string RecycleBinId { get; set; }

        public class SetRecycleBinCommandHandler : IRequestHandler<SetRecycleBinCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetRecycleBinCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetRecycleBinCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                _database.RecycleBinId = message.RecycleBinId ?? Constants.EmptyId;
            }
        }
    }
}