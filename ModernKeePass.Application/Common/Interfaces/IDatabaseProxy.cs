using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IDatabaseProxy
    {
        bool IsOpen { get; }
        string Name { get; }
        string RootGroupId { get; }
        string RecycleBinId { get; set; }
        string CipherId { get; set; }
        string KeyDerivationId { get; set; }
        string Compression { get; set; }
        bool IsRecycleBinEnabled { get; set; }

        Task Open(FileInfo fileInfo, Credentials credentials);
        Task ReOpen();
        Task Create(FileInfo fileInfo, Credentials credentials, DatabaseVersion version = DatabaseVersion.V2);
        Task SaveDatabase();
        Task SaveDatabase(string filePath);
        void SetRecycleBin(string id);
        Task UpdateCredentials(Credentials credentials);
        void CloseDatabase();

        Task AddEntry(string parentGroupId, string entryId);
        Task InsertEntry(string parentGroupId, string entryId, int messageIndex);
        Task AddGroup(string parentGroupId, string groupId);
        void UpdateEntry(string entryId, string fieldName, object fieldValue);
        void UpdateGroup(string groupId);
        Task RemoveEntry(string parentGroupId, string entryId, bool isToBeDeleted);
        Task RemoveGroup(string parentGroupId, string groupId, bool isToBeDeleted);
        EntryEntity CreateEntry(string parentGroupId);
        GroupEntity CreateGroup(string parentGroupId, string nameId, bool isRecycleBin = false);
        void SortEntries(string groupId);
        void SortSubGroups(string groupId);
        EntryEntity GetEntry(string id);
        GroupEntity GetGroup(string id);
    }
}