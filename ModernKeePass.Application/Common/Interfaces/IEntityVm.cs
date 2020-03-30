﻿using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface IEntityVm
    {
        string Id { get; set; }
        string Title { get; set; }
        Icon Icon { get; set; }
    }
}