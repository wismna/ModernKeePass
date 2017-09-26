using NUnit.Framework;
using System;
using System.IO;
using System.Text;

#if KeePassLib
using KeePassLib.Cryptography;
#else
using ModernKeePassLib.Cryptography;
#endif

namespace ModernKeePassLib.Test.Shared.Cryptography
{
  [TestFixture ()]
  public class HashingStreamExTests
  {
    const string data = "test";

    // The expected hash includes the \n added by WriteLine
    static readonly byte[] sha256HashOfData = {
      0xf2, 0xca, 0x1b, 0xb6, 0xc7, 0xe9, 0x07, 0xd0,
      0x6d, 0xaf, 0xe4, 0x68, 0x7e, 0x57, 0x9f, 0xce,
      0x76, 0xb3, 0x7e, 0x4e, 0x93, 0xb7, 0x60, 0x50,
      0x22, 0xda, 0x52, 0xe6, 0xcc, 0xc2, 0x6f, 0xd2
    };

    [Test ()]
    public void TestRead ()
    {
      // if we use larger size, StreamReader will read past newline and cause bad hash
      var bytes = new byte[data.Length + 1];
      using (var ms = new MemoryStream (bytes)) {
        using (var sw = new StreamWriter (ms)) {
          // set NewLine to ensure we don't run into cross-platform issues on Windows
          sw.NewLine = "\n";
          sw.WriteLine (data);
        }
      }
      using (var ms = new MemoryStream (bytes)) {
        using (var hs = new HashingStreamEx (ms, false, null)) {
          using (var sr = new StreamReader (hs)) {
            var read = sr.ReadLine ();
            Assert.That (read, Is.EqualTo (data));
          }
          // When the StreamReader is disposed, it calls Dispose on the
          //HasingStreamEx, which computes the hash.
          Assert.That (hs.Hash, Is.EqualTo (sha256HashOfData));
        }
      }
    }

    [Test ()]
    public void TestWrite ()
    {
      var bytes = new byte[16];
      using (var ms = new MemoryStream (bytes)) {
        using (var hs = new HashingStreamEx (ms, true, null)) {
          using (var sw = new StreamWriter (hs)) {
            // set NewLine to ensure we don't run into cross-platform issues on Windows
            sw.NewLine = "\n";
            sw.WriteLine (data);
          }
          // When the StreamWriter is disposed, it calls Dispose on the
          //HasingStreamEx, which computes the hash.
          Assert.That (hs.Hash, Is.EqualTo (sha256HashOfData));
        }
      }
      using (var ms = new MemoryStream (bytes)) {
        using (var sr = new StreamReader (ms)) {
          var read = sr.ReadLine ();
          Assert.That (read, Is.EqualTo (data));
        }
      }
    }
  }
}

