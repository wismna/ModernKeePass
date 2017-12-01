using System;
using Windows.Foundation.Collections;
using Windows.Storage;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Services
{
    public class SettingsService : ISettings
    {
        private readonly IPropertySet _values = ApplicationData.Current.LocalSettings.Values;

        public T GetSetting<T>(string property)
        {
            try
            {
                return (T)Convert.ChangeType(_values[property], typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        public void PutSetting<T>(string property, T value)
        {
            if (_values.ContainsKey(property))
                _values[property] = value;
            else _values.Add(property, value);
        }
    }
}
