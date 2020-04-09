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
        private readonly StorageItemAccessList _fal = StorageApplicationPermissions.FutureAccessList;

        public int EntryCount => _mru.Entries.Count;

        public async Task<FileInfo> Get(string token, bool updateAccessTime = false)
        {
            try
            {
                var file = await _mru.GetFileAsync(token,
                    updateAccessTime ? AccessCacheOptions.None : AccessCacheOptions.SuppressAccessTimeUpdate).AsTask().ConfigureAwait(false);
                _fal.AddOrReplace(token, file);
                return new FileInfo
                {
                    Id = token,
                    Name = file.DisplayName,
                    Path = file.Path
                };
            }
            catch (Exception)
            {
                _mru.Remove(token);
                return null;
            }
        }

        public IEnumerable<FileInfo> GetAll()
        {
            return _mru.Entries.Select(e => new FileInfo
            {
                Id = e.Token,
                Name = e.Metadata?.Substring(e.Metadata.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1),
                Path = e.Metadata
            });
        }

        public async Task Add(FileInfo recentItem)
        {
            var file = await _fal.GetFileAsync(recentItem.Id).AsTask();
            _mru.Add(file, file.Path);
        }

        public void ClearAll()
        {
            foreach (var entry in _mru.Entries)
            {
                if (_fal.ContainsItem(entry.Token)) _fal.Remove(entry.Token);
                _mru.Remove(entry.Token);
            }
        }
    }
}