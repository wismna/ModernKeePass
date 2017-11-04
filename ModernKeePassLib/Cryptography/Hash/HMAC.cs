using System;
using Org.BouncyCastle.Crypto.Macs;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMAC : IDisposable
    {
        protected HMac Hmac;

        public byte[] Hash
        {
            get
            {
                var result = new byte[Hmac.GetMacSize()];
                Hmac.DoFinal(result, 0);
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

            byte[] resBuf = new byte[Hmac.GetMacSize()];
            Hmac.BlockUpdate(value, 0, length);
            Hmac.DoFinal(resBuf, 0);

            return resBuf;
        }

        public void TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            Hmac.BlockUpdate(inputBuffer, inputOffset, inputCount);
            if ((outputBuffer != null) && ((inputBuffer != outputBuffer) || (inputOffset != outputOffset)))
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            Hmac.BlockUpdate(inputBuffer, inputOffset, inputCount);
            byte[] outputBytes = new byte[inputCount];
            if (inputCount != 0)
                Buffer.BlockCopy(inputBuffer, inputOffset, outputBytes, 0, inputCount);
            return outputBytes;
        }

        public void Initialize()
        {
            Hmac.Reset();
        }
        
        public void Dispose()
        {
            Hmac.Reset();
        }
    }
}
