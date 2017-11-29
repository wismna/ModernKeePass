using System.Collections.Generic;
using ModernKeePass.Services;

namespace ModernKeePass.ViewModels
{
    public class SettingsNewVm
    {

        public bool IsCreateSample
        {
            get { return SettingsService.GetSetting<bool>("Sample"); }
            set { SettingsService.PutSetting("Sample", value); }
        }

        public IEnumerable<string> FileFormats => new []{"2", "4"};

        public string FileFormatVersion
        {
            get { return SettingsService.GetSetting<string>("DefaultFileFormat"); }
            set { SettingsService.PutSetting("DefaultFileFormat", value); }
        }
    }
}
