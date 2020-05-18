using System.Threading.Tasks;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Security.Commands.GenerateKeyFile
{
    public class GenerateKeyFileCommand : IRequest
    {
        public string KeyFilePath { get; set; }
        public bool AddAdditionalEntropy { get; set; }

        public class GenerateKeyFileCommandHandler : IAsyncRequestHandler<GenerateKeyFileCommand>
        {
            private readonly ICredentialsProxy _security;
            private readonly IFileProxy _file;
            private readonly ICryptographyClient _cryptography;

            public GenerateKeyFileCommandHandler(ICredentialsProxy security, IFileProxy file, ICryptographyClient cryptography)
            {
                _security = security;
                _file = file;
                _cryptography = cryptography;
            }

            public async Task Handle(GenerateKeyFileCommand message)
            {
                byte[] entropy = null;
                if (message.AddAdditionalEntropy)
                {
                    entropy = _cryptography.Random(10);
                }
                var keyFile = _security.GenerateKeyFile(entropy);
                await _file.WriteBinaryContentsToFile(message.KeyFilePath, keyFile);
            }
        }
    }
}