using NUnit.Framework;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI;
#if KeePassLib
using KeePassLib;
using KeePassLib.Keys;
using KeePassLib.Security;
using KeePassLib.Serialization;
using KeePassLib.Collections;
#else
using ModernKeePassLibPCL;
using ModernKeePassLibPCL.Keys;
using ModernKeePassLibPCL.Security;
using ModernKeePassLibPCL.Serialization;
using ModernKeePassLibPCL.Collections;
#endif

namespace ModernKeePassLib.Test.Shared.Serialization
{
    [TestFixture()]
    public class KdbxFileTests
    {
        const string testLocalizedAppName = "My Localized App Name";

        const string testDatabaseName = "My Database Name";
        const string testDatabaseDescription = "My Database Description";
        const string testDefaultUserName = "My Default User Name";
        const string testColor = "#FF0000"; // Red

        const string testRootGroupName = "My Root Group Name";
        const string testRootGroupNotes = "My Root Group Notes";
        const string testRootGroupDefaultAutoTypeSequence = "My Root Group Default Auto Type Sequence";

        const string testDatabase = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>\r\n" +
          "<KeePassFile>\r\n" +
          "\t<Meta>\r\n" +
          "\t\t<Generator>" + testLocalizedAppName + "</Generator>\r\n" +
          "\t\t<DatabaseName>" + testDatabaseName + "</DatabaseName>\r\n" +
          "\t\t<DatabaseNameChanged>2015-03-14T03:15:26Z</DatabaseNameChanged>\r\n" +
          "\t\t<DatabaseDescription>" + testDatabaseDescription + "</DatabaseDescription>\r\n" +
          "\t\t<DatabaseDescriptionChanged>2015-03-14T03:15:26Z</DatabaseDescriptionChanged>\r\n" +
          "\t\t<DefaultUserName>" + testDefaultUserName + "</DefaultUserName>\r\n" +
          "\t\t<DefaultUserNameChanged>2015-03-14T03:15:26Z</DefaultUserNameChanged>\r\n" +
          "\t\t<MaintenanceHistoryDays>365</MaintenanceHistoryDays>\r\n" +
          //"\t\t<Color>" + testColor + "</Color>\r\n" +
          "\t\t<Color></Color>\r\n" +
          "\t\t<MasterKeyChanged>2015-03-14T03:15:26Z</MasterKeyChanged>\r\n" +
          "\t\t<MasterKeyChangeRec>-1</MasterKeyChangeRec>\r\n" +
          "\t\t<MasterKeyChangeForce>-1</MasterKeyChangeForce>\r\n" +
          "\t\t<MemoryProtection>\r\n" +
          "\t\t\t<ProtectTitle>False</ProtectTitle>\r\n" +
          "\t\t\t<ProtectUserName>False</ProtectUserName>\r\n" +
          "\t\t\t<ProtectPassword>True</ProtectPassword>\r\n" +
          "\t\t\t<ProtectURL>False</ProtectURL>\r\n" +
          "\t\t\t<ProtectNotes>False</ProtectNotes>\r\n" +
          "\t\t</MemoryProtection>\r\n" +
          "\t\t<RecycleBinEnabled>True</RecycleBinEnabled>\r\n" +
          "\t\t<RecycleBinUUID>AAAAAAAAAAAAAAAAAAAAAA==</RecycleBinUUID>\r\n" +
          "\t\t<RecycleBinChanged>2015-03-14T03:15:26Z</RecycleBinChanged>\r\n" +
          "\t\t<EntryTemplatesGroup>AAAAAAAAAAAAAAAAAAAAAA==</EntryTemplatesGroup>\r\n" +
          "\t\t<EntryTemplatesGroupChanged>2015-03-14T03:15:26Z</EntryTemplatesGroupChanged>\r\n" +
          "\t\t<HistoryMaxItems>10</HistoryMaxItems>\r\n" +
          "\t\t<HistoryMaxSize>6291456</HistoryMaxSize>\r\n" +
          "\t\t<LastSelectedGroup>AAAAAAAAAAAAAAAAAAAAAA==</LastSelectedGroup>\r\n" +
          "\t\t<LastTopVisibleGroup>AAAAAAAAAAAAAAAAAAAAAA==</LastTopVisibleGroup>\r\n" +
          "\t\t<Binaries />\r\n" +
          "\t\t<CustomData />\r\n" +
          "\t</Meta>\r\n" +
          "\t<Root>\r\n" +
          "\t\t<Group>\r\n" +
          "\t\t\t<UUID>AAAAAAAAAAAAAAAAAAAAAA==</UUID>\r\n" +
          "\t\t\t<Name>" + testRootGroupName + "</Name>\r\n" +
          "\t\t\t<Notes>" + testRootGroupNotes + "</Notes>\r\n" +
          "\t\t\t<IconID>49</IconID>\r\n" +
          "\t\t\t<Times>\r\n" +
          "\t\t\t\t<CreationTime>2015-03-14T03:15:26Z</CreationTime>\r\n" +
          "\t\t\t\t<LastModificationTime>2015-03-14T03:15:26Z</LastModificationTime>\r\n" +
          "\t\t\t\t<LastAccessTime>2015-03-14T03:15:26Z</LastAccessTime>\r\n" +
          "\t\t\t\t<ExpiryTime>2015-03-14T03:15:26Z</ExpiryTime>\r\n" +
          "\t\t\t\t<Expires>False</Expires>\r\n" +
          "\t\t\t\t<UsageCount>0</UsageCount>\r\n" +
          "\t\t\t\t<LocationChanged>2015-03-14T03:15:26Z</LocationChanged>\r\n" +
          "\t\t\t</Times>\r\n" +
          "\t\t\t<IsExpanded>True</IsExpanded>\r\n" +
          "\t\t\t<DefaultAutoTypeSequence>" + testRootGroupDefaultAutoTypeSequence + "</DefaultAutoTypeSequence>\r\n" +
          "\t\t\t<EnableAutoType>null</EnableAutoType>\r\n" +
          "\t\t\t<EnableSearching>null</EnableSearching>\r\n" +
          "\t\t\t<LastTopVisibleEntry>AAAAAAAAAAAAAAAAAAAAAA==</LastTopVisibleEntry>\r\n" +
          "\t\t</Group>\r\n" +
          "\t\t<DeletedObjects />\r\n" +
          "\t</Root>\r\n" +
          "</KeePassFile>";

