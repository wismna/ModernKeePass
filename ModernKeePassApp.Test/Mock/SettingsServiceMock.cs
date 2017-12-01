using System;
using ModernKeePass.Interfaces;

namespace ModernKeePassApp.Test.Mock
{
    public class SettingsServiceMock : ISettings
    {
        public T GetSetting<T>(string property)
        {
            return default(T);
        }

        public void PutSetting<T>(string property, T value)
        {
            throw new NotImplementedException();
        }
    }
}
