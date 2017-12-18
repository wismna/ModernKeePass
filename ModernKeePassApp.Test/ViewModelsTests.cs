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
        private RecentServiceMock _recent = new RecentServiceMock();
        private ResourceServiceMock _resource = new ResourceServiceMock();

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
            Assert.AreEqual(7, firstGroup.Count());

            database.Status = 1;
            mainVm = new MainVm(null, null, database, _resource, _recent);
            Assert.IsNotNull(mainVm.SelectedItem);
            Assert.AreEqual(typeof(OpenDatabasePage), ((MainMenuItemVm) mainVm.SelectedItem).PageType);

            database.Status = 2;
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
            Assert.IsTrue(compositeKeyVm.OpenDatabase(false).GetAwaiter().GetResult());
            compositeKeyVm.StatusType = 1;
            compositeKeyVm.Password = "test";
            Assert.AreEqual(0, compositeKeyVm.StatusType);
            Assert.AreEqual(15.0, compositeKeyVm.PasswordComplexityIndicator);
        }

        [TestMethod]
        public void TestDonateVm()
        {
            var donateVm = new DonateVm(new LicenseServiceMock());
            Assert.AreEqual(4, donateVm.Donations.Count);
        }

        [TestMethod]
        public void TestOpenVm()
        {
            var database = new DatabaseServiceMock
            {
                Status = 1,
                DatabaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Data\TestDatabase.kdbx")
                    .GetAwaiter().GetResult()
            };
            var openVm = new OpenVm(database);
            Assert.IsTrue(openVm.ShowPasswordBox);
            Assert.AreEqual("MockDatabase", openVm.Name);
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
            var database = new DatabaseServiceMock
            {
                Status = 2
            };
            var saveVm = new SaveVm(database);
            saveVm.Save(false);
            Assert.AreEqual(2, database.Status);
            saveVm.Save();
            Assert.AreEqual(0, database.Status);
        }

        [TestMethod]
        public void TestSettingsVm()
        {
            var settingsVm = new SettingsVm(new DatabaseServiceMock(), _resource);
            Assert.AreEqual(1, settingsVm.MenuItems.Count());
            var firstGroup = settingsVm.MenuItems.FirstOrDefault();
            // All groups have an empty title, so all settings are put inside the empty group
            Assert.AreEqual(3, firstGroup.Count());
            Assert.IsNotNull(settingsVm.SelectedItem);
            var selectedItem = (ListMenuItemVm) settingsVm.SelectedItem;
            Assert.AreEqual(typeof(SettingsNewDatabasePage), selectedItem.PageType);
        }

        [TestMethod]
        public void TestEntryVm()
        {
            var database = new DatabaseServiceMock
            {
                Status = 2
            };
            var entryVm = new EntryVm(new PwEntry(true, true), new GroupVm(), database)
            {
                Name = "Test",
                UserName = "login",
                Password = "password"
            };
        }

        [TestMethod]
        public void TestGroupVm()
        {
            var database = new DatabaseServiceMock
            {
                Status = 2
            };
            var entryVm = new GroupVm(new PwGroup(true, true), new GroupVm(), database)
            {
                Name = "Test"
            };
        }
    }
}
