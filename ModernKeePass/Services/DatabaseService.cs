using System;
using Windows.Storage;
using Microsoft.HockeyApp;
using ModernKeePass.Exceptions;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Collections;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Interfaces;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Security;
using ModernKeePassLib.Serialization;

namespace ModernKeePass.Services
{
    public class DatabaseService: SingletonServiceBase<DatabaseService>, IDatabaseService
    {
        private readonly PwDatabase _pwDatabase = new PwDatabase();
        private readonly ISettingsService _settings;
        private StorageFile _databaseFile;
        private GroupVm _recycleBin;
        private CompositeKey _compositeKey;

        public GroupVm RootGroup { get; set; }

        public GroupVm RecycleBin
        {
            get { return _recycleBin; }
            set
            {
                _recycleBin = value;
                _pwDatabase.RecycleBinUuid = _recycleBin?.IdUuid;
            }
        }
        
        public string Name => _databaseFile?.Name;
        
        public bool RecycleBinEnabled
        {
            get { return _pwDatabase.RecycleBinEnabled; }
            set { _pwDatabase.RecycleBinEnabled = value; }
        }
        
        public PwUuid DataCipher
        {
            get { return _pwDatabase.DataCipherUuid; }
            set { _pwDatabase.DataCipherUuid = value; }
        }

        public PwCompressionAlgorithm CompressionAlgorithm
        {
            get { return _pwDatabase.Compression; }
            set { _pwDatabase.Compression = value; }
        }

        public KdfParameters KeyDerivation
        {
            get { return _pwDatabase.KdfParameters; }
            set { _pwDatabase.KdfParameters = value; }
        }

        public bool IsOpen => _pwDatabase.IsOpen;
        public bool IsFileOpen => !_pwDatabase.IsOpen && _databaseFile != null;
        public bool IsClosed => _databaseFile == null;
        public bool HasChanged { get; set; }
        
        public DatabaseService() : this(SettingsService.Instance)
        {
        }

        public DatabaseService(ISettingsService settings)
        {
            _settings = settings;
        }
        

        /// <summary>
        /// Open a KeePass database
        /// </summary>
        /// <param name="databaseFile">The database file</param>
        /// <param name="key">The database composite key</param>
        /// <param name="createNew">True to create a new database before opening it</param>
        /// <returns>An error message, if any</returns>
        public void Open(StorageFile databaseFile, CompositeKey key, bool createNew = false)
        {
            try
            {
                if (databaseFile == null)
                {
                    throw new ArgumentNullException(nameof(databaseFile));
                }
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                
                _compositeKey = key;
                var ioConnection = IOConnectionInfo.FromFile(databaseFile);
                if (createNew)
                {
                    _pwDatabase.New(ioConnection, key);

                    //Get settings default values
                    if (_settings.GetSetting<bool>("Sample")) CreateSampleData();
                    var fileFormat = _settings.GetSetting<string>("DefaultFileFormat");
                    switch (fileFormat)
                    {
                        case "4":
                            KeyDerivation = KdfPool.Get("Argon2").GetDefaultParameters();
                            break;
                    }
                }
                else _pwDatabase.Open(ioConnection, key, new NullStatusLogger());

                _databaseFile = databaseFile;
                RootGroup = new GroupVm(_pwDatabase.RootGroup, null, RecycleBinEnabled ? _pwDatabase.RecycleBinUuid : null);
            }
            catch (InvalidCompositeKeyException ex)
            {
                HockeyClient.Current.TrackException(ex);
                throw new ArgumentException(ex.Message, ex);
            }
        }

        public void ReOpen()
        {
            Open(_databaseFile, _compositeKey);
        }

        /// <summary>
        /// Commit the changes to the currently opened database to file
        /// </summary>
        public void Save()
        {
            if (!IsOpen) return;
            try
            {
                _pwDatabase.Save(new NullStatusLogger());
            }
            catch (Exception e)
            {
                throw new SaveException(e);
            }
        }

        /// <summary>
        /// Save the current database to another file and open it
        /// </summary>
        /// <param name="file">The new database file</param>
        public void Save(StorageFile file)
        {
            var oldFile = _databaseFile;
            _databaseFile = file;
            try
            {
                _pwDatabase.SaveAs(IOConnectionInfo.FromFile(_databaseFile), true, new NullStatusLogger());
            }
            catch
            {
                _databaseFile = oldFile;
                throw;
            }
        }

        /// <summary>
        /// Close the currently opened database
        /// </summary>
        public void Close(bool releaseFile = true)
        {
            _pwDatabase?.Close();
            if (releaseFile) _databaseFile = null;
        }

        public void AddDeletedItem(PwUuid id)
        {
            _pwDatabase.DeletedObjects.Add(new PwDeletedObject(id, DateTime.UtcNow));
        }

        public void CreateRecycleBin(string title)
        {
            RecycleBin = RootGroup.AddNewGroup(title);
            RecycleBin.IsSelected = true;
            RecycleBin.IconId = (int)PwIcon.TrashBin;
        }

        public void UpdateCompositeKey(CompositeKey newCompositeKey)
        {
            if (newCompositeKey != null) _compositeKey = newCompositeKey;
        }
        
        private void CreateSampleData()
        {
            _pwDatabase.RootGroup.AddGroup(new PwGroup(true, true, "Banking", PwIcon.Count), true);
            _pwDatabase.RootGroup.AddGroup(new PwGroup(true, true, "Email", PwIcon.EMail), true);
            _pwDatabase.RootGroup.AddGroup(new PwGroup(true, true, "Internet", PwIcon.World), true);

            var pe = new PwEntry(true, true);
            pe.Strings.Set(PwDefs.TitleField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectTitle,
                "Sample Entry"));
            pe.Strings.Set(PwDefs.UserNameField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectUserName,
                "Username"));
            pe.Strings.Set(PwDefs.UrlField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectUrl,
                PwDefs.HomepageUrl));
            pe.Strings.Set(PwDefs.PasswordField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectPassword,
                "Password"));
            pe.Strings.Set(PwDefs.NotesField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectNotes,
                "You may safely delete this sample"));
            _pwDatabase.RootGroup.AddEntry(pe, true);

            pe = new PwEntry(true, true);
            pe.Strings.Set(PwDefs.TitleField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectTitle,
                "Sample Entry #2"));
            pe.Strings.Set(PwDefs.UserNameField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectUserName,
                "Michael321"));
            pe.Strings.Set(PwDefs.UrlField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectUrl,
                PwDefs.HelpUrl + "kb/testform.html"));
            pe.Strings.Set(PwDefs.PasswordField, new ProtectedString(_pwDatabase.MemoryProtection.ProtectPassword,
                "12345"));
            pe.AutoType.Add(new AutoTypeAssociation("*Test Form - KeePass*", string.Empty));
            _pwDatabase.RootGroup.AddEntry(pe, true);
        }
    }
}
