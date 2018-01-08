using System;
using System.Threading.Tasks;
using ModernKeePass.Interfaces;
using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.KeyDerivation;
using ModernKeePassLib.Keys;
using Windows.Storage;

namespace ModernKeePassApp.Test.Mock
{
    public class DatabaseServiceMock : IDatabase
    {
        private bool _isOpen;
        private bool _isFileOpen;
        private bool _isClosed;


        public PwCompressionAlgorithm CompressionAlgorithm { get; set; }

        public StorageFile DatabaseFile { get; set; }

        public PwUuid DataCipher { get; set; }

        public KdfParameters KeyDerivation { get; set; }

        public bool IsOpen
        {
            get { return _isOpen; }
        }

        public bool IsFileOpen
        {
            get { return _isFileOpen; }
        }

        public bool IsClosed
        {
            get { return _isClosed; }
        }

        public string Name => "MockDatabase";

        public GroupVm RecycleBin { get; set; }

        public bool RecycleBinEnabled { get; set; }

        public GroupVm RootGroup { get; set; }
        
        public void AddDeletedItem(PwUuid id)
        {
            throw new NotImplementedException();
        }
        
        public Task Close()
        {
            return Task.Run(() =>
            {
                _isClosed = true;
                _isOpen = false;
            });
        }

        public void CreateRecycleBin()
        {
            throw new NotImplementedException();
        }

        public Task Open(CompositeKey key, bool createNew)
        {
            return Task.Run(() =>
            {
                _isOpen = true;
                _isClosed = false;
            });
        }

        public void Save()
        {
            // Do Nothing
        }

        public void Save(StorageFile file)
        {
            throw new NotImplementedException();
        }
        
        public void UpdateCompositeKey(CompositeKey key)
        {
            throw new NotImplementedException();
        }
    }
}
