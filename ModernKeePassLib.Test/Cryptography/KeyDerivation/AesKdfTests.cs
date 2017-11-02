using System;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography.KeyDerivation
{
    [TestFixture]
    public class AesKdfTests
    {
        [Test]
        public void TestAesKdf()
        {
            // Up to KeePass 2.34, the OtpKeyProv plugin used the public
            // CompositeKey.TransformKeyManaged method (and a finalizing
            // SHA-256 computation), which became an internal method of
            // the AesKdf class in KeePass 2.35, thus OtpKeyProv now
            // uses the AesKdf class; here we ensure that the results
            // are the same
            var r = CryptoRandom.NewWeakRandom();
            var pbKey = new byte[32];
            r.NextBytes(pbKey);
            var pbSeed = new byte[32];
            r.NextBytes(pbSeed);
            var uRounds = (ulong)r.Next(1, 0x7FFF);

            var pbMan = new byte[pbKey.Length];
            Array.Copy(pbKey, pbMan, pbKey.Length);
            Assert.That(AesKdf.TransformKeyManaged(pbMan, pbSeed, uRounds), Is.True);
            pbMan = CryptoUtil.HashSha256(pbMan);

            var kdf = new AesKdf();
            var p = kdf.GetDefaultParameters();
            p.SetUInt64(AesKdf.ParamRounds, uRounds);
            p.SetByteArray(AesKdf.ParamSeed, pbSeed);
            var pbKdf = kdf.Transform(pbKey, p);

            Assert.That(MemUtil.ArraysEqual(pbMan, pbKdf), Is.True);
        }
    }
}