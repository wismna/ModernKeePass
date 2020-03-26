using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Services;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Infrastructure.KeePass;
using NSubstitute;
using NUnit.Framework;
using FileInfo = ModernKeePass.Domain.Dtos.FileInfo;

namespace ModernKeePass.KeePassDatabaseTests
{
    [TestFixture]
    public class DatabaseTests
    {
        private IDatabaseProxy _database;
        private FileInfo _fileInfo;
        private readonly Credentials _credentials = new Credentials
        {
            Password = "test"
        };

        [SetUp]
        public void SetUp()
        {
            var settingsService = Substitute.For<ISettingsService>();
            var fileProxy = Substitute.For<IFileProxy>();
            fileProxy.OpenBinaryFile(Arg.Any<string>()).Returns(async parameters =>
            {
                await using var stream = File.Open((string) parameters[0], FileMode.OpenOrCreate);
                var contents = new byte[stream.Length];
                await stream.ReadAsync(contents, 0, (int) stream.Length);
                return contents;
            });
            fileProxy.WriteBinaryContentsToFile(Arg.Any<string>(), Arg.Any<byte[]>()).Returns(async parameters =>
            {
                await using var stream = File.Open((string)parameters[0], FileMode.OpenOrCreate);
                var contents = (byte[]) parameters[1];
                await stream.WriteAsync(contents, 0, contents.Length);
            });
            var fileService = new FileService(fileProxy);
            var mapper = new Mapper(new MapperConfiguration(cfg => { cfg.AddProfile(typeof(EntryMappingProfile)); }));
            _database = new KeePassDatabaseClient(settingsService, fileService, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _database.CloseDatabase();
            if (!string.IsNullOrEmpty(_fileInfo?.Path)) File.Delete(_fileInfo.Path);
        }

        [Test]
        public async Task TestOpen()
        {
            var FileInfo = new FileInfo
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx")
            };

            var rootGroup = await _database.Open(FileInfo, _credentials);
            Assert.That(rootGroup.Name, Is.EqualTo("TestDatabase"));
            Assert.That(rootGroup.Entries.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task TestCreateAndSave()
        {
            _fileInfo = new FileInfo
            {
                Path = Path.Combine(Path.GetTempPath(), "NewDatabase.kdbx")
            };

            await _database.Create(_fileInfo, _credentials);
            await _database.SaveDatabase();
            _database.CloseDatabase();

            Assert.DoesNotThrowAsync(async () => { await _database.Open(_fileInfo, _credentials); });
        }
        
        [Test]
        public async Task TestSaveAs()
        {
            var originalFileInfo = new FileInfo
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx")
            };
            _fileInfo = new FileInfo
            {
                Path = Path.Combine(Path.GetTempPath(), "SavedDatabase.kdbx")
            };

            await _database.Open(originalFileInfo, _credentials);
            await _database.SaveDatabase(_fileInfo);
            _database.CloseDatabase();

            Assert.DoesNotThrowAsync(async () => { await _database.Open(_fileInfo, _credentials); });
        }
        
        [Test]
        public async Task TestAddGroup()
        {
            var originalFileInfo = new FileInfo
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx")
            };
            _fileInfo = new FileInfo
            {
                Path = Path.Combine(Path.GetTempPath(), "SavedDatabase.kdbx")
            };
            var newGroup = new GroupEntity {Name = "New Group Test"};

            var rootGroup = await _database.Open(originalFileInfo, _credentials);
            await _database.AddEntity(rootGroup, newGroup);
            await _database.SaveDatabase(_fileInfo);
            _database.CloseDatabase();
            rootGroup = await _database.Open(_fileInfo, _credentials);

            Assert.That(newGroup.Id, Is.Not.Empty);
            Assert.That(rootGroup.SubGroups.Count, Is.EqualTo(7));
            Assert.That(rootGroup.SubGroups.Last().Name, Is.EqualTo("New Group Test"));
        }
        
        [Test]
        public async Task TestAddEntry()
        {
            var originalFileInfo = new FileInfo
            {
                Path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TestDatabase.kdbx")
            };
            _fileInfo = new FileInfo
            {
                Path = Path.Combine(Path.GetTempPath(), "SavedDatabase.kdbx")
            };
            var newEntry = new EntryEntity
            {
                Name = "New Entry Test"
            };

            var rootGroup = await _database.Open(originalFileInfo, _credentials);
            await _database.AddEntity(rootGroup, newEntry);
            await _database.SaveDatabase(_fileInfo);
            _database.CloseDatabase();
            rootGroup = await _database.Open(_fileInfo, _credentials);

            Assert.That(newEntry.Id, Is.Not.Empty);
            Assert.That(rootGroup.Entries.Count, Is.EqualTo(3));
            Assert.That(rootGroup.Entries.Last().Name, Is.EqualTo("New Entry Test"));
        }
    }
}
