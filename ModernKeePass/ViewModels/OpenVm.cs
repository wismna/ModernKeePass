using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: NotifyPropertyChangedBase
    {
        public bool ShowPasswordBox => _database?.Status == (int) DatabaseService.DatabaseStatus.Opening;

        public string Name => _database?.Name;

        private readonly IDatabase _database;

        public OpenVm() : this((Application.Current as App)?.Database) { }

        public OpenVm(IDatabase database)
        {
            _database = database;
            if (database == null || database.Status != (int) DatabaseService.DatabaseStatus.Opening) return;
            OpenFile(database.DatabaseFile);
        }
        
        public void OpenFile(StorageFile file)
        {
            _database.DatabaseFile = file;
            OnPropertyChanged("Name");
            OnPropertyChanged("ShowPasswordBox");
            AddToRecentList(file);
        }
        
        private void AddToRecentList(StorageFile file)
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            mru.Add(file, file.DisplayName);
        }
    }
}
