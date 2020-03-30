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
                if (_database.IsOpen) _database.CloseDatabase();
                else throw new DatabaseClosedException();
            }
        }
    }
}