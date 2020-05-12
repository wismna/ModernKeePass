using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IEntityVm
    {
        string Id { get; set; }
        Icon Icon { get; set; }
        string ParentGroupId { get; set; }
        string ParentGroupName { get; set; }
    }
}