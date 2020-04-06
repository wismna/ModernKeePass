using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class SettingsNewVm
    {
        private readonly ISettingsProxy _settings;

        public SettingsNewVm() : this(App.Services.GetService<ISettingsProxy>())
        { }

        public SettingsNewVm(ISettingsProxy settings)
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
