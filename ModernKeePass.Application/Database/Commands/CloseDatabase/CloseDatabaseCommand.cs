using MediatR;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.CloseDatabase
{
    public class CloseDatabaseCommand: IRequest
    {
        public class CloseDatabaseCommandHandler : IAsyncRequestHandler<CloseDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;

            public CloseDatabaseCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }
            public async Task Handle(CloseDatabaseCommand message)
            {
                if (_database.IsOpen) _database.CloseDatabase();
                else throw new DatabaseClosedException();
            }
        }
    }
}