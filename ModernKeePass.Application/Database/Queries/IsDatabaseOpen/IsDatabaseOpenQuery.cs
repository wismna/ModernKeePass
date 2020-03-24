using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Database.Queries.IsDatabaseOpen
{
    public class IsDatabaseOpenQuery: IRequest<bool>
    {
        public class IsDatabaseOpenQueryHandler: IRequestHandler<IsDatabaseOpenQuery, bool>
        {
            private readonly IDatabaseProxy _databaseProxy;

            public IsDatabaseOpenQueryHandler(IDatabaseProxy databaseProxy)
            {
                _databaseProxy = databaseProxy;
            }

            public bool Handle(IsDatabaseOpenQuery message)
            {
                return _databaseProxy.IsOpen;
            }

        }
    }
}