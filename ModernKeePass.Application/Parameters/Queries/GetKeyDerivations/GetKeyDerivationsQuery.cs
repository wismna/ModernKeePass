using System.Collections.Generic;
using System.Linq;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Parameters.Models;

namespace ModernKeePass.Application.Parameters.Queries.GetKeyDerivations
{
    public class GetKeyDerivationsQuery : IRequest<IEnumerable<KeyDerivationVm>>
    {
        public class GetKeyDerivationsQueryHandler : IRequestHandler<GetKeyDerivationsQuery, IEnumerable<KeyDerivationVm>>
        {
            private readonly ICryptographyClient _cryptography;

            public GetKeyDerivationsQueryHandler(ICryptographyClient cryptography)
            {
                _cryptography = cryptography;
            }

            public IEnumerable<KeyDerivationVm> Handle(GetKeyDerivationsQuery message)
            {
                return _cryptography.KeyDerivations.Select(c => new KeyDerivationVm
                {
                    Id = c.Id,
                    Name = c.Name
                });
            }
        }
    }
}