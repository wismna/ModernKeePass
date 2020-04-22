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
        public static IServiceCollection AddInfrastructureCommon(this IServiceCollection services)
        {
            services.AddTransient(typeof(IDateTime), typeof(MachineDateTime));
            return services;
        }
        
        public static IServiceCollection AddInfrastructureKeePass(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IDatabaseProxy), typeof(KeePassDatabaseClient));
            services.AddTransient(typeof(ICryptographyClient), typeof(KeePassCryptographyClient));
            services.AddTransient(typeof(ICredentialsProxy), typeof(KeePassCredentialsClient));
            return services;
        }

        public static IServiceCollection AddInfrastructureUwp(this IServiceCollection services)
        {
            services.AddScoped(typeof(IFileProxy), typeof(StorageFileClient));
            services.AddTransient(typeof(ISettingsProxy), typeof(UwpSettingsClient));
            services.AddTransient(typeof(IRecentProxy), typeof(UwpRecentFilesClient));
            services.AddTransient(typeof(IResourceProxy), typeof(UwpResourceClient));
            services.AddTransient(typeof(INotificationService), typeof(ToastNotificationService));
            return services;
        }
    }
}