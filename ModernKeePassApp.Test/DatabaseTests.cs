using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;

namespace ModernKeePassApp.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();

        [TestMethod]
        public void TestCreate()
        {
            Assert.AreEqual((int) DatabaseHelper.DatabaseStatus.Closed, _database.Status);
            _database.DatabaseFile = ApplicationData.Current.TemporaryFolder.CreateFileAsync("NewDatabase.kdbx").GetAwaiter().GetResult();
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.Opening, _database.Status);
            OpenOrCreateDatabase(true);
            _database.Close();
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.Closed, _database.Status);
        }

        [TestMethod]
        public void TestOpen()
        {
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.Closed, _database.Status);
            _database.DatabaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Databases\TestDatabase.kdbx").GetAwaiter().GetResult();
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.Opening, _database.Status);
            OpenOrCreateDatabase(false);
        }

        [TestMethod]
        public void TestSave()
        {
            TestOpen();
            Assert.IsTrue(_database.Save(ApplicationData.Current.TemporaryFolder.CreateFileAsync("SaveDatabase.kdbx").GetAwaiter().GetResult()));
            _database.Close();
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.Closed, _database.Status);
            TestOpen();
        }

        private void OpenOrCreateDatabase(bool createNew)
        {
            _database.Open(null, createNew);
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.NoCompositeKey, _database.Status);
            var compositeKey = new CompositeKeyVm(_database)
            {
                HasPassword = true,
                Password = "test"
            };
            compositeKey.OpenDatabase(createNew);
            Assert.AreEqual((int)DatabaseHelper.DatabaseStatus.Opened, _database.Status);
        }
    }
}
