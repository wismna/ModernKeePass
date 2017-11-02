using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMACSHA256: HMAC
    {
        public HMACSHA256(byte[] key)
        {
            Hmac = new HMac(new Sha256Digest());
            Hmac.Init(new KeyParameter(key));
        }
    }
}
