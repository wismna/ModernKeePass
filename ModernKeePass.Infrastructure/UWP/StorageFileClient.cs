using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Infrastructure.UWP
{
    public class StorageFileClient: IFileProxy
    {
        public async Task<FileInfo> OpenFile(string name, string extension, bool addToRecent)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(extension);

            // Application now has read/write access to the picked file
            var file = await picker.PickSingleFileAsync().AsTask();
            if (file == null) return null;

            var token = addToRecent
                ? StorageApplicationPermissions.MostRecentlyUsedList.Add(file, file.Path)
                : StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            return new FileInfo
            {
                Id = token,
                Name = file.Name,
                Path = file.Path
            };
        }

        public async Task<FileInfo> CreateFile(string name, string extension, string description, bool addToRecent)
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = name
            };
            savePicker.FileTypeChoices.Add(description, new List<string> { extension });

            var file = await savePicker.PickSaveFileAsync().AsTask();
            if (file == null) return null;

            var token = addToRecent
                ? StorageApplicationPermissions.MostRecentlyUsedList.Add(file, file.Path)
                : StorageApplicationPermissions.FutureAccessList.Add(file, file.Name);
            return new FileInfo
            {
                Id = token,
                Name = file.Name,
                Path = file.Path
            };
        }

        public async Task<byte[]> ReadBinaryFile(string path)
        {
            var file = await GetFile(path);
            var result = await FileIO.ReadBufferAsync(file).AsTask();
            return result.ToArray();
        }

        public async Task<IList<string>> ReadTextFile(string path)
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