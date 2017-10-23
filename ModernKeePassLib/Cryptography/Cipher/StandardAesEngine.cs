/*
  KeePass Password Safe - The Open-Source Password Manager
  Copyright (C) 2003-2017 Dominik Reichl <dominik.reichl@t-online.de>

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
using System.Diagnostics;
#if ModernKeePassLib
using ModernKeePassLib.Resources;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
#else
using System.Security.Cryptography;
#endif

namespace ModernKeePassLib.Cryptography.Cipher
{
	/// <summary>
	/// Standard AES cipher implementation.
	/// </summary>
	public sealed class StandardAesEngine : ICipherEngine
	{
#if !ModernKeePassLib && !KeePassUAP
		private const CipherMode m_rCipherMode = CipherMode.CBC;
		private const PaddingMode m_rCipherPadding = PaddingMode.PKCS7;
#endif

		private static PwUuid g_uuidAes = null;

		/// <summary>
		/// UUID of the cipher engine. This ID uniquely identifies the
		/// AES engine. Must not be used by other ciphers.
		/// </summary>
		public static PwUuid AesUuid
		{
			get
			{
				PwUuid pu = g_uuidAes;
				if(pu == null)
				{
					pu = new PwUuid(new byte[] {
						0x31, 0xC1, 0xF2, 0xE6, 0xBF, 0x71, 0x43, 0x50,
						0xBE, 0x58, 0x05, 0x21, 0x6A, 0xFC, 0x5A, 0xFF });
					g_uuidAes = pu;
				}

				return pu;
			}
		}

		/// <summary>
		/// Get the UUID of this cipher engine as <c>PwUuid</c> object.
		/// </summary>
		public PwUuid CipherUuid
		{
			get { return StandardAesEngine.AesUuid; }
		}

		/// <summary>
		/// Get a displayable name describing this cipher engine.
		/// </summary>
		public string DisplayName
		{
			get
			{
				return ("AES/Rijndael (" + KLRes.KeyBits.Replace(@"{PARAM}",
					"256") + ", FIPS 197)");
			}
		}

		private static void ValidateArguments(Stream stream, bool bEncrypt, byte[] pbKey, byte[] pbIV)
		{
			Debug.Assert(stream != null); if(stream == null) throw new ArgumentNullException("stream");

			Debug.Assert(pbKey != null); if(pbKey == null) throw new ArgumentNullException("pbKey");
			Debug.Assert(pbKey.Length == 32);
			if(pbKey.Length != 32) throw new ArgumentException("Key must be 256 bits wide!");

			Debug.Assert(pbIV != null); if(pbIV == null) throw new ArgumentNullException("pbIV");
			Debug.Assert(pbIV.Length == 16);
			if(pbIV.Length != 16) throw new ArgumentException("Initialization vector must be 128 bits wide!");

			if(bEncrypt)
			{
				Debug.Assert(stream.CanWrite);
				if(!stream.CanWrite) throw new ArgumentException("Stream must be writable!");
			}
			else // Decrypt
			{
				Debug.Assert(stream.CanRead);
				if(!stream.CanRead) throw new ArgumentException("Encrypted stream must be readable!");
			}
		}

		private static Stream CreateStream(Stream s, bool bEncrypt, byte[] pbKey, byte[] pbIV)
		{
			StandardAesEngine.ValidateArguments(s, bEncrypt, pbKey, pbIV);

			byte[] pbLocalIV = new byte[16];
			Array.Copy(pbIV, pbLocalIV, 16);

			byte[] pbLocalKey = new byte[32];
			Array.Copy(pbKey, pbLocalKey, 32);

#if ModernKeePassLib
		    AesEngine aes = new AesEngine();
		    CbcBlockCipher cbc = new CbcBlockCipher(aes);
		    PaddedBufferedBlockCipher bc = new PaddedBufferedBlockCipher(cbc,
		        new Pkcs7Padding());
		    KeyParameter kp = new KeyParameter(pbLocalKey);
		    ParametersWithIV prmIV = new ParametersWithIV(kp, pbLocalIV);
		    bc.Init(bEncrypt, prmIV);

		    IBufferedCipher cpRead = (bEncrypt ? null : bc);
		    IBufferedCipher cpWrite = (bEncrypt ? bc : null);
		    return new CipherStream(s, cpRead, cpWrite);
#elif KeePassUAP
			return StandardAesEngineExt.CreateStream(s, bEncrypt, pbLocalKey, pbLocalIV);
#else
			SymmetricAlgorithm a = CryptoUtil.CreateAes();
			if(a.BlockSize != 128) // AES block size
			{
				Debug.Assert(false);
				a.BlockSize = 128;
			}

			a.IV = pbLocalIV;
			a.KeySize = 256;
			a.Key = pbLocalKey;
			a.Mode = m_rCipherMode;
			a.Padding = m_rCipherPadding;

			ICryptoTransform iTransform = (bEncrypt ? a.CreateEncryptor() : a.CreateDecryptor());
			Debug.Assert(iTransform != null);
			if(iTransform == null) throw new SecurityException("Unable to create AES transform!");

			return new CryptoStream(s, iTransform, bEncrypt ? CryptoStreamMode.Write :
				CryptoStreamMode.Read);
#endif
		}

		public Stream EncryptStream(Stream sPlainText, byte[] pbKey, byte[] pbIV)
		{
			return StandardAesEngine.CreateStream(sPlainText, true, pbKey, pbIV);
		}

		public Stream DecryptStream(Stream sEncrypted, byte[] pbKey, byte[] pbIV)
		{
			return StandardAesEngine.CreateStream(sEncrypted, false, pbKey, pbIV);
		}
	}
}
