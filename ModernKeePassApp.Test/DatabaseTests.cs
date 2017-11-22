using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePass.Common;
using ModernKeePass.ViewModels;
using ModernKeePassLib.Keys;

namespace ModernKeePassApp.Test
{
    [TestClass]
    public class DatabaseTests
    {
        private readonly DatabaseHelper _database = new DatabaseHelper();

        [TestMethod]
        public void TestCreate()
        {
            Assert.AreEqual(_database.Status, (int) DatabaseHelper.DatabaseStatus.Closed);
            _database.DatabaseFile = ApplicationData.Current.TemporaryFolder.CreateFileAsync("NewDatabase.kdbx").GetAwaiter().GetResult();
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.Opening);
            OpenOrCreateDatabase(true);
            _database.Close();
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.Closed);
        }

        [TestMethod]
        public void TestOpen()
        {
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.Closed);
            _database.DatabaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Databases\TestDatabase.kdbx").GetAwaiter().GetResult();
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.Opening);
            OpenOrCreateDatabase(false);
        }

        [TestMethod]
        public void TestSave()
        {
            TestOpen();
            Assert.IsTrue(_database.Save(ApplicationData.Current.TemporaryFolder.CreateFileAsync("SaveDatabase.kdbx").GetAwaiter().GetResult()));
            _database.Close();
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.Closed);
            TestOpen();
        }

        private void OpenOrCreateDatabase(bool createNew)
        {
            _database.Open(null, createNew);
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.NoCompositeKey);
            var compositeKey = new CompositeKey();
            if (!createNew)
            {
                _database.Open(compositeKey);
                Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.CompositeKeyError);
            }
            compositeKey.AddUserKey(new KcpPassword("test"));

            _database.Open(compositeKey, createNew);
            /*var compositeKey = new CompositeKeyVm(_database);
            compositeKey.OpenDatabase(createNew);*/
            Assert.AreEqual(_database.Status, (int)DatabaseHelper.DatabaseStatus.Opened);
        }
    }
}
