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
            private readonly IDatabaseSettingsProxy _databaseSettings;

            public GetCompressionsQueryHandler(IDatabaseSettingsProxy databaseSettings)
            {
                _databaseSettings = databaseSettings;
            }

            public IEnumerable<string> Handle(GetCompressionsQuery message)
            {
                return _databaseSettings.CompressionAlgorithms.OrderBy(c => c);
            }
        }
    }
}