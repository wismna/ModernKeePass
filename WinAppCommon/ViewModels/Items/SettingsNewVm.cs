using System.Collections.Generic;
using System.Linq;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Common;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsNewVm
    {
        private readonly ISettingsProxy _settings;
        
        public SettingsNewVm(ISettingsProxy settings)
        {
            _settings = settings;
        }

        public bool IsCreateSample
        {
            get { return _settings.GetSetting<bool>(Constants.Settings.Sample); }
            set { _settings.PutSetting(Constants.Settings.Sample, value); }
        }

        public IEnumerable<DatabaseFormat> FileFormats => new []
        {
            new DatabaseFormat
            {
                Version = "4",
                DisplayText = "4 (Argon2, ChaCha20)"
            }, 
            new DatabaseFormat
            {
                Version = "3",
                DisplayText = "3 (AES-KDF, AES/Rijndael)"
            }
        };

        public DatabaseFormat DatabaseFormatVersion
        {
            get
            {
                var version = _settings.GetSetting<string>(Constants.Settings.DefaultFileFormat);
                return FileFormats.FirstOrDefault(f => f.Version == version);
            }
            set { _settings.PutSetting(Constants.Settings.DefaultFileFormat, value.Version); }
        }
    }

    public class DatabaseFormat
    {
        public string Version { get; set; }
        public string DisplayText { get; set; }
    }
}
