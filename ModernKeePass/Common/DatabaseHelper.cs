using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Interfaces;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;

namespace ModernKeePass.Common
{
    public class DatabaseHelper
    {
        public enum DatabaseStatus
        {
            Closed = 0,
            Opening = 1,
            Opened = 2
        }
        private readonly PwDatabase _pwDatabase = new PwDatabase();
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
        
        public DatabaseStatus Status { get; private set; } = DatabaseStatus.Closed;
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
                _databaseFile = value;
                Status = DatabaseStatus.Opening;
            }
        }

        public StorageFile KeyFile { get; set; }

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

        /// <summary>
        /// Open a KeePass database
        /// </summary>
        /// <param name="password">The database password</param>
        /// <param name="createNew">True to create a new database before opening it</param>
        /// <returns>An error message, if any</returns>
        public string Open(string password, bool createNew = false)
        {
            var key = new CompositeKey();
            try
            {
                if (password != null) key.AddUserKey(new KcpPassword(password));
                if (KeyFile != null) key.AddUserKey(new KcpKeyFile(IOConnectionInfo.FromFile(KeyFile)));
                var ioConnection = IOConnectionInfo.FromFile(DatabaseFile);
                if (createNew) _pwDatabase.New(ioConnection, key);
                else _pwDatabase.Open(ioConnection, key, new NullStatusLogger());

                if (_pwDatabase.IsOpen)
                {
                    Status = DatabaseStatus.Opened;
                    RootGroup = new GroupVm(_pwDatabase.RootGroup, null, RecycleBinEnabled ? _pwDatabase.RecycleBinUuid : null);
                }
            }
            catch (ArgumentNullException)
            {
                return "Password cannot be empty";
            }
            catch (InvalidCompositeKeyException)
            {
                return "Wrong password";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        /// <summary>
        /// Save the current database to another file and open it
        /// </summary>
        /// <param name="file">The new database file</param>
        internal void Save(StorageFile file)
        {
            DatabaseFile = file;
            _pwDatabase.SaveAs(IOConnectionInfo.FromFile(DatabaseFile), true, new NullStatusLogger());
            Status = DatabaseStatus.Opened;
        }

        /// <summary>
        /// Commit the changes to the currently opened database to file
        /// </summary>
        public void Save()
        {
            // TODO: Save is disabled for now for Argon2Kdf because it corrupts DB (read works)
            if (_pwDatabase == null || !_pwDatabase.IsOpen || KdfPool.Get(_pwDatabase.KdfParameters.KdfUuid) is Argon2Kdf) return;
            _pwDatabase.Save(new NullStatusLogger());
        }

        /// <summary>
        /// Close the currently opened database
        /// </summary>
        public void Close()
        {
            _pwDatabase?.Close();
            Status = DatabaseStatus.Closed;
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
    }
}
