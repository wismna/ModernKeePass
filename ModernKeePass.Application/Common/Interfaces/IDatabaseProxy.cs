using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IDatabaseProxy
    {
        string ZeroId { get; }
        bool IsOpen { get; }
        string Name { get; }
        string RootGroupId { get; }
        string RecycleBinId { get; set; }
        string CipherId { get; set; }
        string KeyDerivationId { get; set; }
        string Compression { get; set; }
        bool IsRecycleBinEnabled { get; set; }
        string FileAccessToken { get; set; }

        Task Open(byte[] file, Credentials credentials);
        Task ReOpen(byte[] file);
        Task Create(Credentials credentials, string name, DatabaseVersion version = DatabaseVersion.V2);
        Task<byte[]> SaveDatabase();
        Task<byte[]> SaveDatabase(byte[] newFileContents);
        void UpdateCredentials(Credentials credentials);
        void CloseDatabase();

        Task AddEntry(string parentGroupId, string entryId);
        Task MoveEntry(string parentGroupId, string entryId, int index);
        Task AddGroup(string parentGroupId, string groupId);
        void UpdateEntry(string entryId, string fieldName, object fieldValue);
        void UpdateGroup(string groupId);
        Task RemoveEntry(string parentGroupId, string entryId);
        Task RemoveGroup(string parentGroupId, string groupId);
        void DeleteEntity(string entityId);
        EntryEntity CreateEntry(string parentGroupId);
        GroupEntity CreateGroup(string parentGroupId, string name, bool isRecycleBin = false);
        void SortEntries(string groupId);
        void SortSubGroups(string groupId);
        EntryEntity GetEntry(string id);
        GroupEntity GetGroup(string id);
    }
}