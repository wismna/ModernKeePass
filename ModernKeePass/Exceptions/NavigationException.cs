using System;

namespace ModernKeePass.Exceptions
{
    public class NavigationException: Exception
    {
        public NavigationException(Type pageType) : base($"Failed to load Page {pageType.FullName}")
        {
        }
    }
}