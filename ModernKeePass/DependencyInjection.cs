using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ModernKeePass
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppAutomapper(this IServiceCollection services)
        {
            var applicationAssembly = typeof(Application.DependencyInjection).GetTypeInfo().Assembly;
            var infrastructureAssembly = typeof(Infrastructure.DependencyInjection).GetTypeInfo().Assembly;
            services.AddAutoMapper(applicationAssembly, infrastructureAssembly);

            return services;
        }
    }
}