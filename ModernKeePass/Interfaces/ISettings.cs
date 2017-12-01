namespace ModernKeePass.Interfaces
{
    public interface ISettings
    {
        T GetSetting<T>(string property);
        void PutSetting<T>(string property, T value);
    }
}