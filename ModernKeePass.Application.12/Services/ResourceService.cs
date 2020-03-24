using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class ResourceService: IResourceService
    {
        private readonly IResourceProxy _resourceProxy;

        public ResourceService(IResourceProxy resourceProxy)
        {
            _resourceProxy = resourceProxy;
        }

        public string GetResourceValue(string key)
        {
            return _resourceProxy.GetResourceValue(key);
        }
    }
}