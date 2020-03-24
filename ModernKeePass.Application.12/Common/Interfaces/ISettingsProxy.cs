namespace ModernKeePass.Application.Common.Interfaces
{
    public interface ISettingsProxy
    {
        T GetSetting<T>(string property, T defaultValue = default);
        void PutSetting<T>(string property, T value);
    }
}