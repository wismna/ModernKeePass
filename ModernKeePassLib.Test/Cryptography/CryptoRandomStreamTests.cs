using NUnit.Framework;
using System;

#if KeePassLib
using KeePassLib.Cryptography;
#else
using ModernKeePassLibPCL.Cryptography;
#endif

namespace ModernKeePassLib.Test.Shared.Cryptography
{
  [TestFixture ()]
  public class CryptoRandomStreamTests
  {
    void TestGetRandomBytes(CryptoRandomStream stream)
    {
      const uint length = 16;
      var bytes1 = stream.GetRandomBytes (length);
      Assert.That (bytes1.Length, Is.EqualTo (length));
      var bytes2 = stream.GetRandomBytes (length);
      Assert.That (bytes2, Is.Not.EqualTo (bytes1));
    }

    [Test ()]
    public void TestGetRandomBytesCrsAlgorithmSalsa20 ()
    {
      var stream = new CryptoRandomStream (CrsAlgorithm.Salsa20, new byte[16]);
      TestGetRandomBytes (stream);
    }

    [Test ()]
    public void TestGetRandomBytesCrsAlgorithmArcFourVariant ()
    {
      var stream = new CryptoRandomStream (CrsAlgorithm.ArcFourVariant, new byte[16]);
      TestGetRandomBytes (stream);
    }

    void TestGetRandomInt64 (CryptoRandomStream stream)
    {
      var value1 = stream.GetRandomUInt64 ();
      var value2 = stream.GetRandomUInt64 ();
      Assert.That (value2, Is.Not.EqualTo (value1));
    }

    [Test ()]
    public void TestGetRandomInt64AlgorithmSalsa20 ()
    {
      var stream = new CryptoRandomStream (CrsAlgorithm.Salsa20, new byte[16]);
      TestGetRandomInt64 (stream);
    }

    [Test ()]
    public void TestGetRandomInt64AlgorithmArcFourVariant ()
    {
      var stream = new CryptoRandomStream (CrsAlgorithm.ArcFourVariant, new byte[16]);
      TestGetRandomInt64 (stream);
    }
  }
}

