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
            private readonly IDatabaseSettingsProxy _databaseSettings;

            public GetKeyDerivationsQueryHandler(IDatabaseSettingsProxy databaseSettings)
            {
                _databaseSettings = databaseSettings;
            }

            public IEnumerable<KeyDerivationVm> Handle(GetKeyDerivationsQuery message)
            {
                return _databaseSettings.KeyDerivations.Select(c => new KeyDerivationVm
                {
                    Id = c.Id,
                    Name = c.Name
                });
            }
        }
    }
}