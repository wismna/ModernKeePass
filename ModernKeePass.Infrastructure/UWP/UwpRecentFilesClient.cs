using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using Windows.Storage;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpRecentFilesClient: IRecentProxy
    {
        private readonly StorageItemMostRecentlyUsedList _mru = StorageApplicationPermissions.MostRecentlyUsedList;

        public int EntryCount => _mru.Entries.Count;

        public async Task<byte[]> Get(string token, bool updateAccessTime = true)
        {
            try
            {
                var file = await _mru.GetFileAsync(token,
                    updateAccessTime ? AccessCacheOptions.None : AccessCacheOptions.SuppressAccessTimeUpdate).AsTask().ConfigureAwait(false);

                var result = await FileIO.ReadBufferAsync(file).AsTask();
                return result.ToArray();
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
        
        public void ClearAll()
        {
            foreach (var entry in _mru.Entries)
            {
                _mru.Remove(entry.Token);
            }
        }
    }
}