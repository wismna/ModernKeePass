using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ModernKeePass.Interfaces;
using Windows.Storage;
using System.Collections.Generic;

namespace ModernKeePassApp.Test.Mock
{
    class RecentServiceMock : IRecentService
    {
        private Dictionary<string, IStorageItem> _recentItems = new Dictionary<string, IStorageItem>();

        public int EntryCount => 0;

        public void Add(IStorageItem file, string metadata)
        {
            _recentItems.Add(metadata, file);
        }

        public void ClearAll()
        {
            _recentItems.Clear();
        }

        public ObservableCollection<IRecentItem> GetAllFiles(bool removeIfNonExistant = true)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageItem> GetFileAsync(string token)
        {
            return Task.Run(() => _recentItems[token]);
        }
    }
}
