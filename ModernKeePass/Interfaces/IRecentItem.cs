using Windows.Storage;

namespace ModernKeePass.Interfaces
{
    public interface IRecentItem
    {
        StorageFile DatabaseFile { get; }
        string Token { get; }
        string Name { get; }
    }
}