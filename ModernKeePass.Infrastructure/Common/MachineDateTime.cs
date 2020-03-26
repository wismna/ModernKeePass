using System;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Infrastructure.Common
{
    public class MachineDateTime: IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}