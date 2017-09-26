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
		public static bool SupportsStrCmpNaturally {
			get { throw new NotImplementedException(); }
		}

		public static int StrCmpNaturally (string s1, string s2)
		{
			throw new NotImplementedException();
		}
	}

	internal enum DataProtectionScope
	{
		CurrentUser,
		LocalMachine
	}

	internal static class ProtectedData
	{
		public static byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
		{
			throw new NotImplementedException();
		}

		public static byte[] Unprotect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
		{
			throw new NotImplementedException();
		}
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

