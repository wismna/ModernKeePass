using System;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Keys;
using Windows.Storage;

namespace ModernKeePassApp.Test.Mock
{
    public class DatabaseServiceMock : IDatabaseService
    {
        private bool _isOpen;
        private CompositeKey _compositeKey;
        private StorageFile _databaseFile;

        public PwCompressionAlgorithm CompressionAlgorithm { get; set; }
        
        public PwUuid DataCipher { get; set; }

        public KdfParameters KeyDerivation { get; set; }

        public bool IsOpen => _isOpen;

        public bool HasChanged { get; set; }

        public string Name => "MockDatabase";

        public GroupVm RecycleBin { get; set; }

        public bool RecycleBinEnabled { get; set; }

        public GroupVm RootGroup { get; set; }
        
        public void AddDeletedItem(PwUuid id)
        {
            throw new NotImplementedException();
        }
        
        public void Close(bool releaseFile = true)
        {
            _isOpen = false;
        }

        public void UpdateCompositeKey(CompositeKey newCompositeKey)
        {
            _compositeKey = newCompositeKey;
        }

        public void CreateRecycleBin(string title)
        {
            throw new NotImplementedException();
        }
        
        public void Open(StorageFile databaseFile, CompositeKey key, bool createNew = false)
        {
            _databaseFile = databaseFile;
            _compositeKey = key;
            _isOpen = true;
        }

        public void ReOpen()
        {
            Open(_databaseFile, _compositeKey);
        }

        public void Save()
        {
            // Do Nothing
        }

        public void Save(StorageFile file)
        {
            throw new NotImplementedException();
        }
    }
}
