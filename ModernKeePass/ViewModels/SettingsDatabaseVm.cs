using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using ModernKeePass.Common;
using ModernKeePass.Interfaces;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.Cipher;

namespace ModernKeePass.ViewModels
{
    public class SettingsDatabaseVm: NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly App _app = (App)Application.Current;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private GroupVm _selectedItem;

        public bool HasRecycleBin
        {
            get { return _app.Database.RecycleBinEnabled; }
            set
            {
                _app.Database.RecycleBinEnabled = value;
                OnPropertyChanged();
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
                    if (CipherPool.GlobalPool[inx].CipherUuid.Equals(_app.Database.DataCipher)) return inx;
                }
                return -1;
            }
            set { _app.Database.DataCipher = CipherPool.GlobalPool[value].CipherUuid; }
        }

        public IEnumerable<string> Compressions => Enum.GetNames(typeof(PwCompressionAlgorithm)).Take((int)PwCompressionAlgorithm.Count);

        public string CompressionName
        {
            get { return Enum.GetName(typeof(PwCompressionAlgorithm), _app.Database.CompressionAlgorithm); }
            set { _app.Database.CompressionAlgorithm = (PwCompressionAlgorithm)Enum.Parse(typeof(PwCompressionAlgorithm), value); }
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

        public SettingsDatabaseVm()
        {
            Groups = _app.Database.RootGroup.Groups;
        }

        // TODO: Move to another setting class (or a static class)
        private T GetSetting<T>(string property)
        {
            try
            {
                return (T) Convert.ChangeType(_localSettings.Values[property], typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }
    }
}
