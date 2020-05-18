using System.Collections.Generic;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Application.Entry.Models;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Group.Models
{
    public class GroupVm: IEntityVm, IMapFrom<GroupEntity>
    {
        public string ParentGroupId { get; set; }
        public string ParentGroupName { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public Icon Icon { get; set; }
        public List<GroupVm> Groups { get; set; }
        public List<EntryVm> Entries { get; set; }
        
        public override string ToString()
        {
            return Title;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupEntity, GroupVm>()
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                .MaxDepth(2);
        }
    }
}