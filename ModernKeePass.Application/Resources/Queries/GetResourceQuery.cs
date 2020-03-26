using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Resources.Queries
{
    public class GetResourceQuery: IRequest<string>
    {
        public string Key { get; set; }
        public class GetResourceQueryHandler: IRequestHandler<GetResourceQuery, string>
        {
            private readonly IResourceProxy _resourceProxy;

            public GetResourceQueryHandler(IResourceProxy resourceProxy)
            {
                _resourceProxy = resourceProxy;
            }
            public string Handle(GetResourceQuery message)
            {
                return _resourceProxy.GetResourceValue(message.Key);
            }
        }
    }
}