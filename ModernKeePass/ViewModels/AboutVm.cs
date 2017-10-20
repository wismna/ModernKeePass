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
                var version = package.Id.Version;

                return $"{package.DisplayName} version {version.Major}.{version.Minor}";
            }
        }
    }
}
