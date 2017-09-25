using NUnit.Framework;
using System;

#if KeePassLib
using KeePassLib.Keys;
#else
using ModernKeePassLibPCL.Keys;
#endif

namespace ModernKeePassLib.Test.Shared.Keys
{
  [TestFixture ()]
  public class CompositeKeyTests
  {
    [Test ()]
    public void TestGenerateKey32 ()
    {
      var originalKey = new byte[32];
      var expectedKey = new byte[32] {
        0xF0, 0xED, 0x57, 0xD5, 0xF0, 0xDA, 0xF3, 0x47,
        0x90, 0xD0, 0xDB, 0x43, 0x25, 0xC6, 0x81, 0x2C,
        0x81, 0x6A, 0x0D, 0x94, 0x96, 0xA9, 0x03, 0xE1,
        0x20, 0xD4, 0x3A, 0x3E, 0x45, 0xAD, 0x02, 0x65
      };
      const ulong rounds = 1;

      var composite = new CompositeKey ();
      var key = composite.GenerateKey32 (originalKey, rounds);
      Assert.That (key, Is.Not.Null);
      var keyData = key.ReadData ();
      Assert.That (keyData, Is.EqualTo (expectedKey));
    }
  }
}
