using System.ComponentModel;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class OpenVm: INotifyPropertyChanged
    {
        public bool ShowPasswordBox => _database?.Status == (int) DatabaseHelper.DatabaseStatus.Opening;

        public string Name => _database?.Name;

        private readonly IDatabase _database;

        public OpenVm() : this((Application.Current as App)?.Database) { }

        public OpenVm(IDatabase database)
        {
            _database = database;
            if (database == null || database.Status != (int) DatabaseHelper.DatabaseStatus.Opening) return;
            OpenFile(database.DatabaseFile);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenFile(StorageFile file)
        {
            _database.DatabaseFile = file;
            NotifyPropertyChanged("Name");
            NotifyPropertyChanged("ShowPasswordBox");
            AddToRecentList(file);
        }
        
        private void AddToRecentList(StorageFile file)
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            mru.Add(file, file.DisplayName);
        }
    }
}
