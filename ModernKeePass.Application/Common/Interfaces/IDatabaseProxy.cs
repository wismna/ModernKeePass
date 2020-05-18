using System.Collections.Generic;
using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IDatabaseProxy
    {
        // PW Database properties
        bool IsOpen { get; }
        string Name { get; }
        string RootGroupId { get; }
        string RecycleBinId { get; set; }
        string CipherId { get; set; }
        string KeyDerivationId { get; set; }
        string Compression { get; set; }
        bool IsRecycleBinEnabled { get; set; }

        // Custom properties
        string FileAccessToken { get; set; }
        int Size { get; set; }
        bool IsDirty { get; set; }
        int MaxHistoryCount { get; set; }
        long MaxHistorySize { get; set; }

        Task Open(byte[] file, Credentials credentials);
        Task ReOpen(byte[] file);
        Task Create(Credentials credentials, string name, DatabaseVersion version = DatabaseVersion.V4);
        Task<byte[]> SaveDatabase();
        void UpdateCredentials(Credentials credentials);
        void CloseDatabase();

        EntryEntity GetEntry(string id);
        Task AddEntry(string parentGroupId, string entryId);
        Task MoveEntry(string parentGroupId, string entryId, int index);
        Task UpdateEntry(string entryId, string fieldName, object fieldValue, bool isProtected);
        void DeleteField(string entryId, string fieldName);
        Task RemoveEntry(string parentGroupId, string entryId);
        EntryEntity CreateEntry(string parentGroupId);
        void SortEntries(string groupId);

        GroupEntity GetGroup(string id); 
        Task AddGroup(string parentGroupId, string groupId);
        Task MoveGroup(string parentGroupId, string groupId, int index);
        void UpdateGroup(GroupEntity group);
        Task RemoveGroup(string parentGroupId, string groupId);
        void DeleteEntity(string entityId);
        GroupEntity CreateGroup(string parentGroupId, string name, bool isRecycleBin = false);
        void SortSubGroups(string groupId);
        
        void AddAttachment(string entryId, string attachmentName, byte[] attachmentContent);
        void DeleteAttachment(string entryId, string attachmentName);

        EntryEntity AddHistory(string entryId);
        EntryEntity RestoreFromHistory(string entryId, int historyIndex);
        void DeleteHistory(string entryId, int historyIndex);

        IEnumerable<EntryEntity> Search(string groupId, string text);
        IEnumerable<BaseEntity> GetAllGroups(string groupId);
    }
}