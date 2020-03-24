using System;
using System.Drawing;

namespace ModernKeePass.Domain.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public DateTimeOffset LastModificationDate { get; set; }
    }
}