using AutoMapper;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Application.Group.Models;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Application.Database.Models
{
    public class DatabaseVm: IMapFrom<DatabaseEntity>
    {
        public bool IsOpen { get; set; }
        public string Name { get; set; }
        public GroupVm RootGroup { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DatabaseEntity, DatabaseVm>()
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.RootGroup, opts => opts.MapFrom(s => s.RootGroupEntity));
        }
    }
}