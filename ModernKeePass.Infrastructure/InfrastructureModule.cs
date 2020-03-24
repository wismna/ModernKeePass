using Autofac;
using AutoMapper;
using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Infrastructure.KeePass;
using ModernKeePass.Infrastructure.UWP;

namespace ModernKeePass.Infrastructure
{
    public class InfrastructureModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<KeePassDatabaseClient>().As<IDatabaseProxy>().SingleInstance();
            builder.RegisterType<KeePassPasswordClient>().As<IPasswordProxy>().SingleInstance();
            builder.RegisterType<KeePassCryptographyClient>().As<ICryptographyClient>();
            builder.RegisterType<UwpSettingsClient>().As<ISettingsProxy>();
            builder.RegisterType<UwpResourceClient>().As<IResourceProxy>();
            builder.RegisterType<UwpRecentFilesClient>().As<IRecentProxy>();
            builder.RegisterType<StorageFileClient>().As<IFileProxy>();

            // Register Automapper profiles
            builder.RegisterType<EntryMappingProfile>().As<Profile>();
        }
    }
}