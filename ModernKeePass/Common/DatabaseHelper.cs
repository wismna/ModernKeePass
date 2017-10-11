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
        private readonly PwDatabase _pwDatabase = new PwDatabase();

        public GroupVm RootGroup { get; set; }
        public DatabaseStatus Status { get; private set; } = DatabaseStatus.Closed;

        public string Name { get; private set; }
        public string Open(StorageFile databaseFile, string password)
        {
            var key = new CompositeKey();
            try
            {
                key.AddUserKey(new KcpPassword(password));
                _pwDatabase.Open(IOConnectionInfo.FromFile(databaseFile), key, new NullStatusLogger());

                if (_pwDatabase.IsOpen)
                {
                    Name = databaseFile.Name;
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

        public void Save()
        {
            if (_pwDatabase != null && _pwDatabase.IsOpen)
                _pwDatabase.Save(new NullStatusLogger());
        }

        public void Close()
        {
            _pwDatabase?.Close();
            Status = DatabaseStatus.Closed;
        }
    }
}
