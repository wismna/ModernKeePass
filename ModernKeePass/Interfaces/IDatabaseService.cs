﻿using System.Threading.Tasks;
using Windows.Storage;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Keys;

namespace ModernKeePass.Interfaces
{
    public interface IDatabaseService
    {
        string Name { get; }
        bool RecycleBinEnabled { get; set; }
        GroupVm RootGroup { get; set; }
        GroupVm RecycleBin { get; set; }
        PwUuid DataCipher { get; set; }
        PwCompressionAlgorithm CompressionAlgorithm { get; set; }
        KdfParameters KeyDerivation { get; set; }
        bool IsOpen { get; }
        bool HasChanged { get; set; }

        Task Open(StorageFile databaseFile, CompositeKey key, bool createNew = false);
        Task ReOpen();
        void Save();
        Task Save(StorageFile file);
        void CreateRecycleBin(string title);
        void AddDeletedItem(PwUuid id);
        void Close(bool releaseFile = true);
        void UpdateCompositeKey(CompositeKey newCompositeKey);
    }
}