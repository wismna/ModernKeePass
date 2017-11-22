using ModernKeePass.ViewModels;
using ModernKeePassLib;
using ModernKeePassLib.Keys;

namespace ModernKeePass.Interfaces
{
    public interface IDatabase
    {
        bool RecycleBinEnabled { get; set; }
        int Status { get; set; }
        GroupVm RootGroup { get; set; }
        GroupVm RecycleBin { get; set; }

        void Open(CompositeKey key, bool createNew);
        void UpdateCompositeKey(CompositeKey key);
        bool Save();
        void CreateRecycleBin();
        void AddDeletedItem(PwUuid id);
    }
}