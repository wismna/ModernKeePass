namespace ModernKeePass.Interfaces
{
    public interface ISettingsService
    {
        T GetSetting<T>(string property);
        void PutSetting<T>(string property, T value);
    }
}