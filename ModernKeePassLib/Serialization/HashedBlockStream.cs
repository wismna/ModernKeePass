/*
  KeePass Password Safe - The Open-Source Password Manager
  Copyright (C) 2003-2014 Dominik Reichl <dominik.reichl@t-online.de>

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
using System.IO;
#if ModernKeePassLibPCL
using System.Linq;
using Windows.Security.Cryptography;
#else
using System.Security.Cryptography;
#endif
using System.Diagnostics;
using System.Text;

using ModernKeePassLibPCL.Native;
using ModernKeePassLibPCL.Utility;
using Windows.Security.Cryptography.Core;

#if KeePassLibSD
using KeePassLibSD;
#endif

namespace ModernKeePassLibPCL.Serialization
{
	public sealed class HashedBlockStream : Stream
	{
		private const int m_nDefaultBufferSize = 1024 * 1024; // 1 MB

		private Stream m_sBaseStream;
		private bool m_bWriting;
		private bool m_bVerify;
		private bool m_bEos = false;

		private BinaryReader m_brInput;
		private BinaryWriter m_bwOutput;

		private byte[] m_pbBuffer;
		private int m_nBufferPos = 0;

		private uint m_uBufferIndex = 0;

		public override bool CanRead
		{
			get { return !m_bWriting; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return m_bWriting; }
		}

		public override long Length
		{
			get { throw new NotSupportedException(); }
		}

		public override long Position
		{
			get { throw new NotSupportedException(); }
			set { throw new NotSupportedException(); }
		}

		public HashedBlockStream(Stream sBaseStream, bool bWriting)
		{
			Initialize(sBaseStream, bWriting, 0, true);
		}

		public HashedBlockStream(Stream sBaseStream, bool bWriting, int nBufferSize)
		{
			Initialize(sBaseStream, bWriting, nBufferSize, true);
		}

		public HashedBlockStream(Stream sBaseStream, bool bWriting, int nBufferSize,
			bool bVerify)
		{
			Initialize(sBaseStream, bWriting, nBufferSize, bVerify);
		}

		private void Initialize(Stream sBaseStream, bool bWriting, int nBufferSize,
			bool bVerify)
		{
		    if (sBaseStream != null) m_sBaseStream = sBaseStream;
            else throw new ArgumentNullException(nameof(sBaseStream));
            if (nBufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(nBufferSize));

			if(nBufferSize == 0)
                nBufferSize = m_nDefaultBufferSize;
			m_bWriting = bWriting;
			m_bVerify = bVerify;

			UTF8Encoding utf8 = StrUtil.Utf8;
			if(m_bWriting == false) // Reading mode
			{
				if(m_sBaseStream.CanRead == false)
					throw new InvalidOperationException();

				m_brInput = new BinaryReader(sBaseStream, utf8);

				m_pbBuffer = new byte[0];
			}
			else // Writing mode
			{
				if(m_sBaseStream.CanWrite == false)
					throw new InvalidOperationException();

				m_bwOutput = new BinaryWriter(sBaseStream, utf8);

				m_pbBuffer = new byte[nBufferSize];
			}
		}

		public override void Flush()
		{
			if(m_bWriting) m_bwOutput.Flush();
		}

#if ModernKeePassLibPCL || KeePassRT
		protected override void Dispose(bool disposing)
		{
			if(!disposing) return;
#else
		public override void Close()
		{
#endif
			if(m_sBaseStream != null)
			{
				if(m_bWriting == false) // Reading mode
				{
                    try { m_brInput.Dispose(); } catch { }

					m_brInput = null;
				}
				else // Writing mode
				{
					if(m_nBufferPos == 0) // No data left in buffer
						WriteHashedBlock(); // Write terminating block
					else
					{
						WriteHashedBlock(); // Write remaining buffered data
						WriteHashedBlock(); // Write terminating block
					}

					Flush();
					m_bwOutput.Dispose();
					m_bwOutput = null;
				}

                try { m_sBaseStream.Dispose(); } catch { }
				m_sBaseStream = null;
			}
		}

		public override long Seek(long lOffset, SeekOrigin soOrigin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long lValue)
		{
			throw new NotSupportedException();
		}

		public override int Read(byte[] pbBuffer, int nOffset, int nCount)
		{
			if(m_bWriting) throw new InvalidOperationException();

			int nRemaining = nCount;
			while(nRemaining > 0)
			{
				if(m_nBufferPos == m_pbBuffer.Length)
				{
					if(ReadHashedBlock() == false)
						return nCount - nRemaining; // Bytes actually read
				}

				int nCopy = Math.Min(m_pbBuffer.Length - m_nBufferPos, nRemaining);

				Array.Copy(m_pbBuffer, m_nBufferPos, pbBuffer, nOffset, nCopy);

				nOffset += nCopy;
				m_nBufferPos += nCopy;

				nRemaining -= nCopy;
			}

			return nCount;
		}

		private bool ReadHashedBlock()
		{
			if(m_bEos) return false; // End of stream reached already

			m_nBufferPos = 0;

			if(m_brInput.ReadUInt32() != m_uBufferIndex)
				throw new InvalidDataException();
			++m_uBufferIndex;

			byte[] pbStoredHash = m_brInput.ReadBytes(32);
			if((pbStoredHash == null) || (pbStoredHash.Length != 32))
				throw new InvalidDataException();

			int nBufferSize = 0;
			try { nBufferSize = m_brInput.ReadInt32(); }
			catch(NullReferenceException) // Mono bug workaround (LaunchPad 783268)
			{
				if(!NativeLib.IsUnix()) throw;
			}

			if(nBufferSize < 0)
				throw new InvalidDataException();

			if(nBufferSize == 0)
			{
				for(int iHash = 0; iHash < 32; ++iHash)
				{
					if(pbStoredHash[iHash] != 0)
						throw new InvalidDataException();
				}

				m_bEos = true;
				m_pbBuffer = new byte[0];
				return false;
			}

			m_pbBuffer = m_brInput.ReadBytes(nBufferSize);
			if((m_pbBuffer == null) || ((m_pbBuffer.Length != nBufferSize) && m_bVerify))
				throw new InvalidDataException();

			if(m_bVerify)
			{
#if ModernKeePassLibPCL
                /*var sha256 = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
				var pbComputedHash = sha256.HashData(m_pbBuffer);*/
                var sha256 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
                var buffer = sha256.HashData(CryptographicBuffer.CreateFromByteArray(m_pbBuffer));
                byte[] pbComputedHash;
                CryptographicBuffer.CopyToByteArray(buffer, out pbComputedHash);
