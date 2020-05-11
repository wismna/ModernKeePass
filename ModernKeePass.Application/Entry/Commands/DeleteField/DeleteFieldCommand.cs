using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Entry.Commands.DeleteField
{
    public class DeleteFieldCommand: IRequest
    {
        public string EntryId { get; set; }
        public string FieldName { get; set; }

        public class DeleteFieldCommandHandler : IRequestHandler<DeleteFieldCommand>
        {
            private readonly IDatabaseProxy _database;

            public DeleteFieldCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public void Handle(DeleteFieldCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                _database.DeleteField(message.EntryId, message.FieldName);
            }
        }
    }
}