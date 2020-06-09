using System.Collections.Generic;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Common.Models;

namespace ModernKeePass.Application.Common.Services
{
    public class BreadcrumbService: IBreadcrumbService
    {
        private readonly Stack<BreadcrumbItem> _breadcrumb;

        public BreadcrumbService()
        {
            _breadcrumb = new Stack<BreadcrumbItem>();
        }

        public void Push(BreadcrumbItem item)
        {
            _breadcrumb.Push(item);
        }

        public BreadcrumbItem Pop(int count = 1)
        {
            if (_breadcrumb.Count == 0) return null;
            for (var i = 1; i < count; i++)
            {
                _breadcrumb.Pop();
            }

            return _breadcrumb.Pop();
        }

        public IEnumerable<BreadcrumbItem> GetItems()
        {
            return _breadcrumb;
        }
    }
}