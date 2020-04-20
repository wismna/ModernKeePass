using System;
using System.Collections.Generic;
using System.Drawing;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Domain.Entities
{
    public class EntryEntity: BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; } = new Dictionary<string, string>();
        public IEnumerable<EntryEntity> History { get; set; } = new List<EntryEntity>();
        public Icon Icon { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool HasExpirationDate { get; set; }
    }
}