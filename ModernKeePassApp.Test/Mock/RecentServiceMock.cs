using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ModernKeePass.Interfaces;
using Windows.Storage;

namespace ModernKeePassApp.Test.Mock
{
    class RecentServiceMock : IRecentService
    {
        public int EntryCount => 0;

        public void Add(IStorageItem file, string metadata)
        {
            throw new NotImplementedException();
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<IRecentItem> GetAllFiles(bool removeIfNonExistant = true)
        {
            throw new NotImplementedException();
        }

        public Task<IStorageItem> GetFileAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
