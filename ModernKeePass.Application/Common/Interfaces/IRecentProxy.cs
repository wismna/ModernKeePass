using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IRecentProxy
    {
        int EntryCount { get; }
        Task<byte[]> Get(string token, bool updateAccessTime = true);
        IEnumerable<FileInfo> GetAll();
        void ClearAll();
    }
}