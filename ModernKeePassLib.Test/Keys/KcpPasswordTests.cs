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
  public class KcpPasswordTests
  {
    const string testPassword = "password";

    [Test ()]
    public void TestConstruct ()
    {
      var expectedHash = new byte[32] {
        0x5E, 0x88, 0x48, 0x98, 0xDA, 0x28, 0x04, 0x71,
        0x51, 0xD0, 0xE5, 0x6F, 0x8D, 0xC6, 0x29, 0x27,
        0x73, 0x60, 0x3D, 0x0D, 0x6A, 0xAB, 0xBD, 0xD6,
        0x2A, 0x11, 0xEF, 0x72, 0x1D, 0x15, 0x42, 0xD8
      };

      var key = new KcpPassword (testPassword);
      var keyData = key.KeyData.ReadData ();
      Assert.That (keyData, Is.EqualTo (expectedHash));
    }
  }
}

