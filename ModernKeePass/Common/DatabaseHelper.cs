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
        private PwDatabase _pwDatabase = new PwDatabase();
        private StorageFile databaseFile;

        public GroupVm RootGroup { get; set; }
        public bool IsOpen { get; set; }
        public string Name { get; set; }

        public DatabaseHelper(StorageFile databaseFile)
        {
            this.databaseFile = databaseFile;
        }
        public string Open(string password)
        {
            var key = new CompositeKey();
            try
            {
                key.AddUserKey(new KcpPassword(password));
                _pwDatabase.Open(IOConnectionInfo.FromFile(databaseFile), key, new NullStatusLogger());
                //_pwDatabase.Open(IOConnectionInfo.FromPath(databaseFile.Path), key, new NullStatusLogger());
                IsOpen = _pwDatabase.IsOpen;
                Name = databaseFile.DisplayName;
                RootGroup = new GroupVm(_pwDatabase.RootGroup);
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
            //_pwDatabase.Close();
        }
    }
}
