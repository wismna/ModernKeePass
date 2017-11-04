using System;
using Org.BouncyCastle.Crypto;

namespace ModernKeePassLib.Cryptography.Hash
{
    public abstract class HashAlgorithm: IDisposable
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

        public bool CanReuseTransform => true;
        public bool CanTransformMultipleBlocks => true;

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

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            Digest.BlockUpdate(inputBuffer, inputOffset, inputCount);
            byte[] outputBytes = new byte[inputCount];
            if (inputCount != 0)
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBytes, 0, inputCount);
            return outputBytes;
        }

        public void Dispose()
        {
            Digest.Reset();
        }
    }
}
