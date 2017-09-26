using System;
using System.IO;

#if KeePassLib
using KeePassLib.Cryptography.Cipher;
#else
using ModernKeePassLib.Cryptography.Cipher;
#endif

using NUnit.Framework;

namespace ModernKeePassLib.Test.Shared.Cryptography.Cipher
{
  [TestFixture ()]
  public class StandardAesEngineTests
  {
    [Test ()]
    public void TestEncryptStream ()
    {
      // Test vector (official ECB test vector #356)
      var pbIV = new byte[16];
      var pbTestKey = new byte[32];
      var pbTestData = new byte[16];
      var pbReferenceCT = new byte[16] {
        0x75, 0xD1, 0x1B, 0x0E, 0x3A, 0x68, 0xC4, 0x22,
        0x3D, 0x88, 0xDB, 0xF0, 0x17, 0x97, 0x7D, 0xD7
      };

      pbTestData[0] = 0x04;

      var outStream = new MemoryStream (new byte[16]);
      var aes = new StandardAesEngine ();
      var inStream = aes.EncryptStream (outStream, pbTestKey, pbIV);
      new BinaryWriter (inStream).Write (pbTestData);
      Assert.That (outStream.Position, Is.EqualTo (16));
      outStream.Position = 0;
      var outBytes = new BinaryReader (outStream).ReadBytes (16);
      Assert.That(outBytes, Is.EqualTo (pbReferenceCT));
    }

    [Test ()]
    public void TestDecryptStream ()
    {
      // Test vector (official ECB test vector #356)
      var pbIV = new byte[16];
      var pbTestKey = new byte[32];
      var pbTestData = new byte[16];
      var pbReferenceCT = new byte[16] {
        0x75, 0xD1, 0x1B, 0x0E, 0x3A, 0x68, 0xC4, 0x22,
        0x3D, 0x88, 0xDB, 0xF0, 0x17, 0x97, 0x7D, 0xD7
      };

      pbTestData[0] = 0x04;

      // Possible Mono Bug? This only works with size >= 48
      var inStream = new MemoryStream (new byte[48]);
      inStream.Write (pbReferenceCT, 0, pbReferenceCT.Length);
      inStream.Position = 0;
      var aes = new StandardAesEngine ();
      var outStream = aes.DecryptStream (inStream, pbTestKey, pbIV);
      var outBytes = new BinaryReader (outStream).ReadBytes (16);
      Assert.That(outBytes, Is.EqualTo (pbTestData));
    }
  }
}
