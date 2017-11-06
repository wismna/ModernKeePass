using System.Text;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Utility
{
    [TestFixture]
    public class MemUtilTests
    {
        private byte[] _pb = CryptoRandom.Instance.GetRandomBytes((uint)CryptoRandom.NewWeakRandom().Next(0, 0x2FFFF));

        [Test]
        public void TestGzip()
        {
            var pbCompressed = MemUtil.Compress(_pb);
            Assert.That(MemUtil.ArraysEqual(MemUtil.Decompress(pbCompressed), _pb), Is.True);
        }

        [Test]
        public void TestMemUtil()
        {
            Encoding enc = StrUtil.Utf8;
            _pb = enc.GetBytes("012345678901234567890a");
            byte[] pbN = enc.GetBytes("9012");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.EqualTo(9));

            pbN = enc.GetBytes("01234567890123");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.EqualTo(0));

            pbN = enc.GetBytes("a");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.EqualTo(21));

            pbN = enc.GetBytes("0a");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.EqualTo(20));

            pbN = enc.GetBytes("1");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.EqualTo(1));

            pbN = enc.GetBytes("b");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.LessThan(0));

            pbN = enc.GetBytes("012b");
            Assert.That(MemUtil.IndexOf(_pb, pbN), Is.LessThan(0));
        }

        [Test]
        public void TestBase32()
        {
            byte[] pbRes = MemUtil.ParseBase32("MY======");
            byte[] pbExp = Encoding.UTF8.GetBytes("f");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);

            pbRes = MemUtil.ParseBase32("MZXQ====");
            pbExp = Encoding.UTF8.GetBytes("fo");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);

            pbRes = MemUtil.ParseBase32("MZXW6===");
            pbExp = Encoding.UTF8.GetBytes("foo");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);

            pbRes = MemUtil.ParseBase32("MZXW6YQ=");
            pbExp = Encoding.UTF8.GetBytes("foob");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);

            pbRes = MemUtil.ParseBase32("MZXW6YTB");
            pbExp = Encoding.UTF8.GetBytes("fooba");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);

            pbRes = MemUtil.ParseBase32("MZXW6YTBOI======");
            pbExp = Encoding.UTF8.GetBytes("foobar");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);

            pbRes = MemUtil.ParseBase32("JNSXSIDQOJXXM2LEMVZCAYTBONSWIIDPNYQG63TFFV2GS3LFEBYGC43TO5XXEZDTFY======");
            pbExp = Encoding.UTF8.GetBytes("Key provider based on one-time passwords.");
            Assert.That(MemUtil.ArraysEqual(pbRes, pbExp), Is.True);
        }

        [Test]
        public void TestMemUtil2()
        { 
            var i = 0 - 0x10203040;
            var pbRes = MemUtil.Int32ToBytes(i);
            Assert.That(MemUtil.ByteArrayToHexString(pbRes), Is.EqualTo("C0CFDFEF"));
            Assert.That(MemUtil.BytesToUInt32(pbRes), Is.EqualTo((uint)i));
            Assert.That(MemUtil.BytesToInt32(pbRes), Is.EqualTo(i));
        }
    }
}