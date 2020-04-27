using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CloseDatabase
{
    public class CloseDatabaseCommand: IRequest
    {
        public class CloseDatabaseCommandHandler : IRequestHandler<CloseDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;

            public CloseDatabaseCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }
            public void Handle(CloseDatabaseCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();
                _database.CloseDatabase();

                // Cleanup
                _database.FileAccessToken = null;
                _database.Size = 0;
            }
        }
    }
}