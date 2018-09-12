using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage.AccessCache;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePass.ViewModels;
using ModernKeePass.Views;
using ModernKeePassApp.Test.Mock;
using ModernKeePassLib;

namespace ModernKeePassApp.Test
{
    [TestClass]
    public class ViewModelsTests
    {
        private readonly RecentServiceMock _recent = new RecentServiceMock();
        private readonly ResourceServiceMock _resource = new ResourceServiceMock();

        [TestMethod]
        public void TestAboutVm()
        {
            var aboutVm = new AboutVm(Package.Current);
            Assert.AreEqual("1.0", aboutVm.Version);
        }

        [TestMethod]
        public void TestMainVm()
        {
            var database = new DatabaseServiceMock();

            var mainVm = new MainVm(null, null, database, _resource, _recent);
            Assert.AreEqual(1, mainVm.MainMenuItems.Count());
            var firstGroup = mainVm.MainMenuItems.FirstOrDefault();
            Assert.AreEqual(7, firstGroup?.Count());

            var databaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Data\TestDatabase.kdbx")
                .GetAwaiter().GetResult();
            mainVm = new MainVm(null, null, database, _resource, _recent, databaseFile);
            Assert.IsNotNull(mainVm.SelectedItem);
            Assert.AreEqual(typeof(OpenDatabasePage), ((MainMenuItemVm) mainVm.SelectedItem).PageType);

            database.Open(databaseFile, null);
            mainVm = new MainVm(null, null, database, _resource, _recent);
            Assert.IsNotNull(mainVm.SelectedItem);
            Assert.AreEqual(2, mainVm.MainMenuItems.Count());
            Assert.AreEqual(typeof(SaveDatabasePage), ((MainMenuItemVm) mainVm.SelectedItem).PageType);
        }

        [TestMethod]
        public void TestCompositeKeyVm()
        {
            var database = new DatabaseServiceMock();
            var compositeKeyVm = new CompositeKeyVm(database, _resource);
            Assert.IsTrue(compositeKeyVm.OpenDatabase(null, false).GetAwaiter().GetResult());
            compositeKeyVm.StatusType = 1;
            compositeKeyVm.Password = "test";
            Assert.AreEqual(0, compositeKeyVm.StatusType);
            Assert.AreEqual(15.0, compositeKeyVm.PasswordComplexityIndicator);
        }

        [TestMethod]
        public void TestOpenVm()
        {
            var databaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Data\TestDatabase.kdbx")
                .GetAwaiter().GetResult();
            var openVm = new OpenVm();
            Assert.IsFalse(openVm.IsFileSelected);
            openVm.OpenFile(databaseFile, new RecentServiceMock());
            Assert.AreEqual("TestDatabase.kdbx", openVm.Name);
        }

        /*[TestMethod]
        public void TestNewVm()
        {
        }*/

        [TestMethod]
        public void TestRecentVm()
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            mru.Add(Package.Current.InstalledLocation.GetFileAsync(@"Data\TestDatabase.kdbx")
                .GetAwaiter().GetResult(), "MockDatabase");
            var recentVm = new RecentVm();
            Assert.IsTrue(recentVm.RecentItems.Count == 1);
            recentVm.SelectedItem = recentVm.RecentItems.FirstOrDefault() as RecentItemVm;
            Assert.IsTrue(recentVm.SelectedItem.IsSelected);
            mru.Clear();
        }

        [TestMethod]
        public void TestSaveVm()
        {
            var database = new DatabaseServiceMock();
            var saveVm = new SaveVm(database);
            database.Open(null, null);
            saveVm.Save(false);
            Assert.IsTrue(database.IsOpen);
            saveVm.Save();
            Assert.IsFalse(database.IsOpen);
        }

        [TestMethod]
        public void TestSettingsVm()
        {
            var settingsVm = new SettingsVm(new DatabaseServiceMock(), _resource);
            Assert.AreEqual(1, settingsVm.MenuItems.Count());
            var firstGroup = settingsVm.MenuItems.FirstOrDefault();
            // All groups have an empty title, so all settings are put inside the empty group
            Assert.AreEqual(4, firstGroup?.Count());
            Assert.IsNotNull(settingsVm.SelectedItem);
            var selectedItem = (ListMenuItemVm) settingsVm.SelectedItem;
            Assert.AreEqual(typeof(SettingsNewDatabasePage), selectedItem.PageType);
        }

        [TestMethod]
        public void TestEntryVm()
        {
            var database = new DatabaseServiceMock();
            var entryVm = new EntryVm(new PwEntry(true, true), new GroupVm(), database, _resource)
            {
                Name = "Test",
                UserName = "login",
                Password = "password"
            };
        }

        [TestMethod]
        public void TestGroupVm()
        {
            var database = new DatabaseServiceMock();
            var entryVm = new GroupVm(new PwGroup(true, true), new GroupVm(), database)
            {
                Name = "Test"
            };
        }
    }
}
