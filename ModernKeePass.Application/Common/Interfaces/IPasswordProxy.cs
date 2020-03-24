using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IPasswordProxy
    {
        string GeneratePassword(PasswordGenerationOptions options);
        uint EstimatePasswordComplexity(string password);
        byte[] GenerateKeyFile(byte[] additionalEntropy);
    }
}