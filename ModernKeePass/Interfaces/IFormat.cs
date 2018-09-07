using Windows.Storage;

namespace ModernKeePass.Interfaces
{
    public interface IFormat
    {
        IPwEntity Import(IStorageFile source);
    }
}