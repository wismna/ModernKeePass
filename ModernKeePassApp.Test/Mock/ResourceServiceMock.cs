using ModernKeePass.Interfaces;

namespace ModernKeePassApp.Test.Mock
{
    class ResourceServiceMock : IResourceService
    {
        public string GetResourceValue(string key)
        {
            return string.Empty;
        }
    }
}
