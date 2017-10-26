using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace ModernKeePassLib.Cryptography.Hash
{
    public class HMACSHA256: HMAC
    {
        public HMACSHA256(byte[] key)
        {
            _hmac = new HMac(new Sha256Digest());
            _hmac.Init(new KeyParameter(key));
        }
        
        /*internal void TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            _hmac.BlockUpdate(inputBuffer, inputOffset, inputCount);
        }

        internal void TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            _hmac.DoFinal(inputBuffer, inputOffset);
        }

        internal void Initialize()
        {
            _hmac.Reset();
        }*/
    }
}
