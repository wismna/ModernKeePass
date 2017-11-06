using Windows.Security.Cryptography.Core;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA256Managed : HashAlgorithm
    {
        public SHA256Managed() : base(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256).CreateHash()) {}
    }
}
