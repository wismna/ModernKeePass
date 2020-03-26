using AutoMapper;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Domain.Dtos;

namespace ModernKeePass.Application.Recent.Models
{
    public class RecentVm: IMapFrom<FileInfo>
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<FileInfo, RecentVm>()
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Path, opts => opts.MapFrom(s => s.Path));
        }
    }
}