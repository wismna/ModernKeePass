using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;
using ModernKeePassLib.Native;

namespace ModernKeePassLib.Cryptography
{
    public static class ProtectedData
    {
        public static byte[] Protect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
        {
            var provider =
                new DataProtectionProvider(scope == DataProtectionScope.CurrentUser ? "LOCAL=user" : "LOCAL=machine");
            // Encode the plaintext input message to a buffer.
            var buffMsg = userData.AsBuffer();

            // Encrypt the message.
            IBuffer buffProtected;
            try
            {
                buffProtected = provider.ProtectAsync(buffMsg).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw;
            }
            
            return buffProtected.ToArray();
        }


        public static byte[] Unprotect(byte[] userData, byte[] optionalEntropy, DataProtectionScope scope)
        {
            var provider =
                new DataProtectionProvider(scope == DataProtectionScope.CurrentUser ? "LOCAL=user" : "LOCAL=machine");
            // Decode the encrypted input message to a buffer.
            var buffMsg = userData.AsBuffer();

            // Decrypt the message.
            IBuffer buffUnprotected;
            try
            {
                buffUnprotected = provider.UnprotectAsync(buffMsg).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                throw;
            }

            return buffUnprotected.ToArray();
        }
    }
}
