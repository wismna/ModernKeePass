using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<FileInfo> Get(string token)
        {
            var recentEntry = _mru.Entries.FirstOrDefault(e => e.Token == token);
            var file = await _mru.GetFileAsync(token);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(recentEntry.Metadata, file);
            return new FileInfo
            {
                Name = file.DisplayName,
                Path = recentEntry.Metadata
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
            var file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(recentItem.Path);
            _mru.Add(file, recentItem.Path);
        }

        public void ClearAll()
        {
            for (var i = _mru.Entries.Count; i > 0; i--)
            {
                var entry = _mru.Entries[i];
                StorageApplicationPermissions.FutureAccessList.Remove(entry.Metadata);
                _mru.Remove(entry.Token);
            }
        }
    }
}