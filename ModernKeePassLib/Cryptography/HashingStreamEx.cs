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

using ModernKeePassLib.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace ModernKeePassLib.Cryptography
{
    public sealed class HashingStreamEx : Stream
    {
        private Stream m_sBaseStream;
        private bool m_bWriting;
        private Queue<byte[]> m_DataToHash;
        //private HashAlgorithm m_hash;

        public byte[] Hash
        {
            get
            {
                int len = 0;
                foreach (byte[] block in m_DataToHash)
                {
                    len += block.Length;
                }
                byte[] dataToHash = new byte[len];
                int pos = 0;
                while(m_DataToHash.Count > 0)
                {
                    byte[] block = m_DataToHash.Dequeue();
                    Array.Copy(block, 0, dataToHash, pos, block.Length);
                    pos += block.Length;
                }

                byte[] hash = SHA256Managed.Instance.ComputeHash(dataToHash);
                return hash;
            }
        }

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
            get { return m_sBaseStream.Length; }
        }

        public override long Position
        {
            get { return m_sBaseStream.Position; }
            set { throw new NotSupportedException(); }
        }

        public HashingStreamEx(Stream sBaseStream, bool bWriting, CryptographicHash hashAlgorithm)
        {
            if (sBaseStream == null) throw new ArgumentNullException("sBaseStream");

            m_sBaseStream = sBaseStream;
            m_bWriting = bWriting;

#if KeePassWinRT

            m_DataToHash = new Queue<byte[]>();


#else
#if !KeePassLibSD
			m_hash = (hashAlgorithm ?? new SHA256Managed());
#else // KeePassLibSD
			m_hash = null;

			try { m_hash = HashAlgorithm.Create("SHA256"); }
			catch(Exception) { }
			try { if(m_hash == null) m_hash = HashAlgorithm.Create(); }
			catch(Exception) { }
#endif
#endif // KeePassWinRT


#if TODO 
            // Bert TODO: For the time being, only built-in Hash algorithm are supported.
            if (m_hash == null) { Debug.Assert(false); return; }
			// Validate hash algorithm
			if((!m_hash.CanReuseTransform) || (!m_hash.CanTransformMultipleBlocks) ||
				(m_hash.InputBlockSize != 1) || (m_hash.OutputBlockSize != 1))
			{
#if DEBUG
				MessageService.ShowWarning("Broken HashAlgorithm object in HashingStreamEx.");
#endif
				m_hash = null;
			}
#endif
        }

        public override void Flush()
        {
            m_sBaseStream.Flush();
        }
        
		/*public override void Close()
		{

			if(m_hash != null)
			{
				try
				{
					m_hash.TransformFinalBlock(new byte[0], 0, 0);

					m_pbFinalHash = m_hash.Hash;
				}
				catch(Exception) { Debug.Assert(false); }

				m_hash = null;
			}

			m_sBaseStream.Close();

		}*/

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

            if (m_bWriting) throw new InvalidOperationException();

            int nRead = m_sBaseStream.Read(pbBuffer, nOffset, nCount);

            // Mono bug workaround (LaunchPad 798910)
            int nPartialRead = nRead;
            while ((nRead < nCount) && (nPartialRead != 0))
            {
                nPartialRead = m_sBaseStream.Read(pbBuffer, nOffset + nRead,
                    nCount - nRead);
                nRead += nPartialRead;
            }

            byte[] pbOrg = new byte[nRead];
            Array.Copy(pbBuffer, pbOrg, nRead);
            m_DataToHash.Enqueue(pbOrg);
			
            return nRead;
        }

        public override void Write(byte[] pbBuffer, int nOffset, int nCount)
        {
			if(!m_bWriting) throw new InvalidOperationException();

#if DEBUG
			byte[] pbOrg = new byte[pbBuffer.Length];
			Array.Copy(pbBuffer, pbOrg, pbBuffer.Length);
#endif
            // TODO: implement this
			/*if((m_hash != null) && (nCount > 0))
				m_hash.TransformBlock(pbBuffer, nOffset, nCount, pbBuffer, nOffset);*/

#if DEBUG
			Debug.Assert(MemUtil.ArraysEqual(pbBuffer, pbOrg));
#endif

			m_sBaseStream.Write(pbBuffer, nOffset, nCount);
            
        }
    }
}
