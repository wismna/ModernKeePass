using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;

namespace ModernKeePassLibPCL.Cryptography
{
    public static class CryptographicHashExtensions
    {
        public static int TransformBlock(this CryptographicHash hash, byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
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

            hash.Append(buffer.AsBuffer());
            if (outputBuffer != null)
            {
                Array.Copy(inputBuffer, inputOffset, outputBuffer, outputOffset, inputCount);
            }

            return inputCount;
        }
        
        public static byte[] TransformFinalBlock(this CryptographicHash hash, byte[] inputBuffer, int inputOffset, int inputCount)
        {
            hash.TransformBlock(inputBuffer, inputOffset, inputCount, null, 0);
            if (inputCount == inputBuffer.Length)
            {
                return inputBuffer;
            }
            else
            {
                var buffer = new byte[inputCount];
                Array.Copy(inputBuffer, inputOffset, buffer, 0, inputCount);
                return buffer;
            }
        }
    }
}
