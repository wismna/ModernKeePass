using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels.Settings
{
    public class GeneralVm
    {
        private readonly ISettingsProxy _settings;
        
        public bool IsSaveSuspend
        {
            get { return _settings.GetSetting(Constants.Settings.SaveSuspend, true); }
            set { _settings.PutSetting(Constants.Settings.SaveSuspend, value); }
        }

        public int CopyExpiration
        {
            get { return _settings.GetSetting(Constants.Settings.ClipboardTimeout, 10); }
            set { _settings.PutSetting(Constants.Settings.ClipboardTimeout, value); }
        }

        public GeneralVm(ISettingsProxy settings)
        {
            _settings = settings;
        }

    }
}