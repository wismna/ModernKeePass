using ModernKeePass.ViewModels;
using NUnit.Framework;
using Windows.ApplicationModel;

namespace ModernKeePassApp.ViewModelTest
{
    [TestFixture]
    public class AboutViewModelTests
    {
        [Test]
        public void Should_Display_App_Version()
        {
            var aboutVm = new AboutViewModel(Package.Current);
            Assert.AreEqual("1.0", aboutVm.Version);
        }
    }
}