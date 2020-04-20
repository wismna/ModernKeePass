using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels.ListItems
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
            get { return _settings.GetSetting(Constants.Settings.SaveSuspend, true); }
            set { _settings.PutSetting(Constants.Settings.SaveSuspend, value); }
        }
    }
}