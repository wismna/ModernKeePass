using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpRecentFilesClient: IRecentProxy
    {
        private readonly StorageItemMostRecentlyUsedList _mru = StorageApplicationPermissions.MostRecentlyUsedList;

        public int EntryCount => _mru.Entries.Count;

        public async Task<FileInfo> Get(string token, bool updateAccessTime = false)
        {
            var file = await _mru.GetFileAsync(token, updateAccessTime ? AccessCacheOptions.None : AccessCacheOptions.SuppressAccessTimeUpdate);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(token, file);
            return new FileInfo
            {
                Id = token,
                Name = file.DisplayName,
                Path = file.Path
            };
        }

        public async Task<IEnumerable<FileInfo>> GetAll()
        {
            var result = new List<FileInfo>();
            foreach (var entry in _mru.Entries)
            {
                try
                {
                    var recentItem = await Get(entry.Token);
                    result.Add(recentItem);
                }
                catch (Exception)
                {
                    _mru.Remove(entry.Token);
                }
            }
            return result;
        }

        public async Task Add(FileInfo recentItem)
        {
            var file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(recentItem.Id);
            _mru.Add(file);
        }

        public void ClearAll()
        {
            for (var i = _mru.Entries.Count; i > 0; i--)
            {
                var entry = _mru.Entries[i];
                StorageApplicationPermissions.FutureAccessList.Remove(entry.Token);
                _mru.Remove(entry.Token);
            }
        }
    }
}