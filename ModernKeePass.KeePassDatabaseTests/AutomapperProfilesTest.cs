using System;
using System.Collections.Generic;
using System.Drawing;
using AutoMapper;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Infrastructure.KeePass;
using ModernKeePassLib;
using ModernKeePassLib.Security;
using NUnit.Framework;

namespace ModernKeePass.KeePassDatabaseTests
{
    [TestFixture]
    public class AutomapperProfilesTest
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new Mapper(new MapperConfiguration(conf => conf.AddProfile(new EntryMappingProfile())));
        }

        [Test]
        public void Assert_Mapping_Configuration_Is_Valid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Test]
        public void FromDtoToModel_Should_Map_PwEntry_To_Entry()
        {
            var pwEntry = new PwEntry(true, true)
            {
                ExpiryTime = DateTime.Now,
                BackgroundColor = Color.White,
                ForegroundColor = Color.Black
            };
            pwEntry.Strings.Set(PwDefs.TitleField, new ProtectedString(true, "Test"));
            pwEntry.Strings.Set(PwDefs.UserNameField, new ProtectedString(true, "toto"));
            pwEntry.Strings.Set(PwDefs.PasswordField, new ProtectedString(true, "password"));
            pwEntry.Strings.Set(PwDefs.UrlField, new ProtectedString(true, "http://google.com"));
            pwEntry.Strings.Set(PwDefs.NotesField, new ProtectedString(true, "blabla"));
            pwEntry.Strings.Set("additional", new ProtectedString(true, "custom"));

            var entry = _mapper.Map<PwEntry, EntryEntity>(pwEntry);

            Assert.That(entry.ExpirationDate, Is.Not.EqualTo(default(DateTimeOffset)));
            Assert.That(entry.BackgroundColor, Is.EqualTo(Color.White));
            Assert.That(entry.ForegroundColor, Is.EqualTo(Color.Black));
            Assert.That(entry.Name, Is.EqualTo("Test"));
            Assert.That(entry.UserName, Is.EqualTo("toto"));
            Assert.That(entry.Password, Is.EqualTo("password"));
            Assert.That(entry.Url, Is.EqualTo(new Uri("http://google.com")));
            Assert.That(entry.Notes, Is.EqualTo("blabla"));
            Assert.That(entry.AdditionalFields, Is.Not.Empty);
            Assert.That(entry.AdditionalFields["additional"], Is.EqualTo("custom"));
        }

        [Test]
        public void FromModelToDto_Should_Map_Entry_To_PwEntry()
        {
            var entry = new EntryEntity
            {
                Id = "VGhlIHF1aWNrIGJyb3duIA==",
                Name = "Test",
                UserName = "toto",
                Password = "password",
                Url = new Uri("http://google.com"),
                Notes = "blabla",
                ExpirationDate = DateTimeOffset.Now,
                BackgroundColor = Color.White,
                ForegroundColor = Color.Black,
                AdditionalFields = new Dictionary<string, string> { 
                    {
                        "additional", "custom"
                    }
                }
            };
            var pwEntry = new PwEntry(false, false);

            _mapper.Map(entry, pwEntry);

            Assert.That(pwEntry.ExpiryTime, Is.Not.EqualTo(default(DateTime)));
            Assert.That(pwEntry.BackgroundColor, Is.EqualTo(Color.White));
            Assert.That(pwEntry.ForegroundColor, Is.EqualTo(Color.Black));
            Assert.That(pwEntry.Strings.GetSafe(PwDefs.TitleField).ReadString(), Is.EqualTo("Test"));
            Assert.That(pwEntry.Strings.GetSafe(PwDefs.UserNameField).ReadString(), Is.EqualTo("toto"));
            Assert.That(pwEntry.Strings.GetSafe(PwDefs.PasswordField).ReadString(), Is.EqualTo("password"));
            Assert.That(pwEntry.Strings.GetSafe(PwDefs.UrlField).ReadString(), Is.EqualTo(new Uri("http://google.com")));
            Assert.That(pwEntry.Strings.GetSafe(PwDefs.NotesField).ReadString(), Is.EqualTo("blabla"));
            Assert.That(pwEntry.Strings.GetSafe("additional").ReadString(), Is.EqualTo("custom"));
        }
    }
}