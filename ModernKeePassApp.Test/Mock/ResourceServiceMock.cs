using System;
using ModernKeePass.Interfaces;

namespace ModernKeePassApp.Test.Mock
{
    class ResourceServiceMock : IResource
    {
        public string GetResourceValue(string key)
        {
            return string.Empty;
        }
    }
}
