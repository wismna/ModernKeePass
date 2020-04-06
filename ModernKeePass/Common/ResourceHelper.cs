using Windows.ApplicationModel.Resources;

namespace ModernKeePass.Common
{
    public class ResourceHelper
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