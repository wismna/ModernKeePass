using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePass.Services;
using ModernKeePass.ViewModels;

namespace ModernKeePassApp.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private readonly DatabaseService _database = new DatabaseService();

        [TestMethod]
        public void TestCreate()
        {
            Assert.AreEqual((int) DatabaseService.DatabaseStatus.Closed, _database.Status);
            _database.DatabaseFile = ApplicationData.Current.TemporaryFolder.CreateFileAsync("NewDatabase.kdbx").GetAwaiter().GetResult();
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Opening, _database.Status);
            OpenOrCreateDatabase(true);
            _database.Close();
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Closed, _database.Status);
        }

        [TestMethod]
        public void TestOpen()
        {
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Closed, _database.Status);
            _database.DatabaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Databases\TestDatabase.kdbx").GetAwaiter().GetResult();
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Opening, _database.Status);
            OpenOrCreateDatabase(false);
        }

        [TestMethod]
        public void TestSave()
        {
            TestOpen();
            _database.Save(ApplicationData.Current.TemporaryFolder.CreateFileAsync("SaveDatabase.kdbx").GetAwaiter().GetResult());
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Opened, _database.Status);
            _database.Close();
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Closed, _database.Status);
            TestOpen();
        }

        private void OpenOrCreateDatabase(bool createNew)
        {
            _database.Open(null, createNew);
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.NoCompositeKey, _database.Status);
            var compositeKey = new CompositeKeyVm(_database)
            {
                HasPassword = true,
                Password = "test"
            };
            compositeKey.OpenDatabase(createNew);
            Assert.AreEqual((int)DatabaseService.DatabaseStatus.Opened, _database.Status);
        }
    }
}
