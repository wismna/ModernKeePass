using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.DeleteHistory
{
    public class DeleteHistoryCommand : IRequest
    {
        public EntryVm Entry { get; set; }
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

                _database.DeleteHistory(message.Entry.Id, message.HistoryIndex);
                message.Entry.History.RemoveAt(message.HistoryIndex);
            }
        }
    }
}