using System;

namespace ModernKeePass.Domain.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public GroupEntity Parent { get; set; }
        public DateTimeOffset LastModificationDate { get; set; }
    }
}