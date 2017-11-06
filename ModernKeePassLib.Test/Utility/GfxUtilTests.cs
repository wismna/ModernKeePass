using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePassLib.Utility;

namespace ModernKeePassLib.Test.Utility
{
    [TestClass ()]
    public class GfxUtilTests
    {
        // 16x16 all white PNG file, base64 encoded
        const string testImageData =
            "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAACXBIWXMAAAsTAAA" +
            "LEwEAmpwYAAAAB3RJTUUH3wMOFgIgmTCUMQAAABl0RVh0Q29tbWVudABDcmVhdG" +
            "VkIHdpdGggR0lNUFeBDhcAAAAaSURBVCjPY/z//z8DKYCJgUQwqmFUw9DRAABVb" +
            "QMdny4VogAAAABJRU5ErkJggg==";

        [TestMethod]
        public void TestLoadImage ()
        {
            var testData = Convert.FromBase64String (testImageData);
            var image = GfxUtil.ScaleImage(testData, 16, 16).GetAwaiter().GetResult();
            //var image = GfxUtil.LoadImage(testData);
            Assert.AreEqual(image.Width, 16);
            Assert.AreEqual(image.Height, 16);
        }
    }
}
