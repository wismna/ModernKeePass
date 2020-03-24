using System.Collections.Generic;
using System.Drawing;
using AutoMapper;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Group.Models
{
    public class GroupVm: IMapFrom<GroupEntity>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Icon Icon { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public List<GroupVm> SubGroups { get; set; } = new List<GroupVm>();
        public List<EntryVm> Entries { get; set; } = new List<EntryVm>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupEntity, GroupVm>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => s.Icon))
                .ForMember(d => d.ForegroundColor, opts => opts.MapFrom(s => s.ForegroundColor))
                .ForMember(d => d.BackgroundColor, opts => opts.MapFrom(s => s.BackgroundColor))
                .ForMember(d => d.Entries, opts => opts.MapFrom(s => s.Entries))
                .ForMember(d => d.SubGroups, opts => opts.MapFrom(s => s.SubGroups));
        }
    }
}