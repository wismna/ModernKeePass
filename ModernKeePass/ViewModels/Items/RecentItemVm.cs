using System;
using Windows.Storage;
using ModernKeePass.Common;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using ModernKeePass.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class RecentItemVm: NotifyPropertyChangedBase, ISelectableModel
    {
        private bool _isSelected;

        public RecentItemVm() {}
        public RecentItemVm(AccessListEntry entry, StorageFile file)
        {
            Token = entry.Token;
            Name = entry.Metadata;
            DatabaseFile = file;
        }

        public StorageFile DatabaseFile { get; }
        public string Token { get; }
        public string Name { get; }
        public string Path => DatabaseFile?.Path;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public void OpenDatabaseFile()
        {
            OpenDatabaseFile((Application.Current as App)?.Database);
        }

        public void OpenDatabaseFile(IDatabase database)
        {
            database.DatabaseFile = DatabaseFile;
        }

        public async void UpdateAccessTime()
        {
            var mru = StorageApplicationPermissions.MostRecentlyUsedList;
            await mru.GetFileAsync(Token);
        }
    }
}
