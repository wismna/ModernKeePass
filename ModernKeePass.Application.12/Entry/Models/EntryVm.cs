using System.Drawing;
using AutoMapper;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Entry.Models
{
    public class EntryVm: IMapFrom<EntryEntity>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Icon Icon { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntryEntity, EntryVm>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => s.Icon))
                .ForMember(d => d.ForegroundColor, opts => opts.MapFrom(s => s.ForegroundColor))
                .ForMember(d => d.BackgroundColor, opts => opts.MapFrom(s => s.BackgroundColor));
        }
    }
}