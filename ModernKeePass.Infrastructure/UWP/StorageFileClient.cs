using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.UWP
{
    public class StorageFileClient: IFileProxy
    {
        public async Task<byte[]> OpenBinaryFile(string path)
        {
            var file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(path);
            var result = await FileIO.ReadBufferAsync(file);
            return result.ToArray();
        }

        public async Task<IList<string>> OpenTextFile(string path)
        {
            var file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(path);
            var result = await FileIO.ReadLinesAsync(file);
            return result;
        }

        public void ReleaseFile(string path)
        {
            StorageApplicationPermissions.FutureAccessList.Remove(path);
        }

        public async Task WriteBinaryContentsToFile(string path, byte[] contents)
        {
            var file = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(path);
            await FileIO.WriteBytesAsync(file, contents);
        }
    }
}