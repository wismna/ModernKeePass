using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography.Core;
using Validation;

namespace ModernKeePassLib.Cryptography.Hash
{
    public abstract class HashAlgorithm: IDisposable
    {
        /// <summary>
        /// The platform-specific hash object.
        /// </summary>
        private readonly CryptographicHash _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashAlgorithm"/> class.
        /// </summary>
        /// <param name="hash">The platform hash.</param>
        internal HashAlgorithm(CryptographicHash hash)
        {
            Requires.NotNull(hash, "Hash");
            _hash = hash;
        }
        
        public bool CanReuseTransform => true;
        public bool CanTransformMultipleBlocks => true;

        public byte[] Hash => _hash.GetValueAndReset().ToArray();

        public  void Append(byte[] data)
        {
            _hash.Append(data.AsBuffer());
        }
        
        public  byte[] GetValueAndReset()
        {
            return _hash.GetValueAndReset().ToArray();
        }

        #region ICryptoTransform methods
        
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] buffer;
            if (inputCount < inputBuffer.Length)
            {
                buffer = new byte[inputCount];
                Array.Copy(inputBuffer, inputOffset, buffer, 0, inputCount);
            }
            else
            {
                buffer = inputBuffer;
            }

            Append(buffer);
            if (outputBuffer != null)
            {
                Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            }

            return inputCount;
        }
        
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            this.TransformBlock(inputBuffer, inputOffset, inputCount, null, 0);
            if (inputCount == inputBuffer.Length)
            {
                return inputBuffer;
            }
            var buffer = new byte[inputCount];
            Array.Copy(inputBuffer, inputOffset, buffer, 0, inputCount);
            return buffer;
        }

        public byte[] ComputeHash(byte[] value)
        {
            return ComputeHash(value, 0, value.Length);
        }

        public byte[] ComputeHash(byte[] value, int offset, int length)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            TransformFinalBlock(value, offset, length);
            var resBuf = GetValueAndReset();

            return resBuf;
        }
        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        #endregion
    }
}
