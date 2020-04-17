using System;

namespace ModernKeePass.Events
{
    public class PasswordEventArgs: EventArgs
    {
        public string RootGroupId { get; set; }

        public PasswordEventArgs(string groupId)
        {
            RootGroupId = groupId;
        }
    }
}