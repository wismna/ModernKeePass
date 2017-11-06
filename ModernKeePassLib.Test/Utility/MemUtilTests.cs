using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;

namespace ModernKeePassLib.Test.Utility
{
    [TestClass]
    public class MemUtilTests
    {
        private byte[] _pb = CryptoRandom.Instance.GetRandomBytes((uint)CryptoRandom.NewWeakRandom().Next(0, 0x2FFFF));

        [TestMethod]
        public void TestGzip()
        {
            var pbCompressed = MemUtil.Compress(_pb);
            Assert.IsTrue(MemUtil.ArraysEqual(MemUtil.Decompress(pbCompressed), _pb));
        }

        [TestMethod]
        public void TestMemUtil()
        {
            Encoding enc = StrUtil.Utf8;
            _pb = enc.GetBytes("012345678901234567890a");
            byte[] pbN = enc.GetBytes("9012");
            Assert.AreEqual(MemUtil.IndexOf(_pb, pbN), 9);

            pbN = enc.GetBytes("01234567890123");
            Assert.AreEqual(MemUtil.IndexOf(_pb, pbN), 0);

            pbN = enc.GetBytes("a");
            Assert.AreEqual(MemUtil.IndexOf(_pb, pbN), 21);

            pbN = enc.GetBytes("0a");
            Assert.AreEqual(MemUtil.IndexOf(_pb, pbN), 20);

            pbN = enc.GetBytes("1");
            Assert.AreEqual(MemUtil.IndexOf(_pb, pbN), 1);

            pbN = enc.GetBytes("b");
            Assert.IsTrue(MemUtil.IndexOf(_pb, pbN) < 0);

            pbN = enc.GetBytes("012b");
            Assert.IsTrue(MemUtil.IndexOf(_pb, pbN) < 0);
        }

        [TestMethod]
        public void TestBase32()
        {
            byte[] pbRes = MemUtil.ParseBase32("MY======");
            byte[] pbExp = Encoding.UTF8.GetBytes("f");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXQ====");
            pbExp = Encoding.UTF8.GetBytes("fo");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6===");
            pbExp = Encoding.UTF8.GetBytes("foo");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6YQ=");
            pbExp = Encoding.UTF8.GetBytes("foob");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6YTB");
            pbExp = Encoding.UTF8.GetBytes("fooba");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("MZXW6YTBOI======");
            pbExp = Encoding.UTF8.GetBytes("foobar");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));

            pbRes = MemUtil.ParseBase32("JNSXSIDQOJXXM2LEMVZCAYTBONSWIIDPNYQG63TFFV2GS3LFEBYGC43TO5XXEZDTFY======");
            pbExp = Encoding.UTF8.GetBytes("Key provider based on one-time passwords.");
            Assert.IsTrue(MemUtil.ArraysEqual(pbRes, pbExp));
        }

        [TestMethod]
        public void TestMemUtil2()
        { 
            var i = 0 - 0x10203040;
            var pbRes = MemUtil.Int32ToBytes(i);
            Assert.AreEqual(MemUtil.ByteArrayToHexString(pbRes), "C0CFDFEF");
            Assert.AreEqual(MemUtil.BytesToUInt32(pbRes), (uint)i);
            Assert.AreEqual(MemUtil.BytesToInt32(pbRes), i);
        }
    }
}