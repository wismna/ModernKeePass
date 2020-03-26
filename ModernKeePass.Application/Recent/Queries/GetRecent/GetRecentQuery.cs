using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Recent.Models;

namespace ModernKeePass.Application.Recent.Queries.GetRecent
{
    public class GetRecentQuery : IRequest<RecentVm>
    {
        public string Token { get; set; }

        public class GetRecentQueryHandler : IAsyncRequestHandler<GetRecentQuery, RecentVm>
        {
            private readonly IRecentProxy _recent;
            private readonly IMapper _mapper;

            public GetRecentQueryHandler(IRecentProxy recent, IMapper mapper)
            {
                _recent = recent;
                _mapper = mapper;
            }

            public async Task<RecentVm> Handle(GetRecentQuery message)
            {
                var fileInfo = await _recent.Get(message.Token);
                return _mapper.Map<RecentVm>(fileInfo);
            }
        }
    }
}