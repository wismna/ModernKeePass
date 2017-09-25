using NUnit.Framework;
using System;

#if KeePassLib
using KeePassLib.Cryptography;
#else
using KeePass2PCL.Cryptography;
#endif

namespace KeePass2PCL.Test.Shared.Cryptography
{
  [TestFixture ()]
  public class CryptoRandomTests
  {
    [Test ()]
    public void TestAddEntropy ()
    {
      // just making sure it does not throw an exception
      CryptoRandom.Instance.AddEntropy (new byte[1]);
    }

    [Test ()]
    public void TestGetRandomBytes ()
    {
      const int length = 32;
      var bytes1 = CryptoRandom.Instance.GetRandomBytes (length);
      Assert.That (bytes1.Length, Is.EqualTo (length));
      var bytes2 = CryptoRandom.Instance.GetRandomBytes (length);
      Assert.That (bytes2, Is.Not.EqualTo (bytes1));
    }

    [Test ()]
    public void TestGeneratedBytesCount ()
    {
      const int length = 1;
      CryptoRandom.Instance.GetRandomBytes (length);
      var count1 = CryptoRandom.Instance.GeneratedBytesCount;
      CryptoRandom.Instance.GetRandomBytes (length);
      var count2 = CryptoRandom.Instance.GeneratedBytesCount;
      Assert.That (count2, Is.GreaterThan (count1));
    }
  }
}

