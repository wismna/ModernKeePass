using System.Collections.Generic;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Cryptography.Models;

namespace ModernKeePass.Application.Cryptography.Queries.GetCiphers
{
    public class GetCiphersQuery: IRequest<IEnumerable<CipherVm>>
    {
        public class GetCiphersQueryHandler: IRequestHandler<GetCiphersQuery, IEnumerable<CipherVm>>
        {
            private readonly ICryptographyClient _cryptography;
            private readonly IMapper _mapper;

            public GetCiphersQueryHandler(ICryptographyClient cryptography, IMapper mapper)
            {
                _cryptography = cryptography;
                _mapper = mapper;
            }

            public IEnumerable<CipherVm> Handle(GetCiphersQuery message)
            {
                yield return _mapper.Map<CipherVm>(_cryptography.Ciphers);
            }
        }
    }
}