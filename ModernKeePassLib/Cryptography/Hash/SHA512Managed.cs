using Org.BouncyCastle.Crypto.Digests;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA512Managed: HashAlgorithm
    {
        public SHA512Managed()
        {
            Digest = new Sha512Digest();
        }
    }
}
