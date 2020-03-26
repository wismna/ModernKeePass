using MediatR;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Database.Commands.SaveDatabase
{
    public class SaveDatabaseCommand : IRequest
    {
        public FileInfo FileInfo { get; set; }

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

                if (message.FileInfo != null) await _database.SaveDatabase(message.FileInfo);
                else await _database.SaveDatabase();
            }
        }
    }
}