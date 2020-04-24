using System.Collections.Generic;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IRecentProxy
    {
        int EntryCount { get; }
        IEnumerable<FileInfo> GetAll();
        void ClearAll();
    }
}