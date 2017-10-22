using System;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMACSHA256: IDisposable

    {
    private readonly HMac _hmac;

    public HMACSHA256(byte[] key)
    {
        _hmac = new HMac(new Sha256Digest());
        _hmac.Init(new KeyParameter(key));
    }

    public byte[] ComputeHash(byte[] value)
    {
        if (value == null) throw new ArgumentNullException("value");

        byte[] resBuf = new byte[_hmac.GetMacSize()];
        _hmac.BlockUpdate(value, 0, value.Length);
        _hmac.DoFinal(resBuf, 0);

        return resBuf;
    }

        public void Dispose()
        {
            _hmac.Reset();
        }
    }
}
