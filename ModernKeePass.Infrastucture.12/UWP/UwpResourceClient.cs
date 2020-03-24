using Windows.ApplicationModel.Resources;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Infrastructure.UWP
{
    public class UwpResourceClient: IResourceProxy
    {
        private const string ResourceFileName = "CodeBehind";
        private readonly ResourceLoader _resourceLoader = ResourceLoader.GetForCurrentView();

        public string GetResourceValue(string key)
        {
            var resource = _resourceLoader.GetString($"/{ResourceFileName}/{key}");
            return resource;
        }
    }
}