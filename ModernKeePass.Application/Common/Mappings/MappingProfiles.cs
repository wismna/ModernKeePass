using AutoMapper;
using ModernKeePass.Application.Database.Models;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Application.Group.Models;

namespace ModernKeePass.Application.Common.Mappings
{
    public class MappingProfiles: Profile
    {
        public void ApplyMappings()
        {
            new EntryVm().Mapping(this);
            new GroupVm().Mapping(this);
        }
    }
}