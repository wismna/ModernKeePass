using System.Collections.Generic;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface ICryptographyClient
    {
        IEnumerable<BaseEntity> Ciphers { get; }
        IEnumerable<BaseEntity> KeyDerivations { get; }
        IEnumerable<string> CompressionAlgorithms { get; }
    }
}