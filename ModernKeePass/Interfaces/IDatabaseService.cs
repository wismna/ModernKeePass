using System.Threading.Tasks;
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
        //int Status { get; set; }
        GroupVm RootGroup { get; set; }
        GroupVm RecycleBin { get; set; }
        StorageFile DatabaseFile { get; set; }
        CompositeKey CompositeKey { get; }
        PwUuid DataCipher { get; set; }
        PwCompressionAlgorithm CompressionAlgorithm { get; set; }
        KdfParameters KeyDerivation { get; set; }
        bool IsOpen { get; }
        bool IsFileOpen { get; }
        bool IsClosed { get; }
        bool HasChanged { get; set; }

        Task Open(CompositeKey key, bool createNew = false);
        Task ReOpen();
        void UpdateCompositeKey(CompositeKey key);
        void Save();
        void Save(StorageFile file);
        void CreateRecycleBin();
        void AddDeletedItem(PwUuid id);
        Task Close(bool releaseFile = true);
    }
}