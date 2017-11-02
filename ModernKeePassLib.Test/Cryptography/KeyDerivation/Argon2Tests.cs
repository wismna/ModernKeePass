using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Utility;
using NUnit.Framework;

namespace ModernKeePassLib.Test.Cryptography.KeyDerivation
{
    [TestFixture]
    public class Argon2Tests
    {
        [Test]
        public void TestArgon2()
        {
            Argon2Kdf kdf = new Argon2Kdf();

            // ======================================================
            // From the official Argon2 1.3 reference code package
            // (test vector for Argon2d 1.3); also on
            // https://tools.ietf.org/html/draft-irtf-cfrg-argon2-00

            var p = kdf.GetDefaultParameters();
            kdf.Randomize(p);

            Assert.That(p.GetUInt32(Argon2Kdf.ParamVersion, 0), Is.EqualTo(0x13U));

            byte[] pbMsg = new byte[32];
            for (int i = 0; i < pbMsg.Length; ++i) pbMsg[i] = 1;

            p.SetUInt64(Argon2Kdf.ParamMemory, 32 * 1024);
            p.SetUInt64(Argon2Kdf.ParamIterations, 3);
            p.SetUInt32(Argon2Kdf.ParamParallelism, 4);

            byte[] pbSalt = new byte[16];
            for (int i = 0; i < pbSalt.Length; ++i) pbSalt[i] = 2;
            p.SetByteArray(Argon2Kdf.ParamSalt, pbSalt);

            byte[] pbKey = new byte[8];
            for (int i = 0; i < pbKey.Length; ++i) pbKey[i] = 3;
            p.SetByteArray(Argon2Kdf.ParamSecretKey, pbKey);

            byte[] pbAssoc = new byte[12];
            for (int i = 0; i < pbAssoc.Length; ++i) pbAssoc[i] = 4;
            p.SetByteArray(Argon2Kdf.ParamAssocData, pbAssoc);

            byte[] pbExpc = new byte[32] {
                0x51, 0x2B, 0x39, 0x1B, 0x6F, 0x11, 0x62, 0x97,
                0x53, 0x71, 0xD3, 0x09, 0x19, 0x73, 0x42, 0x94,
                0xF8, 0x68, 0xE3, 0xBE, 0x39, 0x84, 0xF3, 0xC1,
                0xA1, 0x3A, 0x4D, 0xB9, 0xFA, 0xBE, 0x4A, 0xCB
            };

            byte[] pb = kdf.Transform(pbMsg, p);

            Assert.That(MemUtil.ArraysEqual(pb, pbExpc), Is.True);

            // ======================================================
            // From the official Argon2 1.3 reference code package
            // (test vector for Argon2d 1.0)

            p.SetUInt32(Argon2Kdf.ParamVersion, 0x10);

            pbExpc = new byte[32] {
                0x96, 0xA9, 0xD4, 0xE5, 0xA1, 0x73, 0x40, 0x92,
                0xC8, 0x5E, 0x29, 0xF4, 0x10, 0xA4, 0x59, 0x14,
                0xA5, 0xDD, 0x1F, 0x5C, 0xBF, 0x08, 0xB2, 0x67,
                0x0D, 0xA6, 0x8A, 0x02, 0x85, 0xAB, 0xF3, 0x2B
            };

            pb = kdf.Transform(pbMsg, p);

            Assert.That(MemUtil.ArraysEqual(pb, pbExpc), Is.True);

            // ======================================================
            // From the official 'phc-winner-argon2-20151206.zip'
            // (test vector for Argon2d 1.0)

            p.SetUInt64(Argon2Kdf.ParamMemory, 16 * 1024);

            pbExpc = new byte[32] {
                0x57, 0xB0, 0x61, 0x3B, 0xFD, 0xD4, 0x13, 0x1A,
                0x0C, 0x34, 0x88, 0x34, 0xC6, 0x72, 0x9C, 0x2C,
                0x72, 0x29, 0x92, 0x1E, 0x6B, 0xBA, 0x37, 0x66,
                0x5D, 0x97, 0x8C, 0x4F, 0xE7, 0x17, 0x5E, 0xD2
            };

            pb = kdf.Transform(pbMsg, p);

            Assert.That(MemUtil.ArraysEqual(pb, pbExpc), Is.True);
            
			// ======================================================
			// Computed using the official 'argon2' application
			// (test vectors for Argon2d 1.3)

			p = kdf.GetDefaultParameters();

			pbMsg = StrUtil.Utf8.GetBytes("ABC1234");

			p.SetUInt64(Argon2Kdf.ParamMemory, (1 << 11) * 1024); // 2 MB
			p.SetUInt64(Argon2Kdf.ParamIterations, 2);
			p.SetUInt32(Argon2Kdf.ParamParallelism, 2);

			pbSalt = StrUtil.Utf8.GetBytes("somesalt");
			p.SetByteArray(Argon2Kdf.ParamSalt, pbSalt);

			pbExpc = new byte[32] {
				0x29, 0xCB, 0xD3, 0xA1, 0x93, 0x76, 0xF7, 0xA2,
				0xFC, 0xDF, 0xB0, 0x68, 0xAC, 0x0B, 0x99, 0xBA,
				0x40, 0xAC, 0x09, 0x01, 0x73, 0x42, 0xCE, 0xF1,
				0x29, 0xCC, 0xA1, 0x4F, 0xE1, 0xC1, 0xB7, 0xA3
			};

			pb = kdf.Transform(pbMsg, p);

            Assert.That(MemUtil.ArraysEqual(pb, pbExpc), Is.True);

			p.SetUInt64(Argon2Kdf.ParamMemory, (1 << 10) * 1024); // 1 MB
			p.SetUInt64(Argon2Kdf.ParamIterations, 3);

			pbExpc = new byte[32] {
				0x7A, 0xBE, 0x1C, 0x1C, 0x8D, 0x7F, 0xD6, 0xDC,
				0x7C, 0x94, 0x06, 0x3E, 0xD8, 0xBC, 0xD8, 0x1C,
				0x2F, 0x87, 0x84, 0x99, 0x12, 0x83, 0xFE, 0x76,
				0x00, 0x64, 0xC4, 0x58, 0xA4, 0xDA, 0x35, 0x70
			};

			pb = kdf.Transform(pbMsg, p);

            Assert.That(MemUtil.ArraysEqual(pb, pbExpc), Is.True);
            
			p.SetUInt64(Argon2Kdf.ParamMemory, (1 << 20) * 1024); // 1 GB
			p.SetUInt64(Argon2Kdf.ParamIterations, 2);
			p.SetUInt32(Argon2Kdf.ParamParallelism, 3);

			pbExpc = new byte[32] {
				0xE6, 0xE7, 0xCB, 0xF5, 0x5A, 0x06, 0x93, 0x05,
				0x32, 0xBA, 0x86, 0xC6, 0x1F, 0x45, 0x17, 0x99,
				0x65, 0x41, 0x77, 0xF9, 0x30, 0x55, 0x9A, 0xE8,
				0x3D, 0x21, 0x48, 0xC6, 0x2D, 0x0C, 0x49, 0x11
			};

			pb = kdf.Transform(pbMsg, p);

            Assert.That(MemUtil.ArraysEqual(pb, pbExpc), Is.True);
        }
    }
}