using Windows.ApplicationModel;

namespace ModernKeePass.ViewModels
{
    public class AboutVm
    {
        public string Name { get; } = Package.Current.DisplayName;

        public string Version
        {
            get
            {
                var package = Package.Current;
                var version = package.Id.Version;

                return $"{version.Major}.{version.Minor}";
            }
        }
    }
}
