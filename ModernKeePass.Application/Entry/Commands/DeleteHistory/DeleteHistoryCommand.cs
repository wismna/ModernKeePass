using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.DeleteHistory
{
    public class DeleteHistoryCommand : IRequest
    {
        public string EntryId { get; set; }
        public int HistoryIndex { get; set; }

        public class DeleteHistoryCommandHandler : IRequestHandler<DeleteHistoryCommand>
        {
            private readonly IDatabaseProxy _database;

            public DeleteHistoryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(DeleteHistoryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.DeleteHistory(message.EntryId, message.HistoryIndex);
            }
        }
    }
}