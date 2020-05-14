using System;
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
            // Create a DataProtectionProvider object for the specified descriptor.
            var provider = new DataProtectionProvider();

            // Encode the plaintext input message to a buffer.
            var buffMsg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

            // Encrypt the message.
            var buffProtected = await provider.ProtectAsync(buffMsg);
            
            // Encode buffer to Base64
            var stringProtected = CryptographicBuffer.EncodeToBase64String(buffProtected);

            // Return the encrypted string.
            return stringProtected;
        }

        public async Task<string> UnProtect(string value)
        {
            // Create a DataProtectionProvider object.
            var provider = new DataProtectionProvider();

            // Decode from Base64 string
            var buffProtected = CryptographicBuffer.DecodeFromBase64String(value);

            // Decrypt the protected message specified on input.
            var buffUnprotected = await provider.UnprotectAsync(buffProtected);
            
            // Convert the unprotected message from an IBuffer object to a string.
            var strClearText = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffUnprotected);

            // Return the plaintext string.
            return strClearText;
        }
    }
}