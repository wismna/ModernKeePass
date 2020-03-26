using MediatR;
using ModernKeePass.Application.Common.Interfaces;

namespace ModernKeePass.Application.Settings.Queries
{
    public class GetSettingQuery<T> : IRequest<T>
    {
        public string Key { get; set; }

        public class GetSettingQueryHandler : IRequestHandler<GetSettingQuery<T>, T>
        {
            private readonly ISettingsProxy _settingsProxy;

            public GetSettingQueryHandler(ISettingsProxy settingsProxy)
            {
                _settingsProxy = settingsProxy;
            }
            public T Handle(GetSettingQuery<T> message)
            {
                return _settingsProxy.GetSetting<T>(message.Key);
            }
        }
    }
}