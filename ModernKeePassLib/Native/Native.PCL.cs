using System;

using PlatformID = System.UInt32;

namespace ModernKeePassLib.Native
{
	internal static class NativeLib
	{
		public static ulong MonoVersion {
			get { throw new NotImplementedException(); }
		}

		public static bool IsUnix()
		{
			return true;
		}
	}

	internal static class NativeMethods
	{
		public static bool SupportsStrCmpNaturally => false;

	    internal const int GCRY_CIPHER_AES256 = 9;
	    internal const int GCRY_CIPHER_MODE_ECB = 1;

        public static int StrCmpNaturally (string s1, string s2)
		{
			throw new NotImplementedException();
		}

        internal static void gcry_check_version(IntPtr zero)
        {
            throw new NotImplementedException();
        }

	    public static void gcry_cipher_open(ref IntPtr intPtr, object gcryCipherAes256, object gcryCipherModeEcb, int i)
	    {
	        throw new NotImplementedException();
	    }

	    internal static int gcry_cipher_setkey(IntPtr h, IntPtr pSeed32, IntPtr n32)
        {
            throw new NotImplementedException();
        }

        internal static void gcry_cipher_close(IntPtr h)
        {
            throw new NotImplementedException();
        }

        internal static int gcry_cipher_encrypt(IntPtr h, IntPtr pData32, IntPtr n32, IntPtr zero1, IntPtr zero2)
        {
            throw new NotImplementedException();
        }

	    public static string GetUserRuntimeDir()
	    {
	        throw new NotImplementedException();
	    }
	}

	public enum DataProtectionScope
	{
		CurrentUser,
		LocalMachine
	}
    
	internal enum MemoryProtectionScope
	{
		CrossProcess,
		SameLogon,
		SameProcess
	}

	internal static class ProtectedMemory
	{
		public static byte[] Protect(byte[] userData, MemoryProtectionScope scope)
		{
			throw new NotImplementedException();
		}

		public static byte[] Unprotect(byte[] userData, MemoryProtectionScope scope)
		{
			throw new NotImplementedException();
		}
	}
}

