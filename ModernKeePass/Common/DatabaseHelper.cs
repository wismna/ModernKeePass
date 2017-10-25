using System;
using Windows.Storage;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
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
        private PwDatabase _pwDatabase = new PwDatabase();
        private StorageFile _databaseFile;

        public GroupVm RootGroup { get; set; }
        public DatabaseStatus Status { get; private set; } = DatabaseStatus.Closed;
        public string Name => DatabaseFile?.Name;

        public StorageFile DatabaseFile
        {
            get { return _databaseFile; }
            set
            {
                _databaseFile = value;
                Status = DatabaseStatus.Opening;
            }
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
                key.AddUserKey(new KcpPassword(password));
                var ioConnection = IOConnectionInfo.FromFile(DatabaseFile);
                if (createNew) _pwDatabase.New(ioConnection, key);
                else _pwDatabase.Open(ioConnection, key, new NullStatusLogger());

                if (_pwDatabase.IsOpen)
                {
                    Status = DatabaseStatus.Opened;
                    RootGroup = new GroupVm(_pwDatabase.RootGroup, null);
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
            if (_pwDatabase != null && _pwDatabase.IsOpen)
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
    }
}
