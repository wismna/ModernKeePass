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
        public string Version { get; set; }
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
                if (_database.IsDirty) throw new DatabaseOpenException();

                var version = DatabaseVersion.V2;
                switch (message.Version)
                {
                    case "4":
                        version = DatabaseVersion.V4;
                        break;
                    case "3":
                        version = DatabaseVersion.V3;
                        break;
                }

                await _database.Create(new Credentials
                {
                    KeyFileContents = !string.IsNullOrEmpty(message.KeyFilePath) ? await _file.ReadBinaryFile(message.KeyFilePath) : null,
                    Password = message.Password
                }, message.Name, version);
                _database.FileAccessToken = message.FilePath;

                if (message.CreateSampleData)
                {
                    var bankingGroup = _database.CreateGroup(_database.RootGroupId, "Banking");
                    bankingGroup.Icon = Icon.Shop;
                    _database.UpdateGroup(bankingGroup);

                    var emailGroup = _database.CreateGroup(_database.RootGroupId, "Email");
                    emailGroup.Icon = Icon.Mail;
                    _database.UpdateGroup(emailGroup);

                    var internetGroup = _database.CreateGroup(_database.RootGroupId, "Internet");
                    internetGroup.Icon = Icon.World;
                    _database.UpdateGroup(internetGroup);

                    var sample1 = _database.CreateEntry(_database.RootGroupId);
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Title, "Sample Entry", false);
                    _database.UpdateEntry(sample1.Id, EntryFieldName.UserName, "Username", false);
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Password, "Password", true);
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Url, "https://keepass.info/", false);
                    _database.UpdateEntry(sample1.Id, EntryFieldName.Notes, "You may safely delete this sample", false);

                    var sample2 = _database.CreateEntry(_database.RootGroupId);
                    _database.UpdateEntry(sample2.Id, EntryFieldName.Title, "Sample Entry #2", false);
                    _database.UpdateEntry(sample2.Id, EntryFieldName.UserName, "Michael321", false);
                    _database.UpdateEntry(sample2.Id, EntryFieldName.Password, "12345", true);
                    _database.UpdateEntry(sample2.Id, EntryFieldName.Url, "https://keepass.info/help/kb/testform.html", false);
                }
            }
        }
    }
}