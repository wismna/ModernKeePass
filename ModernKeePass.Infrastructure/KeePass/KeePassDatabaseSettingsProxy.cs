using System;
using System.Collections.Generic;
using System.Linq;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Entities;
using ModernKeePassLib;
using ModernKeePassLib.Cryptography.Cipher;
using ModernKeePassLib.Cryptography.KeyDerivation;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class KeePassDatabaseSettingsProxy: IDatabaseSettingsProxy
    {
        public IEnumerable<BaseEntity> Ciphers
        {
            get
            {
                for (var inx = 0; inx < CipherPool.GlobalPool.EngineCount; inx++)
                {
                    var cipher = CipherPool.GlobalPool[inx];
                    yield return new BaseEntity
                    {
                        Id = cipher.CipherUuid.ToHexString(),
                        Name = cipher.DisplayName
                    };
                }
            }
        }

        public IEnumerable<BaseEntity> KeyDerivations => KdfPool.Engines.Select(e => new BaseEntity
        {
            Id = e.Uuid.ToHexString(),
            Name = e.Name
        });

        public IEnumerable<string> CompressionAlgorithms => Enum.GetNames(typeof(PwCompressionAlgorithm)).Take((int) PwCompressionAlgorithm.Count);
    }
}