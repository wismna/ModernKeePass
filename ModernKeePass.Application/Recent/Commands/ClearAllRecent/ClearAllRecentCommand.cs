using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Recent.Commands.ClearAllRecent
{
    public class ClearAllRecentCommand : IRequest
    {
        public class ClearAllRecentCommandHandler : IRequestHandler<ClearAllRecentCommand>
        {
            private readonly IRecentProxy _recent;

            public ClearAllRecentCommandHandler(IRecentProxy recent)
            {
                _recent = recent;
            }

            public void Handle(ClearAllRecentCommand message)
            {
                _recent.ClearAll();
            }
        }
    }
}