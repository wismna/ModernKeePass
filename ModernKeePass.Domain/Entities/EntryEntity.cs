using System;
using System.Collections.Generic;
using System.Drawing;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Domain.Entities
{
    public class EntryEntity: BaseEntity
    {
        public IEnumerable<FieldEntity> Fields { get; set; } = new List<FieldEntity>();
        public IEnumerable<EntryEntity> History { get; set; } = new List<EntryEntity>();
        public Dictionary<string, byte[]> Attachments { get; set; } = new Dictionary<string, byte[]>();
        public DateTimeOffset ExpirationDate { get; set; }
        public Icon Icon { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool HasExpirationDate { get; set; }
    }
}