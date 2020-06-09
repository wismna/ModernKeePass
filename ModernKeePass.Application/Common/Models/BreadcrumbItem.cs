using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Models
{
    public class BreadcrumbItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public Icon Icon { get; set; }

    }
}