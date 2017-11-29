using System;
using Windows.Storage;

namespace ModernKeePass.Services
{
    public class SettingsService
    {
        public static T GetSetting<T>(string property)
        {
            try
            {
                return (T)Convert.ChangeType(ApplicationData.Current.LocalSettings.Values[property], typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        public static void PutSetting<T>(string property, T value)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey(property))
                localSettings.Values[property] = value;
            else localSettings.Values.Add(property, value);
        }
    }
}
