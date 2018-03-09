namespace ModernKeePass.Interfaces
{
    public interface ISettingsService
    {
        T GetSetting<T>(string property, T defaultValue = default(T));
        void PutSetting<T>(string property, T value);
    }
}