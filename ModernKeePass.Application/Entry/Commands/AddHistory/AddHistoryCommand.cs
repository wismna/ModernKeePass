using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.AddHistory
{
    public class AddHistoryCommand : IRequest
    {
        public string EntryId { get; set; }

        public class AddHistoryCommandHandler : IRequestHandler<AddHistoryCommand>
        {
            private readonly IDatabaseProxy _database;

            public AddHistoryCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(AddHistoryCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.AddHistory(message.EntryId);
            }
        }
    }
}