using Windows.ApplicationModel;

namespace ModernKeePass.ViewModels
{
    public class AboutVm
    {
        public string Version
        {
            get
            {
                var package = Package.Current;
                var packageId = package.Id;
                var version = packageId.Version;

                return $"ModernKeePass version {version.Major}.{version.Minor}";
            }
        }
    }
}
