using System;
using ModernKeePassLib.Native;

namespace ModernKeePassLib.Cryptography
{
    public static class ProtectedData
    {
        public static byte[] Unprotect(byte[] pbEnc, byte[] mPbOptEnt, DataProtectionScope currentUser)
        {
            throw new NotImplementedException();
        }

        public static byte[] Protect(byte[] pbPlain, byte[] mPbOptEnt, DataProtectionScope currentUser)
        {
            throw new NotImplementedException();
        }
    }
}
