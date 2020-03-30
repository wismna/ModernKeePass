using System;
using ModernKeePass.Application.Group.Models;

namespace ModernKeePass.Events
{
    public class PasswordEventArgs: EventArgs
    {
        public GroupVm RootGroup { get; set; }

        public PasswordEventArgs(GroupVm groupVm)
        {
            RootGroup = groupVm;
        }
    }
}