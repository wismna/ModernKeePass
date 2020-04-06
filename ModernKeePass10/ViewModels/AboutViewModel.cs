using Windows.ApplicationModel;

namespace ModernKeePass.ViewModels
{
    public class AboutViewModel
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

        public AboutViewModel() : this(Package.Current) { }

        public AboutViewModel(Package package)
        {
            _package = package;
        }
    }
}