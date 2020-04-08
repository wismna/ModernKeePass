using MediatR;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Exceptions;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Database.Commands.CreateDatabase
{
    public class CreateDatabaseCommand : IRequest
    {
        public string FilePath { get; set; }
        public string Password { get; set; }
        public string KeyFilePath { get; set; }
        public string Name { get; set; }
        public bool CreateSampleData { get; set; }

        public class CreateDatabaseCommandHandler : IAsyncRequestHandler<CreateDatabaseCommand>
        {
            private readonly IDatabaseProxy _database;
            private readonly IFileProxy _file;

            public CreateDatabaseCommandHandler(IDatabaseProxy database, IFileProxy file)
            {
                _database = database;
                _file = file;
            }

            public async Task Handle(CreateDatabaseCommand message)
            {
                if (_database.IsOpen) throw new DatabaseOpenException();

                await _database.Create(new Credentials
                    {
                        KeyFileContents = !string.IsNullOrEmpty(message.KeyFilePath) ? await _file.OpenBinaryFile(message.KeyFilePath) : null,
                        Password = message.Password
                    }, message.Name);
                _database.FileAccessToken = message.FilePath;

                if (message.CreateSampleData)
                {
                    _database.CreateGroup(_database.RootGroupId, "Banking");
                    _database.CreateGroup(_database.RootGroupId, "Email");
                    _database.CreateGroup(_database.RootGroupId, "Internet");

                    var sample1 = _database.CreateEntry(_database.RootGroupId);
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Title, "Sample Entry" );
                    _database.UpdateEntry(sample1.Id, EntryFieldName.UserName, "Username" );
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Password, "Password" );
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Url, "https://keepass.info/" );
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Notes, "You may safely delete this sample" );

                    var sample2 = _database.CreateEntry(_database.RootGroupId);
                    _database.UpdateEntry(sample2.Id, EntryFieldName.Title, "Sample Entry #2" );
                    _database.UpdateEntry(sample2.Id, EntryFieldName.UserName, "Michael321" );
                    _database.UpdateEntry(sample2.Id, EntryFieldName.Password, "12345" );
                    _database.UpdateEntry(sample2.Id, EntryFieldName.Url, "https://keepass.info/help/kb/testform.html" );
                }
            }
        }
    }
}