using System.Threading.Tasks;
using ModernKeePass.Domain.Dtos;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IDatabaseProxy
    {
        bool IsOpen { get; }
        string Name { get; }
        GroupEntity RecycleBin { get; set; }
        BaseEntity Cipher { get; set; }
        BaseEntity KeyDerivation { get; set; }
        string Compression { get; set; }

        Task<DatabaseEntity> Open(FileInfo fileInfo, Credentials credentials);
        Task<DatabaseEntity> Create(FileInfo fileInfo, Credentials credentials);
        Task SaveDatabase();
        Task SaveDatabase(FileInfo FileInfo);
        Task UpdateCredentials(Credentials credentials);
        void CloseDatabase();
        Task AddEntry(GroupEntity parentGroup, EntryEntity entity);
        Task AddGroup(GroupEntity parentGroup, GroupEntity entity);
        Task UpdateEntry(EntryEntity entity);
        Task UpdateGroup(GroupEntity entity);
        Task DeleteEntry(EntryEntity entity);
        Task DeleteGroup(GroupEntity entity);
    }
}