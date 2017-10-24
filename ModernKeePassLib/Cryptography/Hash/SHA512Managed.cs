using Org.BouncyCastle.Crypto.Digests;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA512Managed: DigestManaged
    {
        public SHA512Managed()
        {
            Hash = new Sha512Digest();
        }
    }
}
