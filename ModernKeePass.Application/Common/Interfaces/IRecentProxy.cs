using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IRecentProxy
    {
        int EntryCount { get; }
        Task<FileInfo> Get(string token, bool updateAccessTime = false);
        Task<IEnumerable<FileInfo>> GetAll();
        Task Add(FileInfo recentItem);
        void ClearAll();
    }
}