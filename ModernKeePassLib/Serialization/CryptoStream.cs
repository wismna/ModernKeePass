using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace ModernKeePassLib.Serialization
{
    // An adaptor function to provide a stream interface from an IBuffer.

    class CryptoStream : Stream
    {

        private int m_blockSize = 16 ;
        private byte[] m_decoded;
        private IEnumerator<byte> m_enumerator = null;

        public CryptoStream(Stream s, String strAlgName, bool bEncrypt, byte[] pbKey, byte[] pbIV)
            : base()
        {
            IBuffer iv = CryptographicBuffer.CreateFromByteArray(pbIV);
            SymmetricKeyAlgorithmProvider objAlg = SymmetricKeyAlgorithmProvider.OpenAlgorithm(strAlgName);
            CryptographicKey key = objAlg.CreateSymmetricKey( CryptographicBuffer.CreateFromByteArray(pbKey) );
            if (bEncrypt)
            {
                Debug.Assert(false, "Not implemented yet");
            } 
            else
            {
               // For the time being, WinRT CryptographicEngine doesn't support stream decoding. Bummer.
               // Copy the file to a memory buffer, then decode all at once.

             
                byte[] block = new byte[s.Length]; // We are not at the beginning of the stream
                                                   // There is always less than s.Lenght bytes remaining to be read.
                int readItemCount = s.Read(block, 0, (int) s.Length);
                Array.Resize(ref block, readItemCount); 

                IBuffer input = CryptographicBuffer.CreateFromByteArray(block);
                IBuffer decoded = null;

                try
                {
                    decoded = CryptographicEngine.Decrypt(key, input, iv);
                } catch (System.Exception)
                {
                    throw new Keys.InvalidCompositeKeyException();
                }
                CryptographicBuffer.CopyToByteArray(decoded, out m_decoded);
                m_enumerator = m_decoded.AsEnumerable().GetEnumerator();
            }
        }


        public override bool CanRead     { get { return true; } }
        public override bool CanSeek     { get { return false; } }
        public override bool CanTimeout  { get { return false; } }
        public override bool CanWrite    { get { return false; } }
        public override long Length
        {
            get
            {
                throw new System.NotSupportedException();
            }
        }
      
        public override long Position {
            get
            {
                throw new System.NotSupportedException();
            }
            set
            {
                throw new System.NotSupportedException();
            }
        }


        public override void Flush()
        {
            Debug.Assert(false, "Not yet implemented");
        }

       
        public override int Read(byte[] buffer, int offset, int count)
        {
            // Exceptions:
            //   System.ArgumentException:
            //     The sum of offset and count is larger than the buffer length.
            //
            //   System.ArgumentNullException:
            //     buffer is null.
            //
            //   System.ArgumentOutOfRangeException:
            //     offset or count is negative.
            //
            //   System.IO.IOException:
            //     An I/O error occurs.
            //
            //   System.NotSupportedException:
            //     The stream does not support reading.
            //
            //   System.ObjectDisposedException:
            //     Methods were called after the stream was closed.

            if ((count <0) || (offset <0))
                throw new System.ArgumentOutOfRangeException();
            if (buffer == null)
                throw new System.ArgumentNullException();
            if (m_enumerator == null)
                    throw new System.ArgumentNullException();

            for (int i = 0; i < count; i++)
            {
                if (!m_enumerator.MoveNext())
                    return i;
                buffer[i + offset] = m_enumerator.Current;
            }
            return count;
        }
   
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotSupportedException();
        }

    }
}
