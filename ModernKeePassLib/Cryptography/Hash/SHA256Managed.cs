using Org.BouncyCastle.Crypto.Digests;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA256Managed : HashAlgorithm
    {
        public SHA256Managed()
        {
            Digest = new Sha256Digest();
        }
    }
}
