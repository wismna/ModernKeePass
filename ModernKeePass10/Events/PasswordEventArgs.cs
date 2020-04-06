using System;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Events
{
    public class PasswordEventArgs: EventArgs
    {
        public GroupEntity RootGroupEntity { get; set; }

        public PasswordEventArgs(GroupEntity groupEntity)
        {
            RootGroupEntity = groupEntity;
        }
    }
}