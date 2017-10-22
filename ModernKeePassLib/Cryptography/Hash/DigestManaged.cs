using System;
using Org.BouncyCastle.Crypto;

namespace ModernKeePassLib.Cryptography.Hash
{
    public abstract class DigestManaged : IDisposable
    {
        protected IDigest _hash;

        public byte[] ComputeHash(byte[] value, int offset, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            byte[] resBuf = new byte[_hash.GetDigestSize()];
            _hash.BlockUpdate(value, offset, length);
            _hash.DoFinal(resBuf, 0);

            return resBuf;
        }

        public void Dispose()
        {
            _hash.Reset();
        }
    }
}
