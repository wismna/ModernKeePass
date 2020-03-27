using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.SetFieldValue
{
    public class SetFieldValueCommand : IRequest
    {
        public string EntryId { get; set; }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }

        public class SetFieldValueCommandHandler : IRequestHandler<SetFieldValueCommand>
        {
            private readonly IDatabaseProxy _database;

            public SetFieldValueCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(SetFieldValueCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.UpdateEntry(message.EntryId, message.FieldName, message.FieldValue);
            }
        }
    }
}