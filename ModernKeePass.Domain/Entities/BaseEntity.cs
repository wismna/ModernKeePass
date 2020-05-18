using System;

namespace ModernKeePass.Domain.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentGroupId { get; set; }
        public string ParentGroupName { get; set; }
        public DateTimeOffset ModificationDate { get; set; }
    }
}