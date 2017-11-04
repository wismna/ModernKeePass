using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Cryptography.Hash;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography.Hash
{
    [TestFixture]
    public class HmacTests
    {
        [Test]
        public void TestHmac1()
        {
            // Test vectors from RFC 4231

            var pbKey = new byte[20];
            for (var i = 0; i < pbKey.Length; ++i) pbKey[i] = 0x0B;
            var pbMsg = StrUtil.Utf8.GetBytes("Hi There");
            var pbExpc = new byte[]
            {
                0xB0, 0x34, 0x4C, 0x61, 0xD8, 0xDB, 0x38, 0x53,
                0x5C, 0xA8, 0xAF, 0xCE, 0xAF, 0x0B, 0xF1, 0x2B,
                0x88, 0x1D, 0xC2, 0x00, 0xC9, 0x83, 0x3D, 0xA7,
                0x26, 0xE9, 0x37, 0x6C, 0x2E, 0x32, 0xCF, 0xF7
            };
            HmacEval(pbKey, pbMsg, pbExpc);
        }

        [Test]
        public void TestHmac2()
        {
            var pbKey = new byte[131];
            for (var i = 0; i < pbKey.Length; ++i) pbKey[i] = 0xAA;
            var pbMsg = StrUtil.Utf8.GetBytes(
                "This is a test using a larger than block-size key and " +
                "a larger than block-size data. The key needs to be " +
                "hashed before being used by the HMAC algorithm.");
            var pbExpc = new byte[] {
                0x9B, 0x09, 0xFF, 0xA7, 0x1B, 0x94, 0x2F, 0xCB,
                0x27, 0x63, 0x5F, 0xBC, 0xD5, 0xB0, 0xE9, 0x44,
                0xBF, 0xDC, 0x63, 0x64, 0x4F, 0x07, 0x13, 0x93,
                0x8A, 0x7F, 0x51, 0x53, 0x5C, 0x3A, 0x35, 0xE2
            };
            HmacEval(pbKey, pbMsg, pbExpc);
        }

        [Test]
        public void TestHmacSha1ComputeHash()
        {
            var expectedHash = "AC2C2E614882CE7158F69B7E3B12114465945D01";
            var message = StrUtil.Utf8.GetBytes("testing123");
            var key = StrUtil.Utf8.GetBytes("hello");
            using (var result = new HMACSHA1(key))
            {
                Assert.That(ByteToString(result.ComputeHash(message)), Is.EqualTo(expectedHash));
            }
        }

        [Test]
        public void TestHmacSha256ComputeHash()
        {
            var expectedHash = "09C1BD2DE4E5659C0EFAF9E6AE4723E9CF96B69609B4E562F6AFF1745D7BF4E0";
            var message = StrUtil.Utf8.GetBytes("testing123");
            var key = StrUtil.Utf8.GetBytes("hello");
            using (var result = new HMACSHA256(key))
            {
                Assert.That(ByteToString(result.ComputeHash(message)), Is.EqualTo(expectedHash));
            }
        }

        private static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        [Test]
        public void TestHmacOtp()
        {
            var pbSecret = StrUtil.Utf8.GetBytes("12345678901234567890");
            var vExp = new []{ "755224", "287082", "359152",
                "969429", "338314", "254676", "287922", "162583", "399871",
                "520489" };

            for (var i = 0; i < vExp.Length; ++i)
            {
                Assert.That(HmacOtp.Generate(pbSecret, (ulong)i, 6, false, -1), Is.EqualTo(vExp[i]));
            }
        }

        private static void HmacEval(byte[] pbKey, byte[] pbMsg,
            byte[] pbExpc)
        {
            using (var h = new HMACSHA256(pbKey))
            {
                h.TransformBlock(pbMsg, 0, pbMsg.Length, pbMsg, 0);
                h.TransformFinalBlock(MemUtil.EmptyByteArray, 0, 0);

                byte[] pbHash = h.Hash;
                Assert.That(MemUtil.ArraysEqual(pbHash, pbExpc), Is.True);

                // Reuse the object
                h.Initialize();
                h.TransformBlock(pbMsg, 0, pbMsg.Length, pbMsg, 0);
                h.TransformFinalBlock(MemUtil.EmptyByteArray, 0, 0);

                pbHash = h.Hash;
                Assert.That(MemUtil.ArraysEqual(pbHash, pbExpc), Is.True);
            }
        }
    }
}