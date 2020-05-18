using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IFileProxy
    {
        Task<FileInfo> OpenFile(string name, string extension, bool addToRecent);
        Task<FileInfo> CreateFile(string name, string extension, string description, bool addToRecent);
        Task<byte[]> ReadBinaryFile(string path);
        Task<IList<string>> ReadTextFile(string path);
        Task WriteToLogFile(IEnumerable<string> data);
        Task WriteBinaryContentsToFile(string path, byte[] contents);
        void ReleaseFile(string path);
    }
}