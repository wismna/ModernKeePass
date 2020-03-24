namespace ModernKeePass.Domain.Entities
{
    public class DatabaseEntity
    {
        public string Name { get; set; }

        public GroupEntity RootGroupEntity { get; set; }
    }
}