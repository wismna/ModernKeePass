using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Exceptions;

namespace ModernKeePass.Application.Import.Commands.ImportFromCsv
{
    public class ImportFromCsvCommand : IRequest
    {
        public string FilePath { get; set; }
        public string DestinationGroupId { get; set; }
        public bool HasHeaderRow { get; set; }
        public char Delimiter { get; set; } = ';';
        public Dictionary<int, string> FieldMappings { get; set; }

        public class CreateDatabaseCommandHandler : IAsyncRequestHandler<ImportFromCsvCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IFileProxy _file;

            public CreateDatabaseCommandHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(ImportFromCsvCommand message)
            {
                if (!_database.IsOpen) throw new DatabaseClosedException();

                var fileContents = await _file.ReadTextFile(message.FilePath);
                
                for (var index = message.HasHeaderRow ? 1 : 0; index < fileContents.Count; index++)
                {
                    var line = fileContents[index];
                    var fields = line.Split(message.Delimiter);

                    var entry = _database.CreateEntry(message.DestinationGroupId);
                    for (var i = 0; i < fields.Length; i++)
                    {
                        var fieldMapping = message.FieldMappings[i];
                        await _database.UpdateEntry(entry.Id, fieldMapping, fields[i], fieldMapping == EntryFieldName.Password);
                    }
                }
            }
        }
    }
}