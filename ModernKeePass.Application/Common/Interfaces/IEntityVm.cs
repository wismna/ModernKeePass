using System.Collections.Generic;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IEntityVm
    {
        string Id { get; set; }
        string Title { get; set; }
        Icon Icon { get; set; }
        List<GroupVm> Breadcrumb { get; }
        GroupVm ParentGroup { get; set; }
    }
}