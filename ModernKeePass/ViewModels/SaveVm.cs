using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class SaveVm
    {
        private readonly IDatabaseService _database;
        public SaveVm() : this(DatabaseService.Instance) { }

        public SaveVm(IDatabaseService database)
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