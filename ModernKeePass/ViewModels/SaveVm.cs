using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class SaveVm
    {
        private readonly IDatabase _database;
        public SaveVm() : this((Application.Current as App)?.Database) { }

        public SaveVm(IDatabase database)
        {
            _database = database;
        }

        public async Task Save(bool close = true)
        {
            _database.Save();
            if (close)
                await _database.Close();
        }

        public void Save(StorageFile file)
        {
            _database.Save(file);
        }
    }
}