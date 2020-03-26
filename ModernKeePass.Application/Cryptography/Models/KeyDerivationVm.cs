using AutoMapper;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Application.Cryptography.Models
{
    public class KeyDerivationVm : IMapFrom<BaseEntity>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BaseEntity, CipherVm>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name));
        }
    }
}