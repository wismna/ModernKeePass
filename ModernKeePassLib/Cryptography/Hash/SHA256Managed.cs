using System;
using Org.BouncyCastle.Crypto.Digests;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA256Managed : DigestManaged
    {
        public SHA256Managed()
        {
            _hash = new Sha256Digest();
        }
    }
}
