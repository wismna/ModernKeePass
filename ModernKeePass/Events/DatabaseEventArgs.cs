using System;

namespace ModernKeePass.Events
{
    public class DatabaseEventArgs: EventArgs
    {
        public bool IsOpen { get; set; }
    }
}
