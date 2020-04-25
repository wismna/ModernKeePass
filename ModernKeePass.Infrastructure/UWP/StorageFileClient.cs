using System;
using System.Collections.Generic;
using System.IO;
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
            var file = await GetFile(path);
            var result = await FileIO.ReadBufferAsync(file).AsTask();
            return result.ToArray();
        }

        public async Task<IList<string>> OpenTextFile(string path)
        {
            var file = await GetFile(path);
            var result = await FileIO.ReadLinesAsync(file).AsTask();
            return result;
        }

        public void ReleaseFile(string path)
        {
            StorageApplicationPermissions.FutureAccessList.Remove(path);
        }

        public async Task WriteBinaryContentsToFile(string path, byte[] contents)
        {
            var file = await GetFile(path);
            await FileIO.WriteBytesAsync(file, contents).AsTask();
        }

        private async Task<StorageFile> GetFile(string token)
        {
            if (StorageApplicationPermissions.MostRecentlyUsedList.ContainsItem(token))
            {
                try
                {
                    return await StorageApplicationPermissions.MostRecentlyUsedList.GetFileAsync(token).AsTask();
                }
                catch (Exception)
                {
                    StorageApplicationPermissions.MostRecentlyUsedList.Remove(token);
                    throw new FileNotFoundException();
                }
            }

            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
                return await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token).AsTask();
            throw new FileNotFoundException();
        }
    }
}