using System;
using Org.BouncyCastle.Crypto;

namespace ModernKeePassLib.Cryptography.Hash
{
    public abstract class DigestManaged: IDisposable
    {
        protected IDigest Digest;

        public byte[] Hash
        {
            get
            {
                var result = new byte[Digest.GetDigestSize()];
                Digest.DoFinal(result, 0);
                return result;
            }
        }

        public byte[] ComputeHash(byte[] value)
        {
            return ComputeHash(value, 0, value.Length);
        }

        public byte[] ComputeHash(byte[] value, int offset, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            byte[] resBuf = new byte[Digest.GetDigestSize()];
            Digest.BlockUpdate(value, 0, length);
            Digest.DoFinal(resBuf, 0);

            return resBuf;
        }


        public void TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            Digest.BlockUpdate(inputBuffer, inputOffset, inputCount);
            if ((outputBuffer != null) && ((inputBuffer != outputBuffer) || (inputOffset != outputOffset)))
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
        }

        public void TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            Digest.BlockUpdate(inputBuffer, inputOffset, inputCount);
        }

        public void Dispose()
        {
            Digest.Reset();
        }
    }
}
