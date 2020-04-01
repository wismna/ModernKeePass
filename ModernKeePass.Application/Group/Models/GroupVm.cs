using System;
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
        public List<GroupVm> Breadcrumb { get; } = new List<GroupVm>();
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
                .ForMember(d => d.ParentGroup, opts => opts.Ignore())
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => s.Icon))
                .ForMember(d => d.Entries, opts => opts.Ignore())
                .ForMember(d => d.SubGroups, opts => opts.Ignore());
        }

        internal static GroupVm BuildHierarchy(GroupVm parentGroup, GroupEntity groupEntity, IMapper mapper)
        {
            var groupVm = mapper.Map<GroupVm>(groupEntity);
            groupVm.ParentGroup = parentGroup;
            if (parentGroup != null)
            {
                groupVm.Breadcrumb.AddRange(parentGroup.Breadcrumb);
                groupVm.Breadcrumb.Add(parentGroup);
            }
            groupVm.Entries = groupEntity.Entries.Select(e =>
            {
                var entry = mapper.Map<EntryVm>(e);
                entry.ParentGroup = groupVm;
                entry.Breadcrumb.AddRange(groupVm.Breadcrumb);
                entry.Breadcrumb.Add(groupVm);
                return entry;
            }).OrderBy(e => e.Title).ToList();
            groupVm.SubGroups = groupEntity.SubGroups.Select(g => BuildHierarchy(groupVm, g, mapper)).ToList();

            return groupVm;
        }
    }
}