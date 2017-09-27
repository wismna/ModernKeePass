using System;
using ModernKeePass.Interfaces;

namespace ModernKeePass.Models
{
    public class MainMenuItem: IIsEnabled
    {
        public string Title { get; set; }
        public Type PageType { get; set; }
        public bool IsEnabled => PageType != null;

        public override string ToString()
        {
            return Title;
        }
    }
}
