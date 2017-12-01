using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace ModernKeePass.Interfaces
{
    public interface IRecent
    {
        int EntryCount { get; }
        Task<IStorageItem> GetFileAsync(string token);
        ObservableCollection<IRecentItem> GetAllFiles(bool removeIfNonExistant = true);
        void Add(IStorageItem file, string metadata);
    }
}