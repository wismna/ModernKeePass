using MediatR;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.SaveDatabase
{
    public class SaveDatabaseCommand : IRequest
    {
        public string FilePath { get; set; }

        public class SaveDatabaseCommandHandler : IAsyncRequestHandler<SaveDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;

            public SaveDatabaseCommandHandler(IDatabaseProxy database)
            {
                _database = database;
            }

            public async Task Handle(SaveDatabaseCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                if (string.IsNullOrEmpty(message.FilePath)) await _database.SaveDatabase();
                else await _database.SaveDatabase(message.FilePath);
            }
        }
    }
}