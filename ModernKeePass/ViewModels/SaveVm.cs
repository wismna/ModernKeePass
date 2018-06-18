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

        public void Save(bool close = true)
        {
            _database.Save();
            if (close) _database.Close();
        }

        public void Save(StorageFile file)
        {
            _database.Save(file);
        }
    }
}