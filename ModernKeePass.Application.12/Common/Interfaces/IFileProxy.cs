using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IFileProxy
    {
        Task<byte[]> OpenBinaryFile(string path);
        Task<IList<string>> OpenTextFile(string path);
        Task WriteBinaryContentsToFile(string path, byte[] contents);
        void ReleaseFile(string path);
    }
}