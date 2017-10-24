using Org.BouncyCastle.Crypto.Digests;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA256Managed : DigestManaged
    {
        public SHA256Managed()
        {
            Hash = new Sha256Digest();
        }
    }
}
