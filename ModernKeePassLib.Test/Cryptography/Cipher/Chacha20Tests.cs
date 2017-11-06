using System;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Utility;

namespace ModernKeePassLib.Test.Cryptography.Cipher
{
    [TestClass]
    public class Chacha20Tests
    {
        [TestMethod]
        public void TestChacha20()
        {
            // ======================================================
            // Test vector from RFC 7539, section 2.3.2

            byte[] pbKey = new byte[32];
            for (int i = 0; i < 32; ++i) pbKey[i] = (byte)i;

            byte[] pbIV = new byte[12];
            pbIV[3] = 0x09;
            pbIV[7] = 0x4A;

            byte[] pbExpc = new byte[64] {
                0x10, 0xF1, 0xE7, 0xE4, 0xD1, 0x3B, 0x59, 0x15,
                0x50, 0x0F, 0xDD, 0x1F, 0xA3, 0x20, 0x71, 0xC4,
                0xC7, 0xD1, 0xF4, 0xC7, 0x33, 0xC0, 0x68, 0x03,
                0x04, 0x22, 0xAA, 0x9A, 0xC3, 0xD4, 0x6C, 0x4E,
                0xD2, 0x82, 0x64, 0x46, 0x07, 0x9F, 0xAA, 0x09,
                0x14, 0xC2, 0xD7, 0x05, 0xD9, 0x8B, 0x02, 0xA2,
                0xB5, 0x12, 0x9C, 0xD1, 0xDE, 0x16, 0x4E, 0xB9,
                0xCB, 0xD0, 0x83, 0xE8, 0xA2, 0x50, 0x3C, 0x4E
            };

            byte[] pb = new byte[64];

            using (ChaCha20Cipher c = new ChaCha20Cipher(pbKey, pbIV))
            {
                c.Seek(64, SeekOrigin.Begin); // Skip first block
                c.Encrypt(pb, 0, pb.Length);

                Assert.IsTrue(MemUtil.ArraysEqual(pb, pbExpc));
            }

#if DEBUG
            // ======================================================
            // Test vector from RFC 7539, section 2.4.2

            pbIV[3] = 0;

            pb = StrUtil.Utf8.GetBytes("Ladies and Gentlemen of the clas" +
                @"s of '99: If I could offer you only one tip for " +
                @"the future, sunscreen would be it.");

            pbExpc = new byte[] {
                0x6E, 0x2E, 0x35, 0x9A, 0x25, 0x68, 0xF9, 0x80,
                0x41, 0xBA, 0x07, 0x28, 0xDD, 0x0D, 0x69, 0x81,
                0xE9, 0x7E, 0x7A, 0xEC, 0x1D, 0x43, 0x60, 0xC2,
                0x0A, 0x27, 0xAF, 0xCC, 0xFD, 0x9F, 0xAE, 0x0B,
                0xF9, 0x1B, 0x65, 0xC5, 0x52, 0x47, 0x33, 0xAB,
                0x8F, 0x59, 0x3D, 0xAB, 0xCD, 0x62, 0xB3, 0x57,
                0x16, 0x39, 0xD6, 0x24, 0xE6, 0x51, 0x52, 0xAB,
                0x8F, 0x53, 0x0C, 0x35, 0x9F, 0x08, 0x61, 0xD8,
                0x07, 0xCA, 0x0D, 0xBF, 0x50, 0x0D, 0x6A, 0x61,
                0x56, 0xA3, 0x8E, 0x08, 0x8A, 0x22, 0xB6, 0x5E,
                0x52, 0xBC, 0x51, 0x4D, 0x16, 0xCC, 0xF8, 0x06,
                0x81, 0x8C, 0xE9, 0x1A, 0xB7, 0x79, 0x37, 0x36,
                0x5A, 0xF9, 0x0B, 0xBF, 0x74, 0xA3, 0x5B, 0xE6,
                0xB4, 0x0B, 0x8E, 0xED, 0xF2, 0x78, 0x5E, 0x42,
                0x87, 0x4D
            };

            byte[] pb64 = new byte[64];

            using (ChaCha20Cipher c = new ChaCha20Cipher(pbKey, pbIV))
            {
                c.Encrypt(pb64, 0, pb64.Length); // Skip first block
                c.Encrypt(pb, 0, pb.Length);

                Assert.IsTrue(MemUtil.ArraysEqual(pb, pbExpc));
            }

            // ======================================================
            // Test vector from RFC 7539, appendix A.2 #2

            Array.Clear(pbKey, 0, pbKey.Length);
            pbKey[31] = 1;

            Array.Clear(pbIV, 0, pbIV.Length);
            pbIV[11] = 2;

            pb = StrUtil.Utf8.GetBytes("Any submission to the IETF inten" +
                "ded by the Contributor for publication as all or" +
                " part of an IETF Internet-Draft or RFC and any s" +
                "tatement made within the context of an IETF acti" +
                "vity is considered an \"IETF Contribution\". Such " +
                "statements include oral statements in IETF sessi" +
                "ons, as well as written and electronic communica" +
                "tions made at any time or place, which are addressed to");

            pbExpc = MemUtil.HexStringToByteArray(
                "A3FBF07DF3FA2FDE4F376CA23E82737041605D9F4F4F57BD8CFF2C1D4B7955EC" +
                "2A97948BD3722915C8F3D337F7D370050E9E96D647B7C39F56E031CA5EB6250D" +
                "4042E02785ECECFA4B4BB5E8EAD0440E20B6E8DB09D881A7C6132F420E527950" +
                "42BDFA7773D8A9051447B3291CE1411C680465552AA6C405B7764D5E87BEA85A" +
                "D00F8449ED8F72D0D662AB052691CA66424BC86D2DF80EA41F43ABF937D3259D" +
                "C4B2D0DFB48A6C9139DDD7F76966E928E635553BA76C5C879D7B35D49EB2E62B" +
                "0871CDAC638939E25E8A1E0EF9D5280FA8CA328B351C3C765989CBCF3DAA8B6C" +
                "CC3AAF9F3979C92B3720FC88DC95ED84A1BE059C6499B9FDA236E7E818B04B0B" +
                "C39C1E876B193BFE5569753F88128CC08AAA9B63D1A16F80EF2554D7189C411F" +
                "5869CA52C5B83FA36FF216B9C1D30062BEBCFD2DC5BCE0911934FDA79A86F6E6" +
                "98CED759C3FF9B6477338F3DA4F9CD8514EA9982CCAFB341B2384DD902F3D1AB" +
                "7AC61DD29C6F21BA5B862F3730E37CFDC4FD806C22F221");

            using (MemoryStream msEnc = new MemoryStream())
            {
                using (ChaCha20Stream c = new ChaCha20Stream(msEnc, true, pbKey, pbIV))
                {
                    Random r = CryptoRandom.NewWeakRandom();
                    r.NextBytes(pb64);
                    c.Write(pb64, 0, pb64.Length); // Skip first block

                    int p = 0;
                    while (p < pb.Length)
                    {
                        int cb = r.Next(1, pb.Length - p + 1);
                        c.Write(pb, p, cb);
                        p += cb;
                    }
                    Debug.Assert(p == pb.Length);
                }

                byte[] pbEnc0 = msEnc.ToArray();
                byte[] pbEnc = MemUtil.Mid(pbEnc0, 64, pbEnc0.Length - 64);
                Assert.IsTrue(MemUtil.ArraysEqual(pbEnc, pbExpc));

                using (MemoryStream msCT = new MemoryStream(pbEnc0, false))
                {
                    using (ChaCha20Stream cDec = new ChaCha20Stream(msCT, false,
                        pbKey, pbIV))
                    {
                        byte[] pbPT = MemUtil.Read(cDec, pbEnc0.Length);

                        Assert.IsTrue(cDec.ReadByte() < 0);
                        Assert.IsTrue(MemUtil.ArraysEqual(MemUtil.Mid(pbPT, 0, 64), pb64));
                        Assert.IsTrue(MemUtil.ArraysEqual(MemUtil.Mid(pbPT, 64, pbEnc.Length), pb));
                    }
                }
            }

            // ======================================================
            // Test vector TC8 from RFC draft by J. Strombergson:
            // https://tools.ietf.org/html/draft-strombergson-chacha-test-vectors-01

            pbKey = new byte[32] {
                0xC4, 0x6E, 0xC1, 0xB1, 0x8C, 0xE8, 0xA8, 0x78,
                0x72, 0x5A, 0x37, 0xE7, 0x80, 0xDF, 0xB7, 0x35,
                0x1F, 0x68, 0xED, 0x2E, 0x19, 0x4C, 0x79, 0xFB,
                0xC6, 0xAE, 0xBE, 0xE1, 0xA6, 0x67, 0x97, 0x5D
            };

            // The first 4 bytes are set to zero and a large counter
            // is used; this makes the RFC 7539 version of ChaCha20
            // compatible with the original specification by
            // D. J. Bernstein.
            pbIV = new byte[12] { 0x00, 0x00, 0x00, 0x00,
                0x1A, 0xDA, 0x31, 0xD5, 0xCF, 0x68, 0x82, 0x21
            };

            pb = new byte[128];

            pbExpc = new byte[128] {
                0xF6, 0x3A, 0x89, 0xB7, 0x5C, 0x22, 0x71, 0xF9,
                0x36, 0x88, 0x16, 0x54, 0x2B, 0xA5, 0x2F, 0x06,
                0xED, 0x49, 0x24, 0x17, 0x92, 0x30, 0x2B, 0x00,
                0xB5, 0xE8, 0xF8, 0x0A, 0xE9, 0xA4, 0x73, 0xAF,
                0xC2, 0x5B, 0x21, 0x8F, 0x51, 0x9A, 0xF0, 0xFD,
                0xD4, 0x06, 0x36, 0x2E, 0x8D, 0x69, 0xDE, 0x7F,
                0x54, 0xC6, 0x04, 0xA6, 0xE0, 0x0F, 0x35, 0x3F,
                0x11, 0x0F, 0x77, 0x1B, 0xDC, 0xA8, 0xAB, 0x92,

                0xE5, 0xFB, 0xC3, 0x4E, 0x60, 0xA1, 0xD9, 0xA9,
                0xDB, 0x17, 0x34, 0x5B, 0x0A, 0x40, 0x27, 0x36,
                0x85, 0x3B, 0xF9, 0x10, 0xB0, 0x60, 0xBD, 0xF1,
                0xF8, 0x97, 0xB6, 0x29, 0x0F, 0x01, 0xD1, 0x38,
                0xAE, 0x2C, 0x4C, 0x90, 0x22, 0x5B, 0xA9, 0xEA,
                0x14, 0xD5, 0x18, 0xF5, 0x59, 0x29, 0xDE, 0xA0,
                0x98, 0xCA, 0x7A, 0x6C, 0xCF, 0xE6, 0x12, 0x27,
                0x05, 0x3C, 0x84, 0xE4, 0x9A, 0x4A, 0x33, 0x32
            };

            using (ChaCha20Cipher c = new ChaCha20Cipher(pbKey, pbIV, true))
            {
                c.Decrypt(pb, 0, pb.Length);

                Assert.IsTrue(MemUtil.ArraysEqual(pb, pbExpc));
            }
#endif
        }
    }
}