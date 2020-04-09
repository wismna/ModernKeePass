using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.ViewModels
{
    public class SettingsSaveVm
    {
        private readonly ISettingsProxy _settings;

        public SettingsSaveVm() : this(App.Services.GetRequiredService<ISettingsProxy>())
        { }

        public SettingsSaveVm(ISettingsProxy settings)
        {
            _settings = settings;
        }

        public bool IsSaveSuspend
        {
            get { return _settings.GetSetting("SaveSuspend", true); }
            set { _settings.PutSetting("SaveSuspend", value); }
        }
    }
}