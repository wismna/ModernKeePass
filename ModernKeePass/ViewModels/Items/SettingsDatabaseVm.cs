using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Cryptography.KeyDerivation;

namespace ModernKeePass.ViewModels
{
    // TODO: implement Kdf settings
    public class SettingsDatabaseVm: NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly IDatabase _database;
        private GroupVm _selectedItem;

        public bool HasRecycleBin
        {
            get { return _database.RecycleBinEnabled; }
            set
            {
                _database.RecycleBinEnabled = value;
                OnPropertyChanged("HasRecycleBin");
            }
        }

        public ObservableCollection<GroupVm> Groups { get; set; }

        public IEnumerable<string> Ciphers
        {
            get
            {
                for (var inx = 0; inx < CipherPool.GlobalPool.EngineCount; inx++)
                {
                    yield return CipherPool.GlobalPool[inx].DisplayName;
                }
            }
        }

        public int CipherIndex
        {
            get
            {
                for (var inx = 0; inx < CipherPool.GlobalPool.EngineCount; ++inx)
                {
                    if (CipherPool.GlobalPool[inx].CipherUuid.Equals(_database.DataCipher)) return inx;
                }
                return -1;
            }
            set { _database.DataCipher = CipherPool.GlobalPool[value].CipherUuid; }
        }

        public IEnumerable<string> Compressions => Enum.GetNames(typeof(PwCompressionAlgorithm)).Take((int)PwCompressionAlgorithm.Count);

        public string CompressionName
        {
            get { return Enum.GetName(typeof(PwCompressionAlgorithm), _database.CompressionAlgorithm); }
            set { _database.CompressionAlgorithm = (PwCompressionAlgorithm)Enum.Parse(typeof(PwCompressionAlgorithm), value); }
        }
        public IEnumerable<string> KeyDerivations => KdfPool.Engines.Select(e => e.Name);

        public string KeyDerivationName
        {
            get { return KdfPool.Get(_database.KeyDerivation.KdfUuid).Name; }
            set { _database.KeyDerivation = KdfPool.Engines.FirstOrDefault(e => e.Name == value)?.GetDefaultParameters(); } 
        }

        public ISelectableModel SelectedItem
        {
            get { return Groups.FirstOrDefault(g => g.IsSelected); }
            set
            {
                if (_selectedItem == value) return;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                SetProperty(ref _selectedItem, (GroupVm)value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }

        public SettingsDatabaseVm() : this((Application.Current as App)?.Database) { }

        public SettingsDatabaseVm(IDatabase database)
        {
            _database = database;
            Groups = _database?.RootGroup.Groups;
        }
    }
}
