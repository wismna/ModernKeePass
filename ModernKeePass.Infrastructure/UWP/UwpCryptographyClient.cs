using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpCryptographyClient: ICryptographyClient
    {
        public async Task<string> Protect(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            try
            {
                // Create a DataProtectionProvider object for the specified descriptor.
                var provider = new DataProtectionProvider("LOCAL=user");
                
                // Encode the plaintext input message to a buffer.
                var buffMsg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

                // Encrypt the message.
                var buffProtected = await provider.ProtectAsync(buffMsg).AsTask().ConfigureAwait(false);
                
                // Encode buffer to Base64
                var protectedValue = CryptographicBuffer.EncodeToBase64String(buffProtected);

                // Return the encrypted string.
                return protectedValue;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public async Task<string> UnProtect(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            // Create a DataProtectionProvider object.
            var provider = new DataProtectionProvider();

            // Decode from Base64 string
            var buffProtected = CryptographicBuffer.DecodeFromBase64String(value);

            // Decrypt the protected message specified on input.
            var buffUnprotected = await provider.UnprotectAsync(buffProtected).AsTask().ConfigureAwait(false);
            
            // Convert the unprotected message from an IBuffer object to a string.
            var clearText = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnprotected);

            // Return the plaintext string.
            return clearText;
        }

        public byte[] Random(uint length)
        {
            return CryptographicBuffer.GenerateRandom(length).ToArray();
        }
    }
}