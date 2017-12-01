using Windows.ApplicationModel.Resources;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Services
{
    public class ResourcesService: IResource
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
