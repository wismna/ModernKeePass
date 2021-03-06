﻿using System;
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
        public FieldVm Title { get; set; }
        public FieldVm Username { get; set; }
        public FieldVm Password { get; set; }
        public FieldVm Notes { get; set; }
        public FieldVm Url { get; set; }
        public bool IsValidUrl => Uri.IsWellFormedUriString(Url.Value, UriKind.Absolute);
        public List<FieldVm> AdditionalFields { get; set; }
        public List<EntryVm> History { get; set; }
        public Icon Icon { get; set; }
        public Color ForegroundColor { get; set; }
        public Color BackgroundColor { get; set; }
        public bool HasExpirationDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset ModificationDate { get; set; }
        public Dictionary<string, byte[]> Attachments { get; set; }

        public override string ToString()
        {
            return ModificationDate.ToString("g");
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntryEntity, EntryVm>()
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Fields.FirstOrDefault(f => 
                    f.Name.Equals(EntryFieldName.Title, StringComparison.Ordinal)) ?? new FieldEntity { Name = EntryFieldName.Title, IsProtected = false } ))
                .ForMember(d => d.Username, opts => opts.MapFrom(s => s.Fields.FirstOrDefault(f => 
                    f.Name.Equals(EntryFieldName.UserName, StringComparison.Ordinal)) ?? new FieldEntity { Name = EntryFieldName.UserName, IsProtected = false } ))
                .ForMember(d => d.Password, opts => opts.MapFrom(s => s.Fields.FirstOrDefault(f => 
                    f.Name.Equals(EntryFieldName.Password, StringComparison.Ordinal)) ?? new FieldEntity { Name = EntryFieldName.Password, IsProtected = true } ))
                .ForMember(d => d.Url, opts => opts.MapFrom(s => s.Fields.FirstOrDefault(f => 
                    f.Name.Equals(EntryFieldName.Url, StringComparison.Ordinal)) ?? new FieldEntity { Name = EntryFieldName.Url, IsProtected = false } ))
                .ForMember(d => d.Notes, opts => opts.MapFrom(s => s.Fields.FirstOrDefault(f => 
                    f.Name.Equals(EntryFieldName.Notes, StringComparison.Ordinal)) ?? new FieldEntity { Name = EntryFieldName.Notes, IsProtected = false } ))
                .ForMember(d => d.AdditionalFields, opts => opts.MapFrom(s => 
                    s.Fields.Where(f => !EntryFieldName.StandardFieldNames.Contains(f.Name, StringComparer.Ordinal))))
                .ForMember(d => d.History, opts => opts.MapFrom(s => s.History.Reverse()))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => s.HasExpirationDate && s.ExpirationDate < DateTimeOffset.Now ? Icon.ReportHacked : s.Icon));
        }
    }
}