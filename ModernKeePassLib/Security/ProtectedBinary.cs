/*
  KeePass Password Safe - The Open-Source Password Manager
  Copyright (C) 2003-2012 Dominik Reichl <dominik.reichl@t-online.de>

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Threading.Tasks;

using ModernKeePassLib.Utility;
using ModernKeePassLib.Cryptography;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;


#if KeePassLibSD
using ModernKeePassLibSD;
#endif

namespace ModernKeePassLib.Security
{
    /// <summary>
    /// Represents a protected binary, i.e. a byte array that is encrypted
    /// in memory. A <c>ProtectedBinary</c> object is immutable and
    /// thread-safe.
    /// </summary>
    public sealed class ProtectedBinary : IEquatable<ProtectedBinary>
    {
        private static bool m_bProtectionSupported;
        private IBuffer m_pbDataCrypted;
        private DataProtectionProvider m_pProvider;
        private bool m_bProtected;

        /// <summary>
        /// A flag specifying whether the <c>ProtectedBinary</c> object has
        /// turned on in-memory protection or not.
        /// </summary>
        public bool IsProtected
        {
            get
            {
                Debug.Assert(false, "not yet implemented");
                return false;
#if TODO
                return m_bProtected;
#endif
            }
        }

        /// <summary>
        /// Length of the stored data.
        /// </summary>
        public uint Length
        {
            get
            {

                Debug.Assert(false, "not yet implemented");
                return 0;
#if TODO
                return m_uDataLen;
#endif
            }
        }

        static ProtectedBinary()
        {
            m_bProtectionSupported = true;
        }

        /// <summary>
        /// Construct a new, empty protected binary data object. Protection
        /// is disabled.
        /// </summary>
        public ProtectedBinary()
        {
            Init(false, new byte[0]);
        }

        /// <summary>
        /// Construct a new protected binary data object.
        /// </summary>
        /// <param name="bEnableProtection">If this paremeter is <c>true</c>,
        /// the data will be encrypted in memory. If it is <c>false</c>, the
        /// data is stored in plain-text in the process memory.</param>
        /// <param name="pbData">Value of the protected object.
        /// The input parameter is not modified and
        /// <c>ProtectedBinary</c> doesn't take ownership of the data,
        /// i.e. the caller is responsible for clearing it.</param>
        public ProtectedBinary(bool bEnableProtection, byte[] pbData)
        {
            Init(bEnableProtection, pbData);
        }

        /// <summary>
        /// Construct a new protected binary data object. Copy the data from
        /// a <c>XorredBuffer</c> object.
        /// </summary>
        /// <param name="bEnableProtection">Enable protection or not.</param>
        /// <param name="xbProtected"><c>XorredBuffer</c> object used to
        /// initialize the <c>ProtectedBinary</c> object.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the input
        /// parameter is <c>null</c>.</exception>
        public ProtectedBinary(bool bEnableProtection, XorredBuffer xbProtected)
        {
            Debug.Assert(xbProtected != null); if (xbProtected == null) throw new ArgumentNullException("xbProtected");

            byte[] pb = xbProtected.ReadPlainText();
            Init(bEnableProtection, pb);
            MemUtil.ZeroByteArray(pb);
        }

        private void Init(bool bEnableProtection, byte[] pbData)
        {
            String strDescriptor = "LOCAL=user";
            m_pProvider = new DataProtectionProvider(strDescriptor);

            EncryptAsync(bEnableProtection, pbData);

        }

        private void EncryptAsync(bool bEnableProtection, byte[] pbData)
        {
            IBuffer dataUncrypted = CryptographicBuffer.CreateFromByteArray(pbData);

            Task<IBuffer> task = m_pProvider.ProtectAsync(dataUncrypted).AsTask<IBuffer>();
            task.Wait();

            m_pbDataCrypted = task.Result;
            // TODO: Here the dataUncrypted buffer should be cleared. Now just count on GarbageCollection to destroy.

        }

        /// <summary>
        /// Get a copy of the protected data as a byte array.
        /// Please note that the returned byte array is not protected and
        /// can therefore been read by any other application.
        /// Make sure that your clear it properly after usage.
        /// </summary>
        /// <returns>Unprotected byte array. This is always a copy of the internal
        /// protected data and can therefore be cleared safely.</returns>
        public byte[] ReadData()
        {
            if (m_pbDataCrypted == null) return new byte[0];

            // Decrypt the protected message specified on input.

            Task<IBuffer> task = m_pProvider.UnprotectAsync(m_pbDataCrypted).AsTask<IBuffer>();
            task.Wait();

            IBuffer buffUnprotected = task.Result;
            byte[] strClearText;
            CryptographicBuffer.CopyToByteArray(buffUnprotected, out strClearText);
            return strClearText;

        }

        /// <summary>
        /// Read the protected data and return it protected with a sequence
        /// of bytes generated by a random stream.
        /// </summary>
        /// <param name="crsRandomSource">Random number source.</param>
        /// <returns>Protected data.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the input
        /// parameter is <c>null</c>.</exception>
        public byte[] ReadXorredData(CryptoRandomStream crsRandomSource)
        {
            Debug.Assert(false, "not yet implemented");
            return null;
#if TODO
			Debug.Assert(crsRandomSource != null);
			if(crsRandomSource == null) throw new ArgumentNullException("crsRandomSource");

			byte[] pbData = ReadData();
			uint uLen = (uint)pbData.Length;

			byte[] randomPad = crsRandomSource.GetRandomBytes(uLen);
			Debug.Assert(randomPad.Length == uLen);

			for(uint i = 0; i < uLen; ++i)
				pbData[i] ^= randomPad[i];

			return pbData;
#endif
        }

        public override int GetHashCode()
        {
            Debug.Assert(false, "not yet implemented");
            return 0;
#if TODO
			int h = (m_bProtected ? 0x7B11D289 : 0);

			byte[] pb = ReadData();
			unchecked
			{
				for(int i = 0; i < pb.Length; ++i)
					h = (h << 3) + h + (int)pb[i];
			}
			MemUtil.ZeroByteArray(pb);

			return h;
#endif
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProtectedBinary);
        }

        public bool Equals(ProtectedBinary other)
        {
            Debug.Assert(false, "not yet implemented");
            return false;
#if TODO
			if(other == null) return false; // No assert

			if(m_bProtected != other.m_bProtected) return false;
			if(m_uDataLen != other.m_uDataLen) return false;

			byte[] pbL = ReadData();
			byte[] pbR = other.ReadData();
			bool bEq = MemUtil.ArraysEqual(pbL, pbR);
			MemUtil.ZeroByteArray(pbL);
			MemUtil.ZeroByteArray(pbR);

#if DEBUG
			if(bEq) { Debug.Assert(GetHashCode() == other.GetHashCode()); }
#endif

			return bEq;
#endif
        }
    }
}
