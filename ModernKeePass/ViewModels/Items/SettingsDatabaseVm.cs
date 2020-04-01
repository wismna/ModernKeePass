using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MediatR;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Parameters.Commands.SetCipher;
using ModernKeePass.Application.Parameters.Commands.SetCompression;
using ModernKeePass.Application.Parameters.Commands.SetHasRecycleBin;
using ModernKeePass.Application.Parameters.Commands.SetKeyDerivation;
using ModernKeePass.Application.Parameters.Commands.SetRecycleBin;
using ModernKeePass.Application.Parameters.Models;
using ModernKeePass.Application.Parameters.Queries.GetCiphers;
using ModernKeePass.Application.Parameters.Queries.GetCompressions;
using ModernKeePass.Application.Parameters.Queries.GetKeyDerivations;
using ModernKeePass.Common;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels
{
    // TODO: implement Kdf settings
    public class SettingsDatabaseVm: NotifyPropertyChangedBase, IHasSelectableObject
    {
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;
        private GroupDetailVm _selectedItem;

        public bool HasRecycleBin
        {
            get { return _database.IsRecycleBinEnabled; }
            set
            {
                _mediator.Send(new SetHasRecycleBinCommand {HasRecycleBin = value}).Wait();
                OnPropertyChanged(nameof(HasRecycleBin));
            }
        }

        public bool IsNewRecycleBin
        {
            get { return _database.RecycleBin == null; }
            set
            {
                if (value) _mediator.Send(new SetRecycleBinCommand { RecycleBin = null }).Wait();
            }
        }

        public ObservableCollection<Application.Group.Models.GroupVm> Groups { get; set; }

        public IEnumerable<CipherVm> Ciphers => _mediator.Send(new GetCiphersQuery()).GetAwaiter().GetResult();
        public IEnumerable<string> Compressions => _mediator.Send(new GetCompressionsQuery()).GetAwaiter().GetResult();
        public IEnumerable<KeyDerivationVm> KeyDerivations => _mediator.Send(new GetKeyDerivationsQuery()).GetAwaiter().GetResult();

        public CipherVm SelectedCipher
        {
            get { return Ciphers.FirstOrDefault(c => c.Id == _database.CipherId); }
            set { _mediator.Send(new SetCipherCommand {CipherId = value.Id}).Wait(); }
        }

        public string SelectedCompression
        {
            get { return Compressions.FirstOrDefault(c => c == _database.Compression); }
            set { _mediator.Send(new SetCompressionCommand {Compression = value}).Wait(); }
        }

        public KeyDerivationVm SelectedKeyDerivation
        {
            get { return KeyDerivations.FirstOrDefault(c => c.Id == _database.KeyDerivationId); }
            set { _mediator.Send(new SetKeyDerivationCommand {KeyDerivationId = value.Id}).Wait(); }
        }

        /*public int CipherIndex
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
        }*/


        /*public string CompressionName
        {
            get { return _database.}
            set { _database.CompressionAlgorithm = (PwCompressionAlgorithm)Enum.Parse(typeof(PwCompressionAlgorithm), value); }
        }*/
        /*public string KeyDerivationName
        {
            get { return KdfPool.Get(_database.KeyDerivation.KdfUuid).Name; }
            set { _database.KeyDerivation = KdfPool.Engines.FirstOrDefault(e => e.Name == value)?.GetDefaultParameters(); } 
        }*/

        public ISelectableModel SelectedItem
        {
            get { return Groups.FirstOrDefault(g => g.IsSelected); }
            set
            {
                if (_selectedItem == value || IsNewRecycleBin) return;
                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = false;
                }

                SetProperty(ref _selectedItem, (GroupDetailVm)value);

                if (_selectedItem != null)
                {
                    _selectedItem.IsSelected = true;
                }
            }
        }

        public SettingsDatabaseVm() : this(App.Mediator) { }

        public SettingsDatabaseVm(IMediator mediator)
        {
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            Groups = new ObservableCollection<Application.Group.Models.GroupVm>(_database.RootGroup.SubGroups);
        }
    }
}
