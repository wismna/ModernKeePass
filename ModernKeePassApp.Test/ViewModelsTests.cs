using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage.AccessCache;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePass.ViewModels;
using ModernKeePassApp.Test.Mock;

namespace ModernKeePassApp.Test
{
    [TestClass]
    public class ViewModelsTests
    {
        [TestMethod]
        public void TestAboutVm()
        {
            var aboutVm = new AboutVm(Package.Current);
            Assert.AreEqual("1.0", aboutVm.Version);
        }

        [TestMethod]
        public void TestMainVm()
        {
            var database = new DatabaseHelperMock();
            var mainVm = new MainVm(null, null, database);
            Assert.AreEqual(1, mainVm.MainMenuItems.Count());
            var firstGroup = mainVm.MainMenuItems.FirstOrDefault();
            Assert.AreEqual(5, firstGroup.Count());

            database.Status = 1;
            mainVm = new MainVm(null, null, database);
            Assert.IsNotNull(mainVm.SelectedItem);
            Assert.AreEqual("Open", ((MainMenuItemVm) mainVm.SelectedItem).Title);

            database.Status = 2;
            mainVm = new MainVm(null, null, database);
            Assert.IsNotNull(mainVm.SelectedItem);
            Assert.AreEqual(2, mainVm.MainMenuItems.Count());
            Assert.AreEqual("Save", ((MainMenuItemVm) mainVm.SelectedItem).Title);
        }

        [TestMethod]
        public void TestCompositeKeyVm()
        {
            var database = new DatabaseHelperMock();
            var compositeKeyVm = new CompositeKeyVm(database);
            Assert.IsTrue(compositeKeyVm.OpenDatabase(false).GetAwaiter().GetResult());
            compositeKeyVm.StatusType = 1;
            compositeKeyVm.Password = "test";
            Assert.AreEqual(0, compositeKeyVm.StatusType);
            Assert.AreEqual(15.0, compositeKeyVm.PasswordComplexityIndicator);
        }

        [TestMethod]
        public void TestOpenVm()
        {
            var database = new DatabaseHelperMock
            {
                Status = 1,
                DatabaseFile = Package.Current.InstalledLocation.GetFileAsync(@"Databases\TestDatabase.kdbx")
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
            mru.Add(Package.Current.InstalledLocation.GetFileAsync(@"Databases\TestDatabase.kdbx")
                .GetAwaiter().GetResult(), "MockDatabase");
            var recentVm = new RecentVm();
            Assert.IsTrue(recentVm.RecentItems.Count == 1);
            recentVm.SelectedItem = recentVm.RecentItems.FirstOrDefault();
            Assert.IsTrue(recentVm.SelectedItem.IsSelected);
            mru.Clear();
        }

        /*[TestMethod]
        public void TestSaveVm()
        {
        }*/

        [TestMethod]
        public void TestSettingsVm()
        {
            var settingsVm = new SettingsVm();
            Assert.AreEqual(1, settingsVm.MenuItems.Count());
            var firstGroup = settingsVm.MenuItems.FirstOrDefault();
            Assert.AreEqual(2, firstGroup.Count());
            Assert.IsNotNull(settingsVm.SelectedItem);
            var selectedItem = (ListMenuItemVm) settingsVm.SelectedItem;
            Assert.AreEqual("General", selectedItem.Title);
        }
    }
}
