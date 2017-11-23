using Windows.ApplicationModel;

namespace ModernKeePass.ViewModels
{
    public class AboutVm
    {
        private readonly Package _package;

        public string Name => _package.DisplayName;

        public string Version
        {
            get
            {
                var version = _package.Id.Version;
                return $"{version.Major}.{version.Minor}";
            }
        }

        public AboutVm() : this(Package.Current) { }

        public AboutVm(Package package)
        {
            _package = package;
        }
    }
}
