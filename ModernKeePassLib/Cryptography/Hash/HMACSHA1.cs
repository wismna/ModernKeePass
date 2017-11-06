using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography.Core;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMACSHA1: HashAlgorithm
    {
        public HMACSHA1(byte[] key) : base (MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1).CreateHash(key.AsBuffer())) {}
    }
}
