using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMACSHA1: HMAC
    {
        public HMACSHA1(byte[] key)
        {
            Hmac = new HMac(new Sha1Digest());
            Hmac.Init(new KeyParameter(key));
        }
    }
}
