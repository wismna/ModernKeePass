using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;
using ModernKeePass.Domain.Interfaces;

namespace ModernKeePass.Application.Group.Models
{
    public class GroupVm: IEntityVm, ISelectableModel, IMapFrom<GroupEntity>
    {
        public GroupVm ParentGroup { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public Icon Icon { get; set; }
        public List<GroupVm> SubGroups { get; set; }
        public List<EntryVm> Entries { get; set; }
        public bool IsSelected { get; set; }

        public override string ToString()
        {
            return Title;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupEntity, GroupVm>()
                .ForMember(d => d.ParentGroup, opts => opts.MapFrom(s => s.Parent))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => s.Icon))
                .ForMember(d => d.Entries, opts => opts.MapFrom(s => s.Entries.OrderBy(e => e.Name)))
                .ForMember(d => d.SubGroups, opts => opts.MapFrom(s => s.SubGroups));
        }

    }
}