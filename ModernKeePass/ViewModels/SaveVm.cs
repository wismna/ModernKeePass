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

        public void Save(bool close = true)
        {
            if (close && _database.Save()) _database.Close();
        }

        internal void Save(StorageFile file)
        {
            _database.Save(file);
        }
    }
}