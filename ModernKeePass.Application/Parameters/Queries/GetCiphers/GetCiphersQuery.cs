using System.Collections.Generic;
using System.Linq;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Parameters.Models;

namespace ModernKeePass.Application.Parameters.Queries.GetCiphers
{
    public class GetCiphersQuery: IRequest<IEnumerable<CipherVm>>
    {
        public class GetCiphersQueryHandler: IRequestHandler<GetCiphersQuery, IEnumerable<CipherVm>>
        {
            private readonly IDatabaseSettingsProxy _databaseSettings;

            public GetCiphersQueryHandler(IDatabaseSettingsProxy databaseSettings)
            {
                _databaseSettings = databaseSettings;
            }

            public IEnumerable<CipherVm> Handle(GetCiphersQuery message)
            {
                return _databaseSettings.Ciphers.Select(c => new CipherVm
                {
                    Id = c.Id,
                    Name = c.Name
                });
            }
        }
    }
}