using Microsoft.Extensions.DependencyInjection;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Interfaces;
using ModernKeePass.Infrastructure.Common;
using ModernKeePass.Infrastructure.KeePass;
using ModernKeePass.Infrastructure.UWP;

namespace ModernKeePass.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IDatabaseProxy), typeof(KeePassDatabaseClient));
            services.AddTransient(typeof(ICryptographyClient), typeof(KeePassCryptographyClient));
            services.AddTransient(typeof(IPasswordProxy), typeof(KeePassPasswordClient));
            /*services.AddTransient(typeof(IResourceProxy), typeof(UwpResourceClient));
            services.AddTransient(typeof(ISettingsProxy), typeof(UwpSettingsClient));
            services.AddTransient(typeof(IRecentProxy), typeof(UwpRecentFilesClient));*/
            services.AddScoped(typeof(IFileProxy), typeof(StorageFileClient));
            services.AddTransient(typeof(IDateTime), typeof(MachineDateTime));
            return services;
        }
    }
}