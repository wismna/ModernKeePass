using System;
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
            private readonly IFileProxy _file;

            public SaveDatabaseCommandHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(SaveDatabaseCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                try
                {
                    if (!string.IsNullOrEmpty(message.FilePath))
                    {
                        _database.FileAccessToken = message.FilePath;
                    }

                    var contents = await _database.SaveDatabase();

                    // Test DB integrity
                    _database.CloseDatabase();
                    await _database.ReOpen(contents);

                    // Transactional write to file
                    await _file.WriteBinaryContentsToFile(_database.FileAccessToken, contents);
                }
                catch (Exception exception)
                {
                    throw new SaveException(exception);
                }
            }
        }
    }
}