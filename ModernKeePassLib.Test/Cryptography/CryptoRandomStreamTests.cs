using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;

namespace ModernKeePassLib.Test.Cryptography
{
    [TestClass]
    public class CryptoRandomStreamTests
    {
        private void TestGetRandomBytes(CryptoRandomStream stream)
        {
            const uint length = 16;
            var bytes1 = stream.GetRandomBytes(length);
            Assert.AreEqual(bytes1.Length, (int)length);
            var bytes2 = stream.GetRandomBytes(length);
            Assert.IsFalse(MemUtil.ArraysEqual(bytes2, bytes1));
        }

        [TestMethod]
        public void TestGetRandomBytesCrsAlgorithmSalsa20()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.Salsa20, new byte[16]);
            TestGetRandomBytes(stream);
        }

        [TestMethod]
        public void TestGetRandomBytesCrsAlgorithmArcFourVariant()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.ArcFourVariant, new byte[16]);
            TestGetRandomBytes(stream);
        }

        private void TestGetRandomInt64(CryptoRandomStream stream)
        {
            var value1 = stream.GetRandomUInt64();
            var value2 = stream.GetRandomUInt64();
            Assert.AreNotEqual(value2, value1);
        }

        [TestMethod]
        public void TestGetRandomInt64AlgorithmSalsa20()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.Salsa20, new byte[16]);
            TestGetRandomInt64(stream);
        }

        [TestMethod]
        public void TestGetRandomInt64AlgorithmArcFourVariant()
        {
            var stream = new CryptoRandomStream(CrsAlgorithm.ArcFourVariant, new byte[16]);
            TestGetRandomInt64(stream);
        }
    }
}

