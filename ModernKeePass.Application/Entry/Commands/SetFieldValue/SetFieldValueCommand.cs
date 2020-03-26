using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Queries.IsDatabaseOpen;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.SetFieldValue
{
    public class SetFieldValueCommand : IRequest
    {
        public string EntryId { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }

        public class SetFieldValueCommandHandler : IAsyncRequestHandler<SetFieldValueCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IMediator _mediator;

            public SetFieldValueCommandHandler(IDatabaseProxy database, IMediator mediator)
            {
                _database = database;
                _mediator = mediator;
            }

            public async Task Handle(SetFieldValueCommand message)
            {
                var isDatabaseOpen = await _mediator.Send(new IsDatabaseOpenQuery());
                if (!isDatabaseOpen) throw new DatabaseClosedException();

                _database.UpdateEntry(message.EntryId, message.FieldName, message.FieldValue);
            }
        }
    }
}