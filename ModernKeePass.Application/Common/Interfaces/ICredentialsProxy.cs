using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface ICredentialsProxy
    {
        string GeneratePassword(PasswordGenerationOptions options);
        int EstimatePasswordComplexity(string password);
        byte[] GenerateKeyFile(byte[] additionalEntropy);
    }
}