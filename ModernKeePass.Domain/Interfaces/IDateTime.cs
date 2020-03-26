using System;

namespace ModernKeePass.Domain.Interfaces
{
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}