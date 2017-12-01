using System.Collections.Generic;
using ModernKeePass.Interfaces;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class SettingsNewVm
    {
        private ISettings _settings;

        public SettingsNewVm() : this(new SettingsService())
        { }

        public SettingsNewVm(ISettings settings)
        {
            _settings = settings;
        }

        public bool IsCreateSample
        {
            get { return _settings.GetSetting<bool>("Sample"); }
            set { _settings.PutSetting("Sample", value); }
        }

        public IEnumerable<string> FileFormats => new []{"2", "4"};

        public string FileFormatVersion
        {
            get { return _settings.GetSetting<string>("DefaultFileFormat"); }
            set { _settings.PutSetting("DefaultFileFormat", value); }
        }
    }
}
