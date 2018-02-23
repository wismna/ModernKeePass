using Windows.Storage;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: NotifyPropertyChangedBase
    {
        public bool ShowPasswordBox => _database.IsFileOpen;

        public string Name => _database?.Name;

        private readonly IDatabaseService _database;

        public OpenVm() : this(DatabaseService.Instance) { }

        public OpenVm(IDatabaseService database)
        {
            _database = database;
            if (database == null || !database.IsFileOpen) return;
            OpenFile(database.DatabaseFile);
        }

        public void OpenFile(StorageFile file)
        {
            OpenFile(file, RecentService.Instance);
        }

        public void OpenFile(StorageFile file, IRecentService recent)
        {
            _database.DatabaseFile = file;
            OnPropertyChanged("Name");
            OnPropertyChanged("ShowPasswordBox");
            AddToRecentList(file, recent);
        }
        
        private void AddToRecentList(StorageFile file, IRecentService recent)
        {
            recent.Add(file, file.DisplayName);
        }
    }
}
