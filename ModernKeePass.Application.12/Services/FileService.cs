using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class FileService: IFileService
    {
        private readonly IFileProxy _fileProxy;

        public FileService(IFileProxy fileProxy)
        {
            _fileProxy = fileProxy;
        }
        
        public Task<byte[]> OpenBinaryFile(string path)
        {
            return _fileProxy.OpenBinaryFile(path);
        }

        public Task WriteBinaryContentsToFile(string path, byte[] contents)
        {
            return _fileProxy.WriteBinaryContentsToFile(path, contents);
        }

        public Task<IList<string>> OpenTextFile(string path)
        {
            return _fileProxy.OpenTextFile(path);
        }

        public void ReleaseFile(string path)
        {
            _fileProxy.ReleaseFile(path);
        }
    }
}