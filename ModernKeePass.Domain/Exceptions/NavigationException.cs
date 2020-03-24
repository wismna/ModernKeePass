using System;

namespace ModernKeePass.Domain.Exceptions
{
    public class NavigationException: Exception
    {
        public NavigationException(Type pageType) : base($"Failed to load Page {pageType.FullName}")
        {
        }
    }
}