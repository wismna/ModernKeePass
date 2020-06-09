using System.Collections.Generic;
using ModernKeePass.Application.Common.Models;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IBreadcrumbService
    {
        void Push(BreadcrumbItem item);
        BreadcrumbItem Pop(int count = 1);
        IEnumerable<BreadcrumbItem> GetItems();
    }
}