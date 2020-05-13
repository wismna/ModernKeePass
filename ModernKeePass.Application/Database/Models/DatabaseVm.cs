namespace ModernKeePass.Application.Database.Models
{
    public class DatabaseVm
    {
        public bool IsOpen { get; set; }
        public string Name { get; set; }
        public string RootGroupId { get; set; }
        public string RecycleBinId { get; set; }
        public bool IsRecycleBinEnabled { get; set; }
        public string Compression { get; set; }
        public string CipherId { get; set; }
        public string KeyDerivationId { get; set; }
        public int Size { get; internal set; }
        public bool IsDirty { get; internal set; }
        public int MaxHistoryCount { get; set; }
        public long MaxHistorySize { get; set; }
    }
}