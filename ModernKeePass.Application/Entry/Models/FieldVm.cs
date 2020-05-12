using AutoMapper;
using ModernKeePass.Application.Common.Mappings;
using ModernKeePass.Domain.Entities;

namespace ModernKeePass.Application.Entry.Models
{
    public class FieldVm: IMapFrom<FieldEntity>
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsProtected { get; set; }

        public override string ToString() => Value;
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<FieldEntity, FieldVm>();
        }
    }
}