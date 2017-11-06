using Windows.Security.Cryptography.Core;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class SHA512Managed: HashAlgorithm
    {
        public SHA512Managed() : base(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha512).CreateHash()) {}
    }
}
