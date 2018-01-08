using Windows.Storage;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: NotifyPropertyChangedBase
    {
        public bool ShowPasswordBox => _database.IsFileOpen;

        public string Name => _database?.Name;

        private readonly IDatabase _database;

        public OpenVm() : this((Application.Current as App)?.Database) { }

        public OpenVm(IDatabase database)
        {
            _database = database;
            if (database == null || !database.IsFileOpen) return;
            OpenFile(database.DatabaseFile);
        }

        public void OpenFile(StorageFile file)
        {
            OpenFile(file, new RecentService());
        }

        public void OpenFile(StorageFile file, IRecent recent)
        {
            _database.DatabaseFile = file;
            OnPropertyChanged("Name");
            OnPropertyChanged("ShowPasswordBox");
            AddToRecentList(file, recent);
        }
        
        private void AddToRecentList(StorageFile file, IRecent recent)
        {
            recent.Add(file, file.DisplayName);
        }
    }
}
