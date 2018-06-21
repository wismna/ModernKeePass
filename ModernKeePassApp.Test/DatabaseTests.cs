using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;
using ModernKeePassApp.Test.Mock;

namespace ModernKeePassApp.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private readonly DatabaseService _database = new DatabaseService(new SettingsServiceMock());

        [TestMethod]
        public void TestCreate()
        {
            Assert.IsTrue(_database.IsClosed);
            var databaseFile = ApplicationData.Current.TemporaryFolder.CreateFileAsync("NewDatabase.kdbx").GetAwaiter().GetResult();
            OpenOrCreateDatabase(databaseFile, true);
            _database.Close();
            Assert.IsTrue(_database.IsClosed);
        }

        [TestMethod]
        public void TestOpen()
        {
            Assert.IsTrue(_database.IsClosed);
           var databaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Data\TestDatabase.kdbx").GetAwaiter().GetResult();
            OpenOrCreateDatabase(databaseFile, false);
        }

        [TestMethod]
        public void TestSave()
        {
            TestOpen();
            _database.Save(ApplicationData.Current.TemporaryFolder.CreateFileAsync("SaveDatabase.kdbx").GetAwaiter().GetResult());
            Assert.IsTrue(_database.IsOpen);
            _database.Close();
            Assert.IsTrue(_database.IsClosed);
            TestOpen();
        }

        private void OpenOrCreateDatabase(StorageFile databaseFile, bool createNew)
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => _database.Open(databaseFile, null, createNew));
            var compositeKey = new CompositeKeyVm(_database, new ResourceServiceMock())
            {
                HasPassword = true,
                Password = "test"
            };
            compositeKey.OpenDatabase(databaseFile, createNew).GetAwaiter().GetResult();
            Assert.IsTrue(_database.IsOpen);
        }
    }
}
