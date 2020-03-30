using System.Collections.Generic;
using System.Linq;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Parameters.Queries.GetCompressions
{
    public class GetCompressionsQuery : IRequest<IEnumerable<string>>
    {
        public class GetCompressionsQueryHandler : IRequestHandler<GetCompressionsQuery, IEnumerable<string>>
        {
            private readonly ICryptographyClient _cryptography;

            public GetCompressionsQueryHandler(ICryptographyClient cryptography)
            {
                _cryptography = cryptography;
            }

            public IEnumerable<string> Handle(GetCompressionsQuery message)
            {
                return _cryptography.CompressionAlgorithms.OrderBy(c => c);
            }
        }
    }
}