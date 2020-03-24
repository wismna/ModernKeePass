using AutoMapper;

namespace ModernKeePass.Application.Common.Mappings
{

    public interface IMapFrom<T>
    {
        void Mapping(Profile profile);
    }
}