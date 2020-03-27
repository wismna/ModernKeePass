using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IDatabaseProxy
    {
        bool IsOpen { get; }
        string Name { get; }
        GroupEntity RootGroup { get; }

        string RecycleBinId { get; set; }
        string CipherId { get; set; }
        string KeyDerivationId { get; set; }
        string Compression { get; set; }
        bool IsRecycleBinEnabled { get; }

        Task<GroupEntity> Open(FileInfo fileInfo, Credentials credentials);
        Task<GroupEntity> ReOpen();
        Task<GroupEntity> Create(FileInfo fileInfo, Credentials credentials);
        Task SaveDatabase();
        Task SaveDatabase(FileInfo FileInfo);
        Task UpdateCredentials(Credentials credentials);
        void CloseDatabase();

        Task AddEntry(string parentGroupId, string entryId);
        Task InsertEntry(string parentGroupId, string entryId, int messageIndex);
        Task AddGroup(string parentGroupId, string groupId);
        void UpdateEntry(string entryId, string fieldName, object fieldValue);
        void UpdateGroup(string groupId);
        Task RemoveEntry(string parentGroupId, string entryId);
        Task RemoveGroup(string parentGroupId, string groupId);
        EntryEntity CreateEntry(string parentGroupId);
        GroupEntity CreateGroup(string parentGroupId, string nameId, bool isRecycleBin = false);
        Task DeleteEntry(string entryId);
        Task DeleteGroup(string groupId);

        void SortEntries(string groupId);
        void SortSubGroups(string groupId);
    }
}