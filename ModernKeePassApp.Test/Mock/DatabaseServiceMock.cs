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
        private bool _isClosed;
        private CompositeKey _compositeKey;
        
        public PwCompressionAlgorithm CompressionAlgorithm { get; set; }

        public StorageFile DatabaseFile { get; set; }

        public CompositeKey CompositeKey
        {
            get { return _compositeKey; }
            set { _compositeKey = value; }
        }

        public PwUuid DataCipher { get; set; }

        public KdfParameters KeyDerivation { get; set; }

        public bool IsOpen => _isOpen;

        public bool IsFileOpen => DatabaseFile != null;

        public bool IsClosed => _isClosed;

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
            _isClosed = true;
            _isOpen = false;
        }

        public void CreateRecycleBin(string title)
        {
            throw new NotImplementedException();
        }

        public void Open(CompositeKey key, bool createNew = false)
        {
            _compositeKey = key;
            _isOpen = true;
            _isClosed = false;
        }

        public void ReOpen()
        {
            Open(_compositeKey);
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
