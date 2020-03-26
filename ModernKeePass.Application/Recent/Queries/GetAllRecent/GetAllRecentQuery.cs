using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Recent.Models;

namespace ModernKeePass.Application.Recent.Queries.GetAllRecent
{
    public class GetAllRecentQuery : IRequest<IEnumerable<RecentVm>>
    {
        public class GetAllRecentQueryHandler : IAsyncRequestHandler<GetAllRecentQuery, IEnumerable<RecentVm>>
        {
            private readonly IRecentProxy _recent;
            private readonly IMapper _mapper;

            public GetAllRecentQueryHandler(IRecentProxy recent, IMapper mapper)
            {
                _recent = recent;
                _mapper = mapper;
            }

            public async Task<IEnumerable<RecentVm>> Handle(GetAllRecentQuery message)
            {
                var fileInfo = await _recent.GetAll();
                return _mapper.Map<IEnumerable<RecentVm>>(fileInfo);
            }
        }
    }
}