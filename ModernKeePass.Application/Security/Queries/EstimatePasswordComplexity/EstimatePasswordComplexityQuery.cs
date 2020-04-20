using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity
{
    public class EstimatePasswordComplexityQuery : IRequest<int>
    {
        public string Password { get; set; }

        public class EstimatePasswordComplexityQueryHandler : IRequestHandler<EstimatePasswordComplexityQuery, int>
        {
            private readonly ICredentialsProxy _security;

            public EstimatePasswordComplexityQueryHandler(ICredentialsProxy security)
            {
                _security = security;
            }

            public int Handle(EstimatePasswordComplexityQuery message)
            {
                return _security.EstimatePasswordComplexity(message.Password);
            }
        }
    }
}