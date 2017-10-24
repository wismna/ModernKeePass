using System;
using Org.BouncyCastle.Crypto;

namespace ModernKeePassLib.Cryptography.Hash
{
    public abstract class DigestManaged: IDisposable
    {
        protected IDigest Hash;

        public byte[] ComputeHash(byte[] value)
        {
            return ComputeHash(value, 0, value.Length);
        }

        public byte[] ComputeHash(byte[] value, int offset, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            byte[] resBuf = new byte[Hash.GetDigestSize()];
            Hash.BlockUpdate(value, 0, length);
            Hash.DoFinal(resBuf, 0);

            return resBuf;
        }

        public void Dispose()
        {
            Hash.Reset();
        }
    }
}
