using System;
using Windows.Storage;
using System.Threading.Tasks;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Interfaces;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;

namespace ModernKeePass.Common
{
    public class DatabaseHelper
    {
        private readonly PwDatabase _pwDatabase = new PwDatabase();
        private readonly StorageFile _databaseFile;

        public GroupVm RootGroup { get; set; }

        public bool IsOpen => _pwDatabase.IsOpen;

        public string Name => _databaseFile.Name;

        public DatabaseHelper(StorageFile databaseFile)
        {
            this._databaseFile = databaseFile;
        }
        public string Open(string password)
        {
            var key = new CompositeKey();
            try
            {
                key.AddUserKey(new KcpPassword(password));
                _pwDatabase.Open(IOConnectionInfo.FromFile(_databaseFile), key, new NullStatusLogger());
                //_pwDatabase.Open(IOConnectionInfo.FromPath(databaseFile.Path), key, new NullStatusLogger());

                if (IsOpen) RootGroup = new GroupVm(_pwDatabase.RootGroup);
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
            /*finally
            {
                // TODO: move this when implementing write mode
                _pwDatabase.Close();
            }*/
            return string.Empty;
        }

        public void Save()
        {
            _pwDatabase.Save(new NullStatusLogger());
        }

        public void Close()
        {
            _pwDatabase.Close();
        }
    }
}
