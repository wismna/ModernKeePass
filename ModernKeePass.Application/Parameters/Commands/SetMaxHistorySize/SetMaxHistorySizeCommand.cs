using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Parameters.Commands.SetMaxHistorySize
{
    public class SetMaxHistorySizeCommand : IRequest
    {
        public long MaxHistorySize { get; set; }

        public class SetMaxHistorySizeCommandHandler : IRequestHandler<SetMaxHistorySizeCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetMaxHistorySizeCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetMaxHistorySizeCommand message)
            {
                if (_database.IsOpen) _database.MaxHistorySize = message.MaxHistorySize;
                else throw new DatabaseClosedException();
            }
        }

    }
}