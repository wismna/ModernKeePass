using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ModernKeePass.Domain.Entities;
using ModernKeePassLib;
using ModernKeePassLib.Security;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<KeyValuePair<string, ProtectedString>, FieldEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Key))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value.ReadString()))
                .ForMember(dest => dest.IsProtected, opt => opt.MapFrom(src => src.Value.IsProtected));

            CreateMap<PwEntry, EntryEntity>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentGroup.Uuid.ToHexString()))
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.ParentGroup.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid.ToHexString()))
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.Strings))
                .ForMember(dest => dest.ForegroundColor, opt => opt.MapFrom(src => src.ForegroundColor))
                .ForMember(dest => dest.BackgroundColor, opt => opt.MapFrom(src => src.BackgroundColor))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.ExpiryTime)))
                .ForMember(dest => dest.HasExpirationDate, opt => opt.MapFrom(src => src.Expires))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => IconMapper.MapPwIconToIcon(src.IconId)))
                .ForMember(dest => dest.LastModificationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.LastModificationTime)))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Binaries.Select(b => new KeyValuePair<string, byte[]> (b.Key, b.Value.ReadData()) )));

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