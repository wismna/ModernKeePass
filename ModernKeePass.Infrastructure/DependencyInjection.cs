using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Infrastructure.KeePass;
using ModernKeePass.Infrastructure.UWP;

namespace ModernKeePass.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).GetTypeInfo().Assembly;
            services.AddAutoMapper(assembly);

            services.AddSingleton(typeof(IDatabaseProxy), typeof(KeePassDatabaseClient));
            services.AddTransient(typeof(ICryptographyClient), typeof(KeePassCryptographyClient));
            services.AddTransient(typeof(IPasswordProxy), typeof(KeePassPasswordClient));
            services.AddTransient(typeof(IResourceProxy), typeof(UwpResourceClient));
            services.AddTransient(typeof(ISettingsProxy), typeof(UwpSettingsClient));
            services.AddTransient(typeof(IRecentProxy), typeof(UwpRecentFilesClient));
            services.AddTransient(typeof(IFileProxy), typeof(StorageFileClient));
            return services;
        }
    }
}