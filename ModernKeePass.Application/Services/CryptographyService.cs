using System.Collections.Generic;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class CryptographyService : ICryptographyService
    {
        private readonly ICryptographyClient _cryptographyClient;
        public IEnumerable<Entity> Ciphers => _cryptographyClient.Ciphers;

        public IEnumerable<Entity> KeyDerivations => _cryptographyClient.KeyDerivations;

        public IEnumerable<string> CompressionAlgorithms => _cryptographyClient.CompressionAlgorithms;

        public CryptographyService(ICryptographyClient cryptographyClient)
        {
            _cryptographyClient = cryptographyClient;
        }
    }
}