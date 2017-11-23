using System.Linq;
using Windows.ApplicationModel;
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
            Assert.AreEqual("Open", ((MainMenuItemVm)mainVm.SelectedItem).Title);

            database.Status = 2;
            mainVm = new MainVm(null, null, database);
            Assert.IsNotNull(mainVm.SelectedItem);
            Assert.AreEqual(2, mainVm.MainMenuItems.Count());
            Assert.AreEqual("Save", ((MainMenuItemVm)mainVm.SelectedItem).Title);
        }
    }
}
