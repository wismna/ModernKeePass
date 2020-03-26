using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Settings.Commands
{
    public class PutSettingCommand<T> : IRequest
    {
        public string Key { get; set; }
        public T value { get; set; }

        public class PutSettingCommandHandler : IRequestHandler<PutSettingCommand<T>>
        {
            private readonly ISettingsProxy _settingsProxy;

            public PutSettingCommandHandler(ISettingsProxy settingsProxy)
            {
                _settingsProxy = settingsProxy;
            }
            public void Handle(PutSettingCommand<T> message)
            {
                _settingsProxy.PutSetting(message.Key, message.value);
            }
        }
    }
}