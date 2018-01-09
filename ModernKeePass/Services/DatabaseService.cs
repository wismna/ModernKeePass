using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
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
    public class DatabaseService: IDatabase
    {
        private readonly PwDatabase _pwDatabase = new PwDatabase();
        private readonly ISettings _settings;
        private StorageFile _realDatabaseFile;
        private StorageFile _databaseFile;
        private GroupVm _recycleBin;

        public GroupVm RootGroup { get; set; }

        public GroupVm RecycleBin
        {
            get { return _recycleBin; }
            set
            {
                _recycleBin = value;
                _pwDatabase.RecycleBinUuid = _recycleBin.IdUuid;
            }
        }
        
        public string Name => DatabaseFile?.Name;
        
        public bool RecycleBinEnabled
        {
            get { return _pwDatabase.RecycleBinEnabled; }
            set { _pwDatabase.RecycleBinEnabled = value; }
        }
        
        public StorageFile DatabaseFile
        {
            get { return _databaseFile; }
            set
            {
                if (IsOpen)
                {
                    throw new DatabaseOpenedException();
                }
                _databaseFile = value;
            }
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

        public DatabaseService() : this(new SettingsService())
        { }

        public DatabaseService(ISettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Open a KeePass database
        /// </summary>
        /// <param name="key">The database composite key</param>
        /// <param name="createNew">True to create a new database before opening it</param>
        /// <returns>An error message, if any</returns>
        public async Task Open(CompositeKey key, bool createNew = false)
        {
            // TODO: Check if there is an existing backup file
            try
            {
                if (key == null)
                {
                    throw new ArgumentNullException(nameof(key));
                }
                var ioConnection = IOConnectionInfo.FromFile(DatabaseFile);
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

                if (!_pwDatabase.IsOpen) return;

                // Copy database in temp directory and use this file for operations
                if (_settings.GetSetting<bool>("AntiCorruption"))
                {
                    _realDatabaseFile = _databaseFile;
                    var backupFile =
                        await ApplicationData.Current.RoamingFolder.CreateFileAsync(Name,
                            CreationCollisionOption.FailIfExists);
                    Save(backupFile);
                }

                //Status = (int)DatabaseStatus.Opened;
                RootGroup = new GroupVm(_pwDatabase.RootGroup, null, RecycleBinEnabled ? _pwDatabase.RecycleBinUuid : null);
            }
            catch (InvalidCompositeKeyException ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Commit the changes to the currently opened database to file
        /// </summary>
        public void Save()
        {
            if (!_pwDatabase.IsOpen) return;
            try
            {
                _pwDatabase.Save(new NullStatusLogger());

                // Test if save worked correctly
                if (_settings.GetSetting<bool>("AntiCorruption"))
                {
                    _pwDatabase.Open(_pwDatabase.IOConnectionInfo, _pwDatabase.MasterKey, new NullStatusLogger());
                }
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
            var oldFile = DatabaseFile;
            DatabaseFile = file;
            try
            {
                _pwDatabase.SaveAs(IOConnectionInfo.FromFile(DatabaseFile), true, new NullStatusLogger());
            }
            catch
            {
                DatabaseFile = oldFile;
                throw;
            }
        }

        /// <summary>
        /// Close the currently opened database
        /// </summary>
        public async Task Close()
        {
            _pwDatabase?.Close();

            // Restore the backup DB to the original one
            if (_settings.GetSetting<bool>("AntiCorruption"))
            {
                if (_pwDatabase.Modified)
                    Save(_realDatabaseFile);
                await DatabaseFile.DeleteAsync();
            }
            DatabaseFile = null;
        }

        public void AddDeletedItem(PwUuid id)
        {
            _pwDatabase.DeletedObjects.Add(new PwDeletedObject(id, DateTime.UtcNow));
        }

        public void CreateRecycleBin()
        {
            RecycleBin = RootGroup.AddNewGroup("Recycle bin");
            RecycleBin.IsSelected = true;
            RecycleBin.IconSymbol = Symbol.Delete;
        }

        public void UpdateCompositeKey(CompositeKey key)
        {
            _pwDatabase.MasterKey = key;
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
