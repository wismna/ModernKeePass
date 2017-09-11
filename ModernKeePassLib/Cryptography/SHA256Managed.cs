using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace ModernKeePassLib.Cryptography
{
    // Singleton adaptor that provides a part of the .net SHA256Managed class

    class SHA256Managed
    {
        private static SHA256Managed instance;
        private static HashAlgorithmProvider m_AlgProv;

        private SHA256Managed() 
        {
            String strAlgName = "SHA256";
            m_AlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);
            m_AlgProv.CreateHash();
        }

        public static SHA256Managed Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SHA256Managed();
                }
                return instance;
            }
        }

        public byte[] ComputeHash(byte[] buffer )
        {
            IBuffer input = CryptographicBuffer.CreateFromByteArray( buffer);
            IBuffer hashBuffer = m_AlgProv.HashData(input);
            byte[] hash;
            CryptographicBuffer.CopyToByteArray(hashBuffer, out hash);
            return hash;
        }

    }
}
