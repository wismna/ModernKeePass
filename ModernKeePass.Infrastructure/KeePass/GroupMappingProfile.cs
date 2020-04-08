using AutoMapper;
using ModernKeePass.Domain.Entities;
using ModernKeePassLib;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<PwGroup, GroupEntity>()
                .ForMember(d => d.ParentId, opts => opts.MapFrom(s => s.ParentGroup.Uuid.ToHexString()))
                .ForMember(d => d.ParentName, opts => opts.MapFrom(s => s.ParentGroup.Name))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Uuid.ToHexString()))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => IconMapper.MapPwIconToIcon(s.IconId)))
                .ForMember(d => d.LastModificationDate, opts => opts.MapFrom(s => s.LastModificationTime))
                .ForMember(d => d.Entries, opts => opts.MapFrom(s => s.Entries))
                .ForMember(d => d.SubGroups, opts => opts.MapFrom(s => s.Groups))
                .MaxDepth(2);
        }
    }
}