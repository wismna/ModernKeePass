using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsNewVm
    {
        private readonly ISettingsProxy _settings;

        public SettingsNewVm() : this(App.Services.GetRequiredService<ISettingsProxy>())
        { }

        public SettingsNewVm(ISettingsProxy settings)
        {
            _settings = settings;
        }

        public bool IsCreateSample
        {
            get { return _settings.GetSetting<bool>(Constants.Settings.Sample); }
            set { _settings.PutSetting(Constants.Settings.Sample, value); }
        }

        public IEnumerable<string> FileFormats => new []{"2", "4"};

        public string FileFormatVersion
        {
            get { return _settings.GetSetting<string>(Constants.Settings.DefaultFileFormat); }
            set { _settings.PutSetting(Constants.Settings.DefaultFileFormat, value); }
        }
    }
}
