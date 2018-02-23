using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ModernKeePass.Interfaces;
using Windows.Storage;
using Windows.Storage.AccessCache;
using ModernKeePass.ViewModels;

namespace ModernKeePass.Services
{
    public class RecentService : SingletonServiceBase<RecentService>, IRecentService
    {
        private readonly StorageItemMostRecentlyUsedList _mru = StorageApplicationPermissions.MostRecentlyUsedList;
        
        public int EntryCount => _mru.Entries.Count;

        public ObservableCollection<IRecentItem> GetAllFiles(bool removeIfNonExistant = true)
        {
            var result = new ObservableCollection<IRecentItem>();
            foreach (var entry in _mru.Entries)
            {
                try
                {
                    var file = _mru.GetFileAsync(entry.Token, AccessCacheOptions.SuppressAccessTimeUpdate).GetAwaiter().GetResult();
                    result.Add(new RecentItemVm(entry.Token, entry.Metadata, file));
                }
                catch (Exception)
                {
                    if (removeIfNonExistant) _mru.Remove(entry.Token);
                }
            }
            return result;
        }

        public void Add(IStorageItem file, string metadata)
        {
            _mru.Add(file, metadata);
        }

        public void ClearAll()
        {
            _mru.Clear();
        }

        public async Task<IStorageItem> GetFileAsync(string token)
        {
            return await _mru.GetFileAsync(token);
        }
    }
}
