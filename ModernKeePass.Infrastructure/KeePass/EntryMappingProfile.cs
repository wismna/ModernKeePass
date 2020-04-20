using System;
using System.Linq;
using AutoMapper;
using ModernKeePass.Domain.Entities;
using ModernKeePassLib;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class EntryMappingProfile: Profile
    {
        public EntryMappingProfile()
        {
            CreateMap<PwEntry, EntryEntity>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentGroup.Uuid.ToHexString()))
                .ForMember(dest => dest.ParentName, opt => opt.MapFrom(src => src.ParentGroup.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid.ToHexString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.TitleField)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.UserNameField)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.PasswordField)))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.UrlField)))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.NotesField)))
                .ForMember(dest => dest.ForegroundColor, opt => opt.MapFrom(src => src.ForegroundColor))
                .ForMember(dest => dest.BackgroundColor, opt => opt.MapFrom(src => src.BackgroundColor))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.ExpiryTime)))
                .ForMember(dest => dest.HasExpirationDate, opt => opt.MapFrom(src => src.Expires))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => IconMapper.MapPwIconToIcon(src.IconId)))
                .ForMember(dest => dest.AdditionalFields, opt => opt.MapFrom(src =>
                    src.Strings.Where(s => !PwDefs.GetStandardFields().Contains(s.Key))
                        .ToDictionary(s => s.Key, s => GetEntryValue(src, s.Key))))
                .ForMember(dest => dest.LastModificationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.LastModificationTime)));
        }
        
        private string GetEntryValue(PwEntry entry, string key) => entry.Strings.GetSafe(key).ReadString();
    }
}