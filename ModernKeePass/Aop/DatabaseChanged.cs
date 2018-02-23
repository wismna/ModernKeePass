using System;
using ModernKeePass.Services;

namespace ModernKeePass.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class DatabaseChangedAttribute: Attribute
    {
        public DatabaseChangedAttribute()
        {
            DatabaseService.Instance.HasChanged = true;
        }
    }
}
