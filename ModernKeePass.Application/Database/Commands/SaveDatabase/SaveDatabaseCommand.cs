using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            private readonly ILogger _logger;

            public SaveDatabaseCommandHandler(IDatabaseProxy database, IFileProxy file, ILogger logger)
            {
                _database = database;
                _file = file;
                _logger = logger;
            }

            public async Task Handle(SaveDatabaseCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                try
                {
                    var timeToSave = Stopwatch.StartNew();
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
                    timeToSave.Stop();

                    _logger.LogTrace("SaveCommand", new Dictionary<string, string>
                    {
                        { "duration", timeToSave.ElapsedMilliseconds.ToString()},
                        { "size", _database.Size.ToString()}
                    });
                }
                catch (Exception exception)
                {
                    throw new SaveException(exception);
                }
            }
        }
    }
}