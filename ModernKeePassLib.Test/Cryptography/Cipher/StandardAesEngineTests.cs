using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Utility;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Test.Cryptography.Cipher
{
    [TestClass()]
    public class StandardAesEngineTests
    {
        // Test vector (official ECB test vector #356)
        private byte[] pbReferenceCT = new byte[16]
        {
            0x75, 0xD1, 0x1B, 0x0E, 0x3A, 0x68, 0xC4, 0x22,
            0x3D, 0x88, 0xDB, 0xF0, 0x17, 0x97, 0x7D, 0xD7
        };

        [TestMethod]
        public void TestEncryptStream()
        {
            byte[] pbIV = new byte[16];
            byte[] pbTestKey = new byte[32];
            byte[] pbTestData = new byte[16];
            pbTestData[0] = 0x04;

            var outStream = new MemoryStream(new byte[16]);
            var aes = new StandardAesEngine();
            var inStream = aes.EncryptStream(outStream, pbTestKey, pbIV);
            new BinaryWriter(inStream).Write(pbTestData);
            Assert.AreEqual(16, outStream.Position);
            outStream.Position = 0;
            var outBytes = new BinaryReaderEx(outStream, Encoding.UTF8, string.Empty).ReadBytes(16);
            Assert.IsTrue(MemUtil.ArraysEqual(outBytes, pbReferenceCT));
        }

        [TestMethod]
        public void TestDecryptStream()
    {
            byte[] pbIV = new byte[16];
            byte[] pbTestKey = new byte[32];
            byte[] pbTestData = new byte[16];
            pbTestData[0] = 0x04;

            // Possible Mono Bug? This only works with size >= 48
            var inStream = new MemoryStream(new byte[32]);
            inStream.Write(pbReferenceCT, 0, pbReferenceCT.Length);
            inStream.Position = 0;
            var aes = new StandardAesEngine();
            var outStream = aes.DecryptStream(inStream, pbTestKey, pbIV);
            var outBytes = new BinaryReaderEx(outStream, Encoding.UTF8, string.Empty).ReadBytes(16);
            Assert.IsTrue(MemUtil.ArraysEqual(outBytes, pbTestData));
        }

        [TestMethod]
        public void TestBouncyCastleAes()
        {
            byte[] pbIV = new byte[16];
            byte[] pbTestKey = new byte[32];
            byte[] pbTestData = new byte[16];
            /*int i;
            for (i = 0; i < 16; ++i) pbIV[i] = 0;
            for (i = 0; i < 32; ++i) pbTestKey[i] = 0;
            for (i = 0; i < 16; ++i) pbTestData[i] = 0;*/
            pbTestData[0] = 0x04;

            var aesEngine = new AesEngine();
            //var parametersWithIv = new ParametersWithIV(new KeyParameter(pbTestKey), pbIV);
            aesEngine.Init(true, new KeyParameter(pbTestKey));
            Assert.AreEqual(aesEngine.GetBlockSize(), pbTestData.Length);
            aesEngine.ProcessBlock(pbTestData, 0, pbTestData, 0);
            Assert.IsTrue(MemUtil.ArraysEqual(pbTestData, pbReferenceCT));
        }
    }
}
