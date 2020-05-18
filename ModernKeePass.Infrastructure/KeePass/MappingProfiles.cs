using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
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
                .ForMember(dest => dest.Value, opt => opt.ResolveUsing<ProtectedStringResolver>())
                .ForMember(dest => dest.IsProtected, opt => opt.MapFrom(src => src.Value.IsProtected));

            CreateMap<PwEntry, EntryEntity>()
                .ForMember(dest => dest.ParentGroupId, opt => opt.MapFrom(src => src.ParentGroup.Uuid.ToHexString()))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uuid.ToHexString()))
                .ForMember(dest => dest.Fields, opt => opt.MapFrom(src => src.Strings))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.ExpiryTime)))
                .ForMember(dest => dest.HasExpirationDate, opt => opt.MapFrom(src => src.Expires))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => IconMapper.MapPwIconToIcon(src.IconId)))
                .ForMember(dest => dest.ModificationDate, opt => opt.MapFrom(src => new DateTimeOffset(src.LastModificationTime)))
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Binaries.Select(b => new KeyValuePair<string, byte[]> (b.Key, b.Value.ReadData()) )));

            CreateMap<PwGroup, GroupEntity>()
                .ForMember(d => d.ParentGroupId, opts => opts.MapFrom(s => s.ParentGroup.Uuid.ToHexString()))
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Uuid.ToHexString()))
                .ForMember(d => d.Icon, opts => opts.MapFrom(s => IconMapper.MapPwIconToIcon(s.IconId)))
                .ForMember(d => d.ModificationDate, opts => opts.MapFrom(s => s.LastModificationTime))
                .MaxDepth(2);
        }
    }

    public class ProtectedStringResolver : IValueResolver<KeyValuePair<string, ProtectedString>, FieldEntity, string>
    {
        private readonly ICryptographyClient _cryptography;

        public ProtectedStringResolver(ICryptographyClient cryptography)
        {
            _cryptography = cryptography;
        }
        
        public string Resolve(KeyValuePair<string, ProtectedString> source, FieldEntity destination, string destMember, ResolutionContext context)
        {
            // TODO: this variable will contain (temporarily) the decrypted string
            var decryptedString = source.Value.ReadString();
            return source.Value.IsProtected ? _cryptography.Protect(decryptedString).GetAwaiter().GetResult() : decryptedString;
        }
    }
}