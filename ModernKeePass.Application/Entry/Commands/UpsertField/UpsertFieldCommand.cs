using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.UpsertField
{
    public class UpsertFieldCommand : IRequest
    {
        public string EntryId { get; set; }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }

        public class UpsertFieldCommandHandler : IRequestHandler<UpsertFieldCommand>
        {
            private readonly IDatabaseProxy _database;

            public UpsertFieldCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(UpsertFieldCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.UpdateEntry(message.EntryId, message.FieldName, message.FieldValue);
            }
        }
    }
}