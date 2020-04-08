using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Infrastructure.KeePass;
using NSubstitute;
using NUnit.Framework;

namespace ModernKeePass.KeePassDatabaseTests
{
    [TestFixture]
    public class DatabaseTests
    {
        private IDatabaseProxy _database;
        private IFileProxy _fileProxy;
        private readonly Credentials _credentials = new Credentials
        {
            Password = "test"
        };


        [SetUp]
        public void SetUp()
        {
            var dateTime = Substitute.For<IDateTime>();
            _fileProxy = Substitute.For<IFileProxy>();
            _fileProxy.OpenBinaryFile(Arg.Any<string>()).Returns(async parameters =>
            {
                await using var stream = File.Open((string) parameters[0], FileMode.OpenOrCreate);
                var contents = new byte[stream.Length];
                await stream.ReadAsync(contents, 0, (int) stream.Length);
                return contents;
            });
            _fileProxy.WriteBinaryContentsToFile(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(async parameters =>
            {
                await using var stream = File.Open((string)parameters[0], FileMode.OpenOrCreate);
                var contents = (byte[]) parameters[1];
                await stream.WriteAsync(contents, 0, contents.Length);
            });
            var mapper = new Mapper(new MapperConfiguration(cfg => { cfg.AddProfile(typeof(EntryMappingProfile)); }));
            _database = new KeePassDatabaseClient(mapper, dateTime);
        }

        [TearDown]
        public void TearDown()
        {
            _database.CloseDatabase();
            //if (!string.IsNullOrEmpty(_fileInfo?.Path)) File.Delete(_fileInfo.Path);
        }

        [Test]
        public async Task TestOpen()
        {
            var file = await _fileProxy.OpenBinaryFile(Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx"));

            await _database.Open(file, _credentials);
            var rootGroup = _database.GetGroup(_database.RootGroupId);

            Assert.That(_database.Name, Is.EqualTo("TestDatabase"));
            Assert.That(rootGroup.Entries.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task TestCreateAndSave()
        {
            var path = Path.Combine(Path.GetTempPath(), "NewDatabase.kdbx");
            var newFile = await _fileProxy.OpenBinaryFile(path);

            await _database.Create(_credentials, "NewDatabase");
            var result = await _database.SaveDatabase();
            await _fileProxy.WriteBinaryContentsToFile(path, result);
            _database.CloseDatabase();

            Assert.DoesNotThrowAsync(async () => { await _database.Open(newFile, _credentials); });
        }
        
        [Test]
        public async Task TestSaveAs()
        {
            var currentPath = Path.Combine(Path.GetTempPath(), "SavedDatabase.kdbx");
            var originalFile = await _fileProxy.OpenBinaryFile(Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx"));
            var currentFile = await _fileProxy.OpenBinaryFile(currentPath);
            await _database.Open(originalFile, _credentials);

            var result = await _database.SaveDatabase(currentFile);
            await _fileProxy.WriteBinaryContentsToFile(currentPath, result);
            _database.CloseDatabase();

            Assert.DoesNotThrowAsync(async () => { await _database.Open(currentFile, _credentials); });
        }
        
        [Test]
        public async Task TestAddGroup()
        {
            var currentPath = Path.Combine(Path.GetTempPath(), "SavedDatabase.kdbx");
            var originalFile = await _fileProxy.OpenBinaryFile(Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx"));
            var currentFile = await _fileProxy.OpenBinaryFile(currentPath);
            var newGroup = new GroupEntity {Name = "New Group Test"};
            await _database.Open(originalFile, _credentials);

            await _database.AddGroup(_database.RootGroupId, newGroup.Id);
            var result = await _database.SaveDatabase(currentFile);
            await _fileProxy.WriteBinaryContentsToFile(currentPath, result);
            _database.CloseDatabase();
            
            await _database.Open(currentFile, _credentials);
            var rootGroup = _database.GetGroup(_database.RootGroupId);

            Assert.That(newGroup.Id, Is.Not.Empty);
            Assert.That(rootGroup.SubGroups.Count, Is.EqualTo(7));
            Assert.That(rootGroup.SubGroups.Last().Name, Is.EqualTo("New Group Test"));
        }
        
        [Test]
        public async Task TestAddEntry()
        {
            var currentPath = Path.Combine(Path.GetTempPath(), "SavedDatabase.kdbx");
            var newEntry = new EntryEntity
            {
                Name = "New Entry Test"
            };
            var originalFile = await _fileProxy.OpenBinaryFile(Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx"));
            var currentFile = await _fileProxy.OpenBinaryFile(currentPath);
            await _database.Open(originalFile, _credentials);

            await _database.AddEntry(_database.RootGroupId, newEntry.Id);
            var result = await _database.SaveDatabase(currentFile);
            await _fileProxy.WriteBinaryContentsToFile(currentPath, result);
            _database.CloseDatabase();

            await _database.Open(currentFile, _credentials);
            var rootGroup = _database.GetGroup(_database.RootGroupId);

            Assert.That(newEntry.Id, Is.Not.Empty);
            Assert.That(rootGroup.Entries.Count, Is.EqualTo(3));
            Assert.That(rootGroup.Entries.Last().Name, Is.EqualTo("New Entry Test"));
        }
    }
}
