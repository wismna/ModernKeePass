using System.Threading.Tasks;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class SecurityService: ISecurityService
    {
        private readonly IPasswordProxy _passwordProxy;
        private readonly IFileService _fileService;

        public SecurityService(IPasswordProxy passwordProxy, IFileService fileService)
        {
            _passwordProxy = passwordProxy;
            _fileService = fileService;
        }

        public string GeneratePassword(PasswordGenerationOptions options)
        {
            return _passwordProxy.GeneratePassword(options);
        }

        public uint EstimatePasswordComplexity(string password)
        {
            return _passwordProxy.EstimatePasswordComplexity(password);
        }

        public async Task GenerateKeyFile(string filePath)
        {
            var fileContents = _passwordProxy.GenerateKeyFile(null);
            await _fileService.WriteBinaryContentsToFile(filePath, fileContents);
        }
    }
}