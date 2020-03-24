using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Services
{
    public class SettingsService: ISettingsService
    {
        private readonly ISettingsProxy _settingsProxy;

        public SettingsService(ISettingsProxy settingsProxy)
        {
            _settingsProxy = settingsProxy;
        }

        public T GetSetting<T>(string property, T defaultValue = default)
        {
            return _settingsProxy.GetSetting<T>(property, defaultValue);
        }

        public void PutSetting<T>(string property, T value)
        {
            _settingsProxy.PutSetting<T>(property, value);
        }
    }
}