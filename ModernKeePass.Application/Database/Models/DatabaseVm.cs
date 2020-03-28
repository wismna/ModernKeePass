using ModernKeePass.Application.Group.Models;

namespace ModernKeePass.Application.Database.Models
{
    public class DatabaseVm
    {
        public bool IsOpen { get; set; }
        public string Name { get; set; }
        public GroupVm RootGroup { get; set; }
        public GroupVm RecycleBin { get; set; }
        public bool IsRecycleBinEnabled { get; set; }
        public string Compression { get; set; }
        public string CipherId { get; set; }
        public string KeyDerivationId { get; set; }
    }
}