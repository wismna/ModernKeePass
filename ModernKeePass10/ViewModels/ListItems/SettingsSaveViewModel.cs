using Autofac;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.ViewModels.ListItems
{
    public class SettingsSaveViewModel
    {
        private readonly ISettingsService _settings;

        public SettingsSaveViewModel() : this(App.Container.Resolve<ISettingsService>())
        { }

        public SettingsSaveViewModel(ISettingsService settings)
        {
            _settings = settings;
        }

        public bool IsSaveSuspend
        {
            get => _settings.GetSetting("SaveSuspend", true);
            set => _settings.PutSetting("SaveSuspend", value);
        }
    }
}