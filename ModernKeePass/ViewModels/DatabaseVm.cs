using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;

using ModernKeePassLib;
using ModernKeePassLib.Keys;
using ModernKeePassLib.Serialization;
using ModernKeePassLib.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class DatabaseVm
    {
        private PwDatabase _database = new PwDatabase();

        public string Name { get; set; }
        public ObservableCollection<GroupVm> Groups { get; set; }

        public async void Open(StorageFile databaseFile, string password)
        {
            var key = new CompositeKey();
            key.AddUserKey(new KcpPassword(password));
            try
            {
                await _database.Open(IOConnectionInfo.FromFile(databaseFile), key, new NullStatusLogger());
                if (!_database.IsOpen) return;
                Name = databaseFile.DisplayName;
                Groups = new ObservableCollection<GroupVm>(_database.RootGroup.Groups.Select(g => new GroupVm { Name = g.Name }));
            }
            finally
            {
                _database.Close();
            }
        }
    }
}
