using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Security.Queries.EstimatePasswordComplexity
{
    public class EstimatePasswordComplexityQuery : IRequest<uint>
    {
        public string Password { get; set; }

        public class EstimatePasswordComplexityQueryHandler : IRequestHandler<EstimatePasswordComplexityQuery, uint>
        {
            private readonly IPasswordProxy _security;

            public EstimatePasswordComplexityQueryHandler(IPasswordProxy security)
            {
                _security = security;
            }

            public uint Handle(EstimatePasswordComplexityQuery message)
            {
                return _security.EstimatePasswordComplexity(message.Password);
            }
        }
    }
}