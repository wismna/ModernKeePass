using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using ModernKeePassLib.Cryptography.KeyDerivation;

namespace ModernKeePass.ViewModels
{
    public class SettingsNewVm
    {
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public bool IsCreateSample
        {
            get { return GetSetting<bool>("Sample"); }
            set { PutSetting("Sample", value); }
        }

        public IEnumerable<string> KeyDerivations => KdfPool.Engines.Select(e => e.Name);

        public string KeyDerivationName
        {
            get { return GetSetting<string>("KeyDerivation"); }
            set { PutSetting("KeyDerivation", value); }
        }

        // TODO: Move this to a common class
        private T GetSetting<T>(string property)
        {
            try
            {
                return (T)Convert.ChangeType(_localSettings.Values[property], typeof(T));
            }
            catch (InvalidCastException)
            {
                return default(T);
            }
        }

        private void PutSetting<T>(string property, T value)
        {
            if (_localSettings.Values.ContainsKey(property))
                _localSettings.Values[property] = value;
            else _localSettings.Values.Add(property, value);
        }
    }
}
