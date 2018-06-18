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
            _database.DatabaseFile = ApplicationData.Current.TemporaryFolder.CreateFileAsync("NewDatabase.kdbx").GetAwaiter().GetResult();
            Assert.IsTrue(_database.IsFileOpen);
            OpenOrCreateDatabase(true);
            _database.Close();
            Assert.IsTrue(_database.IsClosed);
        }

        [TestMethod]
        public void TestOpen()
        {
            Assert.IsTrue(_database.IsClosed);
            _database.DatabaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Data\TestDatabase.kdbx").GetAwaiter().GetResult();
            Assert.IsTrue(_database.IsFileOpen);
            OpenOrCreateDatabase(false);
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

        private void OpenOrCreateDatabase(bool createNew)
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => _database.Open(null, createNew));
            var compositeKey = new CompositeKeyVm(_database, new ResourceServiceMock())
            {
                HasPassword = true,
                Password = "test"
            };
            compositeKey.OpenDatabase(createNew).GetAwaiter().GetResult();
            Assert.IsTrue(_database.IsOpen);
        }
    }
}
