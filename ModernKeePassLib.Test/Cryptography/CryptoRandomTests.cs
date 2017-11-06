using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePassLib.Cryptography;

namespace ModernKeePassLib.Test.Cryptography
{
    [TestClass()]
    public class CryptoRandomTests
    {
        [TestMethod]
        public void TestAddEntropy()
        {
            // just making sure it does not throw an exception
            CryptoRandom.Instance.AddEntropy(new byte[1]);
        }

        [TestMethod]
        public void TestGetRandomBytes()
        {
            const int length = 32;
            var bytes1 = CryptoRandom.Instance.GetRandomBytes(length);
            Assert.AreEqual(bytes1.Length, length);
            var bytes2 = CryptoRandom.Instance.GetRandomBytes(length);
            Assert.AreNotEqual(bytes2, bytes1);
        }

        [TestMethod]
        public void TestGeneratedBytesCount()
        {
            const int length = 1;
            CryptoRandom.Instance.GetRandomBytes(length);
            var count1 = CryptoRandom.Instance.GeneratedBytesCount;
            CryptoRandom.Instance.GetRandomBytes(length);
            var count2 = CryptoRandom.Instance.GeneratedBytesCount;
            Assert.IsTrue(count2 > count1);
        }
    }
}

