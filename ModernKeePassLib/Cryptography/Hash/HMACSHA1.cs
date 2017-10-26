using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMACSHA1: HMAC
    {
        public HMACSHA1(byte[] key)
        {
            _hmac = new HMac(new Sha1Digest());
            _hmac.Init(new KeyParameter(key));
        }
    }
}
