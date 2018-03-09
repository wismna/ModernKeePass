using System;
using ModernKeePass.Interfaces;

namespace ModernKeePassApp.Test.Mock
{
    public class SettingsServiceMock : ISettingsService
    {
        public T GetSetting<T>(string property, T defaultValue = default(T))
        {
            return defaultValue;
        }

        public void PutSetting<T>(string property, T value)
        {
            throw new NotImplementedException();
        }
    }
}
