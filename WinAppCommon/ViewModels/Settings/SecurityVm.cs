using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using MediatR;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Database.Queries.GetDatabase;
using ModernKeePass.Application.Parameters.Commands.SetCipher;
using ModernKeePass.Application.Parameters.Commands.SetCompression;
using ModernKeePass.Application.Parameters.Commands.SetKeyDerivation;
using ModernKeePass.Application.Parameters.Models;
using ModernKeePass.Application.Parameters.Queries.GetCiphers;
using ModernKeePass.Application.Parameters.Queries.GetCompressions;
using ModernKeePass.Application.Parameters.Queries.GetKeyDerivations;

namespace ModernKeePass.ViewModels.Settings
{
    // TODO: implement Kdf settings
    public class SecurityVm: ObservableObject
    {
        private readonly IMediator _mediator;
        private readonly DatabaseVm _database;

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

        public SecurityVm(IMediator mediator)
        {
            _mediator = mediator;
            _database = _mediator.Send(new GetDatabaseQuery()).GetAwaiter().GetResult();
            var ciphers = _mediator.Send(new GetCiphersQuery()).GetAwaiter().GetResult();
            Ciphers = new ObservableCollection<CipherVm>(ciphers);
            var keyDerivations = _mediator.Send(new GetKeyDerivationsQuery()).GetAwaiter().GetResult();
            KeyDerivations = new ObservableCollection<KeyDerivationVm>(keyDerivations);
        }
    }
}
