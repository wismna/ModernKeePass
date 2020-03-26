using System;
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
            private readonly IPasswordProxy _security;
            private readonly IFileProxy _file;

            public GenerateKeyFileCommandHandler(IPasswordProxy security, IFileProxy file)
            {
                _security = security;
                _file = file;
            }

            public async Task Handle(GenerateKeyFileCommand message)
            {
                byte[] entropy = null;
                if (message.AddAdditionalEntropy)
                {
                    entropy = new byte[10];
                    var random = new Random();
                    random.NextBytes(entropy);
                }
                var keyFile = _security.GenerateKeyFile(entropy);
                await _file.WriteBinaryContentsToFile(message.KeyFilePath, keyFile);
            }
        }
    }
}