#else
				SHA256Managed sha256 = new SHA256Managed();
				byte[] pbComputedHash = sha256.ComputeHash(m_pbBuffer);
#endif
                if ((pbComputedHash == null) || (pbComputedHash.Length != 32))
					throw new InvalidOperationException();

				for(int iHashPos = 0; iHashPos < 32; ++iHashPos)
				{
					if(pbStoredHash[iHashPos] != pbComputedHash[iHashPos])
						throw new InvalidDataException();
				}
			}

			return true;
		}

		public override void Write(byte[] pbBuffer, int nOffset, int nCount)
		{
			if(!m_bWriting) throw new InvalidOperationException();

			while(nCount > 0)
			{
				if(m_nBufferPos == m_pbBuffer.Length)
					WriteHashedBlock();

				int nCopy = Math.Min(m_pbBuffer.Length - m_nBufferPos, nCount);

				Array.Copy(pbBuffer, nOffset, m_pbBuffer, m_nBufferPos, nCopy);

				nOffset += nCopy;
				m_nBufferPos += nCopy;

				nCount -= nCopy;
			}
		}

		private void WriteHashedBlock()
		{
			m_bwOutput.Write(m_uBufferIndex);
			++m_uBufferIndex;

			if(m_nBufferPos > 0)
			{
#if ModernKeePassLibPCL
                /*var sha256 = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
				var pbHash = sha256.HashData(m_pbBuffer.Where((x, i) => i < m_nBufferPos).ToArray());*/
                var sha256 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
                var buffer = sha256.HashData(CryptographicBuffer.CreateFromByteArray(m_pbBuffer.Where((x, i) => i < m_nBufferPos).ToArray()));
                byte[] pbHash;
                CryptographicBuffer.CopyToByteArray(buffer, out pbHash);
#else

				SHA256Managed sha256 = new SHA256Managed();

#if !KeePassLibSD
				byte[] pbHash = sha256.ComputeHash(m_pbBuffer, 0, m_nBufferPos);
#else
				byte[] pbHash;
				if(m_nBufferPos == m_pbBuffer.Length)
					pbHash = sha256.ComputeHash(m_pbBuffer);
				else
				{
					byte[] pbData = new byte[m_nBufferPos];
					Array.Copy(m_pbBuffer, 0, pbData, 0, m_nBufferPos);
					pbHash = sha256.ComputeHash(pbData);
				}
#endif

#endif

                m_bwOutput.Write(pbHash);
			}
			else
			{
				m_bwOutput.Write((ulong)0); // Zero hash
				m_bwOutput.Write((ulong)0);
				m_bwOutput.Write((ulong)0);
				m_bwOutput.Write((ulong)0);
			}

			m_bwOutput.Write(m_nBufferPos);
			
			if(m_nBufferPos > 0)
				m_bwOutput.Write(m_pbBuffer, 0, m_nBufferPos);

			m_nBufferPos = 0;
		}
	}
}