        const string testDate = "2015-03-14T03:15:26Z";

        [Test()]
        public void TestLoad()
        {
            var database = new PwDatabase();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(testDatabase)))
            {
                var file = new KdbxFile(database);
                file.Load(ms, KdbxFormat.PlainXml, null);
            }
            //Assert.That(database.Color.ToArgb(), Is.EqualTo(Color.Red.ToArgb()));
            Assert.That(database.Compression, Is.EqualTo(PwCompressionAlgorithm.GZip));
            //Assert.That (database.CustomData, Is.EqualTo ());
            Assert.That(database.CustomIcons, Is.Empty);
        }

        [Test()]
        public void TestSave()
        {
            var buffer = new byte[4096];
            using (var ms = new MemoryStream(buffer))
            {
                var database = new PwDatabase();
                database.New(new IOConnectionInfo(), new CompositeKey());
                var date = DateTime.Parse(testDate);
                PwDatabase.LocalizedAppName = testLocalizedAppName;
                database.Name = testDatabaseName;
                database.NameChanged = date;
                database.Description = testDatabaseDescription;
                database.DescriptionChanged = date;
                database.DefaultUserName = testDefaultUserName;
                database.DefaultUserNameChanged = date;
                //database.Color = Color.Red;
                database.MasterKeyChanged = date;
                database.RecycleBinChanged = date;
                database.EntryTemplatesGroupChanged = date;
                database.RootGroup.Uuid = PwUuid.Zero;
                database.RootGroup.Name = testRootGroupName;
                database.RootGroup.Notes = testRootGroupNotes;
                database.RootGroup.DefaultAutoTypeSequence = testRootGroupDefaultAutoTypeSequence;
                database.RootGroup.CreationTime = date;
                database.RootGroup.LastModificationTime = date;
                database.RootGroup.LastAccessTime = date;
                database.RootGroup.ExpiryTime = date;
                database.RootGroup.LocationChanged = date;
                var file = new KdbxFile(database);
                file.Save(ms, null, KdbxFormat.PlainXml, null);
            }
            var fileContents = Encoding.UTF8.GetString(buffer).Replace("\0", "");
            if (typeof(KdbxFile).Namespace.StartsWith("KeePassLib.")
                && Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                // Upstream KeePassLib does not specify line endings for XmlTextWriter,
                // so it uses native line endings.
                fileContents = fileContents.Replace("\n", "\r\n");
            }
            Assert.That(fileContents, Is.EqualTo(testDatabase));
        }

        [Test]
        public void TestSearch()
        {
            var database = new PwDatabase();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(testDatabase)))
            {
                var file = new KdbxFile(database);
                file.Load(ms, KdbxFormat.PlainXml, null);
            }
            var sp = new SearchParameters()
            {
                SearchString = "sfsoiwsefsi"
            };
            var listStorage = new PwObjectList<PwEntry>();
            database.RootGroup.SearchEntries(sp, listStorage);
            Assert.AreEqual(0U, listStorage.UCount);
            var entry = new PwEntry(true, true);
            entry.Strings.Set("Title", new ProtectedString(false, "NaMe"));
            database.RootGroup.AddEntry(entry, true);
            sp.SearchString = "name";
            database.RootGroup.SearchEntries(sp, listStorage);
            Assert.AreEqual(1U, listStorage.UCount);
        }
    }
}
