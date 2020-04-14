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
                    byte[] contents;
                    if (string.IsNullOrEmpty(message.FilePath))
                    {
                        contents = await _database.SaveDatabase();

                        // Test DB integrity before writing changes to file
                        _database.CloseDatabase();
                        var file = await _file.OpenBinaryFile(_database.FileAccessToken);
                        await _database.ReOpen(file);

                        await _file.WriteBinaryContentsToFile(_database.FileAccessToken, contents);
                    }
                    else
                    {
                        var newFileContents = await _file.OpenBinaryFile(message.FilePath);
                        contents = await _database.SaveDatabase(newFileContents);
                        await _file.WriteBinaryContentsToFile(message.FilePath, contents);

                        _file.ReleaseFile(_database.FileAccessToken);
                        _database.FileAccessToken = message.FilePath;
                    }

                    _database.IsDirty = false;
                }
                catch (ArgumentException exception)
                {
                    throw new SaveException(exception);
                }
            }
        }
    }
}