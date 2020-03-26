using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Recent.Queries.HasRecent
{
    public class HasRecentQuery : IRequest<bool>
    {
        public class HasRecentQueryHandler : IRequestHandler<HasRecentQuery, bool>
        {
            private readonly IRecentProxy _recent;

            public HasRecentQueryHandler(IRecentProxy recent)
            {
                _recent = recent;
            }

            public bool Handle(HasRecentQuery message)
            {
                return _recent.EntryCount > 0;
            }
        }
    }
}