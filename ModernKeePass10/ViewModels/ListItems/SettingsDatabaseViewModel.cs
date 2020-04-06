using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using ModernKeePass.Domain.AOP;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    // TODO: implement Kdf settings
    public class SettingsDatabaseViewModel: NotifyPropertyChangedBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly ICryptographyService _cryptographyService;

        public bool HasRecycleBin
        {
            get => _databaseService.IsRecycleBinEnabled;
            set { 
                // TODO: do something here
            }
        }

        public bool IsNewRecycleBin
        {
            get => _databaseService.RecycleBin == null;
            set
            {
                // TODO: make this work
                if (value) _databaseService.AddEntity(_databaseService.RootGroupEntity, new Entity{Name = "Recycle Bin"});
            }
        }

        public ObservableCollection<GroupEntity> Groups { get; set; }

        public ObservableCollection<Entity> Ciphers => new ObservableCollection<Entity>(_cryptographyService.Ciphers);

        public IEnumerable<string> Compressions => _cryptographyService.CompressionAlgorithms;

        public IEnumerable<Entity> KeyDerivations => _cryptographyService.KeyDerivations;

        public Entity SelectedCipher
        {
            get => Ciphers.FirstOrDefault(c => c.Id == _databaseService.Cipher.Id);
            set
            {
                if (_databaseService.Cipher != value)
                {
                    _databaseService.Cipher = value;
                    //OnPropertyChanged(nameof(SelectedCipher));
                }
            }
        }
        public Entity SelectedKeyDerivation
        {
            get => _databaseService.KeyDerivation;
            set
            {
                if (_databaseService.KeyDerivation != value)
                {
                    _databaseService.KeyDerivation = value;
                    OnPropertyChanged(nameof(SelectedKeyDerivation));
                }
            }
        }
        public string SelectedCompression
        {
            get => _databaseService.Compression;
            set
            {
                if (_databaseService.Compression != value)
                {
                    _databaseService.Compression = value;
                    OnPropertyChanged(nameof(SelectedCompression));
                }
            }
        }
        public GroupEntity SelectedRecycleBin
        {
            get => _databaseService.RecycleBin;
            set
            {
                if (_databaseService.RecycleBin != value)
                {
                    _databaseService.RecycleBin = value;
                    OnPropertyChanged(nameof(SelectedRecycleBin));
                }
            }
        }

        public SettingsDatabaseViewModel() : this(App.Container.Resolve<IDatabaseService>(), App.Container.Resolve<ICryptographyService>()) { }

        public SettingsDatabaseViewModel(IDatabaseService databaseService, ICryptographyService cryptographyService)
        {
            _databaseService = databaseService;
            _cryptographyService = cryptographyService;
            Groups = new ObservableCollection<GroupEntity>(_databaseService.RootGroupEntity.SubGroups);
        }
    }
}
