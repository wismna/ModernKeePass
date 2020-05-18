using System.Threading.Tasks;
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
        public bool IsProtected { get; set; } = true;

        public class UpsertFieldCommandHandler : IAsyncRequestHandler<UpsertFieldCommand>
        {
            private readonly IDatabaseProxy _database;

            public UpsertFieldCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(UpsertFieldCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                await _database.UpdateEntry(message.EntryId, message.FieldName, message.FieldValue, message.IsProtected);
            }
        }
    }
}