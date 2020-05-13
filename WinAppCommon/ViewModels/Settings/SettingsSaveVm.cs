using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels.Settings
{
    public class SettingsSaveVm
    {
        private readonly ISettingsProxy _settings;
        
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