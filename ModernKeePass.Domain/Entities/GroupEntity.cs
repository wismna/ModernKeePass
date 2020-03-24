using System.Collections.Generic;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Domain.Entities
{
    public class GroupEntity : BaseEntity
    {
        public List<GroupEntity> SubGroups { get; set; } = new List<GroupEntity>();
        public List<EntryEntity> Entries { get; set; } = new List<EntryEntity>();
        public Icon Icon { get; set; }
    }
}