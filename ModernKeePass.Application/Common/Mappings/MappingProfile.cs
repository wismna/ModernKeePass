using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace ModernKeePass.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(typeof(MappingProfile).GetTypeInfo().Assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.ExportedTypes
                .Where(t => t.GetTypeInfo().ImplementedInterfaces.Any(i =>
                    i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetTypeInfo().GetDeclaredMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}