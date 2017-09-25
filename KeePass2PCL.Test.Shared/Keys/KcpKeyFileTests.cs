using NUnit.Framework;
using System;
using System.IO;

#if KeePassLib
using KeePassLib.Keys;
#else
using KeePass2PCL.Keys;
#endif

namespace KeePass2PCL.Test.Shared.Keys
{
  [TestFixture ()]
  public class KcpKeyFileTests
  {
    const string testCreateFile = "TestCreate.xml";
    const string testKey = "0123456789";

    const string expectedFileStart =
      "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
      "<KeyFile>\r\n" +
      "\t<Meta>\r\n" +
      "\t\t<Version>1.00</Version>\r\n" +
      "\t</Meta>\r\n" +
      "\t<Key>\r\n" +
      "\t\t<Data>";

    const string expectedFileEnd = "\t</Key>\r\n" +
      "</KeyFile>\r\n";

    [Test ()]
    public void TestConstruct ()
    {
      var expectedKeyData = new byte[32] {
        0xC1, 0xB1, 0x12, 0x77, 0x23, 0xB8, 0x99, 0xB8,
        0xB9, 0x3B, 0x1B, 0xFF, 0x6C, 0xBE, 0xA1, 0x5B,
        0x8B, 0x99, 0xAC, 0xBD, 0x99, 0x51, 0x85, 0x95,
        0x31, 0xAA, 0x14, 0x3D, 0x95, 0xBF, 0x63, 0xFF
      };

      var fullPath = Path.Combine(Path.GetTempPath(), testCreateFile);
      using (var fs = new FileStream(fullPath, FileMode.Create)) {
        using (var sw = new StreamWriter(fs)) {
          sw.Write (expectedFileStart);
          sw.Write (testKey);
          sw.Write (expectedFileEnd);
        }
      }

      try {
        var keyFile = new KcpKeyFile (fullPath);
        var keyData = keyFile.KeyData.ReadData ();
        Assert.That (keyData, Is.EqualTo (expectedKeyData));
      } finally {
        File.Delete (fullPath);
      }
    }

    [Test ()]
    public void TestCreate ()
    {
      var fullPath = Path.Combine(Path.GetTempPath(), testCreateFile);
      File.Create(fullPath).Close();
      KcpKeyFile.Create (fullPath, null);
      try {
        var fileContents = File.ReadAllText (fullPath);
        Assert.That (fileContents.Length, Is.EqualTo (187));
        Assert.That (fileContents, Does.StartWith (expectedFileStart));
        Assert.That (fileContents, Does.EndWith (expectedFileEnd));
      } finally {
        File.Delete (fullPath);
      }
    }
  }
}

