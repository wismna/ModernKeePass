using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.RestoreHistory
{
    public class RestoreHistoryCommand : IRequest
    {
        public string EntryId { get; set; }
        public int HistoryIndex { get; set; }

        public class RestoreHistoryCommandHandler : IRequestHandler<RestoreHistoryCommand>
        {
            private readonly IDatabaseProxy _database;

            public RestoreHistoryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(RestoreHistoryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.RestoreFromHistory(message.EntryId, message.HistoryIndex);
            }
        }
    }
}