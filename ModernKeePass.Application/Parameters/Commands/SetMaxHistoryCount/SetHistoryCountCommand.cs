using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Parameters.Commands.SetMaxHistoryCount
{
    public class SetMaxHistoryCountCommand : IRequest
    {
        public int MaxHistoryCount { get; set; }

        public class SetMaxHistoryCountCommandHandler : IRequestHandler<SetMaxHistoryCountCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetMaxHistoryCountCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetMaxHistoryCountCommand message)
            {
                if (_database.IsOpen) _database.MaxHistoryCount = message.MaxHistoryCount;
                else throw new DatabaseClosedException();
            }
        }
    }
}