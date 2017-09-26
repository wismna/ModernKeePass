using NUnit.Framework;
using System;

#if KeePassLib
using KeePassLib.Keys;
#else
using ModernKeePassLib.Keys;
#endif

namespace ModernKeePassLib.Test.Shared.Keys
{
  [TestFixture ()]
  public class KcpCustomKeyTests
  {
    static readonly byte[] testData = {
      0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
    };

    [Test ()]
    public void TestConstruct ()
    {
      var expectedHash = new byte[32] {
        0xAF, 0x55, 0x70, 0xF5, 0xA1, 0x81, 0x0B, 0x7A,
        0xF7, 0x8C, 0xAF, 0x4B, 0xC7, 0x0A, 0x66, 0x0F,
        0x0D, 0xF5, 0x1E, 0x42, 0xBA, 0xF9, 0x1D, 0x4D,
        0xE5, 0xB2, 0x32, 0x8D, 0xE0, 0xE8, 0x3D, 0xFC
      };

      var key = new KcpCustomKey ("test1", testData, false);
      var keyData = key.KeyData.ReadData ();
      Assert.That (keyData, Is.EqualTo (testData));

      key = new KcpCustomKey ("test2", testData, true);
      keyData = key.KeyData.ReadData ();
      Assert.That (keyData, Is.EqualTo (expectedHash));
    }
  }
}

