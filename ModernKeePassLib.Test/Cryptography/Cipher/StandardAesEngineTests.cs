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
        private readonly byte[] _pbReferenceCt = 
        {
            0x75, 0xD1, 0x1B, 0x0E, 0x3A, 0x68, 0xC4, 0x22,
            0x3D, 0x88, 0xDB, 0xF0, 0x17, 0x97, 0x7D, 0xD7
        };
        private readonly byte[] _pbIv = new byte[16];
        private readonly byte[] _pbTestKey = new byte[32];
        private readonly byte[] _pbTestData =
        {
            0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };

        [TestMethod]
        public void TestEncryptStream()
        {
            using (var outStream = new MemoryStream(new byte[16]))
            {
                var aes = new StandardAesEngine();
                using (var inStream = aes.EncryptStream(outStream, _pbTestKey, _pbIv))
                {
                    new BinaryWriter(inStream).Write(_pbTestData);
                    Assert.AreEqual(16, outStream.Position);
                    outStream.Position = 0;
                    var outBytes = new BinaryReaderEx(outStream, Encoding.UTF8, string.Empty).ReadBytes(16);
                    Assert.IsTrue(MemUtil.ArraysEqual(outBytes, _pbReferenceCt));
                }
            }
        }

        [TestMethod]
        public void TestDecryptStream()
        {
            // Possible Mono Bug? This only works with size >= 48
            using (var inStream = new MemoryStream(new byte[32]))
            {
                inStream.Write(_pbReferenceCt, 0, _pbReferenceCt.Length);
                inStream.Position = 0;
                var aes = new StandardAesEngine();
                using (var outStream = aes.DecryptStream(inStream, _pbTestKey, _pbIv))
                {
                    var outBytes = new BinaryReaderEx(outStream, Encoding.UTF8, string.Empty).ReadBytes(16);
                    Assert.IsTrue(MemUtil.ArraysEqual(outBytes, _pbTestData));
                }
            }
        }

        [TestMethod]
        public void TestBouncyCastleAes()
        {
            var aesEngine = new AesEngine();
            //var parametersWithIv = new ParametersWithIV(new KeyParameter(pbTestKey), pbIV);
            aesEngine.Init(true, new KeyParameter(_pbTestKey));
            Assert.AreEqual(aesEngine.GetBlockSize(), _pbTestData.Length);
            aesEngine.ProcessBlock(_pbTestData, 0, _pbTestData, 0);
            Assert.IsTrue(MemUtil.ArraysEqual(_pbTestData, _pbReferenceCt));
        }
    }
}
