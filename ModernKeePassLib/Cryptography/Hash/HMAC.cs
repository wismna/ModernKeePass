using System;
using Org.BouncyCastle.Crypto.Macs;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMAC : IDisposable
    {
        protected HMac _hmac;
        
        public byte[] ComputeHash(byte[] value)
        {
            return ComputeHash(value, 0, value.Length);
        }

        public byte[] ComputeHash(byte[] value, int offset, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            byte[] resBuf = new byte[_hmac.GetMacSize()];
            _hmac.BlockUpdate(value, 0, length);
            _hmac.DoFinal(resBuf, 0);

            return resBuf;
        }

        public void Dispose()
        {
            _hmac.Reset();
        }
    }
}
