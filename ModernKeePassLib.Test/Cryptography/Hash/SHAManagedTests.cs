using ModernKeePassLib.Cryptography.Hash;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography.Hash
{
    [TestFixture]
    public class SHAManagedTests
    {
        [Test]
        public void TestSha256ComputeHash()
        {
            var expectedHash = "B822F1CD2DCFC685B47E83E3980289FD5D8E3FF3A82DEF24D7D1D68BB272EB32";
            var message = StrUtil.Utf8.GetBytes("testing123");
            using (var result = new SHA256Managed())
            {
                Assert.That(ByteToString(result.ComputeHash(message)), Is.EqualTo(expectedHash));
            }
        }

        [Test]
        public void TestSha512ComputeHash()
        {
            var expectedHash = "4120117B3190BA5E24044732B0B09AA9ED50EB1567705ABCBFA78431A4E0A96B1152ED7F4925966B1C82325E186A8100E692E6D2FCB6702572765820D25C7E9E";
            var message = StrUtil.Utf8.GetBytes("testing123");
            using (var result = new SHA512Managed())
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
    }
}