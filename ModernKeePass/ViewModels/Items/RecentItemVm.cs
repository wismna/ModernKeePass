using Windows.Storage;
using ModernKeePass.Common;
using Windows.UI.Xaml;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class RecentItemVm: NotifyPropertyChangedBase, ISelectableModel, IRecentItem
    {
        private bool _isSelected;
        
        public StorageFile DatabaseFile { get; }
        public string Token { get; }
        public string Name { get; }
        public string Path => DatabaseFile?.Path;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public RecentItemVm() {}
        public RecentItemVm(string token, string metadata, IStorageItem file)
        {
            Token = token;
            Name = metadata;
            DatabaseFile = file as StorageFile;
        }

        public void OpenDatabaseFile()
        {
            OpenDatabaseFile((Application.Current as App)?.Database);
        }

        public void OpenDatabaseFile(IDatabase database)
        {
            database.DatabaseFile = DatabaseFile;
        }

        public void UpdateAccessTime()
        {
            UpdateAccessTime(new RecentService());
        }

        public async void UpdateAccessTime(IRecent recent)
        {
            await recent.GetFileAsync(Token);
        }
    }
}
