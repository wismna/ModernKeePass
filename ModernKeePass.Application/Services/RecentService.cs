using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class RecentService: IRecentService
    {
        private readonly IRecentProxy _recentProxy;

        public RecentService(IRecentProxy recentProxy)
        {
            _recentProxy = recentProxy;
        }

        public bool HasEntries => _recentProxy.EntryCount > 0;

        public async Task<FileInfo> Get(string token)
        {
            return await _recentProxy.Get(token);
        }

        public async Task<IEnumerable<FileInfo>> GetAll()
        {
            return await _recentProxy.GetAll();
        }

        public async Task Add(FileInfo recentItem)
        {
            await _recentProxy.Add(recentItem);
        }

        public void ClearAll()
        {
            _recentProxy.ClearAll();
        }
    }
}