using System;
using System.Linq;
using AutoMapper;
using ModernKeePass.Domain.Entities;
using ModernKeePassLib;
using ModernKeePassLib.Security;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class EntryMappingProfile: Profile
    {
        public EntryMappingProfile()
        {
            FromModelToDto();
            FromDtoToModel();
        }

        private void FromDtoToModel()
        {
            Uri url;
            CreateMap<PwEntry, EntryEntity>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentGroup.Uuid.ToHexString()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid.ToHexString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.TitleField)))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.UserNameField)))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.PasswordField)))
                .ForMember(dest => dest.Url, opt =>
                {
                    opt.PreCondition(src => Uri.TryCreate(GetEntryValue(src, PwDefs.UrlField), UriKind.Absolute, out url));
                    opt.MapFrom(src => new Uri(GetEntryValue(src, PwDefs.UrlField)));
                })
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => GetEntryValue(src, PwDefs.NotesField)))
                .ForMember(dest => dest.ForegroundColor, opt => opt.MapFrom(src => src.ForegroundColor))
                .ForMember(dest => dest.BackgroundColor, opt => opt.MapFrom(src => src.BackgroundColor))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.ExpiryTime)))
                .ForMember(dest => dest.HasExpirationDate, opt => opt.MapFrom(src => src.Expires))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => IconMapper.MapPwIconToIcon(src.IconId)))
                .ForMember(dest => dest.AdditionalFields, opt => opt.MapFrom(src =>
                    src.Strings.Where(s => !PwDefs.GetStandardFields().Contains(s.Key)).ToDictionary(s => s.Key, s => GetEntryValue(src, s.Key))))
                .ForMember(dest => dest.LastModificationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.LastModificationTime)));
        }

        private void FromModelToDto()
        {
            CreateMap<EntryEntity, PwEntry>().ConvertUsing<EntryToPwEntryDictionaryConverter>();
        }

        private string GetEntryValue(PwEntry entry, string key) => entry.Strings.GetSafe(key).ReadString();
    }

    public class EntryToPwEntryDictionaryConverter : ITypeConverter<EntryEntity, PwEntry>
    {
        public PwEntry Convert(EntryEntity source, PwEntry destination, ResolutionContext context)
        {
            //destination.Uuid = new PwUuid(System.Convert.FromBase64String(source.Id));
            destination.ExpiryTime = source.ExpirationDate.DateTime;
            destination.Expires = source.HasExpirationDate;
            destination.LastModificationTime = source.LastModificationDate.DateTime;
            destination.BackgroundColor = source.BackgroundColor;
            destination.ForegroundColor = source.ForegroundColor;
            destination.IconId = IconMapper.MapIconToPwIcon(source.Icon);
            SetEntryValue(destination, PwDefs.TitleField, source.Name);
            SetEntryValue(destination, PwDefs.UserNameField, source.UserName);
            SetEntryValue(destination, PwDefs.PasswordField, source.Password);
            SetEntryValue(destination, PwDefs.UrlField, source.Url?.ToString());
            SetEntryValue(destination, PwDefs.NotesField, source.Notes);
            foreach (var additionalField in source.AdditionalFields)
            {
                SetEntryValue(destination, additionalField.Key, additionalField.Value);
            }
            return destination;
        }
        private void SetEntryValue(PwEntry entry, string key, string newValue)
        {
            if (newValue != null) entry.Strings.Set(key, new ProtectedString(true, newValue));
        }
    }
}