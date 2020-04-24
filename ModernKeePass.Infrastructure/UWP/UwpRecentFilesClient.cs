using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage.AccessCache;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpRecentFilesClient: IRecentProxy
    {
        private readonly StorageItemMostRecentlyUsedList _mru = StorageApplicationPermissions.MostRecentlyUsedList;

        public int EntryCount => _mru.Entries.Count;
        
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