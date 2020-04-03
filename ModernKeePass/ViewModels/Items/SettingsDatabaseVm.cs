using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Group.Queries.GetGroup;
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

namespace ModernKeePass.ViewModels
{
    // TODO: implement Kdf settings
    public class SettingsDatabaseVm: NotifyPropertyChangedBase
    {
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;

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
            get { return string.IsNullOrEmpty(_database.RecycleBinId); }
            set
            {
                if (value) _mediator.Send(new SetRecycleBinCommand { RecycleBinId = null }).Wait();
            }
        }

        public ObservableCollection<IEntityVm> Groups { get; }
        public ObservableCollection<CipherVm> Ciphers { get; }
        public IEnumerable<string> Compressions => _mediator.Send(new GetCompressionsQuery()).GetAwaiter().GetResult();
        public ObservableCollection<KeyDerivationVm> KeyDerivations { get; }

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

        public IEntityVm SelectedRecycleBin
        {
            get { return Groups.FirstOrDefault(g => g.Id == _database.RecycleBinId); }
            set { _mediator.Send(new SetRecycleBinCommand { RecycleBinId = value.Id}).Wait(); }
        }

        public SettingsDatabaseVm() : this(App.Mediator) { }

        public SettingsDatabaseVm(IMediator mediator)
        {
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            var rootGroup = _mediator.Send(new GetGroupQuery { Id = _database.RootGroupId }).GetAwaiter().GetResult();
            Groups = new ObservableCollection<IEntityVm>(rootGroup.SubGroups);
            var ciphers = _mediator.Send(new GetCiphersQuery()).GetAwaiter().GetResult();
            Ciphers = new ObservableCollection<CipherVm>(ciphers);
            var keyDerivations = _mediator.Send(new GetKeyDerivationsQuery()).GetAwaiter().GetResult();
            KeyDerivations = new ObservableCollection<KeyDerivationVm>(keyDerivations);
        }
    }
}
