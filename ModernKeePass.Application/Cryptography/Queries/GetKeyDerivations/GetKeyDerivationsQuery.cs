using System.Collections.Generic;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Cryptography.Models;

namespace ModernKeePass.Application.Cryptography.Queries.GetKeyDerivations
{
    public class GetKeyDerivationsQuery : IRequest<IEnumerable<KeyDerivationVm>>
    {
        public class GetKeyDerivationsQueryHandler : IRequestHandler<GetKeyDerivationsQuery, IEnumerable<KeyDerivationVm>>
        {
            private readonly ICryptographyClient _cryptography;
            private readonly IMapper _mapper;

            public GetKeyDerivationsQueryHandler(ICryptographyClient cryptography, IMapper mapper)
            {
                _cryptography = cryptography;
                _mapper = mapper;
            }

            public IEnumerable<KeyDerivationVm> Handle(GetKeyDerivationsQuery message)
            {
                yield return _mapper.Map<KeyDerivationVm>(_cryptography.KeyDerivations);
            }
        }
    }
}