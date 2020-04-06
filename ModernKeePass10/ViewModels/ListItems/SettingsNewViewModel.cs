using System.Collections.Generic;
using Autofac;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsNewViewModel
    {
        private readonly ISettingsService _settings;

        public SettingsNewViewModel() : this(App.Container.Resolve<ISettingsService>())
        { }

        public SettingsNewViewModel(ISettingsService settings)
        {
            _settings = settings;
        }

        public bool IsCreateSample
        {
            get => _settings.GetSetting<bool>("Sample");
            set => _settings.PutSetting("Sample", value);
        }

        public IEnumerable<string> FileFormats => new []{"2", "4"};

        public string FileFormatVersion
        {
            get => _settings.GetSetting("DefaultFileFormat", "2");
            set => _settings.PutSetting("DefaultFileFormat", value);
        }
    }
}
