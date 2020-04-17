using Windows.ApplicationModel.Resources;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpResourceClient: IResourceProxy
    {
        private const string ResourceFileName = "CodeBehind";
        private ResourceLoader _resourceLoader;

        public string GetResourceValue(string key)
        {
            if (_resourceLoader == null) _resourceLoader = ResourceLoader.GetForCurrentView();
            var resource = _resourceLoader.GetString($"/{ResourceFileName}/{key}");
            return resource;
        }
    }
}