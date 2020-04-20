using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Domain.Entities;
using ModernKeePass.Domain.Enums;

namespace ModernKeePass.Application.Entry.Models
{
    public class EntryVm: IEntityVm, IMapFrom<EntryEntity>
    {
        public string ParentGroupId { get; set; }
        public string ParentGroupName { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Notes { get; set; }
        public string Url { get; set; }
        public bool HasUrl => !string.IsNullOrEmpty(Url);
        public Dictionary<string, string> AdditionalFields { get; set; }
        public List<EntryVm> History { get; set; }
        public Icon Icon { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool HasExpirationDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset ModificationDate { get; set; }

        public override string ToString()
        {
            return ModificationDate.ToString("g");
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntryEntity, EntryVm>()
                .ForMember(d => d.ParentGroupId, opts => opts.MapFrom(s => s.ParentId))
                .ForMember(d => d.ParentGroupName, opts => opts.MapFrom(s => s.ParentName))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.UserName))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Password))
                .ForMember(d => d.Url, opts => opts.MapFrom(s => s.Url))
                .ForMember(d => d.Notes, opts => opts.MapFrom(s => s.Notes))
                .ForMember(d => d.AdditionalFields, opts => opts.MapFrom(s => s.AdditionalFields))
                .ForMember(d => d.History, opts => opts.MapFrom(s => s.History.Reverse()))
                .ForMember(d => d.HasExpirationDate, opts => opts.MapFrom(s => s.HasExpirationDate))
                .ForMember(d => d.ExpirationDate, opts => opts.MapFrom(s => s.ExpirationDate))
                .ForMember(d => d.ModificationDate, opts => opts.MapFrom(s => s.LastModificationDate))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => s.HasExpirationDate && s.ExpirationDate < DateTimeOffset.Now ? Icon.ReportHacked : s.Icon))
                .ForMember(d => d.ForegroundColor, opts => opts.MapFrom(s => s.ForegroundColor))
                .ForMember(d => d.BackgroundColor, opts => opts.MapFrom(s => s.BackgroundColor));
        }
    }
}